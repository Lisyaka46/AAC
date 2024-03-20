using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AAC.GUI
{
    public partial class SettingCheckBox : UserControl
    {
        /// <summary>
        /// Объект информации параметра
        /// </summary>
        private SettingsData.SettingsBoolParameter? BoolParameter_;

        /// <summary>
        /// объект параметра на который ссылается объект
        /// </summary>
        public SettingsData.SettingsBoolParameter? BoolParameter
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
                    Startcs.MainData.Settings.SetParamOption(BoolParameter_.Name, BoolParameter_.Value);
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
