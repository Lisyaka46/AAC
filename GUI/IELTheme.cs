using static AAC.Classes.MainTheme;
using static AAC.GUI.IELTheme.CustomEvents;

namespace AAC.GUI
{
    public partial class IELTheme : UserControl
    {
        /// <summary>
        /// Класс собственных событий объекта
        /// </summary>
        public static class CustomEvents
        {
            /// <summary>
            /// Делегат события активации визуализации действий над темой
            /// </summary>
            /// <param name="IlustTheme">Тема над которой производятся действия</param>
            public delegate void EventActionLeftClick(Theme? IlustTheme);

            /// <summary>
            /// Делегат события при котором активируется визуализация описания темы
            /// </summary>
            /// <param name="DescriptionText">Текст описания темы</param>
            public delegate void EventDescriptionShowOrHide(string? DescriptionText);
        }

        /// <summary>
        /// Тема которую илюстрирует объект
        /// </summary>
        public Theme? IlustrationTheme { get; }

        /// <summary>
        /// Событие активации отображения действий над темой
        /// </summary>
        public event EventActionLeftClick? ActionLeftClick;

        /// <summary>
        /// Событие активации видимости описания
        /// </summary>
        public event EventDescriptionShowOrHide? DescriptionEvent;

        /// <summary>
        /// Инициализировать объект визуализации темы
        /// </summary>
        /// <param name="theme">Тема которую визуализирует объект</param>
        /// <param name="parent">Панель в которой может находиться объект</param>
        /// <param name="location">Стартовая позиция объекта</param>
        public IELTheme(Theme? theme, Panel? parent, Point? location)
        {
            InitializeComponent();
            if (parent != null) Parent = parent;
            Location = location ?? new(0, 0);
            IlustrationTheme = theme;
            if (theme != null)
            {
                ElementName.Text = theme.Value.Name;
                Icon.ImageLocation = theme.Value.IconDirectory;
                Icon.BorderStyle = BorderStyle.None;
                Icon.Refresh();
            }
        }

        /// <summary>
        /// Событие нажатия клавиши мыши над элементом описания темы
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект события</param>
        private void Description_MouseClick(object sender, MouseEventArgs e)
        {
            if (IlustrationTheme != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    DescriptionEvent?.Invoke(IlustrationTheme.Value.Description);
                    ActionLeftClick?.Invoke(null);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    DescriptionEvent?.Invoke(null);
                    ActionLeftClick?.Invoke(IlustrationTheme);
                }
            }
        }

        /// <summary>
        /// Событие ухода курсора мыши от области объекта описания темы
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект события</param>
        private void Description_MouseLeave(object sender, EventArgs e)
        {
            if (IlustrationTheme != null) DescriptionEvent?.Invoke(null);
            Description.BackColor = Color.FromArgb(65, 72, 89);
        }

        /// <summary>
        /// Событие входа курсора мыши в область объекта описания темы
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект события</param>
        private void Description_MouseEnter(object sender, EventArgs e)
        {
            Description.BackColor = Color.FromArgb(75, 82, 99);
        }

        /// <summary>
        /// Событие нажатия клавиши мыши над элементом для создания события открытия дествий над объектом
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект события</param>
        private void ElementTheme_DetectAction(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && IlustrationTheme != null) ActionLeftClick?.Invoke(IlustrationTheme);
        }
    }
}
