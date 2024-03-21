using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AAC.Classes.MainTheme;
using static AAC.Startcs;

namespace AAC.Classes.DataClasses
{
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
}
