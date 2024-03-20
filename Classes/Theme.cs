using System.Data.OleDb;
using static AAC.Startcs;

namespace AAC.Classes
{
    public static class MainTheme
    {
        /// <summary>
        /// Класс информационного объекта описывающего параметр палитры темы
        /// </summary>
        public class ThemeInfoParameter
        {
            /// <summary>
            /// Имя параметра
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Значение параметра
            /// </summary>
            public string Value { get; }

            /// <summary>
            /// Инициализировать объект информации цветового параметра темы
            /// </summary>
            /// <param name="name">Имя параметра</param>
            /// <param name="value">Значение параметра</param>
            private ThemeInfoParameter(string name, string value)
            {
                Name = name;
                Value = value;
            }

            /// <summary>
            /// Создать массив объектов описывающих параметры палитры темы
            /// </summary>
            /// <returns></returns>
            public static ThemeInfoParameter[] ReadDataBaseThemeInfo()
            {
                /*Встроенный массив объектов описывающих параметры палитры темы*/ {
                    return [
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
                }
            }
        }

        /// <summary>
        /// Интерфейс описания объекта темы
        /// </summary>
        private interface ITheme
        {
            /// <summary>
            /// Массив параметров темы
            /// </summary>
            ThemeObjColor[] ObjColors { get; }

            /// <summary>
            /// Имя темы
            /// </summary>
            string Name { get; }

            /// <summary>
            /// Описание темы
            /// </summary>
            string Description { get; }

            /// <summary>
            /// Картинка/Иконка темы
            /// </summary>
            string IconDirectory { get; set; }

            /// <summary>
            /// Директория файла темы
            /// </summary>
            string FileDirectory { get; }
        }

        /// <summary>
        /// Интерфейс описания объекта параметра темы
        /// </summary>
        private interface IObjColor
        {
            /// <summary>
            /// Имя параметра темы
            /// </summary>
            string Name { get; }

            /// <summary>
            /// Описание параметра темы
            /// </summary>
            string Explanation { get; }

            /// <summary>
            /// Параметр цвета темы
            /// </summary>
            Color ElColor { get; set; }
        }

        /// <summary>
        /// Класс объекта параметра темы
        /// </summary>
        public class ThemeObjColor : IObjColor
        {
            /// <summary>
            /// Имя параметра
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Описание параметра
            /// </summary>
            public string Explanation { get; }

            /// <summary>
            /// Параметр цвета темы
            /// </summary>
            public Color ElColor { get; set; }

            /// <summary>
            /// Инициализация объекта параметра темы
            /// </summary>
            /// <param name="Name">Имя темы</param>
            /// <param name="Explanation">Описание темы</param>
            /// <param name="ColorValue">Цвет параметра</param>
            private ThemeObjColor(string Name, string Explanation, Color ColorValue)
            {
                this.Name = Name;
                this.Explanation = Explanation;
                ElColor = ColorValue;
            }

            /// <summary>
            /// Создать массив полных параметров темы по массиву цветов
            /// </summary>
            /// <param name="Colors">Массив цветов</param>
            /// <returns></returns>
            public static ThemeObjColor[] MassThemeObj(ThemeInfoParameter[] AllParamTheme, Color[] Colors, ThemeObjColor[]? DefaultObjColors)
            {
                List<ThemeObjColor> ObjParameters = [];
                for (int i = 0; i < AllParamTheme.Length; i++)
                {
                    if (i < Colors.Length) ObjParameters.Add(new(AllParamTheme[i].Name, AllParamTheme[i].Value, Colors[i]));
                    else ObjParameters.Add(DefaultObjColors?[i] ?? new(AllParamTheme[i].Name, AllParamTheme[i].Value, Color.White));
                }
                return [..ObjParameters];
            }
        }

        /// <summary>
        /// Класс реализации темы
        /// </summary>
        public struct Theme : ITheme
        {
            /// <summary>
            /// Параметры темы
            /// </summary>
            public ThemeObjColor[] ObjColors { get; }

            /// <summary>
            /// Имя темы
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Описание темы
            /// </summary>
            public string Description { get; }

