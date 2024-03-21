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
                    if (!DLLMethods.LockWorkStation())
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