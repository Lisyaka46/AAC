using System.CodeDom;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static AAC.Forms_Functions;
using AAC.Classes;

namespace AAC
{
    public static class App
    {
        /// <summary>
        /// Основная форма
        /// </summary>
        public static MainApplication MainForm { get; set; } = null;

        /// <summary>
        /// Форма описания всех команд зарегестрируемых в проект
        /// </summary>
        public static FormExplanationCommands InformationCommand { get; set; } = null;

        /// <summary>
        /// Форма визуализации данных журнала
        /// </summary>
        public static LogForm Log { get; set; } = null;

        /// <summary>
        /// Форма стартовой анимации
        /// </summary>
        public static FormAnimateStart Starting { get; set; } = null;

        /// <summary>
        /// Диалоговое окно изменения кастомных gif анимаций на главной форме
        /// </summary>
        public static DialogCustomImage DialogCustomImage_Form { get; set; } = null;

        /// <summary>
        /// Форма благодарностей
        /// </summary>
        public static DialogThanks Thanks { get; set; } = null;

        /// <summary>
        /// Формы Настроек CLR
        /// </summary>
        public static class Settings
        {
            /// <summary>
            /// Главная форма настроек
            /// </summary>
            public static Forms.FormMainSettings WindowSettings { get; set; } = null;

            /// <summary>
            /// Форма редактирования тем для CLR
            /// </summary>
            public static Forms.FormThemesEditor ThemesCreated { get; set; } = null;

        }

        /// <summary>
        /// Класс форм отвечающих за создание
        /// </summary>
        public static class Create
        {
            /// <summary>
            /// Диалоговое окно создания ярлыка CLR
            /// </summary>
            public static Forms.Dialogs.DialogCreateLabel DialogCreateLabel { get; set; } = null;

            /// <summary>
            /// Диалоговое окно редактирования и создания темы
            /// </summary>
            public static Forms.Dialogs.DialogCreateTheme DialogCreateTheme { get; set; } = null;
        }
    }
    /// <summary>
    /// Описание объекта журнала
    /// </summary>
    /// <remarks>
    /// Инициализировать объект журнала
    /// </remarks>
    /// <param name="MaxLength">Максимальная вместимость журнала</param>
    public class Log(int MaxLength)
    {
        /// <summary>
        /// Инициализировать объект журнала
        /// </summary>
        /// <param name="Text">Текст объекта журнала</param>
        public class ObjLogInfo(string Text)
        {
            /// <summary>
            /// Текст объекта журнала
            /// </summary>
            public string Text { get; } = $"{DateTime.Now:dd/MM/yyyy} {DateTime.Now:HH/mm/ss/fff} >> {Text}";

            /// <summary>
            /// Время создания объекта журнала
            /// </summary>
            public DateTime DateTime { get; } = DateTime.Now;
        }

        /// <summary>
        /// Массив объектов журнала
        /// </summary>
        public List<ObjLogInfo> MassLogElements { get; private set; } = [];

        /// <summary>
        /// Максимальная вместимость объектов журнала
        /// </summary>
        public int MaxLength { get; } = MaxLength;

        /// <summary>
        /// Делегат события добавления объекта в журнал
        /// </summary>
        /// <param name="Text">Добавляемые данные</param>
        /// <param name="index">Индексированая позиция объекта журнала</param>
        public delegate void AppendLogElement(string Text, int index);

        /// <summary>
        /// Делегат события смещения объектов журнала
        /// </summary>
        /// <param name="Text">Добавляемые данные</param>
        public delegate void MovingLogElements(string Text);

        /// <summary>
        /// Событие добавления объекта в журнал
        /// </summary>
        public event AppendLogElement? AddLogELement;

        /// <summary>
        /// Событие смещения объектов журнала
        /// </summary>
        public event MovingLogElements? ResizeMoveLogElements;

        /// <summary>
        /// Создать объект журнала
        /// </summary>
        /// <param name="Text">Добавляемый текст</param>
        public void LOGTextAppend(string Text)
        {
            if (MassLogElements.Count == MaxLength) ResizeMoveLogElements?.Invoke(MassLogElements[^1].Text);
            else if (MassLogElements.Count < MaxLength)
            {
                MassLogElements.Add(new(Text));
                AddLogELement?.Invoke(MassLogElements[^1].Text, MassLogElements.Count - 1);
            }
        }
    }
    public static partial class Startcs
    {
        /// <summary>
        /// Объект управления объектами журнала
        /// </summary>
        public static Log ObjLog { get; private set; } = null;

        /// <summary>
        /// Объект главной даты программы
        /// </summary>
        public static Data MainData { get; private set; } = null;

        [LibraryImport("user32.dll")]
        private static partial IntPtr GetForegroundWindow();

        [LibraryImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [LibraryImport("user32.dll", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int LoadKeyboardLayout(string pwszKLID, uint Flags);

        [STAThread]
        static void Main()
        {
            try
            {
                ObjLog = new(50);
                MainData = new();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                App.Starting = new();
                new Thread(() => Application.Run(App.Starting)).Start();
                App.MainForm = new();
                while (App.Starting.Opacity > 0d)
                {
                    App.Starting.Opacity -= 0.009d;
                    App.Starting.Update();
                    Thread.Sleep(1);
                }
                App.Starting.Opacity = 0d;
                App.Starting.Close();
                ObjLog.LOGTextAppend("Программа активируется");
                StartingProgramm();
                App.MainForm.Opacity = 0d;
                Application.Run(App.MainForm);
            }
            catch (Exception ex)
            {
                StackFrame[] Frames = new StackTrace(ex, true).GetFrames();
                ObjLog.LOGTextAppend($"** Исключение старта программы");
                for (int i = 0; i < Frames.Length; i++) 
                {
                    ObjLog.LOGTextAppend($"{i}. Файл {Frames[i].GetFileName() ?? "??"} <{Frames[i].GetFileLineNumber()}/{Frames[i].GetFileColumnNumber()}>" +
                        $"\nText: {Frames[i]}");
                }
                App.Log = new();
                Application.Run(App.Log);
            }
        }
    }
}
