namespace AAC
{
    partial class FormExplanationCommands
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExplanationCommands));
            INameCommand = new Label();
            bDefaultCommand = new Button();
            bAllVoiceCommand = new Button();
            bSoftCommand = new Button();
            lExplanation = new Label();
            lInfoComplete = new Label();
            bListUp = new Button();
            bListDown = new Button();
            lPositionInformationCommand = new Label();
            bCopyCommand = new Button();
            tbSearch = new TextBox();
            cbCommands = new ComboBox();
            bSearch = new Button();
            bItemUpdate = new Button();
            lExampleWriteCommand = new Label();
            lbSeachCommands = new ListBox();
            bClearSearch = new Button();
            lNameSearch = new Label();
            SuspendLayout();
            // 
            // INameCommand
            // 
            INameCommand.AutoEllipsis = true;
            INameCommand.BackColor = Color.Black;
            INameCommand.BorderStyle = BorderStyle.FixedSingle;
            INameCommand.Font = new Font("Arial", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 204);
            INameCommand.ForeColor = Color.LightGreen;
            INameCommand.Location = new Point(4, 61);
            INameCommand.Margin = new Padding(4, 0, 4, 0);
            INameCommand.Name = "INameCommand";
            INameCommand.Size = new Size(191, 24);
            INameCommand.TabIndex = 0;
            INameCommand.Text = "Command";
            INameCommand.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // bDefaultCommand
            // 
            bDefaultCommand.BackColor = Color.Black;
            bDefaultCommand.Cursor = Cursors.Hand;
            bDefaultCommand.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            bDefaultCommand.ForeColor = Color.DarkGray;
            bDefaultCommand.Location = new Point(5, 6);
            bDefaultCommand.Margin = new Padding(4, 3, 4, 3);
            bDefaultCommand.Name = "bDefaultCommand";
            bDefaultCommand.Size = new Size(96, 42);
            bDefaultCommand.TabIndex = 2;
            bDefaultCommand.Text = "Консольные команды";
            bDefaultCommand.UseVisualStyleBackColor = false;
            bDefaultCommand.MouseDown += ButtonMouseDown;
            bDefaultCommand.MouseEnter += ButtonMouseEnter;
            bDefaultCommand.MouseLeave += ButtonMouseLeave;
            bDefaultCommand.MouseMove += ButtonMouseMove;
            bDefaultCommand.MouseUp += ButtonMouseUp;
            // 
            // bAllVoiceCommand
            // 
            bAllVoiceCommand.BackColor = Color.Black;
            bAllVoiceCommand.Cursor = Cursors.Hand;
            bAllVoiceCommand.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            bAllVoiceCommand.ForeColor = Color.DarkGray;
            bAllVoiceCommand.Location = new Point(109, 6);
            bAllVoiceCommand.Margin = new Padding(4, 3, 4, 3);
            bAllVoiceCommand.Name = "bAllVoiceCommand";
            bAllVoiceCommand.Size = new Size(96, 42);
            bAllVoiceCommand.TabIndex = 3;
            bAllVoiceCommand.Text = "Голосовые команды";
            bAllVoiceCommand.UseVisualStyleBackColor = false;
            bAllVoiceCommand.MouseDown += ButtonMouseDown;
            bAllVoiceCommand.MouseEnter += ButtonMouseEnter;
            bAllVoiceCommand.MouseLeave += ButtonMouseLeave;
            bAllVoiceCommand.MouseMove += ButtonMouseMove;
            bAllVoiceCommand.MouseUp += ButtonMouseUp;
            // 
            // bSoftCommand
            // 
            bSoftCommand.BackColor = Color.Black;
            bSoftCommand.Cursor = Cursors.Hand;
            bSoftCommand.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            bSoftCommand.ForeColor = Color.DarkGray;
            bSoftCommand.Location = new Point(213, 6);
            bSoftCommand.Margin = new Padding(4, 3, 4, 3);
            bSoftCommand.Name = "bSoftCommand";
            bSoftCommand.Size = new Size(96, 42);
            bSoftCommand.TabIndex = 4;
            bSoftCommand.Text = "Созданные команды";
            bSoftCommand.UseVisualStyleBackColor = false;
            bSoftCommand.MouseDown += ButtonMouseDown;
            bSoftCommand.MouseEnter += ButtonMouseEnter;
            bSoftCommand.MouseLeave += ButtonMouseLeave;
            bSoftCommand.MouseMove += ButtonMouseMove;
            bSoftCommand.MouseUp += ButtonMouseUp;
            // 
            // lExplanation
            // 
            lExplanation.AutoEllipsis = true;
            lExplanation.BackColor = Color.Black;
            lExplanation.BorderStyle = BorderStyle.Fixed3D;
            lExplanation.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lExplanation.ForeColor = Color.FloralWhite;
            lExplanation.Location = new Point(4, 88);
            lExplanation.Margin = new Padding(4, 0, 4, 0);
            lExplanation.Name = "lExplanation";
            lExplanation.Size = new Size(792, 81);
            lExplanation.TabIndex = 6;
            lExplanation.Text = "Command";
            // 
            // lInfoComplete
            // 
            lInfoComplete.AutoSize = true;
            lInfoComplete.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lInfoComplete.Location = new Point(520, 33);
            lInfoComplete.Margin = new Padding(4, 0, 4, 0);
            lInfoComplete.Name = "lInfoComplete";
            lInfoComplete.Size = new Size(74, 15);
            lInfoComplete.TabIndex = 7;
            lInfoComplete.Text = "Выполнено";
            lInfoComplete.Click += LInfoComplete_Click;
            // 
            // bListUp
            // 
            bListUp.Cursor = Cursors.Hand;
            bListUp.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            bListUp.ForeColor = SystemColors.ActiveCaptionText;
            bListUp.Location = new Point(317, 28);
            bListUp.Margin = new Padding(4, 3, 4, 3);
            bListUp.Name = "bListUp";
            bListUp.Size = new Size(93, 23);
            bListUp.TabIndex = 8;
            bListUp.Text = "Сл. команда";
            bListUp.UseVisualStyleBackColor = true;
            // 
            // bListDown
            // 
            bListDown.Cursor = Cursors.Hand;
            bListDown.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            bListDown.ForeColor = SystemColors.ActiveCaptionText;
            bListDown.Location = new Point(317, 4);
            bListDown.Margin = new Padding(4, 3, 4, 3);
            bListDown.Name = "bListDown";
            bListDown.Size = new Size(93, 23);
            bListDown.TabIndex = 9;
            bListDown.Text = "Пр. команда";
            bListDown.UseVisualStyleBackColor = true;
            // 
            // lPositionInformationCommand
            // 
            lPositionInformationCommand.AutoEllipsis = true;
            lPositionInformationCommand.Font = new Font("Segoe UI Symbol", 9F, FontStyle.Bold);
            lPositionInformationCommand.Location = new Point(650, 4);
            lPositionInformationCommand.Margin = new Padding(4, 0, 4, 0);
            lPositionInformationCommand.Name = "lPositionInformationCommand";
            lPositionInformationCommand.Size = new Size(149, 23);
            lPositionInformationCommand.TabIndex = 10;
            lPositionInformationCommand.Text = "XXXXXXXX/XXXXXXXX";
            lPositionInformationCommand.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // bCopyCommand
            // 
            bCopyCommand.BackColor = Color.Black;
            bCopyCommand.Cursor = Cursors.Hand;
            bCopyCommand.ForeColor = Color.DarkGray;
            bCopyCommand.Location = new Point(663, 61);
            bCopyCommand.Margin = new Padding(4, 3, 4, 3);
            bCopyCommand.Name = "bCopyCommand";
            bCopyCommand.Size = new Size(133, 24);
            bCopyCommand.TabIndex = 11;
            bCopyCommand.Text = "Копировать команду";
            bCopyCommand.UseVisualStyleBackColor = false;
            bCopyCommand.MouseDown += ButtonMouseDown;
            bCopyCommand.MouseEnter += ButtonMouseEnter;
            bCopyCommand.MouseLeave += ButtonMouseLeave;
            bCopyCommand.MouseUp += ButtonMouseUp;
            // 
            // tbSearch
            // 
            tbSearch.BackColor = Color.Black;
            tbSearch.ForeColor = Color.DimGray;
            tbSearch.Location = new Point(528, 304);
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new Size(194, 23);
            tbSearch.TabIndex = 14;
            tbSearch.Text = "Поиск элемента";
            tbSearch.TextAlign = HorizontalAlignment.Center;
            // 
            // cbCommands
            // 
            cbCommands.BackColor = Color.Black;
            cbCommands.Cursor = Cursors.Hand;
            cbCommands.ForeColor = Color.White;
            cbCommands.FormattingEnabled = true;
            cbCommands.ImeMode = ImeMode.On;
            cbCommands.Location = new Point(419, 4);
            cbCommands.Name = "cbCommands";
            cbCommands.Size = new Size(228, 23);
            cbCommands.TabIndex = 16;
            // 
            // bSearch
            // 
            bSearch.BackColor = Color.Black;
            bSearch.Cursor = Cursors.Hand;
            bSearch.ForeColor = Color.DarkGray;
            bSearch.Location = new Point(728, 303);
            bSearch.Name = "bSearch";
            bSearch.Size = new Size(69, 25);
            bSearch.TabIndex = 17;
            bSearch.Text = "Искать";
            bSearch.UseVisualStyleBackColor = false;
            bSearch.MouseDown += ButtonMouseDown;
            bSearch.MouseEnter += ButtonMouseEnter;
            bSearch.MouseLeave += ButtonMouseLeave;
            bSearch.MouseUp += ButtonMouseUp;
            // 
            // bItemUpdate
            // 
            bItemUpdate.BackColor = Color.Black;
            bItemUpdate.Cursor = Cursors.Hand;
            bItemUpdate.ForeColor = Color.DarkGray;
            bItemUpdate.Location = new Point(419, 27);
            bItemUpdate.Name = "bItemUpdate";
            bItemUpdate.Size = new Size(97, 25);
            bItemUpdate.TabIndex = 18;
            bItemUpdate.Text = "Активировать";
            bItemUpdate.UseVisualStyleBackColor = false;
            bItemUpdate.MouseDown += ButtonMouseDown;
            bItemUpdate.MouseEnter += ButtonMouseEnter;
            bItemUpdate.MouseLeave += ButtonMouseLeave;
            bItemUpdate.MouseUp += ButtonMouseUp;
            // 
            // lExampleWriteCommand
            // 
            lExampleWriteCommand.BackColor = Color.Black;
            lExampleWriteCommand.BorderStyle = BorderStyle.FixedSingle;
            lExampleWriteCommand.Font = new Font("Arial", 9.75F, FontStyle.Bold);
            lExampleWriteCommand.ForeColor = Color.FloralWhite;
            lExampleWriteCommand.Location = new Point(201, 62);
            lExampleWriteCommand.Margin = new Padding(4, 0, 4, 0);
            lExampleWriteCommand.Name = "lExampleWriteCommand";
            lExampleWriteCommand.Size = new Size(456, 23);
            lExampleWriteCommand.TabIndex = 19;
            lExampleWriteCommand.Text = "Command";
            lExampleWriteCommand.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lbSeachCommands
            // 
            lbSeachCommands.BackColor = Color.Black;
            lbSeachCommands.ForeColor = Color.White;
            lbSeachCommands.FormattingEnabled = true;
            lbSeachCommands.ItemHeight = 15;
            lbSeachCommands.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" });
            lbSeachCommands.Location = new Point(335, 203);
            lbSeachCommands.Name = "lbSeachCommands";
            lbSeachCommands.Size = new Size(461, 94);
            lbSeachCommands.TabIndex = 20;
            // 
            // bClearSearch
            // 
            bClearSearch.BackColor = Color.Black;
            bClearSearch.Cursor = Cursors.Hand;
            bClearSearch.ForeColor = Color.DarkGray;
            bClearSearch.Location = new Point(335, 302);
            bClearSearch.Name = "bClearSearch";
            bClearSearch.Size = new Size(76, 25);
            bClearSearch.TabIndex = 21;
            bClearSearch.Text = "Очистить";
            bClearSearch.UseVisualStyleBackColor = false;
            // 
            // lNameSearch
            // 
            lNameSearch.AutoSize = true;
            lNameSearch.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lNameSearch.Location = new Point(333, 186);
            lNameSearch.Name = "lNameSearch";
            lNameSearch.Size = new Size(47, 16);
            lNameSearch.TabIndex = 22;
            lNameSearch.Text = "Поиск:";
            // 
            // FormExplanationCommands
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(800, 356);
            Controls.Add(bClearSearch);
            Controls.Add(lbSeachCommands);
            Controls.Add(lExampleWriteCommand);
            Controls.Add(bCopyCommand);
            Controls.Add(lExplanation);
            Controls.Add(lInfoComplete);
            Controls.Add(bItemUpdate);
            Controls.Add(bSearch);
            Controls.Add(cbCommands);
            Controls.Add(tbSearch);
            Controls.Add(lPositionInformationCommand);
            Controls.Add(bListDown);
            Controls.Add(bListUp);
            Controls.Add(bSoftCommand);
            Controls.Add(bAllVoiceCommand);
            Controls.Add(bDefaultCommand);
            Controls.Add(INameCommand);
            Controls.Add(lNameSearch);
            ForeColor = SystemColors.Control;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "FormExplanationCommands";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Information Commands";
            FormClosing += ApplicationInfoCommand_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public Label INameCommand;
        private Button bDefaultCommand;
        private Button bAllVoiceCommand;
        public Label lExplanation;
        private Button bListUp;
        private Button bListDown;
        private Label lPositionInformationCommand;
        private Button bCopyCommand;
        private TextBox tbSearch;
        private ComboBox cbCommands;
        private Button bSearch;
        private Button bItemUpdate;
        public Label lInfoComplete;
        private Button bSoftCommand;
        public Label lExampleWriteCommand;
        private ListBox lbSeachCommands;
        private Button bClearSearch;
        private Label lNameSearch;
    }
}