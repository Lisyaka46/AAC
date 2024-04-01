using AAC.Classes.DataClasses;
using static AAC.Classes.DataClasses.SettingsData;

namespace AAC.GUI
{
    public partial class SettingCheckBox : UserControl
    {
        /// <summary>
        /// Объект информации параметра
        /// </summary>
        private SettingsBoolParameter? BoolParameter_;

        /// <summary>
        /// объект параметра на который ссылается объект
        /// </summary>
        public SettingsBoolParameter? BoolParameter
        {
            get => BoolParameter_;
            set
            {
                if (value != null)
                {
                    BoolParameter_ = value;
                    Checked = BoolParameter_.Realy;
                    return;
                }
            }
        }

        /// <summary>
        /// Объект информации состояния параметра
        /// </summary>
        private bool Checked_;

        /// <summary>
        /// Свойство состояния параметра
        /// </summary>
        public bool Checked
        {
            get => Checked_;
            set
            {
                Checked_ = value;
                ElementCheckBox.Checked = value;
            }
        }

        /// <summary>
        /// Объект информации текста
        /// </summary>
        private string ElementText_;

        /// <summary>
        /// Объект текста параметра
        /// </summary>
        public string ElementText
        {
            get => ElementText_;
            set
            {
                ElementText_ = value;
                ElementCheckBox.Text = value;
                ChangeSize();
            }
        }

        /// <summary>
        /// Делегат события изменения состяния параметра
        /// </summary>
        /// <param name="State">Текущее состояние параметра</param>
        public delegate void EventCheckedChanged(bool State);

        /// <summary>
        /// Событие изменения состояния параметра
        /// </summary>
        public event EventCheckedChanged? CheckedChanged;

        public SettingCheckBox()
        {
            InitializeComponent();
            Font = new("Calibri", 12f);
            ElementCheckBox.CheckedChanged += (sender, e) =>
            {
                if (BoolParameter_ != null)
                {
                    BoolParameter_.Value = ElementCheckBox.Checked;
                    SetParamOption(BoolParameter_.Name, BoolParameter_.Value);
                    CheckedChanged?.Invoke(ElementCheckBox.Checked);
                }
                else throw new NullReferenceException("При изменении параметра ссылка является null");
            };
            FontChanged += (sender, e) => ChangeSize();
            ElementText = ElementCheckBox.Text;
            ElementText_ = ElementCheckBox.Text;
            ElementCheckBox.Location = new(1, 0);
            ChangeSize();
        }

        private void ChangeSize()
        {
            MaximumSize = ElementCheckBox.Size;
            MinimumSize = ElementCheckBox.Size;
            Size = ElementCheckBox.Size;
        }
    }
}
