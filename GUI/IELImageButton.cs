namespace AAC.GUI
{
    public partial class IELImageButton : UserControl
    {
        /// <summary>
        /// Свойство неактивного цвета
        /// </summary>
        private Color? DisactiveColor { get; set; }

        /// <summary>
        /// Свойство Активного цвета
        /// </summary>
        private Color? ActiveColor { get; set; }

        /// <summary>
        /// Оффсет цвета
        /// </summary>
        private int OffsetColor { get; set; }

        /// <summary>
        /// Инициализировать объект кнопки
        /// </summary>
        public IELImageButton()
        {
            InitializeComponent();
            OffsetColor = 20;
        }

        /// <summary>
        /// Событие входа курсора в видимую облать объекта
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект информации о событии</param>
        private void ElementImage_MouseEnter(object sender, EventArgs e)
        {
            if (DisactiveColor == null) DisactiveColor = BackColor;
            BackColor = Color.FromArgb(
                BackColor.R <= (255 - OffsetColor) ? BackColor.R + OffsetColor : 255 - (OffsetColor - (255 - BackColor.R)),
                BackColor.G <= (255 - OffsetColor) ? BackColor.G + OffsetColor : 255 - (OffsetColor - (255 - BackColor.G)),
                BackColor.B <= (255 - OffsetColor) ? BackColor.B + OffsetColor : 255 - (OffsetColor - (255 - BackColor.B))
                );
            if (ActiveColor == null) ActiveColor = BackColor;
        }

        /// <summary>
        /// Событие выхода курсора из видимой облати объекта
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект информации о событии</param>
        private void ElementImage_MouseLeave(object sender, EventArgs e)
        {
            BackColor = DisactiveColor ?? Color.White;
        }

        /// <summary>
        /// Событие нажатия клавиши мыши над объектом
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект информации о событии</param>
        private void IELImageButton_MouseDown(object sender, MouseEventArgs e)
        {
            BackColor = Color.FromArgb(
                BackColor.R <= (255 - OffsetColor) ? BackColor.R + OffsetColor : 255 - (OffsetColor - (255 - BackColor.R)),
                BackColor.G <= (255 - OffsetColor) ? BackColor.G + OffsetColor : 255 - (OffsetColor - (255 - BackColor.G)),
                BackColor.B <= (255 - OffsetColor) ? BackColor.B + OffsetColor : 255 - (OffsetColor - (255 - BackColor.B))
                );
        }

        /// <summary>
        /// Событие отпускания клавиши мыши над объектом
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект информации о событии</param>
        private void IELImageButton_MouseUp(object sender, MouseEventArgs e)
        {
            BackColor = ActiveColor ?? Color.White;
        }
    }
}
