using AAC.Classes;
using AAC.Classes.Commands;
using AAC.Classes.DataClasses;
using IWshRuntimeLibrary;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static AAC.Classes.AnimationDL.Animate;
using static AAC.Classes.AnimationDL.Animate.AnimText;
using static AAC.Startcs;

namespace AAC
{
    public static class Reading
    {
        /// <summary>
        /// Прочитать базу данных консольных команд и создать массив консольных команд
        /// </summary>
        /// <returns>Массив консольных команд</returns>
        public static ConsoleCommand[] ReadConsoleCommandDataBase()
        {
            /*Описание всех консольных, встроенных команд*/ {
                return
                [
                    new ConsoleCommand("settings", [], "Открывает глобальные настройки", (param) =>
                    {
                        Apps.MainForm.BSettings_Click(null, null);
                        return Task.FromResult(CommandStateResult.Completed);
                    }),
                    new ConsoleCommand("clear", [], "Очистка выводимых данных", (param) =>
                    {
                        ObjLog.LOGTextAppend($"Была распознана очистка консоли <tbOutput> (Командой clear)");
                        AnimationDL.StopAnimate(AnimationDL.StyleAnimateObj.AnimText, "tbOutput");
                        Apps.MainForm.tbOutput.Text = string.Empty;
                        return Task.FromResult(CommandStateResult.Completed);
                    }),
                    new ConsoleCommand("print", [new Parameter("Text", true)], "Вывод текста на экран", (param) =>
                    {
                        return Task.FromResult(new CommandStateResult(ResultState.Complete, $">>> {param[0]}\n", string.Empty));
                    }),
                    new ConsoleCommand("reboot", [], "Перезагружает программу", (param) =>
                    {
                        Application.Restart();
                        return Task.FromResult(CommandStateResult.Completed);
                    }),
                    new ConsoleCommand("close", [], "Закрывает программу", (param) =>
                    {
                        Environment.Exit(0);
                        return Task.FromResult(CommandStateResult.Completed);
                    }),
                    new ConsoleCommand("close_process", [new Parameter("Process", true)], "Завершает работу данного процесса", (param) =>
                    {
                        if (!param[0].Equals(string.Empty))
                        {
                            Instr_AnimText animText;
                            string[] ListSystemProcess =
                            [
                                "explorer",
                                "svchost",
                                "lsass",
                                "sihost",
                                "system",
                                "ntoskrnl",
                                "wininit",
                                "dwm",
                                "smss",
                                "services",
                                "winlogon",
                                "csrss",
                                "runtimebroker",
                                "applicationframehost",
                                "ctfmon",
                                "securityhealthservice",
                                "spoolsv"
                            ];
                            string Name = param[0].Replace(".exe", string.Empty);
                            ObjLog.LOGTextAppend($"Была вызвана команда о закрытии процесса <{Name}.exe>");
                            if (!ListSystemProcess.Contains(Name.ToLower()))
                            {
                                Process[] ProcessByName = Process.GetProcessesByName(Name);
                                if (ProcessByName.Length > 0)
                                {
                                    foreach (Process p in ProcessByName) p.Kill();
                                    ObjLog.LOGTextAppend($"Процесс <{Name}*.exe> успешно закрыт!");
                                    if (Apps.MainForm.WindowState == FormWindowState.Normal)
                                    {
                                        animText = new(Apps.MainForm.tbOutput, $">>> {Name}*.exe closed!");
                                        animText.AnimInit(true);
                                    }
                                }
                                else
                                {
                                    ObjLog.LOGTextAppend($"Процесс <{Name}*.exe> не найден");
                                    if (Apps.MainForm.WindowState == FormWindowState.Normal)
                                    {
                                        animText = new(Apps.MainForm.tbOutput, $">>> {Name}*.exe not found!");
                                        animText.AnimInit(true);
                                    }
                                }
                            }
                            else
                            {
                                return Task.FromResult(new CommandStateResult(ResultState.Failed,
                                $">>> ~Execution is blocked closed <{Name}.exe>\n",
                                "Было вызвано исключение об невозможном закрытии процесса так как он явзяется СИСТЕМНЫМ"));
                            }
                        }
                        return Task.FromResult(CommandStateResult.Completed);
                    }),
                    new ConsoleCommand("cmd",
                    [
                        new Parameter("C1", false), new Parameter("C2", false),
                        new Parameter("C3", false), new Parameter("C4", false),
                        new Parameter("C5", false), new Parameter("C6", false)
                    ], "Открывает командную строку Windows с текущей директорией программы", (param) =>
                    {
                        if (param.Length == 0) Process.Start("cmd");
                        else
                        {
                            foreach (string ParameterCommandCMD in param)
                            {
                                if (ParameterCommandCMD.Length > 0) Process.Start("cmd", "/C " + ParameterCommandCMD);
                                else break;
                            }
                        }
                        return Task.FromResult(CommandStateResult.Completed);
                    }),
                    new ConsoleCommand("emptytrash", [], "Очищает корзину", (param) =>
                    {
                        Instr_AnimText animText;
                        ObjLog.LOGTextAppend($"Была вызвана команда очистки корзины");
                        uint result = DLLMethods.SHEmptyRecycleBin(nint.Zero, null, 0);
                        animText = new(Apps.MainForm.tbOutput, ">>> The basket is cleared!\n");
                        animText.AnimInit(true);
                        return Task.FromResult(CommandStateResult.Completed);
                    }),
                    new ConsoleCommand("font_size", [new Parameter("Size", true)], "Изменяет размер выводимого текста в консоли", (param) =>
                    {
                        if (MiniFunctions.Stringint(param[0]))
                            return Task.FromResult(new CommandStateResult(ResultState.Failed,
                                $">>> Failed. font size is not number: {param[0]}\n",
                                "Была вызвана ошибка команды <font_size> что введено было не число"));
                        else if (Convert.ToInt32(param[0]) < 7 || Convert.ToInt32(param[0]) > 40)
                            return Task.FromResult(new CommandStateResult(ResultState.Failed,
                                $">>> Failed. font size range 7 - 40: {param[0]}\n",
                                "Была вызвана ошибка команды <font_size> об несоответствии параметра диапазону значений"));
                        else
                        {
                            MainData.Settings.SetParamOption("Text-Size", param[0]);
                            Apps.MainForm.tbOutput.Font = new Font(Apps.MainForm.tbOutput.Font.Name, Convert.ToInt32(param[0]));
                            Apps.MainForm.tbOutput.Update();
                        }
                        return Task.FromResult(CommandStateResult.Completed);
                    }),
                    new ConsoleCommand("open_link", [new Parameter("Link", true)], "Открывает в браузере заданную ссылку", (param) =>
                    {
                        try
                        {
                            Instr_AnimText animText;
                            Process.Start(new ProcessStartInfo(param[0]) { UseShellExecute = true });
                            ObjLog.LOGTextAppend($"Активировалась ссылка: <{param[0]}>");
                            if (Apps.MainForm.WindowState == FormWindowState.Normal)
                            {
                                animText = new(Apps.MainForm.tbOutput, $">>> Opening a link{$": {param[0]} ..."}\n");
                                animText.AnimInit(true);
                            }
                        }
                        catch
                        {
                            return Task.FromResult(new CommandStateResult(ResultState.Failed,
                                $"Failed activate link \"{param[0]}\"", $"Не удалось открыть ссылку {param[0]}"));
                        }
                        return Task.FromResult(CommandStateResult.Completed);
                    }),
                    new ConsoleCommand("open_directory", [new Parameter("Directory", false)], "Открывает заданную директорию в проводнике", (param) =>
                    {
                        Instr_AnimText animText;
                        ObjLog.LOGTextAppend($"Была вызвана функция открывающая директорию");
                        if (param.Length > 0)
                        {
                            if (Directory.Exists(param[0]))
                                Process.Start("explorer.exe", param[0]);
                            else
                                return Task.FromResult(new CommandStateResult(ResultState.Failed,
                                    $">>> Failed. Inknown path name: {param[0]}\n", "Было вызвано исключение из-за неизвестной директории"));
                        }
                        else
                            Process.Start("explorer.exe", Directory.GetCurrentDirectory());
                        if (Apps.MainForm.WindowState == FormWindowState.Normal)
                        {
                            animText = new(Apps.MainForm.tbOutput,
                                $">>> Opening a directory: {(param.Length == 0 ? "MAIN" : param[0])}...\n");
                            animText.AnimInit(true);
                        }
                        return Task.FromResult(CommandStateResult.Completed);
                    }),
                    new ConsoleCommand("open_file", [new Parameter("File", true)], "Открывает файл по его заданной директории", (param) =>
                    {
                        ObjLog.LOGTextAppend($"Была вызвана команда об открытии файла по директории");
                        if (System.IO.File.Exists(param[0]))
                        {
                            try
                            {
                                if (Path.GetExtension(param[0]).Equals(".url"))
                                    Process.Start(new ProcessStartInfo(param[0]) { UseShellExecute = true });
                                else
                                    Process.Start(param[0]);
                                return Task.FromResult(new CommandStateResult(ResultState.Complete, $">>> Opening a file {param[0]}...\n", string.Empty));
                            }
                            catch
                            {
                                return Task.FromResult(new CommandStateResult(ResultState.Failed,
                                    ">>> Failed. There is no opening program for the file..\n", "Было вызвано исключение из-за неизвестной программе открывающей файл"));
                            }
                        }
                        else
                            return Task.FromResult(new CommandStateResult(ResultState.Failed,
                                $">>> Failed. Inknown path or file: {param[0]}\n",
                                "Было вызвано исключение об отсутствии файла по данной директории"));
                    }),
                    new ConsoleCommand("create_label", [new Parameter("Name", true), new Parameter("Directory", true)], "Создаёт ярлык на рабочем столе с заданным именем на директорию файла", (param) =>
                    {
                        if (Directory.Exists(param[1]))
                        {
                            object shDesktop = "Desktop";
                            string Name = param[0].Replace(".lnk", string.Empty);
                            WshShell shell = new();
                            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @$"\{Name}.lnk";
                            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                            shortcut.TargetPath = param[1];
                            shortcut.Save();
                            return Task.FromResult(new CommandStateResult(ResultState.Complete, $">>> Completed create label {Name} is directory: {param[1]}..\n", string.Empty));
                        }
                        else
                            return Task.FromResult(new CommandStateResult(ResultState.Failed,
                                $"Было вызвано исключение об отсутствии директории для ярлыка <{param[1]}>",
                                $">>> Failed. <{param[1]}> not is directory\n"));
                    }),
                    new ConsoleCommand("help", [], "Открывает окно описания всех команд доступных в программе", (param) =>
                    {
                        try
                        {
                            Apps.InformationCommand.WindowState = FormWindowState.Normal;
                            ObjLog.LOGTextAppend($"Форма пояснения всех команд открыта успешно!");
                        }
                        catch (NullReferenceException)
                        {
                            Apps.InformationCommand = new FormExplanationCommands();
                            Apps.InformationCommand.Show();
                            ObjLog.LOGTextAppend($"Форма пояснения всех команд открыта с ошибкой: <Во избежании была создана новая форма>");
                        }
                        Apps.MainForm.FoldingApplication(null, null);
                        return Task.FromResult(CommandStateResult.Completed);
                    }),
                    new ConsoleCommand("windows_bat", [new Parameter("Name", true)], "Активирует BAT файл с заданным именем в определённой директории", (param) =>
                    {
                        param[0] = param[0].Replace(".bat", string.Empty);
                        ObjLog.LOGTextAppend($"Была вызвана команда о открытии файла Windows <{param[0]}>");
                        if (System.IO.File.Exists($"{Directory.GetCurrentDirectory()}/Data/Bat/{param[0]}.bat"))
                        {
                            ObjLog.LOGTextAppend($"Успешно открыт <{param[0]}.bat>");
                            if (Apps.MainForm.WindowState != FormWindowState.Normal) MainData.MainMP3.PlaySound("Complete");
                            Process.Start($"{Directory.GetCurrentDirectory()}/Data/Bat/{param[0]}.bat");
                        }
                        else
                            return Task.FromResult(new CommandStateResult(ResultState.Failed,
                                $">>> Failed. <{param[0]}> not is directory: BAT.FILE\n",
                                $"Было вызвано исключение об отсутствии <{param[0]}.bat> в директории BAT.FILE"));
                        return Task.FromResult(CommandStateResult.Completed);
                    }),
                    new ConsoleCommand("log", [], "Открывает окно журнала сообщений программы", (param) =>
                    {
                        try
                        {
                            Apps.Log.Show();
                            Apps.Log.WindowState = FormWindowState.Normal;
                            ObjLog.LOGTextAppend("Форма журнала открыта успешно!");
                        }
                        catch
                        {
                            ObjLog.LOGTextAppend("Форма журнала открыта с ошибкой: <Во избежании была создана новая форма>");
                            Apps.Log = new();
                        }
                        Apps.Log.Show();
                        Apps.Log.WindowState = FormWindowState.Normal;
                        return Task.FromResult(CommandStateResult.Completed);
                    }),
                    new ConsoleCommand("save_log", [], "Сохраняет сообщения журнала в отдельный файл TXT", (param) =>
                    {
                        ObjLog.LOGTextAppend($"Была вызвана команда сохранения информации журнала");
                        StreamWriter file_write = new($"{Directory.GetCurrentDirectory()}/Data/Log/{DateTime.Now:HH.mm.ss}.txt");
                        file_write.Write(ObjLog.MassLogElements.Select(i => i.Text).ToArray());
                        file_write.Close();
                        return Task.FromResult(new CommandStateResult(ResultState.Complete, $">>> The data was successfully saved in the file: ..Data/Log/{DateTime.Now:HH.mm.ss}.txt\n", string.Empty));
                    }),
                    new ConsoleCommand("delete_log", [new Parameter("Name", false)], "Удаляет отдельный файл сообщений при указании его имени. При отсутствии имени будет предложено удалить все файлы сообщений", (param) =>
                    {
                        ObjLog.LOGTextAppend($"Была вызвана команда удаления (всех) файлов инф. журнала");
                        string ConsoleOutput;
                        if (param.Length > 0)
                        {
                            if (System.IO.File.Exists($"{Directory.GetCurrentDirectory()}/Data/Log/{param[0]}.txt"))
                            {
                                MiniFunctions.ActivateActionDialog("Удаление файла", $"Удалить файл {param[0]}.txt");
                                if (MainData.Flags.ResultConfirmationAction == DialogWindowStatus.Ok)
                                {
                                    System.IO.File.Delete($"{Directory.GetCurrentDirectory()}/Data/Log/{param[0]}.txt");
                                    ConsoleOutput = $">>> Complete delete log file information {param[0]}.txt\n";
                                }
                                else ConsoleOutput = ">>> Cancel clear log file(s)\n";
                            }
                            else
                                return Task.FromResult(new CommandStateResult(ResultState.Failed,
                                    $">>> Failed. Inknown file: ..Data/Log/{param[0]}.txt\n", "Было вызвано исключение из-за неизвестного файла"));
                        }
                        else
                        {
                            MiniFunctions.ActivateActionDialog("Удаление файлов log", "Удалить все файлы информации log");
                            if (MainData.Flags.ResultConfirmationAction == DialogWindowStatus.Ok)
                            {
                                foreach (string file in Directory.GetFiles($"{Directory.GetCurrentDirectory()}/Data/Log/"))
                                    System.IO.File.Delete($"{file}");
                                ConsoleOutput = ">>> Complete clear log file information\n";
                            }
                            else ConsoleOutput = ">>> Cancel clear log file(s)\n";
                        }
                        return Task.FromResult(new CommandStateResult(ResultState.Complete, ConsoleOutput, string.Empty));
                    }),
                    new ConsoleCommand("pythagorean_three", [new Parameter("BaseLength", true), new Parameter("Catheter I", true)],
                    "Вычисляет пифогорову тройку по формуле комплекстного числа заданной длинны основания и заданного катета представляемого комплекстным числом", (param) =>
                    {
                        if (!MiniFunctions.Stringint(param[0]) && !MiniFunctions.Stringint(param[1]))
                        {
                            int u = Convert.ToInt32(param[0]), v = Convert.ToInt32(param[1]);
                            (int, int) PythagorTree = (u * u - v * v, 2 * u * v);
                            return Task.FromResult(new CommandStateResult(ResultState.Complete, $">>> Pythagorean_three this: {u} + {v}i = {PythagorTree.Item1} + {PythagorTree.Item2}\n", string.Empty));
                        }
                        else
                        {
                            int InvalidIndex = !MiniFunctions.Stringint(param[0]) ? 1 : 0;
                            return Task.FromResult(new CommandStateResult(ResultState.Failed,
                                    $">>> Failed. The parameter #{InvalidIndex} is not a number: {param[InvalidIndex]}\n",
                                    "Было вызвано исключение из-за не цифрового параметра"));
                        }
                    }),
                    new ConsoleCommand("colored", [], "Открывает окно редактора цветовых палитр", (param) =>
                    {
                        try
                        {
                            Apps.ThemesCreated.WindowState = FormWindowState.Normal;
                            Apps.ThemesCreated.Show();
                        }
                        catch { Apps.ThemesCreated = new(); }
                        Apps.ThemesCreated.Show();
                        return Task.FromResult(CommandStateResult.Completed);
                    }),
                    new ConsoleCommand("new_label", [], "Открывает окно редактора создания нового ярлыка программы", (param) =>
                    {
                        Apps.MainForm.GenerateLabel();
                        return Task.FromResult(CommandStateResult.Completed);
                    }),
                ];
            }
        }

