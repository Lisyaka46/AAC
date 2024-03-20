using IWshRuntimeLibrary;
using Microsoft.Speech.Recognition;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static AAC.Classes.AnimationDL.Animate.AnimText;
using static AAC.MiniFunctions;
using static AAC.Startcs;

namespace AAC.Classes
{
    public static partial class TypeCommand
    {
        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool LockWorkStation();

        enum RecycleFlags : uint
        {
            SHERB_NOCONFIRMATION = 0x00000001,
            SHERB_NOPROGRESSUI = 0x00000002,
            SHERB_NOSOUND = 0x00000004
        }
        [LibraryImport("Shell32.dll", StringMarshalling = StringMarshalling.Utf16)]
        private static partial uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

        /// <summary>
        /// Консольная команда
        /// </summary>
        public class ConsoleCommand
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
            /// Создать выполнение команды
            /// </summary>
            /// <param name="ConsoleCommand">Была введена ли эта команда или нет</param>
            public StateResult ExecuteCommand(bool ConsoleCommand) => ExecuteDefaultCommand(this, ConsoleCommand);

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
        }

        /// <summary>
        /// Консольная команда
        /// </summary>
        public class VoiceCommand
        {
            /// <summary>
            /// Фразы команды
            /// </summary>
            public string[] Phrases { get; private set; }

            /// <summary>
            /// ID индификатор команды
            /// </summary>
            public int ID { get; }

            /// <summary>
            /// Описание голосовой команды
            /// </summary>
            public string ExplanationCommand { get; }

            /// <summary>
            /// Инициализировать объект голосовой команды
            /// </summary>
            /// <param name="phrases">Имя</param>
            /// <param name="id">Индификатор</param>
            /// <param name="parameters">Параметры команды</param>
            public VoiceCommand(string[] phrases, int id, string? Explanation)
            {
                Phrases = phrases;
                ID = id;
                if (Explanation != null)
                {
                    if (Explanation.Length > 0) ExplanationCommand = Explanation;
                    else ExplanationCommand = "Нет описания";
                }
                else ExplanationCommand = "Нет описания";
            }

            /// <summary>
            /// Скопировать объект консольной команды
            /// </summary>
            /// <returns>Копия существующего объекта консольной команды</returns>
            //public ConsoleCommand Copy() => new(Name, ID, CommandParameters, ExplanationCommand);

            /// <summary>
            /// Создать выполнение команды
            /// </summary>
            public StateResult ExecuteCommand() => ExecuteVoiceCommand(this);

            /// <summary>
            /// Поиск голосовой команды по её фразе
            /// </summary>
            /// <param name="Phrase">Фраза</param>
            /// <returns>Голосовая команда</returns>
            public static VoiceCommand SearchVoiceCommand(VoiceCommand[] ArrayVoiceCommand, string Phrase)
            {
                string[][] PhraseAllVoiceCommand = ArrayVoiceCommand.Select(i => i.Phrases).ToArray();
                for (int i = 0; i < PhraseAllVoiceCommand.Length; i++)
                    if (PhraseAllVoiceCommand[i].Contains(Phrase)) return ArrayVoiceCommand[i];
                return Empty;
            }

            /// <summary>
            /// Пустой объект голосовой команды
            /// </summary>
            public static VoiceCommand Empty => new([], -1, null);
        }

        /// <summary>
        /// Параметер команды
        /// </summary>
        /// <remarks>
        /// Инициализировать объект параметра команды
        /// </remarks>
        /// <param name="NameParameter">Имя параметра</param>
        /// <param name="value">Значение параметра</param>
        public class Parameter(string NameParameter, string value)
        {
            /// <summary>
            /// Имя параметра команды
            /// </summary>
            public string Name { get; } = NameParameter;

            /// <summary>
            /// Значение параметра команды
            /// </summary>
            public string Value { get; set; } = value;
        }

        /// <summary>
        /// Объект итогового состояния выполнения команды
        /// </summary>
        public class StateResult
        {
            /// <summary>
            /// Итоговое состояние команды
            /// </summary>
            public ResultStateCommand State { get; }

            /// <summary>
            /// Сообщение в LOG
            /// </summary>
            public string LOGMassage { get; }

            /// <summary>
            /// Сообщение в консольную строку
            /// </summary>
            public string Massage { get; }

