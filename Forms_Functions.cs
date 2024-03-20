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

        /// <summary>
        /// Функция запуска основных действий при старте программы
        /// </summary>
        public static void StartingProgramm()
        {
            Task.Run(App.MainForm.AlwaysUpdateWindow);
            //Task.Run(App.MainForm.AlwaysUpdatePositionLabels);
            MainData.AllSpecialColor.RGB.StartUpdate();
            MainData.AllSpecialColor.RGBCC.StartUpdate();
            MainData.AllSpecialColor.SC.StartUpdate();
            App.MainForm.CapsLock_Info.Image = Control.IsKeyLocked(Keys.CapsLock) ?
                    Image.FromFile(@"Data\Image\Up-A.gif") : Image.FromFile(@"Data\Image\Down-a.gif");
            App.MainForm.LActiveitedSoftCommand_Click(null, null);
            ConstAnimMove ConstantFormule = new(App.MainForm.pICON.Location.X, 15, 8);
            ConstantFormule.InitAnimFormule(App.MainForm.pICON, Formules.QuickTransition, new ConstAnimMove(App.MainForm.pICON.Location.Y), AnimationStyle.XY);
        }
    }
}
