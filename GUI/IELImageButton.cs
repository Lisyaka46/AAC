namespace AAC.GUI
{
    public partial class IELImageButton : UserControl
    {
        /// <summary>
        /// Индекс состояния
        /// </summary>
        public int IndexState { get; private set; }

        /// <summary>
        /// Неактивные изображения при состояниях
        /// </summary>
        public List<Image> ImageMouseLeave { get; set; }

        /// <summary>
        /// Активные изображения при состояниях
        /// </summary>
        public List<Image> ImageMouseEnter { get; set; }

        /// <summary>
        /// Оффсет цвета
        /// </summary>
        public readonly int OffsetColor;

        /// <summary>
        /// Инициализировать объект кнопки
        /// </summary>
        public IELImageButton()
        {
            InitializeComponent();
            pb.Size = Size;
            IndexState = 0;
            ImageMouseLeave = [];
            ImageMouseEnter = [];
            OffsetColor = 20;
        }

        /// <summary>
        /// Событие входа курсора в видимую облать объекта
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект информации о событии</param>
        private void ElementImage_MouseEnter(object sender, EventArgs e)
        {
            if (IndexState < ImageMouseEnter.Count)
            {
                pb.Image = ImageMouseEnter[IndexState];
            }
            else throw new Exception($"Для данного состояния \"{IndexState}\" активное изображение не найдено");
        }

        /// <summary>
        /// Событие выхода курсора из видимой облати объекта
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект информации о событии</param>
        private void ElementImage_MouseLeave(object sender, EventArgs e)
        {
            if (IndexState < ImageMouseLeave.Count)
            {
                pb.Image = ImageMouseLeave[IndexState];
            }
            else throw new Exception($"Для данного состояния \"{IndexState}\" неактивное изображение не найдено");
        }

        /// <summary>
        /// Событие нажатия клавиши мыши над объектом
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект информации о событии</param>
        private void IELImageButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (IndexState < ImageMouseLeave.Count)
            {
                pb.Image = ImageMouseLeave[IndexState];
            }
            else throw new Exception($"Для данного состояния \"{IndexState}\" неактивное изображение не найдено");
        }

        /// <summary>
        /// Событие отпускания клавиши мыши над объектом
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект информации о событии</param>
        private void IELImageButton_MouseUp(object sender, MouseEventArgs e)
        {
            IndexState = IndexState < ImageMouseEnter.Count - 1 ? IndexState + 1 : 0;
            pb.Image = ImageMouseEnter[IndexState];
        }
    }
}
