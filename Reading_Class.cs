using AAC.Classes;
using AAC.Classes.Commands;
using AAC.Classes.DataClasses;
using IWshRuntimeLibrary;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
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
                        App.MainForm.BSettings_Click(null, null);
                        return CommandStateResult.Completed;
                    }),
                    new ConsoleCommand("clear", [], "Очистка выводимых данных", (param) =>
                    {
                        ObjLog.LOGTextAppend($"Была распознана очистка консоли <tbOutput> (Командой clear)");
                        AnimationDL.StopAnimate(AnimationDL.StyleAnimateObj.AnimText, "tbOutput");
                        App.MainForm.tbOutput.Text = string.Empty;
                        return CommandStateResult.Completed;
                    }),
                    new ConsoleCommand("print", [new Parameter("Text", true)], "Вывод текста на экран", (param) =>
                    {
                        return new(ResultState.Complete, $">>> {param[0]}\n", string.Empty);
                    }),
                    new ConsoleCommand("reboot", [], "Перезагружает программу", (param) =>
                    {
                        Application.Restart();
                        return CommandStateResult.Completed;
                    }),
                    new ConsoleCommand("close", [], "Закрывает программу", (param) =>
                    {
                        Environment.Exit(0);
                        return CommandStateResult.Completed;
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
                                    if (App.MainForm.WindowState == FormWindowState.Normal)
                                    {
                                        animText = new(App.MainForm.tbOutput, $">>> {Name}*.exe closed!");
                                        animText.AnimInit(true);
                                    }
                                }
                                else
                                {
                                    ObjLog.LOGTextAppend($"Процесс <{Name}*.exe> не найден");
                                    if (App.MainForm.WindowState == FormWindowState.Normal)
                                    {
                                        animText = new(App.MainForm.tbOutput, $">>> {Name}*.exe not found!");
                                        animText.AnimInit(true);
                                    }
                                }
                            }
                            else
                            {
                                return new CommandStateResult(ResultState.Failed,
                                $">>> ~Execution is blocked closed <{Name}.exe>\n",
                                "Было вызвано исключение об невозможном закрытии процесса так как он явзяется СИСТЕМНЫМ");
                            }
                        }
                        return CommandStateResult.Completed;
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
                        return CommandStateResult.Completed;
                    }),
                    new ConsoleCommand("emptytrash", [], "Очищает корзину", (param) =>
                    {
                        Instr_AnimText animText;
                        ObjLog.LOGTextAppend($"Была вызвана команда очистки корзины");
                        uint result = DLLMethods.SHEmptyRecycleBin(nint.Zero, null, 0);
                        animText = new(App.MainForm.tbOutput, ">>> The basket is cleared!\n");
                        animText.AnimInit(true);
                        return CommandStateResult.Completed;
                    }),
                    new ConsoleCommand("font_size", [new Parameter("Size", true)], "Изменяет размер выводимого текста в консоли", (param) =>
                    {
                        if (MiniFunctions.Stringint(param[0]))
                            return new CommandStateResult(ResultState.Failed,
                                $">>> Failed. font size is not number: {param[0]}\n",
                                "Была вызвана ошибка команды <font_size> что введено было не число");
                        else if (Convert.ToInt32(param[0]) < 7 || Convert.ToInt32(param[0]) > 40)
                            return new CommandStateResult(ResultState.Failed,
                                $">>> Failed. font size range 7 - 40: {param[0]}\n",
                                "Была вызвана ошибка команды <font_size> об несоответствии параметра диапазону значений");
                        else
                        {
                            MainData.Settings.SetParamOption("Text-Size", param[0]);
                            App.MainForm.tbOutput.Font = new Font(App.MainForm.tbOutput.Font.Name, Convert.ToInt32(param[0]));
                            App.MainForm.tbOutput.Update();
                        }
                        return CommandStateResult.Completed;
                    }),
                    new ConsoleCommand("open_link", [new Parameter("Link", true)], "Открывает в браузере заданную ссылку", (param) =>
                    {
                        try
                        {
                            Instr_AnimText animText;
                            Process.Start(new ProcessStartInfo(param[0]) { UseShellExecute = true });
                            ObjLog.LOGTextAppend($"Активировалась ссылка: <{param[0]}>");
                            if (App.MainForm.WindowState == FormWindowState.Normal)
                            {
                                animText = new(App.MainForm.tbOutput, $">>> Opening a link{$": {param[0]} ..."}\n");
                                animText.AnimInit(true);
                            }
                        }
                        catch
                        {
                            return new CommandStateResult(ResultState.Failed,
                                $"Failed activate link \"{param[0]}\"", $"Не удалось открыть ссылку {param[0]}");
                        }
                        return CommandStateResult.Completed;
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
                                return new CommandStateResult(ResultState.Failed,
                                    $">>> Failed. Inknown path name: {param[0]}\n", "Было вызвано исключение из-за неизвестной директории");
                        }
                        else
                            Process.Start("explorer.exe", Directory.GetCurrentDirectory());
                        if (App.MainForm.WindowState == FormWindowState.Normal)
                        {
                            animText = new(App.MainForm.tbOutput,
                                $">>> Opening a directory: {(param.Length == 0 ? "MAIN" : param[0])}...\n");
                            animText.AnimInit(true);
                        }
                        return CommandStateResult.Completed;
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
                                return new(ResultState.Complete, $">>> Opening a file {param[0]}...\n", string.Empty);
                            }
                            catch
                            {
                                return new CommandStateResult(ResultState.Failed,
                                    ">>> Failed. There is no opening program for the file..\n", "Было вызвано исключение из-за неизвестной программе открывающей файл");
                            }
                        }
                        else
                            return new CommandStateResult(ResultState.Failed,
                                $">>> Failed. Inknown path or file: {param[0]}\n",
                                "Было вызвано исключение об отсутствии файла по данной директории");
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
                            return new CommandStateResult(ResultState.Complete, $">>> Completed create label {Name} is directory: {param[1]}..\n", string.Empty);
                        }
                        else
                            return new CommandStateResult(ResultState.Failed,
                                $"Было вызвано исключение об отсутствии директории для ярлыка <{param[1]}>",
                                $">>> Failed. <{param[1]}> not is directory\n");
                    }),
                    new ConsoleCommand("help", [], "Открывает окно описания всех команд доступных в программе", (param) =>
                    {
                        try
                        {
                            App.InformationCommand.WindowState = FormWindowState.Normal;
                            ObjLog.LOGTextAppend($"Форма пояснения всех команд открыта успешно!");
                        }
                        catch (NullReferenceException)
                        {
                            App.InformationCommand = new FormExplanationCommands();
                            App.InformationCommand.Show();
                            ObjLog.LOGTextAppend($"Форма пояснения всех команд открыта с ошибкой: <Во избежании была создана новая форма>");
                        }
                        App.MainForm.FoldingApplication(null, null);
                        return CommandStateResult.Completed;
                    }),
                    new ConsoleCommand("windows_bat", [new Parameter("Name", true)], "Активирует BAT файл с заданным именем в определённой директории", (param) =>
                    {
                        param[0] = param[0].Replace(".bat", string.Empty);
                        ObjLog.LOGTextAppend($"Была вызвана команда о открытии файла Windows <{param[0]}>");
                        if (System.IO.File.Exists($"{Directory.GetCurrentDirectory()}/Data/Bat/{param[0]}.bat"))
                        {
                            ObjLog.LOGTextAppend($"Успешно открыт <{param[0]}.bat>");
                            if (App.MainForm.WindowState != FormWindowState.Normal) MainData.MainMP3.PlaySound("Complete");
                            Process.Start($"{Directory.GetCurrentDirectory()}/Data/Bat/{param[0]}.bat");
                        }
                        else
                            return new CommandStateResult(ResultState.Failed,
                                $">>> Failed. <{param[0]}> not is directory: BAT.FILE\n",
                                $"Было вызвано исключение об отсутствии <{param[0]}.bat> в директории BAT.FILE");
                        return CommandStateResult.Completed;
                    }),
                    new ConsoleCommand("log", [], "Открывает окно журнала сообщений программы", (param) =>
                    {
                        try
                        {
                            App.Log.Show();
                            App.Log.WindowState = FormWindowState.Normal;
                            ObjLog.LOGTextAppend("Форма журнала открыта успешно!");
                        }
                        catch
                        {
                            ObjLog.LOGTextAppend("Форма журнала открыта с ошибкой: <Во избежании была создана новая форма>");
                            App.Log = new();
                        }
                        App.Log.Show();
                        App.Log.WindowState = FormWindowState.Normal;
                        return CommandStateResult.Completed;
                    }),
                    new ConsoleCommand("save_log", [], "Сохраняет сообщения журнала в отдельный файл TXT", (param) =>
                    {
                        ObjLog.LOGTextAppend($"Была вызвана команда сохранения информации журнала");
                        StreamWriter file_write = new($"{Directory.GetCurrentDirectory()}/Data/Log/{DateTime.Now:HH.mm.ss}.txt");
                        file_write.Write(ObjLog.MassLogElements.Select(i => i.Text).ToArray());
                        file_write.Close();
                        return new CommandStateResult(ResultState.Complete, $">>> The data was successfully saved in the file: ..Data/Log/{DateTime.Now:HH.mm.ss}.txt\n", string.Empty);
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
                                return new CommandStateResult(ResultState.Failed,
                                    $">>> Failed. Inknown file: ..Data/Log/{param[0]}.txt\n", "Было вызвано исключение из-за неизвестного файла");
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
                        return new CommandStateResult(ResultState.Complete, ConsoleOutput, string.Empty);
                    }),
                    new ConsoleCommand("pythagorean_three", [new Parameter("BaseLength", true), new Parameter("Catheter I", true)],
                    "Вычисляет пифогорову тройку по формуле комплекстного числа заданной длинны основания и заданного катета представляемого комплекстным числом", (param) =>
                    {
                        if (!MiniFunctions.Stringint(param[0]) && !MiniFunctions.Stringint(param[1]))
                        {
                            int u = Convert.ToInt32(param[0]), v = Convert.ToInt32(param[1]);
                            (int, int) PythagorTree = (u * u - v * v, 2 * u * v);
                            return new CommandStateResult(ResultState.Complete, $">>> Pythagorean_three this: {PythagorTree.Item1} + {PythagorTree.Item2}i = {u * u + v * v}\n", string.Empty);
                        }
                        else
                        {
                            int InvalidIndex = !MiniFunctions.Stringint(param[0]) ? 1 : 0;
                            return new CommandStateResult(ResultState.Failed,
                                    $">>> Failed. The parameter #{InvalidIndex} is not a number: {param[InvalidIndex]}\n",
                                    "Было вызвано исключение из-за не цифрового параметра");
                        }
                    }),
                    new ConsoleCommand("colored", [], "Открывает окно редактора цветовых палитр", (param) =>
                    {
                        try
                        {
                            App.Settings.ThemesCreated.WindowState = FormWindowState.Normal;
                            App.Settings.ThemesCreated.Show();
                        }
                        catch { App.Settings.ThemesCreated = new(); }
                        App.Settings.ThemesCreated.Show();
                        return CommandStateResult.Completed;
                    }),
                    new ConsoleCommand("new_label", [], "Открывает окно редактора создания нового ярлыка программы", (param) =>
                    {
                        App.MainForm.GenerateLabel();
                        return CommandStateResult.Completed;
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
            return [];
        }
    }
}