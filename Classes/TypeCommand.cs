using AAC.Classes.Commands;
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
            public CommandStateResult ExecuteCommand() => ExecuteVoiceCommand(this);

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
                        CommandStateResult Result = VoiceCommand.SearchVoiceCommand(MainData.MainCommandData.MassVoiceCommand, Text).ExecuteCommand();
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
                            CommandStateResult Result = VoiceCommand.SearchVoiceCommand(MainData.MainCommandData.MassVoiceCommand, Text).ExecuteCommand();
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
        private static CommandStateResult ExecuteVoiceCommand(VoiceCommand Command)
        {
            switch (Command.ID)
            {
                case 1: // закрыть программу
                    ConsoleCommand.ReadDefaultConsoleCommand("close").ExecuteCommand(false);
                    return CommandStateResult.Completed;
                case 2: // боковая панель
                    App.MainForm.DeveloperPanelClick(null, null);
                    return CommandStateResult.Completed;
                case 3: // ты жив
                    MainData.MainMP3.PlaySound("YesVoice");
                    return CommandStateResult.Completed;
                case 4: // очистить
                    ConsoleCommand.ReadDefaultConsoleCommand("clear").ExecuteCommand(false);
                    return CommandStateResult.Completed;
                case 5: // покажись
                    if (App.MainForm.WindowState != FormWindowState.Normal)
                        App.MainForm.UnfoldingApplication(null, null);
                    return CommandStateResult.Completed;
                case 6: // спрячся
                    App.MainForm.FoldingMoveApplication(null, null);
                    return CommandStateResult.Completed;
                case 7: // открой рабочую директорию
                    Process.Start("explorer.exe", Directory.GetCurrentDirectory());
                    return CommandStateResult.Completed;
                case 8: // сверни всё
                    App.MainForm.Show();
                    ShellGUID("DesktopVisualTrue");
                    return CommandStateResult.Completed;
                case 9: // открой панель управления
                    ShellGUID("CommandPanelWin");
                    return CommandStateResult.Completed;
                case 10: // протестируй
                    return CommandStateResult.Completed;
                case 11: // заблокируй компьютер
                    if (!DLLMethods.LockWorkStation())
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    return CommandStateResult.Completed;
                case 12: // выключи голосовые команды
                    App.MainForm.pbVoiceButton.Image = Image.FromFile(@"Data\Image\Micro\MicroSleepingNotMouse.png");
                    MainData.Flags.AudioCommand = Data.StatusFlags.Sleep;
                    if (App.MainForm.WindowState != FormWindowState.Normal)
                        MainData.MainMP3.PlaySound("Complete");
                    return CommandStateResult.Completed;
                case 13: // закрой окно информации
                    if (App.InformationCommand.Visible)
                    {
                        App.InformationCommand.Close();
                        if (App.MainForm.WindowState != FormWindowState.Normal)
                            MainData.MainMP3.PlaySound("Complete");
                    }
                    return CommandStateResult.Completed;
                case 14: // сколько времени
                    TimeTempus();
                    return CommandStateResult.Completed;
                case 15:
                    App.MainForm.pbVoiceButton.Image = Image.FromFile(@"Data\Image\Micro\MicroActivateNotMouse.png");
                    MainData.Flags.AudioCommand = Data.StatusFlags.Active;
                    return CommandStateResult.Completed;
            }
            return new CommandStateResult(ResultStateCommand.Failed,
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