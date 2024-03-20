namespace AAC.GUI
{
    partial class SettingCheckBox
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
            ElementCheckBox = new CheckBox();
            SuspendLayout();
            // 
            // ElementCheckBox
            // 
            ElementCheckBox.AutoSize = true;
            ElementCheckBox.Location = new Point(1, 0);
            ElementCheckBox.Name = "ElementCheckBox";
            ElementCheckBox.Size = new Size(83, 19);
            ElementCheckBox.TabIndex = 0;
            ElementCheckBox.Text = "checkBox1";
            ElementCheckBox.UseVisualStyleBackColor = true;
            // 
            // SettingCheckBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(ElementCheckBox);
            Name = "SettingCheckBox";
            Size = new Size(84, 21);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox ElementCheckBox;
    }
}
