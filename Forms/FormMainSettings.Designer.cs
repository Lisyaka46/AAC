namespace AAC.Forms
{
    partial class FormMainSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMainSettings));
            pMainMenu = new Panel();
            pIconCollection = new Panel();
            pbAutor = new PictureBox();
            pbMainIcon = new PictureBox();
            lMainVersion = new Label();
            lMainSoftCountAppend = new Label();
            pToolButton = new Panel();
            bToolPAC = new Button();
            bToolOther = new Button();
            bToolColored = new Button();
            bToolMainMenu = new Button();
            pColored = new Panel();
            pSpecialColorParameter = new Panel();
            cbSpecialColorSC = new GUI.SettingCheckBox();
            cbSpecialColorRGB = new GUI.SettingCheckBox();
            pExampleAcentColor = new Panel();
            cbSpecialColorRGBCC = new GUI.SettingCheckBox();
            cbAllSpecialColorActivate = new GUI.SettingCheckBox();
            pOther = new Panel();
            cbHitPanel = new GUI.SettingCheckBox();
            cbMovingBorderScreenForm = new GUI.SettingCheckBox();
            lInformationCountBuffer = new Label();
            lOtherCountBufferPreview = new Label();
            tbOtherCountBuffer = new TrackBar();
            lOtherWarningCountBuffer = new Label();
            cbAltDiactivatePAC = new GUI.SettingCheckBox();
            MainColorDialog = new ColorDialog();
            cbAltOrientationPAC = new GUI.SettingCheckBox();
            pPAC_Parameters = new Panel();
            pKeyAltOrientation = new Panel();
            lNameActivatePAC_Key = new Label();
            tbPAC_KeyDiactivate = new TextBox();
            tbPAC_KeyActivate = new TextBox();
            lNameDiactivatePAC_Key = new Label();
            lNamePAC_Panel = new Label();
            pPAC = new Panel();
            pMainMenu.SuspendLayout();
            pIconCollection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbAutor).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbMainIcon).BeginInit();
            pToolButton.SuspendLayout();
            pColored.SuspendLayout();
            pSpecialColorParameter.SuspendLayout();
            pOther.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbOtherCountBuffer).BeginInit();
            pPAC_Parameters.SuspendLayout();
            pKeyAltOrientation.SuspendLayout();
            pPAC.SuspendLayout();
            SuspendLayout();
            // 
            // pMainMenu
            // 
            pMainMenu.BackColor = Color.FromArgb(149, 216, 219);
            pMainMenu.BorderStyle = BorderStyle.FixedSingle;
            pMainMenu.Controls.Add(pIconCollection);
            pMainMenu.Controls.Add(lMainVersion);
            pMainMenu.Controls.Add(lMainSoftCountAppend);
            pMainMenu.Location = new Point(0, 30);
            pMainMenu.Name = "pMainMenu";
            pMainMenu.Size = new Size(784, 381);
            pMainMenu.TabIndex = 0;
            // 
            // pIconCollection
            // 
            pIconCollection.BorderStyle = BorderStyle.FixedSingle;
            pIconCollection.Controls.Add(pbAutor);
            pIconCollection.Controls.Add(pbMainIcon);
            pIconCollection.Location = new Point(641, 3);
            pIconCollection.Name = "pIconCollection";
            pIconCollection.Size = new Size(138, 140);
            pIconCollection.TabIndex = 5;
            // 
            // pbAutor
            // 
            pbAutor.BorderStyle = BorderStyle.FixedSingle;
            pbAutor.ErrorImage = null;
            pbAutor.ImageLocation = "https://sun1-89.userapi.com/s/v1/ig2/MeoZYiuf6qCbYqL9b1TRXgdi4g0Fhwqth2gJAIPMnHX88MSxR45Uj3U3bZZzIfYCaD2lcri8ToET-2d-vUox59yO.jpg?size=274x274&quality=95&crop=104,211,274,274&ava=1";
            pbAutor.InitialImage = null;
            pbAutor.Location = new Point(3, 3);
            pbAutor.Name = "pbAutor";
            pbAutor.Size = new Size(45, 45);
            pbAutor.SizeMode = PictureBoxSizeMode.StretchImage;
            pbAutor.TabIndex = 3;
            pbAutor.TabStop = false;
            pbAutor.MouseDown += ActivateMoveMouseIconAutor;
            pbAutor.MouseUp += DiactivateMoveMouseIconAutor;
            // 
            // pbMainIcon
            // 
            pbMainIcon.Image = (Image)resources.GetObject("pbMainIcon.Image");
            pbMainIcon.Location = new Point(18, 16);
            pbMainIcon.Name = "pbMainIcon";
            pbMainIcon.Size = new Size(100, 100);
            pbMainIcon.SizeMode = PictureBoxSizeMode.Zoom;
            pbMainIcon.TabIndex = 0;
            pbMainIcon.TabStop = false;
            // 
            // lMainVersion
            // 
            lMainVersion.BackColor = Color.FromArgb(78, 119, 121);
            lMainVersion.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lMainVersion.ForeColor = SystemColors.Control;
            lMainVersion.Location = new Point(0, 358);
            lMainVersion.Name = "lMainVersion";
            lMainVersion.Size = new Size(100, 21);
            lMainVersion.TabIndex = 2;
            lMainVersion.Text = "v: 030324-WGS";
            lMainVersion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lMainSoftCountAppend
            // 
            lMainSoftCountAppend.AutoSize = true;
            lMainSoftCountAppend.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            lMainSoftCountAppend.ForeColor = Color.Black;
            lMainSoftCountAppend.Location = new Point(101, 359);
            lMainSoftCountAppend.Name = "lMainSoftCountAppend";
            lMainSoftCountAppend.Size = new Size(175, 17);
            lMainSoftCountAppend.TabIndex = 4;
            lMainSoftCountAppend.Text = "Добавлено N Soft команд";
            // 
            // pToolButton
            // 
            pToolButton.Controls.Add(bToolPAC);
            pToolButton.Controls.Add(bToolOther);
            pToolButton.Controls.Add(bToolColored);
            pToolButton.Controls.Add(bToolMainMenu);
            pToolButton.Location = new Point(0, 0);
            pToolButton.Name = "pToolButton";
            pToolButton.Size = new Size(784, 30);
            pToolButton.TabIndex = 1;
            // 
            // bToolPAC
            // 
            bToolPAC.AutoSize = true;
            bToolPAC.BackColor = Color.FromArgb(164, 230, 235);
            bToolPAC.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            bToolPAC.ForeColor = Color.Black;
            bToolPAC.Location = new Point(166, 3);
            bToolPAC.Name = "bToolPAC";
            bToolPAC.Size = new Size(93, 25);
            bToolPAC.TabIndex = 3;
            bToolPAC.Text = "Мини-панель";
            bToolPAC.UseVisualStyleBackColor = false;
            bToolPAC.Click += ActivateMenu;
            // 
            // bToolOther
            // 
            bToolOther.AutoSize = true;
            bToolOther.BackColor = Color.FromArgb(164, 230, 235);
            bToolOther.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            bToolOther.ForeColor = Color.Black;
            bToolOther.Location = new Point(714, 3);
            bToolOther.Name = "bToolOther";
            bToolOther.Size = new Size(67, 25);
            bToolOther.TabIndex = 2;
            bToolOther.Text = "Прочее..";
            bToolOther.UseVisualStyleBackColor = false;
            bToolOther.Click += ActivateMenu;
            // 
            // bToolColored
            // 
            bToolColored.AutoSize = true;
            bToolColored.BackColor = Color.FromArgb(164, 230, 235);
            bToolColored.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            bToolColored.ForeColor = Color.Black;
            bToolColored.Location = new Point(100, 3);
            bToolColored.Name = "bToolColored";
            bToolColored.Size = new Size(64, 25);
            bToolColored.TabIndex = 1;
            bToolColored.Text = "Палитра";
            bToolColored.UseVisualStyleBackColor = false;
            bToolColored.Click += ActivateMenu;
            // 
            // bToolMainMenu
            // 
            bToolMainMenu.AutoSize = true;
            bToolMainMenu.BackColor = Color.FromArgb(164, 230, 235);
            bToolMainMenu.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            bToolMainMenu.ForeColor = Color.Black;
            bToolMainMenu.Location = new Point(1, 3);
            bToolMainMenu.Name = "bToolMainMenu";
            bToolMainMenu.Size = new Size(97, 25);
            bToolMainMenu.TabIndex = 0;
            bToolMainMenu.Text = "Главное меню";
            bToolMainMenu.UseVisualStyleBackColor = false;
            bToolMainMenu.Click += ActivateMenu;
            // 
            // pColored
            // 
            pColored.BackColor = Color.FromArgb(149, 216, 219);
            pColored.BorderStyle = BorderStyle.FixedSingle;
            pColored.Controls.Add(pSpecialColorParameter);
            pColored.Controls.Add(cbAllSpecialColorActivate);
            pColored.Location = new Point(805, 412);
            pColored.Name = "pColored";
            pColored.Size = new Size(784, 381);
            pColored.TabIndex = 5;
            // 
            // pSpecialColorParameter
            // 
            pSpecialColorParameter.BorderStyle = BorderStyle.FixedSingle;
            pSpecialColorParameter.Controls.Add(cbSpecialColorSC);
            pSpecialColorParameter.Controls.Add(cbSpecialColorRGB);
            pSpecialColorParameter.Controls.Add(pExampleAcentColor);
            pSpecialColorParameter.Controls.Add(cbSpecialColorRGBCC);
            pSpecialColorParameter.Location = new Point(6, 23);
            pSpecialColorParameter.Name = "pSpecialColorParameter";
            pSpecialColorParameter.Size = new Size(90, 79);
            pSpecialColorParameter.TabIndex = 2;
            // 
            // cbSpecialColorSC
            // 
            cbSpecialColorSC.BoolParameter = null;
            cbSpecialColorSC.Checked = false;
            cbSpecialColorSC.ElementText = "SC";
            cbSpecialColorSC.Font = new Font("Calibri", 12F);
            cbSpecialColorSC.Location = new Point(4, 47);
            cbSpecialColorSC.MaximumSize = new Size(44, 23);
            cbSpecialColorSC.MinimumSize = new Size(44, 23);
            cbSpecialColorSC.Name = "cbSpecialColorSC";
            cbSpecialColorSC.Size = new Size(44, 23);
            cbSpecialColorSC.TabIndex = 8;
            // 
            // cbSpecialColorRGB
            // 
            cbSpecialColorRGB.BoolParameter = null;
            cbSpecialColorRGB.Checked = false;
            cbSpecialColorRGB.ElementText = "RGB";
            cbSpecialColorRGB.Font = new Font("Calibri", 12F);
            cbSpecialColorRGB.Location = new Point(4, 1);
            cbSpecialColorRGB.MaximumSize = new Size(56, 23);
            cbSpecialColorRGB.MinimumSize = new Size(56, 23);
            cbSpecialColorRGB.Name = "cbSpecialColorRGB";
            cbSpecialColorRGB.Size = new Size(56, 23);
            cbSpecialColorRGB.TabIndex = 6;
            // 
            // pExampleAcentColor
            // 
            pExampleAcentColor.BorderStyle = BorderStyle.FixedSingle;
            pExampleAcentColor.Location = new Point(48, 45);
            pExampleAcentColor.Name = "pExampleAcentColor";
            pExampleAcentColor.Size = new Size(25, 25);
            pExampleAcentColor.TabIndex = 5;
            pExampleAcentColor.DoubleClick += ChangeAcientColorSC;
            // 
            // cbSpecialColorRGBCC
            // 
            cbSpecialColorRGBCC.BoolParameter = null;
            cbSpecialColorRGBCC.Checked = false;
            cbSpecialColorRGBCC.ElementText = "RGBCC";
            cbSpecialColorRGBCC.Font = new Font("Calibri", 12F);
            cbSpecialColorRGBCC.Location = new Point(4, 24);
            cbSpecialColorRGBCC.MaximumSize = new Size(74, 23);
            cbSpecialColorRGBCC.MinimumSize = new Size(74, 23);
            cbSpecialColorRGBCC.Name = "cbSpecialColorRGBCC";
            cbSpecialColorRGBCC.Size = new Size(74, 23);
            cbSpecialColorRGBCC.TabIndex = 7;
            // 
            // cbAllSpecialColorActivate
            // 
            cbAllSpecialColorActivate.BoolParameter = null;
            cbAllSpecialColorActivate.Checked = false;
            cbAllSpecialColorActivate.ElementText = "Специальные цвета";
            cbAllSpecialColorActivate.Font = new Font("Calibri", 12F);
            cbAllSpecialColorActivate.Location = new Point(6, 1);
            cbAllSpecialColorActivate.MaximumSize = new Size(166, 23);
            cbAllSpecialColorActivate.MinimumSize = new Size(166, 23);
            cbAllSpecialColorActivate.Name = "cbAllSpecialColorActivate";
            cbAllSpecialColorActivate.Size = new Size(166, 23);
            cbAllSpecialColorActivate.TabIndex = 6;
            // 
            // pOther
            // 
            pOther.BackColor = Color.FromArgb(149, 216, 219);
            pOther.BorderStyle = BorderStyle.FixedSingle;
            pOther.Controls.Add(cbHitPanel);
            pOther.Controls.Add(cbMovingBorderScreenForm);
            pOther.Controls.Add(lInformationCountBuffer);
            pOther.Controls.Add(lOtherCountBufferPreview);
            pOther.Controls.Add(tbOtherCountBuffer);
            pOther.Controls.Add(lOtherWarningCountBuffer);
            pOther.Location = new Point(805, 2);
            pOther.Name = "pOther";
            pOther.Size = new Size(784, 381);
            pOther.TabIndex = 6;
            // 
            // cbHitPanel
            // 
            cbHitPanel.BoolParameter = null;
            cbHitPanel.Checked = false;
            cbHitPanel.ElementText = "Использовать подсказки к командам";
            cbHitPanel.Font = new Font("Calibri", 12F);
            cbHitPanel.Location = new Point(6, 26);
            cbHitPanel.MaximumSize = new Size(282, 23);
            cbHitPanel.MinimumSize = new Size(282, 23);
            cbHitPanel.Name = "cbHitPanel";
            cbHitPanel.Size = new Size(282, 23);
            cbHitPanel.TabIndex = 9;
            // 
            // cbMovingBorderScreenForm
            // 
            cbMovingBorderScreenForm.BoolParameter = null;
            cbMovingBorderScreenForm.Checked = false;
            cbMovingBorderScreenForm.ElementText = "Позволять передвигать главную форму за границы экрана";
            cbMovingBorderScreenForm.Font = new Font("Calibri", 12F);
            cbMovingBorderScreenForm.Location = new Point(6, 4);
            cbMovingBorderScreenForm.MaximumSize = new Size(432, 23);
            cbMovingBorderScreenForm.MinimumSize = new Size(432, 23);
            cbMovingBorderScreenForm.Name = "cbMovingBorderScreenForm";
            cbMovingBorderScreenForm.Size = new Size(432, 23);
            cbMovingBorderScreenForm.TabIndex = 8;
            // 
            // lInformationCountBuffer
            // 
            lInformationCountBuffer.AutoSize = true;
            lInformationCountBuffer.Font = new Font("Calibri", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lInformationCountBuffer.Location = new Point(3, 75);
            lInformationCountBuffer.Name = "lInformationCountBuffer";
            lInformationCountBuffer.Size = new Size(240, 19);
            lInformationCountBuffer.TabIndex = 6;
            lInformationCountBuffer.Text = "Вместимость командного буфера";
            // 
            // lOtherCountBufferPreview
            // 
            lOtherCountBufferPreview.AutoSize = true;
            lOtherCountBufferPreview.BorderStyle = BorderStyle.FixedSingle;
            lOtherCountBufferPreview.Font = new Font("Book Antiqua", 10F, FontStyle.Bold);
            lOtherCountBufferPreview.Location = new Point(247, 96);
            lOtherCountBufferPreview.Name = "lOtherCountBufferPreview";
            lOtherCountBufferPreview.Size = new Size(38, 21);
            lOtherCountBufferPreview.TabIndex = 5;
            lOtherCountBufferPreview.Text = "ZZZ";
            // 
            // tbOtherCountBuffer
            // 
            tbOtherCountBuffer.AutoSize = false;
            tbOtherCountBuffer.LargeChange = 2;
            tbOtherCountBuffer.Location = new Point(5, 97);
            tbOtherCountBuffer.Maximum = 80;
            tbOtherCountBuffer.Minimum = 4;
            tbOtherCountBuffer.Name = "tbOtherCountBuffer";
            tbOtherCountBuffer.Size = new Size(238, 20);
            tbOtherCountBuffer.TabIndex = 4;
            tbOtherCountBuffer.Tag = "countBuffer";
            tbOtherCountBuffer.TickFrequency = 8;
            tbOtherCountBuffer.TickStyle = TickStyle.None;
            tbOtherCountBuffer.Value = 4;
            // 
            // lOtherWarningCountBuffer
            // 
            lOtherWarningCountBuffer.BackColor = Color.IndianRed;
            lOtherWarningCountBuffer.Font = new Font("Arial Rounded MT Bold", 11.25F);
            lOtherWarningCountBuffer.ForeColor = Color.White;
            lOtherWarningCountBuffer.Location = new Point(255, 97);
            lOtherWarningCountBuffer.Name = "lOtherWarningCountBuffer";
            lOtherWarningCountBuffer.Size = new Size(291, 18);
            lOtherWarningCountBuffer.TabIndex = 7;
            lOtherWarningCountBuffer.Text = "Изменения вступят в силу после перезагрузки!";
            lOtherWarningCountBuffer.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cbAltDiactivatePAC
            // 
            cbAltDiactivatePAC.BoolParameter = null;
            cbAltDiactivatePAC.Checked = false;
            cbAltDiactivatePAC.ElementText = "Отключать взаимодействие по клавишам при закрытии панели действий";
            cbAltDiactivatePAC.Font = new Font("Calibri", 12F);
            cbAltDiactivatePAC.Location = new Point(4, 26);
            cbAltDiactivatePAC.MaximumSize = new Size(537, 23);
            cbAltDiactivatePAC.MinimumSize = new Size(537, 23);
            cbAltDiactivatePAC.Name = "cbAltDiactivatePAC";
            cbAltDiactivatePAC.Size = new Size(537, 23);
            cbAltDiactivatePAC.TabIndex = 10;
            // 
            // cbAltOrientationPAC
            // 
            cbAltOrientationPAC.BoolParameter = null;
            cbAltOrientationPAC.Checked = false;
            cbAltOrientationPAC.ElementText = "Использовать свои клавиши для активации взаимодействия с кнопками мини-панели";
            cbAltOrientationPAC.Font = new Font("Calibri", 12F);
            cbAltOrientationPAC.Location = new Point(4, 49);
            cbAltOrientationPAC.MaximumSize = new Size(627, 23);
            cbAltOrientationPAC.MinimumSize = new Size(627, 23);
            cbAltOrientationPAC.Name = "cbAltOrientationPAC";
            cbAltOrientationPAC.Size = new Size(627, 23);
            cbAltOrientationPAC.TabIndex = 11;
            // 
            // pPAC_Parameters
            // 
            pPAC_Parameters.BorderStyle = BorderStyle.FixedSingle;
            pPAC_Parameters.Controls.Add(pKeyAltOrientation);
            pPAC_Parameters.Controls.Add(lNamePAC_Panel);
            pPAC_Parameters.Controls.Add(cbAltOrientationPAC);
            pPAC_Parameters.Controls.Add(cbAltDiactivatePAC);
            pPAC_Parameters.Location = new Point(6, 6);
            pPAC_Parameters.Name = "pPAC_Parameters";
            pPAC_Parameters.Size = new Size(634, 129);
            pPAC_Parameters.TabIndex = 12;
            // 
            // pKeyAltOrientation
            // 
            pKeyAltOrientation.BackColor = Color.FromArgb(139, 206, 219);
            pKeyAltOrientation.BorderStyle = BorderStyle.FixedSingle;
            pKeyAltOrientation.Controls.Add(lNameActivatePAC_Key);
            pKeyAltOrientation.Controls.Add(tbPAC_KeyDiactivate);
            pKeyAltOrientation.Controls.Add(tbPAC_KeyActivate);
            pKeyAltOrientation.Controls.Add(lNameDiactivatePAC_Key);
            pKeyAltOrientation.Location = new Point(6, 74);
            pKeyAltOrientation.Name = "pKeyAltOrientation";
            pKeyAltOrientation.Size = new Size(241, 44);
            pKeyAltOrientation.TabIndex = 13;
            // 
            // lNameActivatePAC_Key
            // 
            lNameActivatePAC_Key.AutoSize = true;
            lNameActivatePAC_Key.Location = new Point(6, 0);
            lNameActivatePAC_Key.Name = "lNameActivatePAC_Key";
            lNameActivatePAC_Key.Size = new Size(61, 15);
            lNameActivatePAC_Key.TabIndex = 14;
            lNameActivatePAC_Key.Text = "Включает";
            // 
            // tbPAC_KeyDiactivate
            // 
            tbPAC_KeyDiactivate.BackColor = Color.FromArgb(159, 226, 229);
            tbPAC_KeyDiactivate.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 204);
            tbPAC_KeyDiactivate.Location = new Point(129, 15);
            tbPAC_KeyDiactivate.Name = "tbPAC_KeyDiactivate";
            tbPAC_KeyDiactivate.ReadOnly = true;
            tbPAC_KeyDiactivate.ShortcutsEnabled = false;
            tbPAC_KeyDiactivate.Size = new Size(100, 23);
            tbPAC_KeyDiactivate.TabIndex = 13;
            tbPAC_KeyDiactivate.TextAlign = HorizontalAlignment.Center;
            // 
            // tbPAC_KeyActivate
            // 
            tbPAC_KeyActivate.BackColor = Color.FromArgb(159, 226, 229);
            tbPAC_KeyActivate.Location = new Point(8, 15);
            tbPAC_KeyActivate.Name = "tbPAC_KeyActivate";
            tbPAC_KeyActivate.ReadOnly = true;
            tbPAC_KeyActivate.ShortcutsEnabled = false;
            tbPAC_KeyActivate.Size = new Size(100, 23);
            tbPAC_KeyActivate.TabIndex = 12;
            tbPAC_KeyActivate.TextAlign = HorizontalAlignment.Center;
            // 
            // lNameDiactivatePAC_Key
            // 
            lNameDiactivatePAC_Key.AutoSize = true;
            lNameDiactivatePAC_Key.Location = new Point(125, 0);
            lNameDiactivatePAC_Key.Name = "lNameDiactivatePAC_Key";
            lNameDiactivatePAC_Key.Size = new Size(70, 15);
            lNameDiactivatePAC_Key.TabIndex = 15;
            lNameDiactivatePAC_Key.Text = "Выключает";
            // 
            // lNamePAC_Panel
            // 
            lNamePAC_Panel.AutoSize = true;
            lNamePAC_Panel.Font = new Font("Calibri", 12F);
            lNamePAC_Panel.Location = new Point(0, 0);
            lNamePAC_Panel.Name = "lNamePAC_Panel";
            lNamePAC_Panel.Size = new Size(103, 19);
            lNamePAC_Panel.TabIndex = 0;
            lNamePAC_Panel.Text = "Мини-панель";
            // 
            // pPAC
            // 
            pPAC.BackColor = Color.FromArgb(149, 216, 219);
            pPAC.Controls.Add(pPAC_Parameters);
            pPAC.Location = new Point(1, 427);
            pPAC.Name = "pPAC";
            pPAC.Size = new Size(784, 381);
            pPAC.TabIndex = 7;
            // 
            // FormMainSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(164, 230, 235);
            ClientSize = new Size(1601, 846);
            Controls.Add(pPAC);
            Controls.Add(pOther);
            Controls.Add(pColored);
            Controls.Add(pToolButton);
            Controls.Add(pMainMenu);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(800, 450);
            Name = "FormMainSettings";
            Opacity = 0.9D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Настройки";
            FormClosing += FormCloseding;
            Shown += SettingsCLR_Shown;
            SizeChanged += ReSize;
            StyleChanged += ReSize;
            pMainMenu.ResumeLayout(false);
            pMainMenu.PerformLayout();
            pIconCollection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbAutor).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbMainIcon).EndInit();
            pToolButton.ResumeLayout(false);
            pToolButton.PerformLayout();
            pColored.ResumeLayout(false);
            pSpecialColorParameter.ResumeLayout(false);
            pOther.ResumeLayout(false);
            pOther.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tbOtherCountBuffer).EndInit();
            pPAC_Parameters.ResumeLayout(false);
            pPAC_Parameters.PerformLayout();
            pKeyAltOrientation.ResumeLayout(false);
            pKeyAltOrientation.PerformLayout();
            pPAC.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pMainMenu;
        private Panel pToolButton;
        public Label lMainVersion;
        public PictureBox pbMainIcon;
        private Button bToolColored;
        private Button bToolMainMenu;
        public Label lMainSoftCountAppend;
        public PictureBox pbAutor;
        public Panel pColored;
        public Panel pSpecialColorParameter;
        private Panel pIconCollection;
        public Button bToolOther;
        public Panel pOther;
        public Label lInformationCountBuffer;
        private Label lOtherCountBufferPreview;
        public TrackBar tbOtherCountBuffer;
        public Label lOtherWarningCountBuffer;
        private Panel pExampleAcentColor;
        private ColorDialog MainColorDialog;
        private GUI.SettingCheckBox cbAllSpecialColorActivate;
        private GUI.SettingCheckBox cbSpecialColorSC;
        private GUI.SettingCheckBox cbSpecialColorRGB;
        private GUI.SettingCheckBox cbSpecialColorRGBCC;
        private GUI.SettingCheckBox cbMovingBorderScreenForm;
        private GUI.SettingCheckBox cbAltDiactivatePAC;
        private GUI.SettingCheckBox cbHitPanel;
        private GUI.SettingCheckBox cbAltOrientationPAC;
        private Panel pPAC_Parameters;
        private Panel pKeyAltOrientation;
        private TextBox tbPAC_KeyDiactivate;
        private TextBox tbPAC_KeyActivate;
        private Label lNamePAC_Panel;
        private Button bToolPAC;
        private Label lNameActivatePAC_Key;
        private Label lNameDiactivatePAC_Key;
        private Panel pPAC;
    }
}