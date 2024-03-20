namespace AAC.GUI
{
    partial class IELLogElement
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
            ActionButton = new Button();
            ElementText = new Label();
            SuspendLayout();
            // 
            // ActionButton
            // 
            ActionButton.ForeColor = Color.Black;
            ActionButton.Location = new Point(2, 3);
            ActionButton.Name = "ActionButton";
            ActionButton.Size = new Size(24, 20);
            ActionButton.TabIndex = 0;
            ActionButton.TabStop = false;
            ActionButton.Text = "...";
            ActionButton.UseVisualStyleBackColor = true;
            ActionButton.Click += ActionButton_Click;
            // 
            // ElementText
            // 
            ElementText.AutoEllipsis = true;
            ElementText.BackColor = Color.Gray;
            ElementText.BorderStyle = BorderStyle.FixedSingle;
            ElementText.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            ElementText.ForeColor = Color.Beige;
            ElementText.Location = new Point(3, 25);
            ElementText.Name = "ElementText";
            ElementText.Size = new Size(309, 23);
            ElementText.TabIndex = 0;
            ElementText.Text = "text";
            // 
            // IELLogElement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.Fixed3D;
            Controls.Add(ElementText);
            Controls.Add(ActionButton);
            Name = "IELLogElement";
            Size = new Size(314, 49);
            Resize += IELLogElement_Resize;
            ResumeLayout(false);
        }

        #endregion

        private Button ActionButton;
        private Label ElementText;
    }
}