            /// <summary>
            /// Инициализировать объект итога выполнения команды
            /// </summary>
            /// <param name="ResultState">Итоговое состояние выполнения</param>
            /// <param name="Massage">Сообщение в консольную строку</param>
            /// <param name="Massage_log">Сообщение в LOG</param>
            internal StateResult(ResultStateCommand ResultState, string Massage, string Massage_log)
            {
                State = ResultState;
                this.Massage = Massage;
                LOGMassage = Massage_log;
            }

            /// <summary>
            /// Успешный итог выполнения команды
            /// </summary>
            public static StateResult Completed => new(ResultStateCommand.Complete, string.Empty, string.Empty);

            /// <summary>
            /// Выполнить обычные действия подведения итога команды
            /// </summary>
            public void Summarize()
            {
                if (LOGMassage.Length > 0) ObjLog.LOGTextAppend(LOGMassage);
                if (State == ResultStateCommand.Failed)
                {
                    MainData.MainMP3.PlaySound("Error");
                    //ConstAnimMove ConstantFormule = new(15, 15, 10);
                    //ConstantFormule.InitAnimFormule(App.MainForm.tbOutput, //!! Formules.Sinusoid, new ConstAnimMove(App.MainForm.tbOutput.Location.Y), AnimationStyle.XY);
                }
                if (Massage.Length > 0) new Instr_AnimText(App.MainForm.tbOutput, Massage).AnimInit(true);
                if (App.MainForm.WindowState == FormWindowState.Normal) App.MainForm.LComplete_Click(null, null);
                //else if (MainData.Flags.FormActivity == Data.BooleanFlags.False && State == ResultStateCommand.Complete) MainData.MainMP3.PlaySound("Complete");
            }
        }

        /// <summary>
        /// Прочитать команду из консоли
        /// </summary>
        /// <param name="TextCommand">Читаемая команда</param>
        /// <returns></returns>
        public static ConsoleCommand ReadDefaultConsoleCommand(string TextCommand)
        {
            ObjLog.LOGTextAppend($"Начинаю читать команду <{TextCommand}>");
            while (TextCommand.Length > 0)
            {
                if (TextCommand[^1] == ' ') TextCommand = TextCommand.Remove(TextCommand.Length - 1);
                else if (TextCommand.Contains("  ")) TextCommand = TextCommand.Replace("  ", " ");
                else break;
            }
            if (TextCommand.Contains('*') && TextCommand[^1] != '*') // command* param1, param2, param3 ...
            {
                TextCommand = TextCommand[0..TextCommand.IndexOf('*')].Replace(" ", "_").ToLower() + TextCommand[TextCommand.IndexOf('*')..];
                ObjLog.LOGTextAppend($"Text: {TextCommand}");
                Match Command = Regex.Match(TextCommand, @"\b[^\*~!@#$<>,.\/\\?|'"";:`%^&*()\[\]{} \-=+]+\* ?");
                string CommandText = Command.Value.ToString().Replace("*", string.Empty).Replace(" ", string.Empty);
                MatchCollection Parameteres = Regex.Matches(TextCommand, @"( |\*|,)([^,]|,,)+");
                int Index = MainData.MainCommandData.MassConsoleCommand.Select(i => i.Name).ToList().IndexOf(CommandText);
                if (Index > -1)
                {
                    ConsoleCommand ObjCommand = MainData.MainCommandData.MassConsoleCommand[Index].Copy();
                    if (ObjCommand.CommandParameters != null)
                    {
                        for (int i = 0; i < ObjCommand.CommandParameters.Length; i++)
                        {
                            if (i < Parameteres.Count) ObjCommand.CommandParameters[i].Value = Parameteres[i].Value[2..];
                            else break;
                        }
                    }
                    return ObjCommand;
                }
                return ConsoleCommand.NotCommand(CommandText);
            }
            else // command
            {
                return ConsoleCommand.SearchConsoleCommand(MainData.MainCommandData.MassConsoleCommand,
                    TextCommand.Replace(" ", "_").Replace("*", string.Empty).ToLower(), false);
            }
        }

