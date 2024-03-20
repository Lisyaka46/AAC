namespace AAC.GUI
{
    partial class IELBuffer
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
            pElements = new Panel();
            lInfoZeroCommandBuffer = new Label();
            ScrollBar = new VScrollBar();
            pElements.SuspendLayout();
            SuspendLayout();
            // 
            // pElements
            // 
            pElements.BackColor = Color.Black;
            pElements.BorderStyle = BorderStyle.FixedSingle;
            pElements.Controls.Add(lInfoZeroCommandBuffer);
            pElements.ForeColor = Color.White;
            pElements.Location = new Point(-1, -1);
            pElements.Name = "pElements";
            pElements.Size = new Size(131, 150);
            pElements.TabIndex = 0;
            // 
            // lInfoZeroCommandBuffer
            // 
            lInfoZeroCommandBuffer.AutoSize = true;
            lInfoZeroCommandBuffer.ForeColor = SystemColors.AppWorkspace;
            lInfoZeroCommandBuffer.Location = new Point(9, 9);
            lInfoZeroCommandBuffer.Name = "lInfoZeroCommandBuffer";
            lInfoZeroCommandBuffer.Size = new Size(111, 30);
            lInfoZeroCommandBuffer.TabIndex = 0;
            lInfoZeroCommandBuffer.Text = "Не выполнено\r\nни одной команды";
            lInfoZeroCommandBuffer.TextAlign = ContentAlignment.TopCenter;
            // 
            // ScrollBar
            // 
            ScrollBar.LargeChange = 1;
            ScrollBar.Location = new Point(131, 0);
            ScrollBar.Maximum = 50;
            ScrollBar.Name = "ScrollBar";
            ScrollBar.Size = new Size(17, 150);
            ScrollBar.TabIndex = 1;
            // 
            // IELBuffer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(ScrollBar);
            Controls.Add(pElements);
            MinimumSize = new Size(0, 41);
            Name = "IELBuffer";
            Size = new Size(148, 148);
            SizeChanged += IELBuffer_SizeChanged;
            pElements.ResumeLayout(false);
            pElements.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pElements;
        private VScrollBar ScrollBar;
        private Label lInfoZeroCommandBuffer;
    }
}
