using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static AAC.Startcs;
using AAC.Classes.DataClasses;
using System.Speech.Recognition;

namespace AAC.Classes.Commands
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
        public readonly int ID;

        /// <summary>
        /// Описание голосовой команды
        /// </summary>
        public readonly string ExplanationCommand;

        /// <summary>
        /// Пустой объект голосовой команды
        /// </summary>
        public static VoiceCommand Empty => new([], -1, null);

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
                if (MainData.Flags.AudioCommand == StatusFlags.Active && !Text.Equals("включи голосовые команды"))
                {
                    CommandStateResult Result = SearchVoiceCommand(MainData.MainCommandData.MassVoiceCommand, Text).ExecuteVoiceCommand();
                    ObjLog.LOGTextAppend($"Выполнил голосовую речь: <{Text}>");
                    Result.Summarize();
                }
                else if (MainData.Flags.AudioCommand == StatusFlags.Sleep && Text.Equals("включи голосовые команды"))
                {
                    CommandStateResult Result = SearchVoiceCommand(MainData.MainCommandData.MassVoiceCommand, Text).ExecuteVoiceCommand();
                    ObjLog.LOGTextAppend($"Выполнил голосовую речь: <{Text}>");
                    Result.Summarize();
                }
            }
            else if (MainData.Flags.AudioCommand == StatusFlags.Active)
            {
                ObjLog.LOGTextAppend($"Голосовая речь слишком не чёткая: ({e.Result.Confidence} < {Data.InputVoiceCommandDevice.FactorAccuracyVoice})");
                if (App.MainForm.WindowState != FormWindowState.Normal) MainData.MainMP3.PlaySound("NotVoice");
            }
        }

        /// <summary>
        /// Вызвать выполнение голосовой команды
        /// </summary>
        /// <returns>Вердикт выполнения голосовой команды</returns>
        private CommandStateResult ExecuteVoiceCommand()
        {
            switch (ID)
            {
                case 1: // закрыть программу
                    ConsoleCommand.ReadConsoleCommand("close");
                    return CommandStateResult.Completed;
                case 2: // боковая панель
                    App.MainForm.DeveloperPanelClick(null, null);
                    return CommandStateResult.Completed;
                case 3: // ты жив
                    MainData.MainMP3.PlaySound("YesVoice");
                    return CommandStateResult.Completed;
                case 4: // очистить
                    ConsoleCommand.ReadConsoleCommand("clear");
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
                    if (DLLMethods.ShellGUID("DesktopVisualTrue")) MainData.MainMP3.PlaySound("Complete");
                    return CommandStateResult.Completed;
                case 9: // открой панель управления
                    if (DLLMethods.ShellGUID("CommandPanelWin")) MainData.MainMP3.PlaySound("Complete");
                    return CommandStateResult.Completed;
                case 10: // протестируй
                    return CommandStateResult.Completed;
                case 11: // заблокируй компьютер
                    if (!DLLMethods.LockWorkStation())
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    return CommandStateResult.Completed;
                case 12: // выключи голосовые команды
                    App.MainForm.pbVoiceButton.Image = Image.FromFile(@"Data\Image\Micro\MicroSleepingNotMouse.png");
                    MainData.Flags.AudioCommand = StatusFlags.Sleep;
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
    TimeTempus();*/
                    return CommandStateResult.Completed;
                case 15:
                    App.MainForm.pbVoiceButton.Image = Image.FromFile(@"Data\Image\Micro\MicroActivateNotMouse.png");
                    MainData.Flags.AudioCommand = StatusFlags.Active;
                    return CommandStateResult.Completed;
            }
            return new CommandStateResult(ResultState.Failed,
                $">>> Voice command ID: {ID} is Invalid",
                $"Голосовая команда ID: {ID} не нраспознана");
        }
    }
}