        /// <summary>
        /// Проверка голосовой команды на распознавание
        /// </summary>
        /// <param name="sender">Объект проверки</param>
        /// <param name="e">Событие проверки</param>
        public static void CheckingVoiceCommand(object? sender, SpeechRecognizedEventArgs e)
        {
            string Text = e.Result.Text;
            ObjLog.LOGTextAppend($"Распознал голосовую речь: <{Text}>");
            if (e.Result.Confidence > Data.InputVoiceCommandDevice.FactorAccuracyVoice)
            {
                if (MainData.Flags.AudioCommand == Data.StatusFlags.Active)
                {
                    if (!Text.Equals("включи голосовые команды"))
                    {
                        StateResult Result = VoiceCommand.SearchVoiceCommand(MainData.MainCommandData.MassVoiceCommand, Text).ExecuteCommand();
                        ObjLog.LOGTextAppend($"Выполнил голосовую речь: <{e.Result.Text}>");
                        Result.Summarize();
                    }
                }
                else
                {
                    if (e.Result.Confidence > Data.InputVoiceCommandDevice.FactorAccuracyVoice)
                    {
                        if (Text.Equals("включи голосовые команды"))
                        {
                            StateResult Result = VoiceCommand.SearchVoiceCommand(MainData.MainCommandData.MassVoiceCommand, Text).ExecuteCommand();
                            ObjLog.LOGTextAppend($"Выполнил голосовую речь: <{e.Result.Text}>");
                            Result.Summarize();
                        }
                    }
                }
            }
            else
            {
                if (MainData.Flags.AudioCommand == Data.StatusFlags.Active)
                {
                    ObjLog.LOGTextAppend($"Голосовая речь слишком не чёткая: ({e.Result.Confidence} < {Data.InputVoiceCommandDevice.FactorAccuracyVoice})");
                    if (App.MainForm.WindowState != FormWindowState.Normal) MainData.MainMP3.PlaySound("NotVoice");
                }
            }
        }

        /// <summary>
        /// Вызвать выполнение команды
        /// </summary>
        /// <param name="Command">Объект команды</param>
        /// <param name="ConsoleCommand">Была введена ли эта команда или нет</param>
        /// <returns>Вердикт выполнения консольной команды</returns>
        private static StateResult ExecuteDefaultCommand(ConsoleCommand Command, bool ConsoleCommand)
        {
            Instr_AnimText animText;
            if (ConsoleCommand) App.MainForm.tbInput.Text = string.Empty;
            ObjLog.LOGTextAppend($"Выполнение команды:\n<{Command.ID} | {Command.Name}>");
            if ((bool)MainData.Settings.Developer_Mode)
            {
                App.MainForm.lDeveloper_NameCommand.Text = $"NC: <\"{Command.Name}\">";
                if (Command.CommandParameters != null)
                    App.MainForm.lDeveloper_ParametersCommand.Text = $"PC: <\"{string.Join(", ", Command.CommandParameters.Select(i => i.Value.Length == 0 ? "null" : i.Value))}\">";
                else App.MainForm.lDeveloper_ParametersCommand.Text = "PC: <>";
                App.MainForm.lDeveloper_CountParametersCommand.Text = $"CPC: {Command.CommandParameters?.Length ?? 0}";
            }

            // Команды
            switch (Command.ID)
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
                    if (Command.CommandParameters != null)
                        return new(ResultStateCommand.Complete, $">>> {Command.CommandParameters[0].Value}\n", string.Empty);
                    else return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command", $"Недостаточно параметров для выполнения команды {Command.Name}");

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
                    if (Command.CommandParameters != null)
                    {
                        if (!Command.CommandParameters[0].Value.Equals(string.Empty))
                        {
                            string[] ListSystemProcess =
                            [
                                "explorer", "svchost", "lsass", "sihost", "system", "ntoskrnl",
                                "wininit", "dwm", "smss", "services", "winlogon", "csrss", "runtimebroker",
                                "applicationframehost", "ctfmon", "securityhealthservice", "spoolsv"
                            ];
                            string Name = Command.CommandParameters[0].Value.Replace(".exe", string.Empty);
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
                            $"Недостаточно параметров для выполнения команды {Command.Name}");
                    return StateResult.Completed;

                // cmd
                case 7:
                    if (Command.CommandParameters == null) Process.Start("cmd");
                    else
                    {
                        foreach (Parameter ParameterCommandCMD in Command.CommandParameters)
                        {
                            if (ParameterCommandCMD.Value.Length > 0) Process.Start("cmd", "/C " + ParameterCommandCMD.Value);
                            else break;
                        }
                    }
                    return StateResult.Completed;

                // emptytrash
                case 8:
                    ObjLog.LOGTextAppend($"Была вызвана команда очистки корзины");
                    uint result = SHEmptyRecycleBin(IntPtr.Zero, null, 0);
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
                    if (Command.CommandParameters != null)
                    {
                        if (Stringint(Command.CommandParameters[0].Value))
                            return new StateResult(ResultStateCommand.Failed,
                                $">>> Failed. font size is not number: {Command.CommandParameters[0].Value}\n",
                                "Была вызвана ошибка команды <font_size> что введено было не число");
                        else if (Convert.ToInt32(Command.CommandParameters[0].Value) < 7 || Convert.ToInt32(Command.CommandParameters[0].Value) > 40)
                            return new StateResult(ResultStateCommand.Failed,
                                $">>> Failed. font size range 7 - 40: {Command.CommandParameters[0].Value}\n",
                                "Была вызвана ошибка команды <font_size> об несоответствии параметра диапазону значений");
                        else
                        {
                            MainData.Settings.SetParamOption("Text-Size", Command.CommandParameters[0].Value);
                            App.MainForm.tbOutput.Font = new Font(App.MainForm.tbOutput.Font.Name, Convert.ToInt32(Command.CommandParameters[0].Value));
                            App.MainForm.tbOutput.Update();
                        }
                    }
                    else
                        return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Command.Name}");
                    return StateResult.Completed;

                // open_link
                case 12:
                    ObjLog.LOGTextAppend($"Была вызвана функция открывающая ссылку в браузере");
                    if (Command.CommandParameters != null)
                    {
                        try
                        {
                            Process.Start(new ProcessStartInfo(Command.CommandParameters[0].Value) { UseShellExecute = true });
                            ObjLog.LOGTextAppend($"Активировалась ссылка: <{Command.CommandParameters[0].Value}>");
                            if (App.MainForm.WindowState == FormWindowState.Normal)
                            {
                                animText = new(App.MainForm.tbOutput, $">>> Opening a link{$": {Command.CommandParameters[0].Value} ..."}\n");
                                animText.AnimInit(true);
                            }
                        }
                        catch
                        {
                            return new StateResult(ResultStateCommand.Failed,
                                $"Failed activate link \"{Command.CommandParameters[0].Value}\"", $"Не удалось открыть ссылку {Command.CommandParameters[0].Value}");
                        }
                    }
                    else
                        return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Command.Name}");
                    return StateResult.Completed;

