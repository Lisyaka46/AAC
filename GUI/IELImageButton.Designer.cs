namespace AAC.GUI
{
    partial class IELImageButton
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
            pb = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pb).BeginInit();
            SuspendLayout();
            // 
            // pb
            // 
            pb.Location = new Point(0, 0);
            pb.Name = "pb";
            pb.Size = new Size(100, 50);
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.TabIndex = 0;
            pb.TabStop = false;
            // 
            // IELImageButton
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pb);
            Cursor = Cursors.Hand;
            Name = "IELImageButton";
            Size = new Size(50, 50);
            MouseDown += IELImageButton_MouseDown;
            MouseEnter += ElementImage_MouseEnter;
            MouseLeave += ElementImage_MouseLeave;
            MouseUp += IELImageButton_MouseUp;
            ((System.ComponentModel.ISupportInitialize)pb).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pb;
    }
}
