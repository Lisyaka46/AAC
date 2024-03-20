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
            pictureBox1 = new PictureBox();
            lExplanation = new Label();
            lInfoComplete = new Label();
            bListUp = new Button();
            bListDown = new Button();
            lPositionInformationCommand = new Label();
            bCopyCommand = new Button();
            tbSearch = new TextBox();
            lSearchText = new Label();
            cbCommands = new ComboBox();
            bSearch = new Button();
            bItemUpdate = new Button();
            lExampleWriteCommand = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // INameCommand
            // 
            INameCommand.BackColor = Color.Black;
            INameCommand.BorderStyle = BorderStyle.Fixed3D;
            INameCommand.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            INameCommand.ForeColor = Color.LightGreen;
            INameCommand.Location = new Point(4, 83);
            INameCommand.Margin = new Padding(4, 0, 4, 0);
            INameCommand.Name = "INameCommand";
            INameCommand.Size = new Size(792, 37);
            INameCommand.TabIndex = 0;
            INameCommand.Text = "Command";
            INameCommand.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // bDefaultCommand
            // 
            bDefaultCommand.BackColor = Color.Black;
            bDefaultCommand.Cursor = Cursors.Hand;
            bDefaultCommand.ForeColor = Color.DarkGray;
            bDefaultCommand.Location = new Point(5, 10);
            bDefaultCommand.Margin = new Padding(4, 3, 4, 3);
            bDefaultCommand.Name = "bDefaultCommand";
            bDefaultCommand.Size = new Size(119, 23);
            bDefaultCommand.TabIndex = 2;
            bDefaultCommand.Text = "Default Commands";
            bDefaultCommand.UseVisualStyleBackColor = false;
            bDefaultCommand.Click += DefaultCommandActivate;
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
            bAllVoiceCommand.ForeColor = Color.DarkGray;
            bAllVoiceCommand.Location = new Point(132, 10);
            bAllVoiceCommand.Margin = new Padding(4, 3, 4, 3);
            bAllVoiceCommand.Name = "bAllVoiceCommand";
            bAllVoiceCommand.Size = new Size(109, 23);
            bAllVoiceCommand.TabIndex = 3;
            bAllVoiceCommand.Text = "Voice Commands";
            bAllVoiceCommand.UseVisualStyleBackColor = false;
            bAllVoiceCommand.Click += AllVoiceCommandActivate;
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
            bSoftCommand.ForeColor = Color.DarkGray;
            bSoftCommand.Location = new Point(249, 10);
            bSoftCommand.Margin = new Padding(4, 3, 4, 3);
            bSoftCommand.Name = "bSoftCommand";
            bSoftCommand.Size = new Size(102, 23);
            bSoftCommand.TabIndex = 4;
            bSoftCommand.Text = "Soft Commands";
            bSoftCommand.UseVisualStyleBackColor = false;
            bSoftCommand.Click += SoftCommandActivate;
            bSoftCommand.MouseDown += ButtonMouseDown;
            bSoftCommand.MouseEnter += ButtonMouseEnter;
            bSoftCommand.MouseLeave += ButtonMouseLeave;
            bSoftCommand.MouseMove += ButtonMouseMove;
            bSoftCommand.MouseUp += ButtonMouseUp;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(756, 2);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(40, 40);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // lExplanation
            // 
            lExplanation.BackColor = Color.Black;
            lExplanation.BorderStyle = BorderStyle.Fixed3D;
            lExplanation.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lExplanation.ForeColor = Color.FloralWhite;
            lExplanation.Location = new Point(4, 166);
            lExplanation.Margin = new Padding(4, 0, 4, 0);
            lExplanation.Name = "lExplanation";
            lExplanation.Size = new Size(792, 151);
            lExplanation.TabIndex = 6;
            lExplanation.Text = "Command";
            // 
            // lInfoComplete
            // 
            lInfoComplete.AutoSize = true;
            lInfoComplete.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lInfoComplete.Location = new Point(356, 14);
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
            bListUp.Font = new Font("Segoe UI Black", 11F, FontStyle.Bold, GraphicsUnit.Point);
            bListUp.ForeColor = SystemColors.ActiveCaptionText;
            bListUp.Location = new Point(695, 10);
            bListUp.Margin = new Padding(4, 3, 4, 3);
            bListUp.Name = "bListUp";
            bListUp.Size = new Size(24, 24);
            bListUp.TabIndex = 8;
            bListUp.Text = "ᐃ";
            bListUp.UseVisualStyleBackColor = true;
            bListUp.Click += ButtonListChange_Click;
            // 
            // bListDown
            // 
            bListDown.Cursor = Cursors.Hand;
            bListDown.Font = new Font("Segoe UI Black", 11F, FontStyle.Bold, GraphicsUnit.Point);
            bListDown.ForeColor = SystemColors.ActiveCaptionText;
            bListDown.Location = new Point(725, 10);
            bListDown.Margin = new Padding(4, 3, 4, 3);
            bListDown.Name = "bListDown";
            bListDown.Size = new Size(24, 24);
            bListDown.TabIndex = 9;
            bListDown.Text = "ᐁ";
            bListDown.UseVisualStyleBackColor = true;
            bListDown.Click += ButtonListChange_Click;
            // 
            // lPositionInformationCommand
            // 
            lPositionInformationCommand.Font = new Font("Segoe UI Symbol", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lPositionInformationCommand.Location = new Point(542, 11);
            lPositionInformationCommand.Margin = new Padding(4, 0, 4, 0);
            lPositionInformationCommand.Name = "lPositionInformationCommand";
            lPositionInformationCommand.Size = new Size(149, 23);
            lPositionInformationCommand.TabIndex = 10;
            lPositionInformationCommand.Text = "XXXXXXXX/XXXXXXXX";
            lPositionInformationCommand.TextAlign = ContentAlignment.MiddleRight;
            // 
            // bCopyCommand
            // 
            bCopyCommand.BackColor = Color.Black;
            bCopyCommand.Cursor = Cursors.Hand;
            bCopyCommand.ForeColor = Color.DarkGray;
            bCopyCommand.Location = new Point(5, 324);
            bCopyCommand.Margin = new Padding(4, 3, 4, 3);
            bCopyCommand.Name = "bCopyCommand";
            bCopyCommand.Size = new Size(104, 24);
            bCopyCommand.TabIndex = 11;
            bCopyCommand.Text = "Copy Command";
            bCopyCommand.UseVisualStyleBackColor = false;
            bCopyCommand.Click += BCopyCommand_Click;
            bCopyCommand.MouseDown += ButtonMouseDown;
            bCopyCommand.MouseEnter += ButtonMouseEnter;
            bCopyCommand.MouseLeave += ButtonMouseLeave;
            bCopyCommand.MouseUp += ButtonMouseUp;
            // 
            // tbSearch
            // 
            tbSearch.BackColor = Color.Black;
            tbSearch.ForeColor = Color.DimGray;
            tbSearch.Location = new Point(542, 329);
            tbSearch.Name = "tbSearch";
            tbSearch.Size = new Size(186, 23);
            tbSearch.TabIndex = 14;
            tbSearch.Text = "Поиск элемента";
            tbSearch.TextAlign = HorizontalAlignment.Center;
            tbSearch.TextChanged += TbSearch_TextChanged;
            tbSearch.Enter += TbSearch_Enter;
            tbSearch.Leave += TbSearch_Leave;
            // 
            // lSearchText
            // 
            lSearchText.BackColor = Color.Black;
            lSearchText.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lSearchText.ForeColor = Color.White;
            lSearchText.Location = new Point(401, 332);
            lSearchText.Name = "lSearchText";
            lSearchText.Size = new Size(138, 16);
            lSearchText.TabIndex = 15;
            lSearchText.Text = "Элементы не найдены";
            lSearchText.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbCommands
            // 
            cbCommands.BackColor = Color.Black;
            cbCommands.Cursor = Cursors.Hand;
            cbCommands.ForeColor = Color.White;
            cbCommands.FormattingEnabled = true;
            cbCommands.ImeMode = ImeMode.On;
            cbCommands.Location = new Point(4, 46);
            cbCommands.Name = "cbCommands";
            cbCommands.Size = new Size(689, 23);
            cbCommands.TabIndex = 16;
            // 
            // bSearch
            // 
            bSearch.BackColor = Color.Black;
            bSearch.Cursor = Cursors.Hand;
            bSearch.ForeColor = Color.DarkGray;
            bSearch.Location = new Point(734, 328);
            bSearch.Name = "bSearch";
            bSearch.Size = new Size(64, 25);
            bSearch.TabIndex = 17;
            bSearch.Text = "Search";
            bSearch.UseVisualStyleBackColor = false;
            bSearch.Click += BSearch_Click;
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
            bItemUpdate.Location = new Point(699, 45);
            bItemUpdate.Name = "bItemUpdate";
            bItemUpdate.Size = new Size(97, 25);
            bItemUpdate.TabIndex = 18;
            bItemUpdate.Text = "Move Element";
            bItemUpdate.UseVisualStyleBackColor = false;
            bItemUpdate.Click += BSearchItem_Click;
            bItemUpdate.MouseDown += ButtonMouseDown;
            bItemUpdate.MouseEnter += ButtonMouseEnter;
            bItemUpdate.MouseLeave += ButtonMouseLeave;
            bItemUpdate.MouseUp += ButtonMouseUp;
            // 
            // lExampleWriteCommand
            // 
            lExampleWriteCommand.BackColor = Color.Black;
            lExampleWriteCommand.BorderStyle = BorderStyle.Fixed3D;
            lExampleWriteCommand.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lExampleWriteCommand.ForeColor = Color.FloralWhite;
            lExampleWriteCommand.Location = new Point(4, 128);
            lExampleWriteCommand.Margin = new Padding(4, 0, 4, 0);
            lExampleWriteCommand.Name = "lExampleWriteCommand";
            lExampleWriteCommand.Size = new Size(792, 29);
            lExampleWriteCommand.TabIndex = 19;
            lExampleWriteCommand.Text = "Command";
            lExampleWriteCommand.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FormExplanationCommands
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(800, 356);
            Controls.Add(lExampleWriteCommand);
            Controls.Add(bCopyCommand);
            Controls.Add(lExplanation);
            Controls.Add(lInfoComplete);
            Controls.Add(bItemUpdate);
            Controls.Add(bSearch);
            Controls.Add(cbCommands);
            Controls.Add(lSearchText);
            Controls.Add(tbSearch);
            Controls.Add(lPositionInformationCommand);
            Controls.Add(bListDown);
            Controls.Add(bListUp);
            Controls.Add(pictureBox1);
            Controls.Add(bSoftCommand);
            Controls.Add(bAllVoiceCommand);
            Controls.Add(bDefaultCommand);
            Controls.Add(INameCommand);
            ForeColor = SystemColors.Control;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "FormExplanationCommands";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Information Commands";
            Deactivate += ApplicationInfoCommandCLR_Deactivate;
            FormClosing += ApplicationInfoCommandCLR_FormClosing;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public Label INameCommand;
        private Button bDefaultCommand;
        private Button bAllVoiceCommand;
        private PictureBox pictureBox1;
        public Label lExplanation;
        private Button bListUp;
        private Button bListDown;
        private Label lPositionInformationCommand;
        private Button bCopyCommand;
        private TextBox tbSearch;
        private Label lSearchText;
        private ComboBox cbCommands;
        private Button bSearch;
        private Button bItemUpdate;
        public Label lInfoComplete;
        private Button bSoftCommand;
        public Label lExampleWriteCommand;
    }
}