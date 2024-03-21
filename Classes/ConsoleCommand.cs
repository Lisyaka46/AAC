﻿using IWshRuntimeLibrary;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static AAC.Classes.AnimationDL.Animate.AnimText;
using static AAC.Classes.TypeCommand;
using static AAC.Startcs;
using Microsoft.Speech.Recognition;
using System.ComponentModel;
using System.Text.RegularExpressions;
using static AAC.MiniFunctions;

namespace AAC.Classes
{
    /// <summary>
    /// Консольная команда
    /// </summary>
    public partial class ConsoleCommand
    {
        /// <summary>
        /// Имя команды
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// ID индификатор команды
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Параметры команды
        /// </summary>
        public Parameter[]? CommandParameters { get; private set; }

        /// <summary>
        /// Описание консольной команды
        /// </summary>
        public string ExplanationCommand { get; }

        /// <summary>
        /// Инициализировать объект консольной команды
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="id">Индификатор</param>
        /// <param name="parameters">Параметры команды</param>
        public ConsoleCommand(string name, int id, Parameter[]? parameters, string? Explanation)
        {
            Name = name;
            ID = id;
            CommandParameters = parameters;
            Explanation ??= string.Empty;
            if (Explanation.Length > 0) ExplanationCommand = Explanation;
            else ExplanationCommand = "Нет описания";
        }

        /// <summary>
        /// Скопировать объект консольной команды
        /// </summary>
        /// <param name="CopyParam">Копировать параметры консольной команды или нет</param>
        /// <returns>Копия существующего объекта консольной команды</returns>
        public ConsoleCommand Copy(bool CopyParam = true) => new(Name, ID, CopyParam ? CommandParameters : null, ExplanationCommand);

        /// <summary>
        /// Сгенерировать пропись имени команды
        /// </summary>
        /// <returns>Итоговая пропись команды</returns>
        public string GenRegTeamNameCommand() => Name[0].ToString().ToUpper() + Name[1..].Replace("_", " ");

        /// <summary>
        /// Сгенерировать пропись всей команды
        /// </summary>
        /// <returns>Итоговая пропись команды</returns>
        public string GenRegTeamAllCommand()
        {
            string Result = GenRegTeamNameCommand();
            if (CommandParameters?.Length > 0)
            {
                Result += "* ";
                for (int i = 0; i < CommandParameters.Length; i++)
                    Result += $"<{CommandParameters[i].Name}>{(i < CommandParameters.Length - 1 ? ", " : string.Empty)}";
            }
            return Result;
        }

        /// <summary>
        /// Создать объект ошибочной команды
        /// </summary>
        /// <param name="TextCommand">Имя команды</param>
        /// <returns>Ошибочная команда</returns>
        public static ConsoleCommand NotCommand(string TextCommand) => new(TextCommand, -1, null, null);

        /// <summary>
        /// Поиск консольной команды по её имени
        /// </summary>
        /// <param name="ArrayConsoleCommand">Массив консольных команд</param>
        /// <param name="TextCommand">Текст консольной команды</param>
        /// <param name="CopyParam">При удачном поиске копировать данные параметров команды или нет</param>
        /// <returns>Найденная консольная команда</returns>
        public static ConsoleCommand SearchConsoleCommand(ConsoleCommand[] ArrayConsoleCommand, string TextCommand, bool CopyParam)
        {
            string[] AllNameCommand = ArrayConsoleCommand.Select(i => i.Name).ToArray();
            for (int i = 0; i < AllNameCommand.Length; i++)
                if (AllNameCommand[i].Equals(TextCommand)) return ArrayConsoleCommand[i].Copy(CopyParam);
            return NotCommand(TextCommand);
        }

