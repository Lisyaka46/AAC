using AAC.Classes;
using AAC.Classes.Commands;
using NAudio.CoreAudioApi;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using WMPLib;
using static AAC.Classes.MainTheme;
using static AAC.Startcs;
using AAC.Classes.DataClasses;
using Microsoft.Speech.Recognition;

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
            Settings = new();
            ConsoleCommand[] MCC = Reading.ReadConsoleCommandDataBase();
            VoiceCommand[] MVC = Reading.ReadVoiceCommandDataBase();
            MainCommandData = new(MCC, MVC);
            MainMP3 = new(10);
            InputVoiceDevice = new(MainCommandData, Settings.Activation_Microphone);
            MainThemeData = new();
            Flags = new();
            AllSpecialColor = new(Settings.SpecialColor_RGB, Settings.SpecialColor_RGBCC, Settings.SpecialColor_SC, Color.Black);
            Divaces = new();
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