        /// <summary>
        /// Прочитать базу данных консольных команд и создать массив консольных команд
        /// </summary>
        /// <returns>Массив консольных команд</returns>
        public static VoiceCommand[] ReadVoiceCommandDataBase()
        {
            return
            [
                new VoiceCommand(["ты работаешь", "ты жив"], "Воспроизводит звук подтверждая что голосовые команды работают", () =>
                {
                    MainData.MainMP3.PlaySound("YesVoice");
                    return Task.FromResult(CommandStateResult.Completed);
                }),
                new VoiceCommand(["закрой программу", "закрыть программу"], "Завершает работу программы", () =>
                {
                    ConsoleCommand.ReadConsoleCommand(MainData.MainCommandData.MassConsoleCommand, "close");
                    return Task.FromResult(CommandStateResult.Completed);
                }),
                new VoiceCommand(["очистить", "очисти вывод", "очисти консоль"], "Очищает вывод консоли", () =>
                {
                    ConsoleCommand.ReadConsoleCommand(MainData.MainCommandData.MassConsoleCommand, "clear");
                    return Task.FromResult(CommandStateResult.Completed);
                }),
                new VoiceCommand(["активируй программу", "появись", "развернуть программу"], "Разворачивает программу делая её активным окном", () =>
                {
                    if (Apps.MainForm.StateAnimWindow != StateAnimateWindow.Active)
                        Apps.MainForm.UnfoldingApplication(null, null);
                    return Task.FromResult(CommandStateResult.Completed);
                }),
                new VoiceCommand(["блок", "заблокировать компьютер", "заблокируй компьютер"], "Блокирует компьютер выводя начальный экран", () =>
                {
                    if (!DLLMethods.LockWorkStation()) throw new Win32Exception(Marshal.GetLastWin32Error());
                    return Task.FromResult(CommandStateResult.Completed);
                }),
                new VoiceCommand(["выключи голосовые команды", "отключи голос", "отключить голос"], "Выключает голосовые команды", () =>
                {
                    Apps.MainForm.VoiceButtonImageUpdate(StatusFlags.Sleep, false);
                    MainData.InputVoiceDevice.Diactivate();
                    MainData.Flags.AudioCommand = StatusFlags.Sleep;
                    if (Apps.MainForm.StateAnimWindow != StateAnimateWindow.Active) MainData.MainMP3.PlaySound("Complete");
                    return Task.FromResult(CommandStateResult.Completed);
                }),
            ];
        }
        /*private CommandStateResult ExecuteVoiceCommand()
        {
            switch (ID)
            {
                case 6: // спрячся
                    Apps.MainForm.FoldingMoveApplication(null, null);
                    return CommandStateResult.Completed;
                case 7: // открой рабочую директорию
                    Process.Start("explorer.exe", Directory.GetCurrentDirectory());
                    return CommandStateResult.Completed;
                case 8: // сверни всё
                    Apps.MainForm.Show();
                    if (DLLMethods.ShellGUID("DesktopVisualTrue")) MainData.MainMP3.PlaySound("Complete");
                    return CommandStateResult.Completed;
                case 9: // открой панель управления
                    if (DLLMethods.ShellGUID("CommandPanelWin")) MainData.MainMP3.PlaySound("Complete");
                    return CommandStateResult.Completed;
                case 12: // выключи голосовые команды
                    Apps.MainForm.pbVoiceButton.Image = Image.FromFile(@"Data\Image\Micro\MicroSleepingNotMouse.png");
                    MainData.Flags.AudioCommand = StatusFlags.Sleep;
                    if (Apps.MainForm.WindowState != FormWindowState.Normal)
                        MainData.MainMP3.PlaySound("Complete");
                    return CommandStateResult.Completed;
                case 13: // закрой окно информации
                    if (Apps.InformationCommand.Visible)
                    {
                        Apps.InformationCommand.Close();
                        if (Apps.MainForm.WindowState != FormWindowState.Normal)
                            MainData.MainMP3.PlaySound("Complete");
                    }
                    return CommandStateResult.Completed;
                case 14: // сколько времени
                    /*
                    using System.Speech.Synthesis;

namespace ConsoleApplication5
    {
        class Program
        {

            static void Main(string[] args)
            {
                SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                synthesizer.Volume = 100;  // 0...100
                synthesizer.Rate = -2;     // -10...10

                // Synchronous
                synthesizer.Speak("Hello World");

                // Asynchronous
                synthesizer.SpeakAsync("Hello World");



            }

        }
    }
    TimeTempus();
                    return CommandStateResult.Completed;
                case 15:
                    Apps.MainForm.pbVoiceButton.Image = Image.FromFile(@"Data\Image\Micro\MicroActivateNotMouse.png");
                    MainData.Flags.AudioCommand = StatusFlags.Active;
                    return CommandStateResult.Completed;
            }
            return new CommandStateResult(ResultState.Failed,
                $">>> Voice command ID: {ID} is Invalid",
                $"Голосовая команда ID: {ID} не нраспознана");
        }*/
    }
}