namespace AAC.GUI
{
    partial class IELParamColorTheme
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            Description = new Label();
            ElementName = new Label();
            ElementColor = new Label();
            SuspendLayout();
            // 
            // Description
            // 
            Description.AutoEllipsis = true;
            Description.BackColor = Color.Transparent;
            Description.ForeColor = Color.Cornsilk;
            Description.Location = new Point(63, 29);
            Description.Name = "Description";
            Description.Size = new Size(476, 29);
            Description.TabIndex = 2;
            Description.Text = "Описание элемента\r\n#1";
            Description.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ElementName
            // 
            ElementName.BackColor = Color.DimGray;
            ElementName.BorderStyle = BorderStyle.FixedSingle;
            ElementName.FlatStyle = FlatStyle.Popup;
            ElementName.Font = new Font("Segoe UI Symbol", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            ElementName.ForeColor = Color.White;
            ElementName.Location = new Point(61, 6);
            ElementName.Name = "ElementName";
            ElementName.Size = new Size(479, 23);
            ElementName.TabIndex = 1;
            ElementName.Text = "Имя Параметра";
            ElementName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ElementColor
            // 
            ElementColor.BackColor = Color.Black;
            ElementColor.BorderStyle = BorderStyle.FixedSingle;
            ElementColor.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            ElementColor.ForeColor = Color.White;
            ElementColor.Location = new Point(4, 6);
            ElementColor.Name = "ElementColor";
            ElementColor.Size = new Size(53, 53);
            ElementColor.TabIndex = 0;
            ElementColor.Text = "Null";
            ElementColor.TextAlign = ContentAlignment.MiddleCenter;
            ElementColor.DoubleClick += ElementColor_DoubleClick;
            ElementColor.MouseEnter += ElementColor_MouseDetectCurcor;
            ElementColor.MouseLeave += ElementColor_MouseDetectCurcor;
            // 
            // IELParamColorTheme
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(35, 42, 59);
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(ElementName);
            Controls.Add(Description);
            Controls.Add(ElementColor);
            Name = "IELParamColorTheme";
            Size = new Size(544, 65);
            ResumeLayout(false);
        }

        #endregion

        private Label Description;
        private Label ElementName;
        private Label ElementColor;
    }
}
