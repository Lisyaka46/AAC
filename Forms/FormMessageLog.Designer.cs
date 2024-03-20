namespace AAC
{
    partial class LogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pAllLogElements = new Panel();
            lScrollValue = new Label();
            pMainPanelLog = new Panel();
            vsbScrollLogElement = new VScrollBar();
            pIndormationObject = new Panel();
            lRowColumn = new Label();
            lFunctionRealize = new Label();
            lNameObject = new Label();
            lActiveDir = new Label();
            pMainPanelLog.SuspendLayout();
            pIndormationObject.SuspendLayout();
            SuspendLayout();
            // 
            // pAllLogElements
            // 
            pAllLogElements.Location = new Point(3, 3);
            pAllLogElements.Name = "pAllLogElements";
            pAllLogElements.Size = new Size(592, 432);
            pAllLogElements.TabIndex = 0;
            // 
            // lScrollValue
            // 
            lScrollValue.AutoSize = true;
            lScrollValue.Location = new Point(648, 164);
            lScrollValue.Name = "lScrollValue";
            lScrollValue.Size = new Size(13, 15);
            lScrollValue.TabIndex = 1;
            lScrollValue.Text = "0";
            // 
            // pMainPanelLog
            // 
            pMainPanelLog.BorderStyle = BorderStyle.FixedSingle;
            pMainPanelLog.Controls.Add(vsbScrollLogElement);
            pMainPanelLog.Controls.Add(pAllLogElements);
            pMainPanelLog.Location = new Point(12, 12);
            pMainPanelLog.Name = "pMainPanelLog";
            pMainPanelLog.Size = new Size(625, 440);
            pMainPanelLog.TabIndex = 2;
            // 
            // vsbScrollLogElement
            // 
            vsbScrollLogElement.LargeChange = 1;
            vsbScrollLogElement.Location = new Point(606, -1);
            vsbScrollLogElement.Name = "vsbScrollLogElement";
            vsbScrollLogElement.Size = new Size(17, 439);
            vsbScrollLogElement.TabIndex = 1;
            vsbScrollLogElement.ValueChanged += ScrollLogElementValueChanged;
            // 
            // pIndormationObject
            // 
            pIndormationObject.BackColor = Color.DarkSeaGreen;
            pIndormationObject.BorderStyle = BorderStyle.Fixed3D;
            pIndormationObject.Controls.Add(lRowColumn);
            pIndormationObject.Controls.Add(lFunctionRealize);
            pIndormationObject.Controls.Add(lNameObject);
            pIndormationObject.Location = new Point(643, 57);
            pIndormationObject.Name = "pIndormationObject";
            pIndormationObject.Size = new Size(529, 94);
            pIndormationObject.TabIndex = 3;
            // 
            // lRowColumn
            // 
            lRowColumn.AutoSize = true;
            lRowColumn.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lRowColumn.ForeColor = Color.Black;
            lRowColumn.Location = new Point(3, 38);
            lRowColumn.Name = "lRowColumn";
            lRowColumn.Size = new Size(96, 15);
            lRowColumn.TabIndex = 2;
            lRowColumn.Text = "Строка-Столбец";
            // 
            // lFunctionRealize
            // 
            lFunctionRealize.AutoSize = true;
            lFunctionRealize.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lFunctionRealize.ForeColor = Color.Black;
            lFunctionRealize.Location = new Point(3, 20);
            lFunctionRealize.Name = "lFunctionRealize";
            lFunctionRealize.Size = new Size(117, 15);
            lFunctionRealize.TabIndex = 1;
            lFunctionRealize.Text = "Вызван из функции";
            // 
            // lNameObject
            // 
            lNameObject.AutoSize = true;
            lNameObject.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lNameObject.ForeColor = Color.Black;
            lNameObject.Location = new Point(3, 2);
            lNameObject.Name = "lNameObject";
            lNameObject.Size = new Size(78, 15);
            lNameObject.TabIndex = 0;
            lNameObject.Text = "Имя объекта";
            // 
            // lActiveDir
            // 
            lActiveDir.AutoEllipsis = true;
            lActiveDir.BorderStyle = BorderStyle.FixedSingle;
            lActiveDir.Location = new Point(643, 13);
            lActiveDir.Name = "lActiveDir";
            lActiveDir.Size = new Size(529, 40);
            lActiveDir.TabIndex = 4;
            lActiveDir.Text = "D: <>";
            // 
            // LogForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1184, 461);
            Controls.Add(lActiveDir);
            Controls.Add(pIndormationObject);
            Controls.Add(pMainPanelLog);
            Controls.Add(lScrollValue);
            ForeColor = Color.YellowGreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimumSize = new Size(600, 250);
            Name = "LogForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LOG";
            FormClosing += LogForm_FormClosing;
            pMainPanelLog.ResumeLayout(false);
            pIndormationObject.ResumeLayout(false);
            pIndormationObject.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        public Panel pAllLogElements;
        private Label lScrollValue;
        private Panel pMainPanelLog;
        private Panel pIndormationObject;
        private VScrollBar vsbScrollLogElement;
        private Label lFunctionRealize;
        private Label lNameObject;
        private Label lRowColumn;
        private Label lActiveDir;
    }
}