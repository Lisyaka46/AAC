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
    public partial class IELLogElement : UserControl
    {
        //
        //public class 

        /// <summary>
        /// Делегат события нажатия на кнопку "События"
        /// </summary>
        public delegate void EventClickActionButton();

        /// <summary>
        /// Событие нажатия на кнопку "События"
        /// </summary>
        public event EventClickActionButton? ClickActionButton;

        private string PObjText { get; set; } = string.Empty;

        //
        public string ObjText
        {
            get { return PObjText; }
            set
            {
                PObjText = value;
                ElementText.Text = value;
            }
        }

        /// <summary>
        /// Инициализировать обычный объект журнала
        /// </summary>
        public IELLogElement()
        {
            InitializeComponent();
            ObjText = string.Empty;
            Size = new(Width, ElementText.Location.Y + ElementText.Height + 10);
            ElementText.Size = new(Width - 10, Height - ElementText.Location.Y - 10);
        }

        /// <summary>
        /// Инициализировать объект журнала со своим размером
        /// </summary>
        public IELLogElement(Size? SizeElement, Panel Parent, string Text, int Index)
        {
            InitializeComponent();
            if (SizeElement.HasValue) Size = SizeElement.Value;
            ElementText.Size = new(Width - 10, Height - ElementText.Location.Y - 10);
            this.Parent = Parent;
            ObjText = Text;
            Location = new(10, Index * 108 + 8);
        }

        /// <summary>
        /// Событие изменения размера объекта журнала
        /// </summary>
        /// <param name="sender">объект создавший событие</param>
        /// <param name="e">Объект самого события</param>
        private void IELLogElement_Resize(object sender, EventArgs e)
        {
            ElementText.Size = new(Width - 10, Height - ElementText.Location.Y - 10);
        }

        /// <summary>
        /// Событие нажатия на кнопку
        /// </summary>
        /// <param name="sender">объект создавший событие</param>
        /// <param name="e">Объект самого события</param>
        private void ActionButton_Click(object sender, EventArgs e)
        {
            ClickActionButton?.Invoke();
        }
    }
}
