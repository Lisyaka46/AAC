namespace AAC.GUI
{
    partial class IELLabelAccess
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
            NameLabel = new Label();
            NumLabel = new Label();
            IconElement = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)IconElement).BeginInit();
            SuspendLayout();
            // 
            // NameLabel
            // 
            NameLabel.BackColor = Color.Transparent;
            NameLabel.Font = new Font("Arial Narrow", 8F);
            NameLabel.ForeColor = Color.White;
            NameLabel.Location = new Point(0, 37);
            NameLabel.Name = "NameLabel";
            NameLabel.Size = new Size(52, 12);
            NameLabel.TabIndex = 1;
            NameLabel.Text = "123456789";
            NameLabel.TextAlign = ContentAlignment.MiddleCenter;
            NameLabel.MouseClick += IconElement_MouseClick;
            // 
            // NumLabel
            // 
            NumLabel.BackColor = Color.Transparent;
            NumLabel.Font = new Font("Arial Black", 6.75F, FontStyle.Bold);
            NumLabel.ForeColor = Color.White;
            NumLabel.Location = new Point(1, 0);
            NumLabel.Name = "NumLabel";
            NumLabel.Size = new Size(53, 11);
            NumLabel.TabIndex = 2;
            NumLabel.Text = "123456";
            NumLabel.TextAlign = ContentAlignment.MiddleRight;
            NumLabel.MouseClick += IconElement_MouseClick;
            // 
            // IconElement
            // 
            IconElement.Location = new Point(1, 13);
            IconElement.Name = "IconElement";
            IconElement.Size = new Size(51, 24);
            IconElement.SizeMode = PictureBoxSizeMode.Zoom;
            IconElement.TabIndex = 0;
            IconElement.TabStop = false;
            IconElement.MouseClick += IconElement_MouseClick;
            IconElement.MouseEnter += IconElement_MouseEnter;
            IconElement.MouseLeave += IconElement_MouseLeave;
            // 
            // IELLabelAccess
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            Controls.Add(NameLabel);
            Controls.Add(IconElement);
            Controls.Add(NumLabel);
            Name = "IELLabelAccess";
            Size = new Size(52, 52);
            MouseClick += IconElement_MouseClick;
            ((System.ComponentModel.ISupportInitialize)IconElement).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Label NameLabel;
        private Label NumLabel;
        private PictureBox IconElement;
    }
}