                // open_directory
                case 13:
                    ObjLog.LOGTextAppend($"Была вызвана функция открывающая директорию");
                    if (Command.CommandParameters != null)
                    {
                        if (Directory.Exists(Command.CommandParameters[0].Value))
                            Process.Start("explorer.exe", Command.CommandParameters[0].Value);
                        else
                            return new StateResult(ResultStateCommand.Failed,
                                $">>> Failed. Inknown path name: {Command.CommandParameters[0].Value}\n", "Было вызвано исключение из-за неизвестной директории");
                    }
                    else
                        Process.Start("explorer.exe", Directory.GetCurrentDirectory());
                    if (App.MainForm.WindowState == FormWindowState.Normal)
                    {
                        animText = new(App.MainForm.tbOutput,
                            $">>> Opening a directory: {(Command.CommandParameters == null ? "MAIN" : Command.CommandParameters[0].Value)}...\n");
                        animText.AnimInit(true);
                    }
                    return StateResult.Completed;

                // open_file
                case 14:
                    ObjLog.LOGTextAppend($"Была вызвана команда об открытии файла по директории");
                    if (Command.CommandParameters != null)
                    {
                        if (System.IO.File.Exists(Command.CommandParameters[0].Value))
                        {
                            try
                            {
                                if (Path.GetExtension(Command.CommandParameters[0].Value).Equals(".url"))
                                    Process.Start(new ProcessStartInfo(Command.CommandParameters[0].Value) { UseShellExecute = true });
                                else
                                    Process.Start(Command.CommandParameters[0].Value);
                                return new(ResultStateCommand.Complete, $">>> Opening a file {Command.CommandParameters[0].Value}...\n", string.Empty);
                            }
                            catch
                            {
                                return new StateResult(ResultStateCommand.Failed,
                                    ">>> Failed. There is no opening program for the file..\n", "Было вызвано исключение из-за неизвестной программе открывающей файл");
                            }
                        }
                        else
                            return new StateResult(ResultStateCommand.Failed,
                                $">>> Failed. Inknown path or file: {Command.CommandParameters[0].Value}\n",
                                "Было вызвано исключение об отсутствии файла по данной директории");
                    }
                    else
                        return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Command.Name}");

                // create_label
                case 15:
                    ObjLog.LOGTextAppend($"Была вызвана команда об создании ярлыка по директории");
                    if (Command.CommandParameters != null)
                    {
                        if (Command.CommandParameters.Length < 2)
                            return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Command.Name}");
                        if (Directory.Exists(Command.CommandParameters[1].Value))
                        {
                            object shDesktop = "Desktop";
                            string Name = Command.CommandParameters[0].Value.Replace(".lnk", string.Empty);
                            WshShell shell = new();
                            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @$"\{Name}.lnk";
                            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                            shortcut.TargetPath = Command.CommandParameters[1].Value;
                            shortcut.Save();
                            animText = new(App.MainForm.tbOutput,
                                $">>> Completed create label {Name} is directory: {Command.CommandParameters[1].Value}..\n");
                            animText.AnimInit(true);
                        }
                        else
                            return new StateResult(ResultStateCommand.Failed,
                                $"Было вызвано исключение об отсутствии директории для ярлыка <{Command.CommandParameters[1].Value}>",
                                $">>> Failed. <{Command.CommandParameters[1].Value}> not is directory\n");
                    }
                    else
                        return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Command.Name}");
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
                    if (Command.CommandParameters != null)
                    {
                        string Name = Command.CommandParameters[0].Value.Replace(".bat", string.Empty);
                        ObjLog.LOGTextAppend($"Была вызвана команда о открытии файла Windows <{Name}.bat>");
                        if (System.IO.File.Exists($"{Directory.GetCurrentDirectory()}\\Data\\Bat\\{Command.CommandParameters[0].Value}.bat"))
                        {
                            ObjLog.LOGTextAppend($"Успешно открыт <{Command.CommandParameters[0].Value}.bat>");
                            if (App.MainForm.WindowState != FormWindowState.Normal) MainData.MainMP3.PlaySound("Complete");
                            Process.Start($"{Directory.GetCurrentDirectory()}\\Data\\Bat\\{Command.CommandParameters[0].Value}.bat");
                        }
                        else
                            return new StateResult(ResultStateCommand.Failed,
                                $">>> Failed. <{Command.CommandParameters[0].Value}> not is directory: BAT.FILE\n",
                                $"Было вызвано исключение об отсутствии <{Command.CommandParameters[0].Value}.bat> в директории BAT.FILE");
                    }
                    else
                        return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Command.Name}");
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
                    if (Command.CommandParameters != null)
                    {
                        if (System.IO.File.Exists($"{Directory.GetCurrentDirectory()}\\Data\\Log\\{Command.CommandParameters[0].Value}.txt"))
                        {
                            ActivateActionDialog("Удаление файла", $"Удалить файл {Command.CommandParameters[0].Value}.txt");
                            if (MainData.Flags.ResultConfirmationAction == Data.DialogWindowStatus.Ok)
                            {
                                System.IO.File.Delete($"{Directory.GetCurrentDirectory()}\\Data\\Log\\{Command.CommandParameters[0].Value}.txt");
                                animText = new(App.MainForm.tbOutput, $">>> Complete delete log file information {Command.CommandParameters[0].Value}.txt\n");
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
                                $">>> Failed. Inknown file: Data\\Log\\{Command.CommandParameters[0].Value}.txt\n", "Было вызвано исключение из-за неизвестного файла");
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
                    if (Command.CommandParameters != null)
                    {
                        if (Command.CommandParameters.Length < 2)
                            return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                            $"Недостаточно параметров для выполнения команды {Command.Name}");
                        if (!Stringint(Command.CommandParameters[0].Value) && !Stringint(Command.CommandParameters[1].Value))
                        {
                            int u = Convert.ToInt32(Command.CommandParameters[0].Value), v = Convert.ToInt32(Command.CommandParameters[1].Value);
                            (int, int) PythagorTree = (u * u - v * v, 2 * u * v);
                            animText = new(App.MainForm.tbOutput,
                                $">>> Pythagorean_three this: {PythagorTree.Item1} + {PythagorTree.Item2}i = {u * u + v * v}\n");
                            animText.AnimInit(true);
                        }
                        else
                        {
                            int InvalidIndex = !Stringint(Command.CommandParameters[0].Value) ? 1 : 0;
                            return new StateResult(ResultStateCommand.Failed,
                                    $">>> Failed. The parameter #{InvalidIndex} is not a number: {Command.CommandParameters[InvalidIndex].Value}\n",
                                    "Было вызвано исключение из-за не цифрового параметра");
                        }
                    }
                    else return new(ResultStateCommand.Failed, "There are not enough parameters to execute the command",
                    $"Недостаточно параметров для выполнения команды {Command.Name}");
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
                    if (Command.Name.Length > 0) return new StateResult(ResultStateCommand.Failed, $">>> ~Invalid Command: \"{Command.Name}\"\n", TextLOG);
                    else return new StateResult(ResultStateCommand.Failed, $">>> ~Invalid NULL Command\n", TextLOG);
            };
        }

        /// <summary>
        /// Вызвать выполнение голосовой команды
        /// </summary>
        /// <param name="Command"></param>
        /// <returns>Вердикт выполнения голосовой команды</returns>
        private static StateResult ExecuteVoiceCommand(VoiceCommand Command)
        {
            switch (Command.ID)
            {
                case 1: // закрыть программу
                    ReadDefaultConsoleCommand("close").ExecuteCommand(false);
                    return StateResult.Completed;
                case 2: // боковая панель
                    App.MainForm.DeveloperPanelClick(null, null);
                    return StateResult.Completed;
                case 3: // ты жив
                    MainData.MainMP3.PlaySound("YesVoice");
                    return StateResult.Completed;
                case 4: // очистить
                    ReadDefaultConsoleCommand("clear").ExecuteCommand(false);
                    return StateResult.Completed;
                case 5: // покажись
                    if (App.MainForm.WindowState != FormWindowState.Normal)
                        App.MainForm.UnfoldingApplication(null, null);
                    return StateResult.Completed;
                case 6: // спрячся
                    App.MainForm.FoldingMoveApplication(null, null);
                    return StateResult.Completed;
                case 7: // открой рабочую директорию
                    Process.Start("explorer.exe", Directory.GetCurrentDirectory());
                    return StateResult.Completed;
                case 8: // сверни всё
                    App.MainForm.Show();
                    ShellGUID("DesktopVisualTrue");
                    return StateResult.Completed;
                case 9: // открой панель управления
                    ShellGUID("CommandPanelWin");
                    return StateResult.Completed;
                case 10: // протестируй
                    return StateResult.Completed;
                case 11: // заблокируй компьютер
                    if (!LockWorkStation())
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    return StateResult.Completed;
                case 12: // выключи голосовые команды
                    App.MainForm.pbVoiceButton.Image = Image.FromFile(@"Data\Image\Micro\MicroSleepingNotMouse.png");
                    MainData.Flags.AudioCommand = Data.StatusFlags.Sleep;
                    if (App.MainForm.WindowState != FormWindowState.Normal)
                        MainData.MainMP3.PlaySound("Complete");
                    return StateResult.Completed;
                case 13: // закрой окно информации
                    if (App.InformationCommand.Visible)
                    {
                        App.InformationCommand.Close();
                        if (App.MainForm.WindowState != FormWindowState.Normal)
                            MainData.MainMP3.PlaySound("Complete");
                    }
                    return StateResult.Completed;
                case 14: // сколько времени
                    TimeTempus();
                    return StateResult.Completed;
                case 15:
                    App.MainForm.pbVoiceButton.Image = Image.FromFile(@"Data\Image\Micro\MicroActivateNotMouse.png");
                    MainData.Flags.AudioCommand = Data.StatusFlags.Active;
                    return StateResult.Completed;
            }
            return new StateResult(ResultStateCommand.Failed,
                $">>> Voice command ID: {Command.ID} is Invalid",
                $"Голосовая команда ID: {Command.ID} не нраспознана");
        }

        /// <summary>
        /// Конечные результаты выполнения команды
        /// </summary>
        public enum ResultStateCommand
        {
            /// <summary>
            /// Команда не выполнилась
            /// </summary>
            Failed = 0,

            /// <summary>
            /// Команда выполнилась успешно
            /// </summary>
            Complete = 1
        }
    }
}