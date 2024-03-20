namespace AAC.GUI
{
    partial class IELLabelInformation
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
            ElementText = new Label();
            SuspendLayout();
            // 
            // ElementText
            // 
            ElementText.AutoSize = true;
            ElementText.Location = new Point(0, 0);
            ElementText.Name = "ElementText";
            ElementText.Size = new Size(98, 15);
            ElementText.TabIndex = 0;
            ElementText.Text = "Текст пояснения";
            // 
            // IELLabelInformation
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ElementText);
            Name = "IELLabelInformation";
            Size = new Size(100, 17);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label ElementText;
    }
}
