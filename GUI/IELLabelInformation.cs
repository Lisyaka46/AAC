namespace AAC.GUI
{
    public partial class IELLabelInformation : UserControl
    {
        private string _InformationText;
        /// <summary>
        /// Текст который визуализирует элемент
        /// </summary>
        public string InformationText
        {
            get { return _InformationText; }
            set
            {
                _InformationText = value;
                ElementText.Text = InformationText;
                ElementText.Location = new(-1, 0);
                Size = new(ElementText.Width + 2, ElementText.Height);

            }
        }

        /// <summary>
        /// Инициализировать обычный элемент пояснения
        /// </summary>
        public IELLabelInformation()
        {
            InitializeComponent();
            _InformationText = string.Empty;
            InformationText = "Текст пояснения";
        }

        /// <summary>
        /// Инициализировать свой элемент пояснения
        /// </summary>
        /// <param name="Text">Добавляемый текст пояснения</param>
        public IELLabelInformation(string Text)
        {
            InitializeComponent();
            _InformationText = string.Empty;
            InformationText = Text;
        }
    }
}
