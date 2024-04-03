namespace AAC.GUI
{
    partial class IELHitCommand
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
            TextElement = new Label();
            SuspendLayout();
            // 
            // TextElement
            // 
            TextElement.AutoSize = true;
            TextElement.Location = new Point(0, 0);
            TextElement.Name = "TextElement";
            TextElement.Size = new Size(38, 15);
            TextElement.TabIndex = 0;
            TextElement.Text = "label1";
            TextElement.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // IELHitCommand
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(TextElement);
            Name = "IELHitCommand";
            Size = new Size(41, 18);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label TextElement;
    }
}
