namespace AAC.GUI
{
    partial class IELButtonMPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IELButtonMPanel));
            Button = new Label();
            lAltNum = new Label();
            pbArrow = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pbArrow).BeginInit();
            SuspendLayout();
            // 
            // Button
            // 
            Button.AutoEllipsis = true;
            Button.BackColor = Color.FromArgb(35, 40, 43);
            Button.BorderStyle = BorderStyle.FixedSingle;
            Button.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            Button.ForeColor = Color.White;
            Button.Location = new Point(16, 0);
            Button.Name = "Button";
            Button.Size = new Size(212, 23);
            Button.TabIndex = 1;
            Button.Text = "Text";
            Button.TextAlign = ContentAlignment.MiddleCenter;
            Button.UseMnemonic = false;
            // 
            // lAltNum
            // 
            lAltNum.AutoSize = true;
            lAltNum.BackColor = Color.FromArgb(35, 40, 43);
            lAltNum.BorderStyle = BorderStyle.FixedSingle;
            lAltNum.Font = new Font("Consolas", 9.6F, FontStyle.Bold);
            lAltNum.ForeColor = Color.White;
            lAltNum.Location = new Point(0, 3);
            lAltNum.Name = "lAltNum";
            lAltNum.Size = new Size(16, 17);
            lAltNum.TabIndex = 9;
            lAltNum.Text = "0";
            // 
            // pbArrow
            // 
            pbArrow.BackColor = Color.Black;
            pbArrow.Image = (Image)resources.GetObject("pbArrow.Image");
            pbArrow.Location = new Point(228, 2);
            pbArrow.Name = "pbArrow";
            pbArrow.Size = new Size(21, 21);
            pbArrow.SizeMode = PictureBoxSizeMode.Zoom;
            pbArrow.TabIndex = 10;
            pbArrow.TabStop = false;
            // 
            // IELButtonMPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColorButton = Color.Black;
            Controls.Add(lAltNum);
            Controls.Add(pbArrow);
            Controls.Add(Button);
            ForeColor = Color.White;
            MaximumSize = new Size(1000, 23);
            MinimumSize = new Size(75, 23);
            Name = "IELButtonMPanel";
            Size = new Size(251, 23);
            ((System.ComponentModel.ISupportInitialize)pbArrow).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lAltNum;
        private PictureBox pbArrow;
        private Label Button;
    }
}
