using static AAC.Forms_Functions;

namespace AAC.GUI
{
    public partial class IELButtonMPanel : UserControl
    {
        /// <summary>
        /// Делегат события активации кнопки
        /// </summary>
        /// <param name="KeyActivity">Вызвано ли с помощью нажатия клавиши</param>
        public delegate void EventActivateButton(bool KeyActivity);

        /// <summary>
        /// Событие активации кнопки
        /// </summary>
        public event EventActivateButton? ActivateButton;

        /// <summary>
        /// Символ клавиши
        /// </summary>
        private Keys AltIndexKey_ { get; set; }

        /// <summary>
        /// Свойство клавиши отвечающая за исполнение команды
        /// </summary>
        public Keys AltIndexKey
        {
            get { return AltIndexKey_; }
            set
            {
                lAltNum.Text = KeyToCharKeyboard(value);
                AltIndexKey_ = value;
            }
        }

        /// <summary>
        /// Состояние активности видимости переходного элемента
        /// </summary>
        private bool SwitchArrow_ { get; set; }

        /// <summary>
        /// Свойство активности переходного элемента
        /// </summary>
        public bool SwitchArrow
        {
            get { return SwitchArrow_; }
            set
            {
                SwitchArrow_ = value;
                pbArrow.Visible = value;
                EventResize();
            }
        }

        /// <summary>
        /// Состояние активности управления элементом с помощью клавиши
        /// </summary>
        private bool AltIndexActivity_ { get; set; }

        /// <summary>
        /// Свойство управления элементом с помощью клавиши
        /// </summary>
        public bool AltIndexActivity
        {
            get
            {
                return AltIndexActivity_;
            }
            set
            {
                AltIndexActivity_ = value;
                lAltNum.Visible = value;
                EventResize();
            }
        }

        /// <summary>
        /// Отображаемый текст в кнопке
        /// </summary>
        private string ButtonText_ { get; set; } = string.Empty;

        /// <summary>
        /// Свойство текста кнопки
        /// </summary>
        public string ButtonText
        {
            get
            {
                return ButtonText_;
            }
            set
            {
                ButtonText_ = value;
                Button.Text = value;
            }
        }

        /// <summary>
        /// Информационный объект цвета
        /// </summary>
        private Color BackColor_;

        /// <summary>
        /// Фоновый цвет объекта
        /// </summary>
        public new Color BackColor
        {
            get => BackColor_;
            set
            {
                BackColor_ = value;
                pbArrow.BackColor = value;
            }
        }

        /// <summary>
        /// Информационный объект цвета
        /// </summary>
        private Color BackColorButton_;

        /// <summary>
        /// Фоновый цвет объекта
        /// </summary>
        public Color BackColorButton
        {
            get => BackColorButton_;
            set
            {
                BackColorButton_ = value;
                lAltNum.BackColor = value;
                Button.BackColor = value;
            }
        }

        /// <summary>
        /// Информационный объект цвета
        /// </summary>
        private Color ForeColor_;

        /// <summary>
        /// Цвет текста объекта
        /// </summary>
        public new Color ForeColor
        {
            get => ForeColor_;
            set
            {
                ForeColor_ = value;
                lAltNum.ForeColor = value;
                Button.ForeColor = value;
            }
        }

        /// <summary>
        /// Инициализировать стандартный объект кнопки
        /// </summary>
        public IELButtonMPanel()
        {
            InitializeComponent();
            ButtonText = "Text";
            AltIndexKey = Keys.D0;
            SwitchArrow = true;
            AltIndexActivity = true;
            BackColorButton = Color.FromArgb(35, 40, 43);
            BackColor = Parent != null ? Parent.BackColor : Color.Transparent;
            Cursor = Cursors.Hand;
            Resize += (sender, e) => { EventResize(); };
            Button.MouseEnter += (sender, e) =>
            {
                BackColorButton = ColorWhile.SetOffsetColor(BackColorButton, 21);
                ForeColor = ColorWhile.SetOffsetColor(ForeColor, -90);
            };
            Button.MouseLeave += (sender, e) =>
            {
                BackColorButton = ColorWhile.SetOffsetColor(BackColorButton, -21);
                ForeColor = ColorWhile.SetOffsetColor(ForeColor, 90);
            };
            Button.Click += (sender, e) => { ActivateButton?.Invoke(false); };
            if (Parent != null) Parent.BackColorChanged += (sender, e) => BackColor = Parent.BackColor;
        }

        /// <summary>
        /// Перевести ключ клавиши в символ клавиатуры по
        ///   <b>!!русской раскладке с англ. символами!!</b>
        /// </summary>
        /// <param name="key">Ключ клавиши</param>
        /// <returns>Символ клавиатуры</returns>
        static string KeyToCharKeyboard(Keys key) => key switch
        {
            Keys.Oem2 => "/",
            Keys.Oem3 => "~",
            Keys.Oem4 => "[",
            Keys.Oem6 => "]",
            Keys.Oem7 => "\'",
            Keys.OemSemicolon => ";",
            Keys.OemMinus => "-",
            Keys.Oemplus => "=",
            Keys.Oemcomma => ",",
            Keys.OemPeriod => ".",
            Keys.OemPipe => "\\",
            _ => key.ToString()[^1].ToString()
        };

        /// <summary>
        /// Событие изменения размера объекта
        /// </summary>
        private void EventResize()
        {
            pbArrow.Location = new(Width - pbArrow.Width, 2);
            Button.Location = new(AltIndexActivity_ ? lAltNum.Width + lAltNum.Location.X : 0, Button.Location.Y);
            Button.Size = new(Width - Button.Location.X - (SwitchArrow_ ? pbArrow.Width : 0), Button.Height);
        }

        /// <summary>
        /// Инициализировать событие через конкретный объект кнопки
        /// </summary>
        /// <param name="KeyActivity">Состояние нажатой клавиши</param>
        /// <exception cref="Exception">Исключение при нулевом событии</exception>
        public void ActivateButtonMPanel(bool KeyActivity)
        {
            if (ActivateButton != null) ActivateButton.Invoke(KeyActivity);
            else throw new Exception($"Событие у элемента \"{Name}\" не создано событие выполняющее нажатие на клавишу \"ActivateButton\"");
        }
    }
}
