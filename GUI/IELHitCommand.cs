using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AAC.GUI
{
    public partial class IELHitCommand : UserControl
    {
        /// <summary>
        /// Номер к которому относится элемент
        /// </summary>
        private int Number_ = 0;

        /// <summary>
        /// Свойство номера к которому относится элемент
        /// </summary>
        public int Number
        {
            get => Number_;
            set
            {
                TextElement.Text = $"{value}. {ElementText_}";
                Number_ = value;
            }
        }

        /// <summary>
        /// Текст элемента
        /// </summary>
        private string ElementText_ = string.Empty;

        /// <summary>
        /// Свойство текста элемента
        /// </summary>
        public string ElementText
        {
            get => ElementText_;
            set
            {
                TextElement.Text = $"{Number}. {value}";
                ElementText_ = value;
            }
        }

        /// <summary>
        /// Используемый цвет при активном курсоре элемента
        /// </summary>
        public Color ActiveColorComponent { get; private set; }

        /// <summary>
        /// Используемый цвет при не активном курсоре элемента
        /// </summary>
        private Color DiactivateColorComponent_ = Color.Black;

        /// <summary>
        /// Свойство цвета не активного курсора элемента
        /// </summary>
        public Color DiactivateColorComponent
        {
            get => DiactivateColorComponent_;
            set
            {
                DiactivateColorComponent_ = value;
                ActiveColorComponent = Color.FromArgb(value.R + (value.R <= 150 ? 55 : -55), value.G + (value.G <= 150 ? 55 : -55), value.B + (value.B <= 150 ? 55 : -55));
                BackColor = value;
            }
        }

        /// <summary>
        /// Делегат события активации элемента
        /// </summary>
        public delegate void EventClickActivateElement();

        /// <summary>
        /// Объект события активации элемента
        /// </summary>
        public event EventClickActivateElement? ClickActivateElement;

        /// <summary>
        /// Инициализировать пустой объект подсказки к команде
        /// </summary>
        public IELHitCommand()
        {
            InitializeComponent();
            DiactivateColorComponent = Color.Aqua;
            TextElement.BackColor = DiactivateColorComponent;
            Number = 0;
            ElementText = string.Empty;
            TextElement.TextAlign = ContentAlignment.MiddleCenter;
            Font = new("Arial", 9.75f, FontStyle.Bold);
            BorderStyle = BorderStyle.FixedSingle;
            Cursor = Cursors.Hand;
            TextElement.MouseEnter += (sender, e) =>
            {
                BackColor = ActiveColorComponent;
                TextElement.BackColor = ActiveColorComponent;
            };
            TextElement.MouseLeave += (sender, e) =>
            {
                BackColor = DiactivateColorComponent;
                TextElement.BackColor = DiactivateColorComponent;
            };
            TextElement.Click += (sender, e) => ClickActivateElement?.Invoke();
            Click += (sender, e) => ClickActivateElement?.Invoke();
            TextElement.SizeChanged += (sender, e) => Size = new(TextElement.Width, TextElement.Height + 2);
        }

        /// <summary>
        /// Инициализировать объект подсказки к команде
        /// </summary>
        /// <param name="Parent">Панель в которой может находится элемент</param>
        /// <param name="ColorComponent">Цвет фона элемента</param>
        /// <param name="Index">Номер элемента</param>
        /// <param name="Text">Текст отображаемый для подсказки</param>
        public IELHitCommand(Panel? Parent = null, KnownColor ColorComponent = KnownColor.Black, int Index = 0, string Text = "Text")
        {
            InitializeComponent();
            DiactivateColorComponent = Color.FromKnownColor(ColorComponent);
            TextElement.BackColor = DiactivateColorComponent;
            Number = Index;
            ElementText = Text;
            this.Parent = Parent;
            TextElement.TextAlign = ContentAlignment.MiddleCenter;
            TextElement.ForeColor = Color.White;
            Font = new("Arial", 9.75f, FontStyle.Bold);
            BorderStyle = BorderStyle.FixedSingle;
            Cursor = Cursors.Hand;
            TextElement.MouseEnter += (sender, e) =>
            {
                BackColor = ActiveColorComponent;
                TextElement.BackColor = ActiveColorComponent;
            };
            TextElement.MouseLeave += (sender, e) =>
            {
                BackColor = DiactivateColorComponent;
                TextElement.BackColor = DiactivateColorComponent;
            };
            TextElement.Click += (sender, e) => ClickActivateElement?.Invoke();
            Click += (sender, e) => ClickActivateElement?.Invoke();
            TextElement.SizeChanged += (sender, e) => Size = new(TextElement.Width, TextElement.Height + 2);
        }

        /// <summary>
        /// Установить активирующий цвет на элемент
        /// </summary>
        public void ActivateColor()
        {
            BackColor = ActiveColorComponent;
            TextElement.BackColor = ActiveColorComponent;
        }

        /// <summary>
        /// Установить не активный цвет на элемент
        /// </summary>
        public void DiactivateColor()
        {
            BackColor = DiactivateColorComponent;
            TextElement.BackColor = DiactivateColorComponent;
        }
    }
}
