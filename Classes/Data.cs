using AAC.Classes;
using AAC.Classes.Commands;
using NAudio.CoreAudioApi;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using WMPLib;
using static AAC.Classes.MainTheme;
using static AAC.Startcs;
using AAC.Classes.DataClasses;
using System.Speech.Recognition;

namespace AAC.Classes
{
    /// <summary>
    /// Все параметры программы
    /// </summary>
    public class Data
    {

        /// <summary>
        /// Объект настроек программы
        /// </summary>
        public readonly SettingsData Settings;

        /// <summary>
        /// Объект даты команд
        /// </summary>
        public readonly CommandData MainCommandData;

        /// <summary>
        /// Объект даты воспроизведения медиафайлов
        /// </summary>
        public readonly MP3 MainMP3;

        /// <summary>
        /// Объект Флагов программы
        /// </summary>
        public readonly FLAGS Flags;

        /// <summary>
        /// Объект голосового девайса разпознающего речь
        /// </summary>
        public readonly InputVoiceCommandDevice InputVoiceDevice;

        /// <summary>
        /// Объект информации тем
        /// </summary>
        public readonly ThemeData MainThemeData;

        /// <summary>
        /// Объект Управления специальными цветами и его объектами
        /// </summary>
        public readonly SpecialColor AllSpecialColor;

        /// <summary>
        /// Объект Управления дивайсами звука
        /// </summary>
        public readonly DivacesAudio Divaces;

        /// <summary>
        /// Инициализировать стартовый объект даты
        /// </summary>
        public Data()
        {
            MainCommandData = new(Reading.ReadConsoleCommandDataBase(), Reading.ReadVoiceCommandDataBase());
            MainMP3 = new(10);
            InputVoiceDevice = new(MainCommandData.MassVoiceCommand);
            MainThemeData = new();
            Settings = new();
            Flags = new();
            AllSpecialColor = new(Settings.SpecialColor_RGB, Settings.SpecialColor_RGBCC, Settings.SpecialColor_SC, Color.Black);
            Divaces = new();
        }

        /// <summary>
        /// Класс голосового девайса разпознающего речь
        /// </summary>
        public class InputVoiceCommandDevice
        {
            /// <summary>
            /// Глобальный дивайс ввода голосовых команд
            /// </summary>
            private readonly SpeechRecognitionEngine RecordInput;

            /// <summary>
            /// Коэффициент точности распознавания голосовых фраз
            /// </summary>
            public static readonly float FactorAccuracyVoice = 0.6f;

            /// <summary>
            /// Встроенные голосовые фразы 
            /// </summary>
            public readonly Choices DefaultChoicesProgramm;

            /// <summary>
            /// Обновить данные фраз
            /// </summary>
            public void UpdateAllGrammars()
            {
                bool Activated = false;
                if (RecordInput.AudioState == AudioState.Silence)
                {
                    RecordInput.RecognizeAsyncStop();
                    Activated = true;
                }
                RecordInput.UnloadAllGrammars();

                GrammarBuilder bulder = new();
                bulder.Append(DefaultChoicesProgramm);
                Grammar grammar = new(bulder);
                RecordInput.LoadGrammarAsync(grammar);
                if (Activated) RecordInput.RecognizeAsync(RecognizeMode.Multiple);
            }

            /// <summary>
            /// Инициализировать объект девайса распознавающего голос
            /// </summary>
            /// <param name="DefaultVoiceCommand">Встроенные фразы</param>
            /// <param name="ActivateDevice">Активировать ли распознавание</param>
            public InputVoiceCommandDevice(VoiceCommand[] DefaultVoiceCommand, bool ActivateDevice = false)
            {
                //RecordInput = new();
                //RecordInput.SetInputToDefaultAudioDevice();
                //RecordInput.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(TypeCommand.CheckingVoiceCommand);

                //DefaultChoicesProgramm = new(DefaultVoiceCommand.Select(i => i.Phrases).First());
                //foreach (TypeCommand.VoiceCommand Element in DefaultVoiceCommand) DefaultChoicesProgramm.Add(Element.Phrases);
                //GrammarBuilder bulder = new();
                //bulder.Append(DefaultChoicesProgramm);
                //Grammar grammar = new(bulder);
                //RecordInput.LoadGrammar(grammar);
                //if (ActivateDevice) RecordInput.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        /// <summary>
        /// Флаги Проекта
        /// </summary>
        public class FLAGS
        {

            /// <summary>
            /// Статус открытия панели разработчика главной формы
            /// </summary>
            public BooleanFlags PanelDeveloper { get; set; } = BooleanFlags.False;

            /// <summary>
            /// Статус Голосовых комманд
            /// </summary>
            public StatusFlags AudioCommand { get; set; } = StatusFlags.NotActive;

            /// <summary>
            /// Статус Активности клавиш на клавиатуре
            /// </summary>
            public DialogWindowStatus ResultConfirmationAction { get; set; } = DialogWindowStatus.Cancel;

            /// <summary>
            /// Статус Активности мини-панели редактор тем
            /// </summary>
            public BooleanFlags MiniPanelpMiniPanelActive { get; set; } = BooleanFlags.False;

            /// <summary>
            /// Статус Активности передвижения панели вывода консоли
            /// </summary>
            public BooleanFlags ActiveMovingMainConsole { get; set; } = BooleanFlags.False;

            //
            public FLAGS()
            {
            }
        }
    }
}
