using System.Data.OleDb;
using static AAC.Startcs;

namespace AAC.Classes
{
    public static class MainTheme
    {
        /// <summary>
        /// Класс информационного объекта описывающего параметр палитры темы
        /// </summary>
        /// <remarks>
        /// Инициализировать объект информации цветового параметра темы
        /// </remarks>
        /// <param name="name">Имя параметра</param>
        /// <param name="value">Значение параметра</param>
        public class ThemeInfoParameter(string name, string value)
        {
            /// <summary>
            /// Имя параметра
            /// </summary>
            public readonly string Name = name;

            /// <summary>
            /// Значение параметра
            /// </summary>
            public readonly string Value = value;
        }

        /// <summary>
        /// Данные о параметрах цветов в темах
        /// </summary>
        public static readonly ThemeInfoParameter[] ThemeInfoParameters =
        [
            new ThemeInfoParameter("Background-Output-Console-Text", "Определяет цвет фона консоли"),
            new ThemeInfoParameter("Background-Mini-Console-Panel", "Определяет цвет фона мини-панели вызываемой в консоли"),
            new ThemeInfoParameter("Background-Up-Panel", "Определяет цвет фона верхней панели программы"),
            new ThemeInfoParameter("Background-Main-Form", "Определяет цвет фона главной формы"),
            new ThemeInfoParameter("Background-Input-Console-Text", "Определяет цвет фона ввода консольной строки"),
            new ThemeInfoParameter("Background-Button-Help", "Определяет цвет фона вопросительной кнопки"),
            new ThemeInfoParameter("Background-Mini-Panel-Settings", "Определяет цвет фона мини-панели настроек"),
            new ThemeInfoParameter("Background-Label-Panel-Element", "Определяет цвет фона панели ярлыков"),
            new ThemeInfoParameter("Background-Hit-Panel-Color", "Определяет цвет фона панели подсказок к командам"),
            new ThemeInfoParameter("Background-Button-Hide-Form", "Определяет цвет фона кнопки сворачивания главной формы"),
            new ThemeInfoParameter("Background-Button-Close-Form", "Определяет цвет фона кнопки закрытия главной формы"),
            new ThemeInfoParameter("Fore-Color-Text-Time", "Определяет цвет текста времени в главной форме"),
            new ThemeInfoParameter("Fore-Color-Text-Data", "Определяет цвет текста даты в главной форме"),
            new ThemeInfoParameter("Fore-Color-Text-Language", "Определяет цвет текста названия языка в главной форме"),
            new ThemeInfoParameter("Fore-Color-Output-Text", "Определяет цвет текста консоли в главной форме"),
        ];

        /// <summary>
        /// Класс реализации темы
        /// </summary>
        public struct Theme
        {
            /// <summary>
            /// Параметры темы
            /// </summary>
            public readonly Color[] Palette;

            /// <summary>
            /// Имя темы
            /// </summary>
            private string Name_ = string.Empty;

            /// <summary>
            /// Имя темы
            /// </summary>
            public string Name
            {
                readonly get => Name_;
                set
                {
                    if (value != null)
                    {
                        if (value.Length > 0)
                        {
                            Name_ = value;
                            return;
                        }
                    }
                    throw new ArgumentException("Строка имени темы не может содержать в себе пустое значение или null");
                }
            }

            /// <summary>
            /// Описание темы
            /// </summary>
            private string Description_ = string.Empty;

            /// <summary>
            /// Описание темы
            /// </summary>
            public string Description
            {
                readonly get => Description_;
                set
                {
                    if (value != null)
                    {
                        if (value.Length > 0)
                        {
                            Description_ = value;
                            return;
                        }
                    }
                    throw new ArgumentException("Строка описания темы не может содержать в себе пустое значение или null");
                }
            }

            /// <summary>
            /// Инициализировать тему
            /// </summary>
            /// <param name="StyleTheme"></param>
            public Theme(string Name, string? Description, Color[] Palette)
            {
                if (ThemeInfoParameters.Length < Palette.Length) throw new ArgumentException("Количество значений цвета отличается от количества ожидаемых цветов");
                this.Name = Name;
                this.Description = Description ?? "Нет описания";
                this.Palette = Palette;
            }
        }
    }
}
