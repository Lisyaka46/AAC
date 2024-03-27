using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static AAC.Startcs;
using AAC.Classes.DataClasses;
using Microsoft.Speech.Recognition;

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
        public readonly string[] Phrases;

        /// <summary>
        /// Описание голосовой команды
        /// </summary>
        public readonly string Explanation;

        /// <summary>
        /// Делегат события выполнения команды
        /// </summary>
        /// <param name="ParametersValue">Параметры команды</param>
        /// <returns>Итог выполнения команды</returns>
        public delegate Task<CommandStateResult> ExecuteCom();

        /// <summary>
        /// Действие которое выполняет команда
        /// </summary>
        private event ExecuteCom Execute;

        /// <summary>
        /// Инициализировать объект голосовой команды
        /// </summary>
        /// <param name="Phrases">Имя</param>
        /// <param name="id">Индификатор</param>
        /// <param name="parameters">Параметры команды</param>
        public VoiceCommand(string[] Phrases, string? Explanation, ExecuteCom Execute)
        {
            this.Phrases = Phrases;
            this.Explanation = Explanation ?? "Нет описания";
            this.Execute = Execute;
        }

        /// <summary>
        /// Инициализировать голосовую команду
        /// </summary>
        /// <returns>Итог выполнения голосовой команды</returns>
        public async Task<CommandStateResult> ExecuteCommand() => await Execute.Invoke();

        /// <summary>
        /// Поиск голосовой команды по её фразе
        /// </summary>
        /// <param name="Phrase">Фраза</param>
        /// <returns>Голосовая команда</returns>
        public static VoiceCommand? SearchVoiceCommand(VoiceCommand[] ArrayVoiceCommand, string Phrase) => ArrayVoiceCommand.FirstOrDefault((i) => i.Phrases.Contains(Phrase));

        /// <summary>
        /// Вызвать выполнение голосовой команды
        /// </summary>
        /// <returns>Вердикт выполнения голосовой команды</returns>
        /*private CommandStateResult ExecuteVoiceCommand()
        {
            switch (ID)
            {
                case 1: // закрыть программу
                    ConsoleCommand.ReadConsoleCommand("close");
                    return CommandStateResult.Completed;
                case 2: // боковая панель
                    Apps.MainForm.DeveloperPanelClick(null, null);
                    return CommandStateResult.Completed;
                case 3: // ты жив
                    MainData.MainMP3.PlaySound("YesVoice");
                    return CommandStateResult.Completed;
                case 4: // очистить
                    ConsoleCommand.ReadConsoleCommand("clear");
                    return CommandStateResult.Completed;
                case 5: // покажись
                    if (Apps.MainForm.WindowState != FormWindowState.Normal)
                        Apps.MainForm.UnfoldingApplication(null, null);
                    return CommandStateResult.Completed;
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
                case 10: // протестируй
                    return CommandStateResult.Completed;
                case 11: // заблокируй компьютер
                    if (!DLLMethods.LockWorkStation())
                        throw new Win32Exception(Marshal.GetLastWin32Error());
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
