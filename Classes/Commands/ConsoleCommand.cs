using IWshRuntimeLibrary;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static AAC.Classes.AnimationDL.Animate.AnimText;
using static AAC.MiniFunctions;
using static AAC.Startcs;
using AAC.Classes.DataClasses;
using static AAC.Classes.Commands.ConsoleCommand;
using System.CodeDom.Compiler;

namespace AAC.Classes.Commands
{
    /// <summary>
    /// Консольная команда
    /// </summary>
    public class ConsoleCommand
    {
        /// <summary>
        /// Делегат события выполнения команды
        /// </summary>
        /// <param name="ParametersValue">Параметры команды</param>
        /// <returns>Итог выполнения команды</returns>
        public delegate CommandStateResult ExecuteCom(string[] ParametersValue);

        /// <summary>
        /// Имя команды
        /// </summary>
        private readonly string Name;

        /// <summary>
        /// Описание консольной команды
        /// </summary>
        public readonly string Explanation;

        /// <summary>
        /// Параметры команды
        /// </summary>
        public readonly Parameter[] Parameters;

        /// <summary>
        /// Действие которое выполняет команда
        /// </summary>
        private event ExecuteCom Execute;

        /// <summary>
        /// Инициализировать объект консольной команды
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <param name="Parameters">Параметры команды</param>
        public ConsoleCommand(string Name, Parameter[] Parameters, string? Explanation, ExecuteCom? Execute)
        {
            this.Name = Name;
            this.Parameters = Parameters;
            this.Explanation = Explanation ?? "Нет описания";
            Execute ??= (parameters) => { return CommandStateResult.Completed; };
            this.Execute = Execute;

        }

        /// <summary>
        /// Сгенерировать пропись команды
        /// </summary>
        /// <returns>Строка прописи команды</returns>
        public string WritingCommandAll()
        {
            string Output = WritingCommandName();
            if (Parameters.Length > 0) Output += $"* <{string.Join(", <", Parameters.Select(i => $"{i.Name}{(i.Absolutly ? string.Empty : "?")}>"))}";
            return Output;
        }

        /// <summary>
        /// Сгенерировать пропись команды
        /// </summary>
        /// <returns>Строка прописи команды</returns>
        public string WritingCommandName() => $"{char.ToUpper(Name[0])}{Name[1..].ToString().Replace("_", " ") ?? string.Empty}";

        /// <summary>
        /// Сгенерировать пропись команды
        /// </summary>
        /// <returns>Строка прописи команды</returns>
        public string[] WritingCommandParameters()
        {
            List<string> Output = [];
            if (Parameters.Length > 0)
            {
                IEnumerable<string> ParameterNames = Parameters.Select(I => I.Name + ">");
                Output.Add($"{string.Join(", <", ParameterNames)}");
            }
            return [.. Output];
        }

        /// <summary>
        /// Прочитать команду из консоли
        /// </summary>
        /// <param name="TextCommand">Читаемая команда</param>
        /// <returns>Объект консольной команды</returns>
        public static void ReadConsoleCommand(string TextCommand, TextBox? ConsoleText = null)
        {
            ObjLog.LOGTextAppend($"Начинаю читать команду <{TextCommand}>");
            List<string> Parameters = [];
            while (TextCommand.Length > 0)
            {
                if (TextCommand[^1] == ' ') TextCommand = TextCommand.Remove(TextCommand.Length - 1);
                else if (TextCommand.Contains("  ")) TextCommand = TextCommand.Replace("  ", " ");
                else break;
            }
            if (TextCommand.Contains('*') && TextCommand[^1] != '*') // command* param1, param2, param3 ...
            {
                if (TextCommand[TextCommand.IndexOf('*') + 1] != ' ') TextCommand = TextCommand.Replace("*", "* ");
                TextCommand = TextCommand[0..TextCommand.IndexOf('*')].Replace(" ", "_").ToLower() + TextCommand[TextCommand.IndexOf('*')..];
                Parameters = Regex.Matches(TextCommand, @"( |\*|,)([^,]|,,)+").Select(i => i.Value[2..]).ToList();
                TextCommand = Regex.Match(TextCommand, @"\b[^\*~!@#$<>,.\/\\?|'"";:`%^&*()\[\]{} \-=+]+\* ?").Value.ToString().Replace("*", string.Empty).Replace(" ", string.Empty);
            }
            else // command
            {
                TextCommand = TextCommand.Replace(" ", "_").Replace("*", string.Empty).ToLower();
            }
            ConsoleCommand? SearchCommand = MainData.MainCommandData.MassConsoleCommand.SingleOrDefault(i => i.Name.Equals(TextCommand));
            CommandStateResult ResultState;
            if (SearchCommand == null)
            {
                ResultState = new CommandStateResult(Commands.ResultState.Failed, $"Invalid command \"{TextCommand}\"", $"Команда \"{TextCommand}\" не найдена");
            }
            else
            {
                ResultState = SearchCommand.ExecuteCommand([.. Parameters]);
            }
            if (ConsoleText != null) ConsoleText.Text = string.Empty;
            ResultState.Summarize();
        }