            /// <summary>
            /// Картинка/Иконка темы
            /// </summary>
            public string IconDirectory { get; set; }

            /// <summary>
            /// Файл темы
            /// </summary>
            public string FileDirectory { get; }

            /// <summary>
            /// Узнать тема является пустой или нет
            /// </summary>
            /// <returns></returns>
            public readonly bool ThisNull()
            {
                if (ObjColors.Length == 0) return true;
                return false;
            }

            /// <summary>
            /// Инициализировать системную тему
            /// </summary>
            /// <param name="StyleTheme"></param>
            public Theme(ThemeInfoParameter[] AllParamTheme, SystemTheme StyleTheme)
            {
                FileDirectory = string.Empty;

                Name = StyleTheme switch
                {
                    SystemTheme.Default => "Default",
                    SystemTheme.Null => string.Empty,
                    _ => throw new NotImplementedException()
                };

                Description = StyleTheme switch
                {
                    SystemTheme.Default => "Встроенная системная тема программы",
                    SystemTheme.Null => string.Empty,
                    _ => throw new NotImplementedException()
                };

                IconDirectory = StyleTheme switch
                {
                    SystemTheme.Default => $"{Directory.GetCurrentDirectory()}\\Data\\Image\\Starting.gif",
                    SystemTheme.Null => string.Empty,
                    _ => throw new NotImplementedException()
                };

                ObjColors = StyleTheme switch
                {
                    SystemTheme.Default => ThemeObjColor.MassThemeObj(AllParamTheme,
                    [
                        Color.Black, Color.Black, Color.FromArgb(38, 34, 18), Color.Black, Color.Black, Color.Black,
                        Color.DimGray, Color.FromArgb(3, 4, 12), Color.Black, Color.FromArgb(38, 34, 18), Color.FromArgb(38, 34, 18),
                        Color.FromArgb(250, 250, 250), Color.FromArgb(225, 225, 225), Color.FromArgb(250, 240, 180),
                        Color.FromArgb(70, 140, 100)
                    ], null),
                    SystemTheme.Null => [],
                    _ => throw new NotImplementedException()
                };
            }

            /// <summary>
            /// Копировать объект темы со своими цветовыми параметрами темы
            /// </summary>
            /// <param name="Delegat">Делегат темы, откуда копируются свойства</param>
            /// <param name="Colors">Свои цвета темы</param>
            /// <param name="Description">Описание темы</param>
            /// <param name="Name">Имя темы</param>
            /// <param name="IconDirectory">Директория иконки</param>
            public Theme(ThemeInfoParameter[] AllParamTheme, Theme Delegat, Color[]? Colors, string? Name, string? Description, string? IconDirectory)
            {
                FileDirectory = Delegat.FileDirectory;
                this.Name = Name ?? Delegat.Name;
                this.Description = Description ?? Delegat.Description;
                this.IconDirectory = IconDirectory ?? Delegat.IconDirectory;
                ObjColors = Colors != null ? ThemeObjColor.MassThemeObj(AllParamTheme, Colors, Delegat.ObjColors) : Delegat.ObjColors;
            }

            public Theme(ThemeObjColor[]? DefaultObjColors, ThemeInfoParameter[] AllParamTheme, string Name, string FileDirectory, string IconDirectory, Color[] Colors, string? Description)
            {
                if (File.Exists(FileDirectory))
                {
                    this.Name = Name;
                    this.FileDirectory = FileDirectory;
                    ObjColors = ThemeObjColor.MassThemeObj(AllParamTheme, Colors, DefaultObjColors);
                    this.Description = Description ?? "Нет описания";
                    this.IconDirectory = File.Exists(IconDirectory) ? IconDirectory : $"{Directory.GetCurrentDirectory()}\\Data\\Image\\NewTheme.ico";
                }
                else throw new ArgumentNullException(nameof(FileDirectory), "Argument lendth 0 the path");
            }
        }

        /// <summary>
        /// Системные темы
        /// </summary>
        public enum SystemTheme
        {
            /// <summary>
            /// Обычная тема
            /// </summary>
            Default = 0,

            /// <summary>
            /// Нулевая/Пустая тема
            /// </summary>
            Null = 1,
        }
    }
}
