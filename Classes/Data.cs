using AAC.Classes;
using Microsoft.Speech.Recognition;
using NAudio.CoreAudioApi;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using WMPLib;
using static AAC.Classes.MainTheme;
using static AAC.Startcs;

namespace AAC
{
    /// <summary>
    /// Все параметры программы
    /// </summary>
    public partial class Data
    {
        [LibraryImport("wininet.dll", EntryPoint = "InternetCheckConnectionW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool InternetCheckConnection(string lpszUrl, int dwFlags, int dwReserved);

        /// <summary>
        /// Статус подключения к интернету
        /// </summary>
        public static bool InternetConnection => InternetCheckConnection("https://ya.ru/", 1, 0);

        /// <summary>
        /// Объект настроек программы
        /// </summary>
        public SettingsData Settings { get; }

        /// <summary>
        /// Объект даты команд
        /// </summary>
        public CommandData MainCommandData { get; }

        /// <summary>
        /// Объект даты воспроизведения медиафайлов
        /// </summary>
        public MP3 MainMP3 { get; }

        /// <summary>
        /// Объект Флагов программы
        /// </summary>
        public FLAGS Flags { get; }

        /// <summary>
        /// Объект голосового девайса разпознающего речь
        /// </summary>
        public InputVoiceCommandDevice InputVoiceDevice { get; }

        /// <summary>
        /// Объект информации тем
        /// </summary>
        public ThemeData MainThemeData { get; }

        /// <summary>
        /// Объект Управления специальными цветами и его объектами
        /// </summary>
        public SpecialColor AllSpecialColor { get; }

        /// <summary>
        /// Объект Управления дивайсами звука
        /// </summary>
        public DivacesAudio Divaces { get; }

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
        /// Класс данных команд
        /// </summary>
        public class CommandData
        {
            /// <summary>
            /// Массив консольных команд
            /// </summary>
            public ConsoleCommand[] MassConsoleCommand { get; }

            /// <summary>
            /// Массив голосовых команд
            /// </summary>
            public TypeCommand.VoiceCommand[] MassVoiceCommand { get; }

            /// <summary>
            /// Инициализировать объект данных команд
            /// </summary>
            /// <param name="MCC">Консольные команды</param>
            /// <param name="MVC">Голосовые команды</param>
            internal CommandData(ConsoleCommand[] MCC, TypeCommand.VoiceCommand[] MVC)
            {
                MassConsoleCommand = MCC;
                MassVoiceCommand = MVC;
            }
        }

        /// <summary>
        /// Класс девайса воспроизводящего медиафайлы
        /// </summary>
        public class MP3
        {

            /// <summary>
            /// Количество каналов воспроизведения
            /// </summary>
            public int CountChannelMP { get; }

            /// <summary>
            /// Плеер для mp3 файлов
            /// </summary>
            public List<WindowsMediaPlayer> DoMP3player { get; private set; }

            /// <summary>
            /// Свойство индекса канала плеера
            /// </summary>
            public int ActivityChannelMP { get; private set; }

            /// <summary>
            /// Инициализировать объект даты воспроизведения Mp3 файлов
            /// </summary>
            /// <param name="CountChannel">Количество доступных каналов</param>
            public MP3(int CountChannel)
            {
                CountChannelMP = CountChannel;
                DoMP3player = [];
                ActivityChannelMP = 0;
                for (int i = 0; i < CountChannelMP; i++) DoMP3player.Add(new());
            }

            /// <summary>
            /// Воспроизвести звук .mp3
            /// </summary>
            /// <param name="NameSound">Путь к звуковому файлу</param>
            public void PlaySound(string NameSound)
            {
                NameSound = NameSound.Replace(".mp3", string.Empty);
                if (File.Exists($"{Directory.GetCurrentDirectory()}\\Data\\Mp3\\{NameSound}.mp3"))
                {
                    DoMP3player[ActivityChannelMP] = new()
                    {
                        URL = $"{Directory.GetCurrentDirectory()}\\Data\\Mp3\\{NameSound}.mp3"
                    };
                    DoMP3player[ActivityChannelMP].controls.play();
                    ActivityChannelMP = (ActivityChannelMP + 1) % CountChannelMP;
                }
                else
                {
                    ObjLog.LOGTextAppend($"Файл воспроизведения <..Data/Mp3/{NameSound}.mp3> не найден..");
                }
            }
        }

        /// <summary>
        /// Класс голосового девайса разпознающего речь
        /// </summary>
        public class InputVoiceCommandDevice
        {
            /// <summary>
            /// Глобальный дивайс ввода голосовых команд
            /// </summary>
            private SpeechRecognitionEngine RecordInput { get; set; }

            /// <summary>
            /// Коэффициент точности распознавания голосовых фраз
            /// </summary>
            public static float FactorAccuracyVoice { get; } = 0.6f;

            /// <summary>
            /// Встроенные голосовые фразы 
            /// </summary>
            public Choices DefaultChoicesProgramm { get; }

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
            public InputVoiceCommandDevice(TypeCommand.VoiceCommand[] DefaultVoiceCommand, bool ActivateDevice = false)
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
        /// Класс информации тем
        /// </summary>
        public partial class ThemeData
        {
            /// <summary>
            /// Все описания параметров темы
            /// </summary>
            public ThemeInfoParameter[] MassInfoParameters { get; }

            /// <summary>
            /// Массив всех объектов тем в программе
            /// </summary>
            public List<Theme> MassTheme { get; private set; }

            /// <summary>
            /// Активная тема в программе
            /// </summary>
            public Theme ActivateTheme { get; set; }

            /// <summary>
            /// Обычная тема
            /// </summary>
            public Theme Default { get; }

            /// <summary>
            /// Пустая тема
            /// </summary>
            public Theme Null { get; }

            /// <summary>
            /// Инициализировать объект информации тем
            /// </summary>
            internal ThemeData()
            {
                MassInfoParameters = ThemeInfoParameter.ReadDataBaseThemeInfo();
                Default = new(MassInfoParameters, SystemTheme.Default);
                Null = new(MassInfoParameters, SystemTheme.Null);
                MassTheme = new(ReadingAllThemes(MassInfoParameters));
                ActivateTheme = Default;
            }

            /// <summary>
            /// Прочитать все файлы тем 
            /// </summary>
            /// <returns></returns>
            private List<Theme> ReadingAllThemes(ThemeInfoParameter[] infoParameters)
            {
                ObjLog.LOGTextAppend("Программа изучает _THEME");
                Theme theme;
                List<Theme> list = [Default];
                foreach (string File in Directory.GetFiles($"{Directory.GetCurrentDirectory()}\\Data\\Theme\\"))
                {
                    if (Path.GetExtension(File).Equals("._theme"))
                    {
                        theme = ReadFile_theme(infoParameters, File);
                        list.Add(theme);
                        ObjLog.LOGTextAppend($"Прочитана тема: \"{theme.Name}\", Кол-во цветов: {theme.ObjColors.Length}");
                        ObjLog.LOGTextAppend($"Описание темы: \"{theme.Description}\"");
                        ObjLog.LOGTextAppend($"Директория иконки темы: \"{theme.IconDirectory}\"");
                    }
                }
                ObjLog.LOGTextAppend($"Всего прочитанных тем: {list.Count}");
                return list;
            }

            /// <summary>
            /// Прочитать фаил ._theme для инициализации темы в программу
            /// </summary>
            /// <param name="DirectoryFile">Директория читаемого файла</param>
            /// <returns>Theme: Тема прочитанная из файла</returns>
            private Theme ReadFile_theme(ThemeInfoParameter[] infoParameters, string DirectoryFile)
            {
                StreamReader FileRead = new(DirectoryFile);
                string TextFile = FileRead.ReadToEnd();
                FileRead.Close();

                string Name = RegexPatternNameTheme().Match(TextFile).Value.Replace("==", "=").Replace("=;", ";").Replace("NAME:", string.Empty);

                string IconDirectory = RegexPatternIconTheme().Match(TextFile).Value.Replace("==", "=").Replace("=;", ";").Replace("ICON:", string.Empty).Replace(@"\\", @"\");

                string Description = RegexPatternDescriptionTheme().Match(TextFile).Value.Replace("==", "=").Replace("=;", ";").Replace("DESCRIPTION:", string.Empty);

                List<Color> Colors = [];
                string[] Num;
                foreach (Match match in RegexPatternColorPatamTheme().Matches(TextFile).Cast<Match>())
                {
                    Num = RegexPatternColorOneRGB().Matches(match.Value).Select(i => i.Value.Replace(";", string.Empty)).ToArray();
                    ObjLog.LOGTextAppend($"0: {Num[0]} | 1: {Num[1]} | 2: {Num[2]} => L: {Num.Length}");
                    Colors.Add(Color.FromArgb(Convert.ToInt32(Num[0]), Convert.ToInt32(Num[1]), Convert.ToInt32(Num[2])));
                }
                return new Theme(Default.ObjColors, infoParameters, Name, DirectoryFile, IconDirectory, [.. Colors], Description);
            }

            /// <summary>
            /// Переключить тему на обычную
            /// </summary>
            public void ResetDefaultTheme() => ActivateTheme = Default;

            [GeneratedRegex("NAME:([^=;]|==|=;)+")]
            private static partial Regex RegexPatternNameTheme();

            [GeneratedRegex("ICON:([^=;]|==|=;)+")]
            private static partial Regex RegexPatternIconTheme();

            [GeneratedRegex("DESCRIPTION:([^=;]|==|=;)+")]
            private static partial Regex RegexPatternDescriptionTheme();

            [GeneratedRegex(@"=(([0-9]|[1-9]\d|1\d\d|[1-2][0-4]\d|[1-2][0-5][0-5]);){3}")]
            public static partial Regex RegexPatternColorPatamTheme();

            [GeneratedRegex(@"\d{1,3};")]
            private static partial Regex RegexPatternColorOneRGB();
        }

        /// <summary>
        /// Описание объекта управления девайсами звука
        /// </summary>
        public class DivacesAudio
        {
            //
            private MMDeviceEnumerator DivacesEnum { get; set; }

            /// <summary>
            /// Активное устройство воспроизводящее звук
            /// </summary>
            public MMDevice ActiveDevice { get; private set; }

            /// <summary>
            /// Инициализировать объект управления девайсами звука
            /// </summary>
            public DivacesAudio()
            {
                DivacesEnum = new();
                ActiveDevice = DivacesEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            }

            /// <summary>
            /// Изменить активное устройство аудиовывода
            /// </summary>
            /// <returns>Активный девайс воспроизведения звука</returns>
            public void UpdateActivateDivaceAudioOutput()
            {
                ActiveDevice = DivacesEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            }
        }

        /// <summary>
        /// Параметры для управления специальными цветами
        /// </summary>
        /// <remarks>
        /// Инициализировать объект управления специальными цветами
        /// </remarks>
        public class SpecialColor(SettingsData.SettingsBoolParameter ParamRGB, SettingsData.SettingsBoolParameter ParamRGBCC, SettingsData.SettingsBoolParameter ParamSC, Color Acient)
        {
            /// <summary>
            /// Объект специального цвета RGB
            /// </summary>
            public ColorRGB RGB { get; } = new(ParamRGB);

            /// <summary>
            /// Объект специального цвета RGBCC
            /// </summary>
            public ColorRGBCC RGBCC { get; } = new(ParamRGBCC);

            /// <summary>
            /// Объект специального цвета SC
            /// </summary>
            public ColorSC SC { get; } = new(ParamSC, Acient);

            /// <summary>
            /// Специальный цвет RGB
            /// </summary>
            /// <remarks>
            /// Инициализировать объект управления специальным цветом RGB
            /// </remarks>
            /// <param name="SettringsRGB">Параметр который управляет обновлениями специального цвета</param>
            public class ColorRGB(SettingsData.SettingsBoolParameter SettringsRGB)
            {
                /// <summary>
                /// Лист элементов под воздействием постоянного изменения цвета
                /// </summary>
                private List<dynamic> RGBLabel { get; set; } = [];

                /// <summary>
                /// Текущий цвет специального цвета
                /// </summary>
                public Color RealyColor { get; private set; } = Color.FromArgb(255, 0, 0);

                /// <summary>
                /// Объект управляющий обновлениями
                /// </summary>
                private SettingsData.SettingsBoolParameter Parameter { get; } = SettringsRGB;

                /// <summary>
                /// Добавить элемент под контроль специального цвета
                /// </summary>
                /// <param name="Element">Добавляемый элемент</param>
                /// <exception cref="ArgumentException">Ошибка при которой элемент не имеет свойство ForeColor</exception>
                public void AddElement(dynamic Element)
                {
                    try { Element.ForeColor = RealyColor; RGBLabel.Add(Element); }
                    catch { throw new ArgumentException("Argument invalid {Element}-try-.ForeColor ADD(RGB)"); }
                }

                /// <summary>
                /// Постоянный цикл обновления RGB текста
                /// </summary>
                public async void StartUpdate()
                {
                    int i = 0;
                    await Task.Run(() =>
                    {
                        while (true)
                        {
                            if (Parameter.Value && App.MainForm.WindowState == FormWindowState.Normal)
                            {
                                switch (i)
                                {
                                    case 0:
                                        RealyColor = Color.FromArgb(RealyColor.R - 1, RealyColor.G + 1, RealyColor.B);
                                        break;
                                    case 1:
                                        RealyColor = Color.FromArgb(RealyColor.R, RealyColor.G - 1, RealyColor.B + 1);
                                        break;
                                    case 2:
                                        RealyColor = Color.FromArgb(RealyColor.R + 1, RealyColor.G, RealyColor.B - 1);
                                        break;
                                }
                                if (RealyColor.R == 255 || RealyColor.G == 255 || RealyColor.B == 255) i = (i + 1) % 3;
                                RGBLabel = RGBLabel.Select(c =>
                                {
                                    try { c.ForeColor = RealyColor; }
                                    catch { }
                                    return c;
                                }).ToList();
                            }
                            Thread.Sleep(10);
                        }
                    });
                }
            }

            /// <summary>
            /// Специальный цвет RGBCC
            /// </summary>
            /// <remarks>
            /// Инициализировать объект управления специальным цветом RGBCC
            /// </remarks>
            /// <param name="SettringsRGBCC">Параметр который управляет обновлениями специального цвета</param>
            public class ColorRGBCC(SettingsData.SettingsBoolParameter SettringsRGBCC)
            {
                /// <summary>
                /// Список элементов под воздействием постоянного изменения цвета с помощью курсора
                /// </summary>
                private List<dynamic> RGBCCLabel { get; set; } = [];

                /// <summary>
                /// Текущий цвет объекта специального цвета
                /// </summary>
                public Color RealyColor { get; set; } = Color.FromArgb(0, 0, 0);

                /// <summary>
                /// Объект управляющий обновлениями
                /// </summary>
                private SettingsData.SettingsBoolParameter Parameter { get; } = SettringsRGBCC;

                /// <summary>
                /// Добавить элемент под контроль специального цвета
                /// </summary>
                /// <param name="Element">Добавляемый элемент</param>
                /// <exception cref="ArgumentException">Ошибка при которой элемент не имеет свойство ForeColor</exception>
                public void AddElement(dynamic Element)
                {
                    try { Element.ForeColor = RealyColor; RGBCCLabel.Add(Element); }
                    catch { throw new ArgumentException("Argument invalid {Element}-try-.ForeColor ADD(RGBCC)"); }
                }

                /// <summary>
                /// Постоянный цикл обновления RGBCC текста
                /// </summary>
                public async void StartUpdate()
                {
                    await Task.Run(() =>
                    {
                        while (true)
                        {
                            if (Parameter.Value && App.MainForm.WindowState == FormWindowState.Normal)
                            {
                                RealyColor = Color.FromArgb(
                                    Convert.ToInt32(Math.Abs((Math.Atan(Cursor.Position.X) - Cursor.Position.Y) / 5) % 256d),
                                    Convert.ToInt32(Math.Abs((Math.Cos(Cursor.Position.Y) - Cursor.Position.X) / 10) % 256d),
                                    Convert.ToInt32(Math.Abs((1080d - Math.Cos(Cursor.Position.X)) * 2) % 256d)
                                    );
                                RGBCCLabel = RGBCCLabel.Select(c =>
                                {
                                    try { c.ForeColor = RealyColor; }
                                    catch { }
                                    return c;
                                }).ToList();
                            }
                            Thread.Sleep(10);
                        }
                    });
                }
            }

            /// <summary>
            /// Специальный цвет SC
            /// </summary>
            /// <remarks>
            /// Инициализировать объект специального цвета SC
            /// </remarks>
            /// <param name="ParamSC">Параметр от которого будет зависеть специальный цвет</param>
            /// <param name="Actient">Статический акцент специального цвета</param>
            public class ColorSC(SettingsData.SettingsBoolParameter ParamSC, Color Actient)
            {
                /// <summary>
                /// Список элементов под воздействием постоянного изменения цвета с помощью звука
                /// </summary>
                private List<dynamic> SCLabel { get; set; } = [];

                /// <summary>
                /// Текущий цвет объекта специального цвета
                /// </summary>
                public Color RealyColor { get; set; }

                /// <summary>
                /// Статический акцент специального цвета
                /// </summary>
                public Color ActientColorSC { get; set; } = Actient;

                /// <summary>
                /// Параметр от которого зависит специальный цвет
                /// </summary>
                private SettingsData.SettingsBoolParameter Parameter { get; } = ParamSC;

                /// <summary>
                /// Добавить элемент под контроль специального цвета
                /// </summary>
                /// <param name="Element">Добавляемый элемент</param>
                /// <exception cref="ArgumentException">Ошибка при которой элемент не имеет свойство ForeColor</exception>
                public void AddElement(dynamic Element)
                {
                    try { Element.ForeColor = RealyColor; SCLabel.Add(Element); }
                    catch { throw new ArgumentException("Argument invalid {Element}-try-.ForeColor ADD(SC)"); }
                }

                /// <summary>
                /// Обновить цвет SC цвета
                /// </summary>
                public async void StartUpdate()
                {
                    MMDeviceEnumerator Device = new();
                    float PointVolume;
                    //int FullAudio255, ColorRatio, MoveIndicatorOffset1, MoveIndicatorOffset2;
                    int R, G, B;
                    await Task.Run(() =>
                    {
                        while (true)
                        {
                            if (Parameter.Value && App.MainForm.WindowState == FormWindowState.Normal)
                            {
                                PointVolume = (int)(Device.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).AudioMeterInformation.MasterPeakValue * 100);
                                R = (int)(2.55f * PointVolume + ActientColorSC.R);
                                G = (int)(2.55f * PointVolume + ActientColorSC.G);
                                B = (int)(2.55f * PointVolume + ActientColorSC.B);
                                RealyColor = Color.FromArgb(
                                    R > 255 ? 255 - (R - 255) : R,
                                    G > 255 ? 255 - (G - 255) : G,
                                    B > 255 ? 255 - (B - 255) : B);
                                SCLabel = SCLabel.Select(c =>
                                {
                                    try { c.ForeColor = RealyColor; }
                                    catch { }
                                    return c;
                                }).ToList();
                            }
                            Thread.Sleep(10);
                        }
                    });
                }
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
            /// Статус активной страницы в форме Settings
            /// </summary>
            public SettingsPage ActiveSettingsPage { get; set; } = SettingsPage.Null;

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

        /// <summary>
        /// Статус обычного Флага
        /// </summary>
        public enum StatusFlags
        {

            /// <summary>
            /// Не активен
            /// </summary>
            NotActive = 0,

            /// <summary>
            /// Активен
            /// </summary>
            Active = 1,

            /// <summary>
            /// Состояние спящего режима или состояние проверки точного состояния
            /// </summary>
            Sleep = 2
        }

        /// <summary>
        /// Статус Флага диалогового окна
        /// </summary>
        public enum DialogWindowStatus
        {

            /// <summary>
            /// Отмена
            /// </summary>
            Cancel = 0,

            /// <summary>
            /// Одобрено
            /// </summary>
            Ok = 1
        }

        /// <summary>
        /// Статус булевого Флага
        /// </summary>
        public enum BooleanFlags
        {

            /// <summary>
            /// Отрицание Флага
            /// </summary>
            False = 0,

            /// <summary>
            /// Одобрение Флага
            /// </summary>
            True = 1
        }

        /// <summary>
        /// Описание структуры объекта флага
        /// </summary>
        /// <remarks>
        /// Инициализировать объект флага
        /// </remarks>
        /// <param name="Value">Стартовое значение</param>
        public class Flag(bool Value)
        {
            /// <summary>
            /// Делегат события изменения состояния флага
            /// </summary>
            /// <param name="SetBool">Состояние изменённого флага</param>
            public delegate void EventChangeStateFlag(bool SetBool);

            /// <summary>
            /// Событие изменения состояния флага
            /// </summary>
            public event EventChangeStateFlag? ChangeStateFlag;

            /// <summary>
            /// Ресурсное значение флага
            /// </summary>
            private bool _Value = Value;

            /// <summary>
            /// Видимое значение флага
            /// </summary>
            public bool Value
            {
                get => _Value;
                set
                {
                    _Value = value;
                    ChangeStateFlag?.Invoke(_Value);
                }
            }
        }
    }

    /// <summary>
    /// Все страницы формы Settings
    /// </summary>
    public enum SettingsPage
    {
        /// <summary>
        /// Форма отключена, Нет доступа к странице
        /// </summary>
        Null = 0,

        /// <summary>
        /// Главная страница (Главное меню)
        /// </summary>
        MainMenu = 1,

        /// <summary>
        /// Страница управление цветами (Палитра)
        /// </summary>
        Colored = 2,

        /// <summary>
        /// Страница PAC (Мини-панель)
        /// </summary>
        PAC = 3,

        /// <summary>
        /// Страница прочих параметров (Прочее..)
        /// </summary>
        Other = 4,
    }

    /// <summary>
    /// Направления сторон
    /// </summary>
    public enum DirectionsParties
    {
        /// <summary>
        /// Левое направление
        /// </summary>
        Left = 1,

        /// <summary>
        /// Правое направление
        /// </summary>
        Right = 2
    }
}