        /// <summary>
        /// Создать выполнение команды
        /// </summary>
        public CommandStateResult ExecuteCommand(string[] parameters)
        {
            int LengthParam = 0;
            Array.ForEach(Parameters, (i) => { if (i.Absolutly) LengthParam++; });
            if (parameters.Length >= LengthParam)
            {
                ObjLog.LOGTextAppend($"Выполнение команды:\n<{Name}>");
                App.MainForm.lDeveloper_ParametersCommand.Text = $"PC: <{string.Join(", ", parameters.AsEnumerable())}>";
                return Execute.Invoke(parameters);
            }
            else return new CommandStateResult(ResultState.Failed,
                $"There are not enough parameters to execute the \"{Name}\" command",
                $"Недостаточно параметров для выполнения команды \"{Name}\"");
            /*
            Instr_AnimText animText;

            // Команды
            switch (ID)
            {
                // settings
                case 1:
                    App.MainForm.BSettings_Click(null, null);
                    return CommandStateResult.Completed;

                // clear
                case 2:
                    ObjLog.LOGTextAppend($"Была распознана очистка консоли <tbOutput> (Командой clear)");
                    AnimationDL.StopAnimate(AnimationDL.StyleAnimateObj.AnimText, "tbOutput");
                    App.MainForm.tbOutput.Text = string.Empty;
                    return CommandStateResult.Completed;

                // print
                case 3:
                    if (Parameters != null)
                        return new(ResultState.Complete, $">>> {Parameters[0].Value}\n", string.Empty);
                    else return new(ResultState.Failed, "There are not enough parameters to execute the command", $"Недостаточно параметров для выполнения команды {Name}");

                // reboot
                case 4:
                    //App.MainForm.BCloseMainApplication(null, null);
                    //Thread.Sleep(75);
                    Application.Restart();
                    return CommandStateResult.Completed;

                // close
                case 5:
                    Environment.Exit(0);
                    return CommandStateResult.Completed;

                // close_process
                case 6:
                    if (Parameters != null)
                    {
                        if (!Parameters[0].Value.Equals(string.Empty))
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
                            string Name = Parameters[0].Value.Replace(".exe", string.Empty);
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
                    }
                    else
                        return new(ResultState.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");
                    return CommandStateResult.Completed;

                // cmd
                case 7:
                    if (Parameters == null) Process.Start("cmd");
                    else
                    {
                        foreach (Parameter ParameterCommandCMD in Parameters)
                        {
                            if (ParameterCommandCMD.Value.Length > 0) Process.Start("cmd", "/C " + ParameterCommandCMD.Value);
                            else break;
                        }
                    }
                    return CommandStateResult.Completed;

                // emptytrash
                case 8:
                    ObjLog.LOGTextAppend($"Была вызвана команда очистки корзины");
                    uint result = DLLMethods.SHEmptyRecycleBin(nint.Zero, null, 0);
                    animText = new(App.MainForm.tbOutput, ">>> The basket is cleared!\n");
                    animText.AnimInit(true);
                    return CommandStateResult.Completed;

                //
                case 9:
                    return CommandStateResult.Completed;

                //
                case 10:
                    return CommandStateResult.Completed;

                // font_size
                case 11:
                    if (Parameters != null)
                    {
                        if (Stringint(Parameters[0].Value))
                            return new CommandStateResult(ResultState.Failed,
                                $">>> Failed. font size is not number: {Parameters[0].Value}\n",
                                "Была вызвана ошибка команды <font_size> что введено было не число");
                        else if (Convert.ToInt32(Parameters[0].Value) < 7 || Convert.ToInt32(Parameters[0].Value) > 40)
                            return new CommandStateResult(ResultState.Failed,
                                $">>> Failed. font size range 7 - 40: {Parameters[0].Value}\n",
                                "Была вызвана ошибка команды <font_size> об несоответствии параметра диапазону значений");
                        else
                        {
                            MainData.Settings.SetParamOption("Text-Size", Parameters[0].Value);
                            App.MainForm.tbOutput.Font = new Font(App.MainForm.tbOutput.Font.Name, Convert.ToInt32(Parameters[0].Value));
                            App.MainForm.tbOutput.Update();
                        }
                    }
                    else
                        return new(ResultState.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");
                    return CommandStateResult.Completed;

                // open_link
                case 12:
                    ObjLog.LOGTextAppend($"Была вызвана функция открывающая ссылку в браузере");
                    if (Parameters != null)
                    {
                        try
                        {
                            Process.Start(new ProcessStartInfo(Parameters[0].Value) { UseShellExecute = true });
                            ObjLog.LOGTextAppend($"Активировалась ссылка: <{Parameters[0].Value}>");
                            if (App.MainForm.WindowState == FormWindowState.Normal)
                            {
                                animText = new(App.MainForm.tbOutput, $">>> Opening a link{$": {Parameters[0].Value} ..."}\n");
                                animText.AnimInit(true);
                            }
                        }
                        catch
                        {
                            return new CommandStateResult(ResultState.Failed,
                                $"Failed activate link \"{Parameters[0].Value}\"", $"Не удалось открыть ссылку {Parameters[0].Value}");
                        }
                    }
                    else
                        return new(ResultState.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");
                    return CommandStateResult.Completed;

                // open_directory
                case 13:
                    ObjLog.LOGTextAppend($"Была вызвана функция открывающая директорию");
                    if (Parameters != null)
                    {
                        if (Directory.Exists(Parameters[0].Value))
                            Process.Start("explorer.exe", Parameters[0].Value);
                        else
                            return new CommandStateResult(ResultState.Failed,
                                $">>> Failed. Inknown path name: {Parameters[0].Value}\n", "Было вызвано исключение из-за неизвестной директории");
                    }
                    else
                        Process.Start("explorer.exe", Directory.GetCurrentDirectory());
                    if (App.MainForm.WindowState == FormWindowState.Normal)
                    {
                        animText = new(App.MainForm.tbOutput,
                            $">>> Opening a directory: {(Parameters == null ? "MAIN" : Parameters[0].Value)}...\n");
                        animText.AnimInit(true);
                    }
                    return CommandStateResult.Completed;

                // open_file
                case 14:
                    ObjLog.LOGTextAppend($"Была вызвана команда об открытии файла по директории");
                    if (Parameters != null)
                    {
                        if (System.IO.File.Exists(Parameters[0].Value))
                        {
                            try
                            {
                                if (Path.GetExtension(Parameters[0].Value).Equals(".url"))
                                    Process.Start(new ProcessStartInfo(Parameters[0].Value) { UseShellExecute = true });
                                else
                                    Process.Start(Parameters[0].Value);
                                return new(ResultState.Complete, $">>> Opening a file {Parameters[0].Value}...\n", string.Empty);
                            }
                            catch
                            {
                                return new CommandStateResult(ResultState.Failed,
                                    ">>> Failed. There is no opening program for the file..\n", "Было вызвано исключение из-за неизвестной программе открывающей файл");
                            }
                        }
                        else
                            return new CommandStateResult(ResultState.Failed,
                                $">>> Failed. Inknown path or file: {Parameters[0].Value}\n",
                                "Было вызвано исключение об отсутствии файла по данной директории");
                    }
                    else
                        return new(ResultState.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");

                // create_label
                case 15:
                    ObjLog.LOGTextAppend($"Была вызвана команда об создании ярлыка по директории");
                    if (Parameters != null)
                    {
                        if (Parameters.Length < 2)
                            return new(ResultState.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");
                        if (Directory.Exists(Parameters[1].Value))
                        {
                            object shDesktop = "Desktop";
                            string Name = Parameters[0].Value.Replace(".lnk", string.Empty);
                            WshShell shell = new();
                            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @$"\{Name}.lnk";
                            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                            shortcut.TargetPath = Parameters[1].Value;
                            shortcut.Save();
                            animText = new(App.MainForm.tbOutput,
                                $">>> Completed create label {Name} is directory: {Parameters[1].Value}..\n");
                            animText.AnimInit(true);
                        }
                        else
                            return new CommandStateResult(ResultState.Failed,
                                $"Было вызвано исключение об отсутствии директории для ярлыка <{Parameters[1].Value}>",
                                $">>> Failed. <{Parameters[1].Value}> not is directory\n");
                    }
                    else
                        return new(ResultState.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");
                    return CommandStateResult.Completed;

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
                    return CommandStateResult.Completed;

                // windows_bat
                case 17:
                    if (Parameters != null)
                    {
                        string Name = Parameters[0].Value.Replace(".bat", string.Empty);
                        ObjLog.LOGTextAppend($"Была вызвана команда о открытии файла Windows <{Name}.bat>");
                        if (System.IO.File.Exists($"{Directory.GetCurrentDirectory()}\\Data\\Bat\\{Parameters[0].Value}.bat"))
                        {
                            ObjLog.LOGTextAppend($"Успешно открыт <{Parameters[0].Value}.bat>");
                            if (App.MainForm.WindowState != FormWindowState.Normal) MainData.MainMP3.PlaySound("Complete");
                            Process.Start($"{Directory.GetCurrentDirectory()}\\Data\\Bat\\{Parameters[0].Value}.bat");
                        }
                        else
                            return new CommandStateResult(ResultState.Failed,
                                $">>> Failed. <{Parameters[0].Value}> not is directory: BAT.FILE\n",
                                $"Было вызвано исключение об отсутствии <{Parameters[0].Value}.bat> в директории BAT.FILE");
                    }
                    else
                        return new(ResultState.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");
                    return CommandStateResult.Completed;

                // test
                case 18:
                    ObjLog.LOGTextAppend($"Было вызвано программное тестирование");
                    if ((bool)MainData.Settings.Developer_Mode)
                    {
                        //ObjLog.LOGTextAppend("Text.I".Substring(0, "Text.I".IndexOf(".", 0, "Text.I".Length)));
                        SHDocVw.ShellWindows shellWindows = new SHDocVw.ShellWindows();

                        foreach (SHDocVw.InternetExplorer ie in shellWindows)
                        {
                            //MessageBox.Show(ie.LocationName);
                            MessageBox.Show(ie.LocationURL);
                    }
                    else
                    {
                        return new CommandStateResult(ResultState.Failed,
                            ">>> Access denied due to security settings\n", "Было вызвано исключение об малых правах пользователя перед разработчиком");
                    }
                    return CommandStateResult.Completed;

                //
                case 19:
                    return CommandStateResult.Completed;

                //
                case 20:
                    return CommandStateResult.Completed;

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
                    return CommandStateResult.Completed;

                // save_log
                case 22:
                    ObjLog.LOGTextAppend($"Была вызвана команда сохранения информации журнала");
                    StreamWriter file_write = new($"{Directory.GetCurrentDirectory()}\\Data\\Log\\{DateTime.Now:HH.mm.ss}.txt");
                    animText = new(App.MainForm.tbOutput, $">>> The data was successfully saved in the file: Data\\Log\\{DateTime.Now:HH.mm.ss}.txt\n");
                    animText.AnimInit(true);
                    file_write.Write(ObjLog.MassLogElements.Select(i => i.Text).ToArray());
                    file_write.Close();
                    return CommandStateResult.Completed;

                // delete_log
                case 23:
                    ObjLog.LOGTextAppend($"Была вызвана команда удаления (всех) файлов инф. журнала");
                    if (Parameters != null)
                    {
                        if (System.IO.File.Exists($"{Directory.GetCurrentDirectory()}\\Data\\Log\\{Parameters[0].Value}.txt"))
                        {
                            ActivateActionDialog("Удаление файла", $"Удалить файл {Parameters[0].Value}.txt");
                            if (MainData.Flags.ResultConfirmationAction == DialogWindowStatus.Ok)
                            {
                                System.IO.File.Delete($"{Directory.GetCurrentDirectory()}\\Data\\Log\\{Parameters[0].Value}.txt");
                                animText = new(App.MainForm.tbOutput, $">>> Complete delete log file information {Parameters[0].Value}.txt\n");
                                animText.AnimInit(true);
                            }
                            else
                            {
                                animText = new(App.MainForm.tbOutput, ">>> Cancel clear log file(s)\n");
                                animText.AnimInit(true);
                            }
                        }
                        else
                            return new CommandStateResult(ResultState.Failed,
                                $">>> Failed. Inknown file: Data\\Log\\{Parameters[0].Value}.txt\n", "Было вызвано исключение из-за неизвестного файла");
                    }
                    else
                    {
                        ActivateActionDialog("Удаление файлов log", "Удалить все файлы информации log");
                        if (MainData.Flags.ResultConfirmationAction == DialogWindowStatus.Ok)
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
                    return CommandStateResult.Completed;

                // pythagorean_three
                case 24:
                    if (Parameters != null)
                    {
                        if (Parameters.Length < 2)
                            return new(ResultState.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Name}");
                        if (!Stringint(Parameters[0].Value) && !Stringint(Parameters[1].Value))
                        {
                            int u = Convert.ToInt32(Parameters[0].Value), v = Convert.ToInt32(Parameters[1].Value);
                            (int, int) PythagorTree = (u * u - v * v, 2 * u * v);
                            animText = new(App.MainForm.tbOutput,
                                $">>> Pythagorean_three this: {PythagorTree.Item1} + {PythagorTree.Item2}i = {u * u + v * v}\n");
                            animText.AnimInit(true);
                        }
                        else
                        {
                            int InvalidIndex = !Stringint(Parameters[0].Value) ? 1 : 0;
                            return new CommandStateResult(ResultState.Failed,
                                    $">>> Failed. The parameter #{InvalidIndex} is not a number: {Parameters[InvalidIndex].Value}\n",
                                    "Было вызвано исключение из-за не цифрового параметра");
                        }
                    }
                    else return new(ResultState.Failed, "There are not enough parameters to execute the command",
                    $"Недостаточно параметров для выполнения команды {Name}");
                    return CommandStateResult.Completed;

                // colored
                case 25:
                    try
                    {
                        App.Settings.ThemesCreated.WindowState = FormWindowState.Normal;
                        App.Settings.ThemesCreated.Show();
                    }
                    catch { App.Settings.ThemesCreated = new(); }
                    App.Settings.ThemesCreated.Show();
                    return CommandStateResult.Completed;

                // new_label
                case 26:
                    App.MainForm.GenerateLabel();
                    return CommandStateResult.Completed;

                // NOT COMMAND
                default:
                    string TextLOG = "Команда не распозналась и вызвала исключение об ошибке";
                    if (Name.Length > 0) return new CommandStateResult(ResultState.Failed, $">>> ~Invalid Command: \"{Name}\"\n", TextLOG);
                    else return new CommandStateResult(ResultState.Failed, $">>> ~Invalid NULL Command\n", TextLOG);
            };
        */
        }
    }
}
