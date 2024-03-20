namespace AAC.GUI
{
    partial class IELTheme
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
            Icon = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)Icon).BeginInit();
            SuspendLayout();
            // 
            // Description
            // 
            Description.BackColor = Color.FromArgb(65, 72, 89);
            Description.BorderStyle = BorderStyle.FixedSingle;
            Description.Cursor = Cursors.Hand;
            Description.ForeColor = Color.White;
            Description.Location = new Point(45, 26);
            Description.Name = "Description";
            Description.Size = new Size(72, 17);
            Description.TabIndex = 2;
            Description.Text = "Описание";
            Description.TextAlign = ContentAlignment.MiddleCenter;
            Description.MouseClick += Description_MouseClick;
            Description.MouseEnter += Description_MouseEnter;
            Description.MouseLeave += Description_MouseLeave;
            // 
            // ElementName
            // 
            ElementName.AutoEllipsis = true;
            ElementName.BackColor = Color.FromArgb(45, 52, 69);
            ElementName.BorderStyle = BorderStyle.FixedSingle;
            ElementName.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            ElementName.ForeColor = Color.White;
            ElementName.Location = new Point(45, 4);
            ElementName.Name = "ElementName";
            ElementName.Size = new Size(118, 18);
            ElementName.TabIndex = 1;
            ElementName.Tag = "";
            ElementName.Text = "Default";
            ElementName.TextAlign = ContentAlignment.TopCenter;
            ElementName.MouseClick += ElementTheme_DetectAction;
            // 
            // Icon
            // 
            Icon.BorderStyle = BorderStyle.FixedSingle;
            Icon.Location = new Point(2, 4);
            Icon.Name = "Icon";
            Icon.Size = new Size(40, 40);
            Icon.SizeMode = PictureBoxSizeMode.Zoom;
            Icon.TabIndex = 0;
            Icon.TabStop = false;
            Icon.MouseClick += ElementTheme_DetectAction;
            // 
            // IELTheme
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(45, 52, 69);
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(Description);
            Controls.Add(ElementName);
            Controls.Add(Icon);
            Name = "IELTheme";
            Size = new Size(166, 47);
            MouseClick += ElementTheme_DetectAction;
            ((System.ComponentModel.ISupportInitialize)Icon).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label Description;
        private Label ElementName;
        private PictureBox Icon;
    }
}
