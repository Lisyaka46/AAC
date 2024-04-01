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
        /// Массив всех объектов тем в программе
        /// </summary>
        public readonly List<Theme> MassTheme;

        /// <summary>
        /// Массив всех объектов тем в программе
        /// </summary>
        public Theme ActivityTheme => MassTheme[ActivateThemeIndex_];

        /// <summary>
        /// Индекс активной темы
        /// </summary>
        private int ActivateThemeIndex_ = 0;

        /// <summary>
        /// Активный индес темы в программе
        /// </summary>
        public int ActivateThemeIndex
        {
            get => ActivateThemeIndex_;
            set
            {
                if (value >= MassTheme.Count) throw new ArgumentOutOfRangeException(nameof(value), $"Индекс параметра \"{value}\" является больше допустимого: {MassTheme.Count}");
                ActivateThemeIndex_ = value;
            }
        }

        /// <summary>
        /// Директория файла информации всех тем программы
        /// </summary>
        private const string DirectoryFileTheme = "Data/Info/Theme.r1";

        /// <summary>
        /// Инициализировать объект информации тем
        /// </summary>
        public ThemeData()
        {
            MassTheme =
            [
                new("Default", "Системная тема программы",
                [
                    Color.Black, Color.Black, Color.Black, Color.Black,
                    Color.Black, Color.Black, Color.Black, Color.Black,
                    Color.Black, Color.Black, Color.Black, Color.Black,
                    Color.Black, Color.Black, Color.Black,
                ]),
            ];
            MassTheme.AddRange(ReadingAllThemes());
        }

        /// <summary>
        /// Прочитать все файлы тем 
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Theme> ReadingAllThemes()
        {
            ObjLog.LOGTextAppend("Программа изучает _THEME");
            if (!File.Exists(DirectoryFileTheme)) return [];
            string[] AllLines = File.ReadAllLines(DirectoryFileTheme);
            List<Theme> list = AllLines.Select(line =>
            {
                // Name%Description%1;1;1;*1;1;1;*1;1;1;*1;1;1;*1;1;1;*%!
                MatchCollection CategoriesReadTheme = Categories().Matches(line);
                List<Color> Colors = [];
                MatchCollection MassColorsParams = ColorParameters().Matches(CategoriesReadTheme[2].Value);
                Colors.AddRange(MassColorsParams.Select(i =>
                {
                    MatchCollection MassColorNumbers = NumbersColor().Matches(i.Value);
                    return Color.FromArgb(Convert.ToByte(MassColorNumbers[0].Value), Convert.ToByte(MassColorNumbers[1].Value), Convert.ToByte(MassColorNumbers[2].Value));
                }));
                return new Theme(CategoriesReadTheme[0].Value, CategoriesReadTheme[1].Value, [.. Colors.AsEnumerable()]);
            }).ToList();
            return list.AsEnumerable();
        }

        [GeneratedRegex("\\b[^%]+")]
        private static partial Regex Categories();
        [GeneratedRegex("\\b[^*]+")]
        private static partial Regex ColorParameters();
        [GeneratedRegex("\\b(\\d){1,3}")]
        private static partial Regex NumbersColor();
    }
}