        /// <summary>
        /// Создать выполнение команды
        /// </summary>
        /// <param name="InputConsole">Была введена ли эта команда или нет</param>
        public StateResult ExecuteCommand(bool InputConsole)
        {
            Instr_AnimText animText;
            if (InputConsole) App.MainForm.tbInput.Text = string.Empty;
            ObjLog.LOGTextAppend($"Выполнение команды:\n<{ID} | {Name}>");
            if ((bool)MainData.Settings.Developer_Mode)
            {
                App.MainForm.lDeveloper_NameCommand.Text = $"NC: <\"{Name}\">";
                App.MainForm.lDeveloper_ParametersCommand.Text = $"PC: <\"{string.Join(", ", CommandParameters?.Select(i => i.Value.Length == 0 ? "null" : i.Value) ?? [])}\">";
                App.MainForm.lDeveloper_CountParametersCommand.Text = $"CPC: {CommandParameters?.Length ?? 0}";
            }

            // Команды
            switch (ID)
            {
                // settings
                case 1:
                    App.MainForm.BSettings_Click(null, null);
                    return StateResult.Completed;

                // clear
                case 2:
                    ObjLog.LOGTextAppend($"Была распознана очистка консоли <tbOutput> (Командой clear)");
                    AnimationDL.StopAnimate(AnimationDL.StyleAnimateObj.AnimText, "tbOutput");
                    App.MainForm.tbOutput.Text = string.Empty;
                    return StateResult.Completed;

                // print
                case 3:
                    if (CommandParameters != null)
                        return new(ResultStateCommand.Complete, $">>> {CommandParameters[0].Value}\n", string.Empty);
                    else return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command", $"Недостаточно параметров для выполнения команды {Name}");

                // reboot
                case 4:
                    //App.MainForm.BCloseMainApplication(null, null);
                    //Thread.Sleep(75);
                    Application.Restart();
                    return StateResult.Completed;

                // close
                case 5:
                    Environment.Exit(0);
                    return StateResult.Completed;

                // close_process
                case 6:
                    if (CommandParameters != null)
                    {
                        if (!CommandParameters[0].Value.Equals(string.Empty))
                        {
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
                            string Name = CommandParameters[0].Value.Replace(".exe", string.Empty);
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
                                return new StateResult(ResultStateCommand.Failed,
                                $">>> ~Execution is blocked closed <{Name}.exe>\n",
                                "Было вызвано исключение об невозможном закрытии процесса так как он явзяется СИСТЕМНЫМ");
                            }
                        }
                    }
                    else
                        return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");
                    return StateResult.Completed;

                // cmd
                case 7:
                    if (CommandParameters == null) Process.Start("cmd");
                    else
                    {
                        foreach (Parameter ParameterCommandCMD in CommandParameters)
                        {
                            if (ParameterCommandCMD.Value.Length > 0) Process.Start("cmd", "/C " + ParameterCommandCMD.Value);
                            else break;
                        }
                    }
                    return StateResult.Completed;

                // emptytrash
                case 8:
                    ObjLog.LOGTextAppend($"Была вызвана команда очистки корзины");
                    uint result = DLLMethods.SHEmptyRecycleBin(IntPtr.Zero, null, 0);
                    animText = new(App.MainForm.tbOutput, ">>> The basket is cleared!\n");
                    animText.AnimInit(true);
                    return StateResult.Completed;

                //
                case 9:
                    return StateResult.Completed;

                //
                case 10:
                    return StateResult.Completed;

                // font_size
                case 11:
                    if (CommandParameters != null)
                    {
                        if (Stringint(CommandParameters[0].Value))
                            return new StateResult(ResultStateCommand.Failed,
                                $">>> Failed. font size is not number: {CommandParameters[0].Value}\n",
                                "Была вызвана ошибка команды <font_size> что введено было не число");
                        else if (Convert.ToInt32(CommandParameters[0].Value) < 7 || Convert.ToInt32(CommandParameters[0].Value) > 40)
                            return new StateResult(ResultStateCommand.Failed,
                                $">>> Failed. font size range 7 - 40: {CommandParameters[0].Value}\n",
                                "Была вызвана ошибка команды <font_size> об несоответствии параметра диапазону значений");
                        else
                        {
                            MainData.Settings.SetParamOption("Text-Size", CommandParameters[0].Value);
                            App.MainForm.tbOutput.Font = new Font(App.MainForm.tbOutput.Font.Name, Convert.ToInt32(CommandParameters[0].Value));
                            App.MainForm.tbOutput.Update();
                        }
                    }
                    else
                        return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");
                    return StateResult.Completed;

                // open_link
                case 12:
                    ObjLog.LOGTextAppend($"Была вызвана функция открывающая ссылку в браузере");
                    if (CommandParameters != null)
                    {
                        try
                        {
                            Process.Start(new ProcessStartInfo(CommandParameters[0].Value) { UseShellExecute = true });
                            ObjLog.LOGTextAppend($"Активировалась ссылка: <{CommandParameters[0].Value}>");
                            if (App.MainForm.WindowState == FormWindowState.Normal)
                            {
                                animText = new(App.MainForm.tbOutput, $">>> Opening a link{$": {CommandParameters[0].Value} ..."}\n");
                                animText.AnimInit(true);
                            }
                        }
                        catch
                        {
                            return new StateResult(ResultStateCommand.Failed,
                                $"Failed activate link \"{CommandParameters[0].Value}\"", $"Не удалось открыть ссылку {CommandParameters[0].Value}");
                        }
                    }
                    else
                        return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");
                    return StateResult.Completed;

                // open_directory
                case 13:
                    ObjLog.LOGTextAppend($"Была вызвана функция открывающая директорию");
                    if (CommandParameters != null)
                    {
                        if (Directory.Exists(CommandParameters[0].Value))
                            Process.Start("explorer.exe", CommandParameters[0].Value);
                        else
                            return new StateResult(ResultStateCommand.Failed,
                                $">>> Failed. Inknown path name: {CommandParameters[0].Value}\n", "Было вызвано исключение из-за неизвестной директории");
                    }
                    else
                        Process.Start("explorer.exe", Directory.GetCurrentDirectory());
                    if (App.MainForm.WindowState == FormWindowState.Normal)
                    {
                        animText = new(App.MainForm.tbOutput,
                            $">>> Opening a directory: {(CommandParameters == null ? "MAIN" : CommandParameters[0].Value)}...\n");
                        animText.AnimInit(true);
                    }
                    return StateResult.Completed;

                // open_file
                case 14:
                    ObjLog.LOGTextAppend($"Была вызвана команда об открытии файла по директории");
                    if (CommandParameters != null)
                    {
                        if (System.IO.File.Exists(CommandParameters[0].Value))
                        {
                            try
                            {
                                if (Path.GetExtension(CommandParameters[0].Value).Equals(".url"))
                                    Process.Start(new ProcessStartInfo(CommandParameters[0].Value) { UseShellExecute = true });
                                else
                                    Process.Start(CommandParameters[0].Value);
                                return new(ResultStateCommand.Complete, $">>> Opening a file {CommandParameters[0].Value}...\n", string.Empty);
                            }
                            catch
                            {
                                return new StateResult(ResultStateCommand.Failed,
                                    ">>> Failed. There is no opening program for the file..\n", "Было вызвано исключение из-за неизвестной программе открывающей файл");
                            }
                        }
                        else
                            return new StateResult(ResultStateCommand.Failed,
                                $">>> Failed. Inknown path or file: {CommandParameters[0].Value}\n",
                                "Было вызвано исключение об отсутствии файла по данной директории");
                    }
                    else
                        return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");

                // create_label
                case 15:
                    ObjLog.LOGTextAppend($"Была вызвана команда об создании ярлыка по директории");
                    if (CommandParameters != null)
                    {
                        if (CommandParameters.Length < 2)
                            return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");
                        if (Directory.Exists(CommandParameters[1].Value))
                        {
                            object shDesktop = "Desktop";
                            string Name = CommandParameters[0].Value.Replace(".lnk", string.Empty);
                            WshShell shell = new();
                            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @$"\{Name}.lnk";
                            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                            shortcut.TargetPath = CommandParameters[1].Value;
                            shortcut.Save();
                            animText = new(App.MainForm.tbOutput,
                                $">>> Completed create label {Name} is directory: {CommandParameters[1].Value}..\n");
                            animText.AnimInit(true);
                        }
                        else
                            return new StateResult(ResultStateCommand.Failed,
                                $"Было вызвано исключение об отсутствии директории для ярлыка <{CommandParameters[1].Value}>",
                                $">>> Failed. <{CommandParameters[1].Value}> not is directory\n");
                    }
                    else
                        return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");
                    return StateResult.Completed;

                // help
                case 16:
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
                    return StateResult.Completed;

                // windows_bat
                case 17:
                    if (CommandParameters != null)
                    {
                        string Name = CommandParameters[0].Value.Replace(".bat", string.Empty);
                        ObjLog.LOGTextAppend($"Была вызвана команда о открытии файла Windows <{Name}.bat>");
                        if (System.IO.File.Exists($"{Directory.GetCurrentDirectory()}\\Data\\Bat\\{CommandParameters[0].Value}.bat"))
                        {
                            ObjLog.LOGTextAppend($"Успешно открыт <{CommandParameters[0].Value}.bat>");
                            if (App.MainForm.WindowState != FormWindowState.Normal) MainData.MainMP3.PlaySound("Complete");
                            Process.Start($"{Directory.GetCurrentDirectory()}\\Data\\Bat\\{CommandParameters[0].Value}.bat");
                        }
                        else
                            return new StateResult(ResultStateCommand.Failed,
                                $">>> Failed. <{CommandParameters[0].Value}> not is directory: BAT.FILE\n",
                                $"Было вызвано исключение об отсутствии <{CommandParameters[0].Value}.bat> в директории BAT.FILE");
                    }
                    else
                        return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");
                    return StateResult.Completed;

                // test
                case 18:
                    ObjLog.LOGTextAppend($"Было вызвано программное тестирование");
                    if ((bool)MainData.Settings.Developer_Mode)
                    {
                        //ObjLog.LOGTextAppend("Text.I".Substring(0, "Text.I".IndexOf(".", 0, "Text.I".Length)));
                        /*
                        SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindows();

                        foreach (SHDocVw.InternetExplorer ie in shellWindows)
                        {
                            //MessageBox.Show(ie.LocationName);
                            MessageBox.Show(ie.LocationURL);
                        }
                        */
                    }
                    else
                    {
                        return new StateResult(ResultStateCommand.Failed,
                            ">>> Access denied due to security settings\n", "Было вызвано исключение об малых правах пользователя перед разработчиком");
                    }
                    return StateResult.Completed;

                //
                case 19:
                    return StateResult.Completed;

                //
                case 20:
                    return StateResult.Completed;

                // log
                case 21:
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
                    return StateResult.Completed;

                // save_log
                case 22:
                    ObjLog.LOGTextAppend($"Была вызвана команда сохранения информации журнала");
                    StreamWriter file_write = new($"{Directory.GetCurrentDirectory()}\\Data\\Log\\{DateTime.Now:HH.mm.ss}.txt");
                    animText = new(App.MainForm.tbOutput, $">>> The data was successfully saved in the file: Data\\Log\\{DateTime.Now:HH.mm.ss}.txt\n");
                    animText.AnimInit(true);
                    file_write.Write(ObjLog.MassLogElements.Select(i => i.Text).ToArray());
                    file_write.Close();
                    return StateResult.Completed;

                // delete_log
                case 23:
                    ObjLog.LOGTextAppend($"Была вызвана команда удаления (всех) файлов инф. журнала");
                    if (CommandParameters != null)
                    {
                        if (System.IO.File.Exists($"{Directory.GetCurrentDirectory()}\\Data\\Log\\{CommandParameters[0].Value}.txt"))
                        {
                            ActivateActionDialog("Удаление файла", $"Удалить файл {CommandParameters[0].Value}.txt");
                            if (MainData.Flags.ResultConfirmationAction == Data.DialogWindowStatus.Ok)
                            {
                                System.IO.File.Delete($"{Directory.GetCurrentDirectory()}\\Data\\Log\\{CommandParameters[0].Value}.txt");
                                animText = new(App.MainForm.tbOutput, $">>> Complete delete log file information {CommandParameters[0].Value}.txt\n");
                                animText.AnimInit(true);
                            }
                            else
                            {
                                animText = new(App.MainForm.tbOutput, ">>> Cancel clear log file(s)\n");
                                animText.AnimInit(true);
                            }
                        }
                        else
                            return new StateResult(ResultStateCommand.Failed,
                                $">>> Failed. Inknown file: Data\\Log\\{CommandParameters[0].Value}.txt\n", "Было вызвано исключение из-за неизвестного файла");
                    }
                    else
                    {
                        ActivateActionDialog("Удаление файлов log", "Удалить все файлы информации log");
                        if (MainData.Flags.ResultConfirmationAction == Data.DialogWindowStatus.Ok)
                        {
                            foreach (string file in Directory.GetFiles($"{Directory.GetCurrentDirectory()}\\Data\\Log\\"))
                                System.IO.File.Delete($"{file}");
                            animText = new(App.MainForm.tbOutput, ">>> Complete clear log file information\n");
                        }
                        else
                        {
                            animText = new(App.MainForm.tbOutput, ">>> Cancel clear log file(s)\n");
                            animText.AnimInit(true);
                        }
                    }
                    return StateResult.Completed;

                // pythagorean_three
                case 24:
                    if (CommandParameters != null)
                    {
                        if (CommandParameters.Length < 2)
                            return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");
                        if (!Stringint(CommandParameters[0].Value) && !Stringint(CommandParameters[1].Value))
                        {
                            int u = Convert.ToInt32(CommandParameters[0].Value), v = Convert.ToInt32(CommandParameters[1].Value);
                            (int, int) PythagorTree = (u * u - v * v, 2 * u * v);
                            animText = new(App.MainForm.tbOutput,
                                $">>> Pythagorean_three this: {PythagorTree.Item1} + {PythagorTree.Item2}i = {u * u + v * v}\n");
                            animText.AnimInit(true);
                        }
                        else
                        {
                            int InvalidIndex = !Stringint(CommandParameters[0].Value) ? 1 : 0;
                            return new StateResult(ResultStateCommand.Failed,
                                    $">>> Failed. The parameter #{InvalidIndex} is not a number: {CommandParameters[InvalidIndex].Value}\n",
                                    "Было вызвано исключение из-за не цифрового параметра");
                        }
                    }
                    else return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                    $"Недостаточно параметров для выполнения команды {Name}");
                    return StateResult.Completed;

                // colored
                case 25:
                    try
                    {
                        App.Settings.ThemesCreated.WindowState = FormWindowState.Normal;
                        App.Settings.ThemesCreated.Show();
                    }
                    catch { App.Settings.ThemesCreated = new(); }
                    App.Settings.ThemesCreated.Show();
                    return StateResult.Completed;

                // new_label
                case 26:
                    App.MainForm.GenerateLabel();
                    return StateResult.Completed;

                // NOT COMMAND
                default:
                    string TextLOG = "Команда не распозналась и вызвала исключение об ошибке";
                    if (Name.Length > 0) return new StateResult(ResultStateCommand.Failed, $">>> ~Invalid Command: \"{Name}\"\n", TextLOG);
                    else return new StateResult(ResultStateCommand.Failed, $">>> ~Invalid NULL Command\n", TextLOG);
            };
        }
    }
}