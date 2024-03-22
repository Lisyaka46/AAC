using static AAC.Classes.AnimationDL.Animate.AnimFormule;
using static AAC.Startcs;

namespace AAC
{
    public static class Forms_Functions
    {

        /// <summary>
        /// Класс управления обновлением специальных цветов 
        /// </summary>
        public static class ColorWhile
        {
            /// <summary>
            /// Инвертировать цвет
            /// </summary>
            /// <param name="SetColor">Обычный цвет</param>
            /// <returns>Инвертированный цвет от обычного</returns>
            public static Color InvColor(Color SetColor) =>
                Color.FromArgb(Math.Abs(SetColor.R - 255), Math.Abs(SetColor.G - 255), Math.Abs(SetColor.B - 255));

            public static Color SetOffsetColor(Color SetColor, sbyte Offset) =>
                Color.FromArgb(Math.Abs(SetColor.R + Offset), Math.Abs(SetColor.G + Offset), Math.Abs(SetColor.B + Offset));
        }
    }
}
