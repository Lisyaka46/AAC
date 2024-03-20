namespace AAC.Forms
{
    partial class FormThemesEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormThemesEditor));
            pAllRegisteredThemes = new Panel();
            pAllElementsRegThemes = new Panel();
            vSBRegisteredThemes = new VScrollBar();
            pAllRedactorParamColorTheme = new Panel();
            ToolPanelParamColor = new Panel();
            MultiSearchPanel = new Panel();
            MultiSearchCounter = new Label();
            ButtonLeftMultiSearchParamColor = new GUI.IELImageButton();
            ButtonRightMultiSearchParamColor = new GUI.IELImageButton();
            ButtonSearchParamColor = new GUI.IELImageButton();
            SearchParamColorTextBox = new TextBox();
            NameTheme = new Label();
            ButtonCloseChangeParamColor = new GUI.IELImageButton();
            ScrollBarParamColor = new VScrollBar();
            pAllParamColorTheme = new Panel();
            BorderEndParamColor = new Label();
            BorderStartParamColor = new Label();
            lDescription = new Label();
            ColorCreator = new ColorDialog();
            bCreateNewTheme = new Button();
            pMiniPanel = new Panel();
            ButtonDeleteTheme = new GUI.IELImageButton();
            pmpAllButtons = new Panel();
            bSetActiveTheme = new Button();
            bpmpChangedThemeInfo = new Button();
            bmpSaveChangeTheme = new Button();
            bmpButtonBack = new Button();
            lmpName = new Label();
            pAllRegisteredThemes.SuspendLayout();
            pAllRedactorParamColorTheme.SuspendLayout();
            ToolPanelParamColor.SuspendLayout();
            MultiSearchPanel.SuspendLayout();
            pAllParamColorTheme.SuspendLayout();
            pMiniPanel.SuspendLayout();
            pmpAllButtons.SuspendLayout();
            SuspendLayout();
            // 
            // pAllRegisteredThemes
            // 
            pAllRegisteredThemes.BorderStyle = BorderStyle.FixedSingle;
            pAllRegisteredThemes.Controls.Add(pAllElementsRegThemes);
            pAllRegisteredThemes.Controls.Add(vSBRegisteredThemes);
            pAllRegisteredThemes.Location = new Point(-1, 3);
            pAllRegisteredThemes.Name = "pAllRegisteredThemes";
            pAllRegisteredThemes.Size = new Size(200, 443);
            pAllRegisteredThemes.TabIndex = 0;
            // 
            // pAllElementsRegThemes
            // 
            pAllElementsRegThemes.BorderStyle = BorderStyle.FixedSingle;
            pAllElementsRegThemes.Location = new Point(1, 3);
            pAllElementsRegThemes.Name = "pAllElementsRegThemes";
            pAllElementsRegThemes.Size = new Size(177, 435);
            pAllElementsRegThemes.TabIndex = 3;
            // 
            // vSBRegisteredThemes
            // 
            vSBRegisteredThemes.Location = new Point(181, -1);
            vSBRegisteredThemes.Name = "vSBRegisteredThemes";
            vSBRegisteredThemes.Size = new Size(17, 442);
            vSBRegisteredThemes.TabIndex = 1;
            // 
            // pAllRedactorParamColorTheme
            // 
            pAllRedactorParamColorTheme.BorderStyle = BorderStyle.Fixed3D;
            pAllRedactorParamColorTheme.Controls.Add(ToolPanelParamColor);
            pAllRedactorParamColorTheme.Controls.Add(ScrollBarParamColor);
            pAllRedactorParamColorTheme.Controls.Add(pAllParamColorTheme);
            pAllRedactorParamColorTheme.Location = new Point(209, 12);
            pAllRedactorParamColorTheme.Name = "pAllRedactorParamColorTheme";
            pAllRedactorParamColorTheme.Size = new Size(577, 426);
            pAllRedactorParamColorTheme.TabIndex = 1;
            // 
            // ToolPanelParamColor
            // 
            ToolPanelParamColor.BorderStyle = BorderStyle.FixedSingle;
            ToolPanelParamColor.Controls.Add(MultiSearchPanel);
            ToolPanelParamColor.Controls.Add(ButtonSearchParamColor);
            ToolPanelParamColor.Controls.Add(SearchParamColorTextBox);
            ToolPanelParamColor.Controls.Add(NameTheme);
            ToolPanelParamColor.Controls.Add(ButtonCloseChangeParamColor);
            ToolPanelParamColor.Location = new Point(-1, -1);
            ToolPanelParamColor.Name = "ToolPanelParamColor";
            ToolPanelParamColor.Size = new Size(575, 68);
            ToolPanelParamColor.TabIndex = 4;
            // 
            // MultiSearchPanel
            // 
            MultiSearchPanel.BorderStyle = BorderStyle.FixedSingle;
            MultiSearchPanel.Controls.Add(MultiSearchCounter);
            MultiSearchPanel.Controls.Add(ButtonLeftMultiSearchParamColor);
            MultiSearchPanel.Controls.Add(ButtonRightMultiSearchParamColor);
            MultiSearchPanel.Location = new Point(393, 26);
            MultiSearchPanel.Name = "MultiSearchPanel";
            MultiSearchPanel.Size = new Size(55, 36);
            MultiSearchPanel.TabIndex = 4;
            // 
            // MultiSearchCounter
            // 
            MultiSearchCounter.ForeColor = SystemColors.Info;
            MultiSearchCounter.Location = new Point(2, 2);
            MultiSearchCounter.Name = "MultiSearchCounter";
            MultiSearchCounter.Size = new Size(48, 15);
            MultiSearchCounter.TabIndex = 2;
            MultiSearchCounter.Text = "12/12";
            MultiSearchCounter.TextAlign = ContentAlignment.TopCenter;
            // 
            // ButtonLeftMultiSearchParamColor
            // 
            ButtonLeftMultiSearchParamColor.BackgroundImage = (Image)resources.GetObject("ButtonLeftMultiSearchParamColor.BackgroundImage");
            ButtonLeftMultiSearchParamColor.BackgroundImageLayout = ImageLayout.Zoom;
            ButtonLeftMultiSearchParamColor.Location = new Point(1, 14);
            ButtonLeftMultiSearchParamColor.Name = "ButtonLeftMultiSearchParamColor";
            ButtonLeftMultiSearchParamColor.Size = new Size(22, 22);
            ButtonLeftMultiSearchParamColor.TabIndex = 1;
            // 
            // ButtonRightMultiSearchParamColor
            // 
            ButtonRightMultiSearchParamColor.BackgroundImage = (Image)resources.GetObject("ButtonRightMultiSearchParamColor.BackgroundImage");
            ButtonRightMultiSearchParamColor.BackgroundImageLayout = ImageLayout.Zoom;
            ButtonRightMultiSearchParamColor.Location = new Point(29, 14);
            ButtonRightMultiSearchParamColor.Name = "ButtonRightMultiSearchParamColor";
            ButtonRightMultiSearchParamColor.Size = new Size(22, 22);
            ButtonRightMultiSearchParamColor.TabIndex = 0;
            // 
            // ButtonSearchParamColor
            // 
            ButtonSearchParamColor.BackgroundImage = (Image)resources.GetObject("ButtonSearchParamColor.BackgroundImage");
            ButtonSearchParamColor.BackgroundImageLayout = ImageLayout.Zoom;
            ButtonSearchParamColor.Location = new Point(451, 2);
            ButtonSearchParamColor.Name = "ButtonSearchParamColor";
            ButtonSearchParamColor.Size = new Size(23, 23);
            ButtonSearchParamColor.TabIndex = 3;
            // 
            // SearchParamColorTextBox
            // 
            SearchParamColorTextBox.BackColor = Color.FromArgb(75, 82, 99);
            SearchParamColorTextBox.ForeColor = SystemColors.Info;
            SearchParamColorTextBox.Location = new Point(273, 2);
            SearchParamColorTextBox.Name = "SearchParamColorTextBox";
            SearchParamColorTextBox.Size = new Size(176, 23);
            SearchParamColorTextBox.TabIndex = 2;
            SearchParamColorTextBox.Text = "Поиск параметров";
            SearchParamColorTextBox.TextAlign = HorizontalAlignment.Center;
            SearchParamColorTextBox.Enter += SearchEnter;
            SearchParamColorTextBox.Leave += SearchLeave;
            // 
            // NameTheme
            // 
            NameTheme.AutoEllipsis = true;
            NameTheme.BorderStyle = BorderStyle.FixedSingle;
            NameTheme.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            NameTheme.ForeColor = Color.PeachPuff;
            NameTheme.Location = new Point(2, 3);
            NameTheme.Name = "NameTheme";
            NameTheme.Size = new Size(265, 20);
            NameTheme.TabIndex = 1;
            NameTheme.Text = "NAME";
            NameTheme.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ButtonCloseChangeParamColor
            // 
            ButtonCloseChangeParamColor.BackgroundImage = (Image)resources.GetObject("ButtonCloseChangeParamColor.BackgroundImage");
            ButtonCloseChangeParamColor.BackgroundImageLayout = ImageLayout.Zoom;
            ButtonCloseChangeParamColor.Location = new Point(548, 0);
            ButtonCloseChangeParamColor.Name = "ButtonCloseChangeParamColor";
            ButtonCloseChangeParamColor.Size = new Size(25, 25);
            ButtonCloseChangeParamColor.TabIndex = 0;
            // 
            // ScrollBarParamColor
            // 
            ScrollBarParamColor.LargeChange = 2;
            ScrollBarParamColor.Location = new Point(558, 67);
            ScrollBarParamColor.Name = "ScrollBarParamColor";
            ScrollBarParamColor.Size = new Size(17, 355);
            ScrollBarParamColor.TabIndex = 3;
            // 
            // pAllParamColorTheme
            // 
            pAllParamColorTheme.Controls.Add(BorderEndParamColor);
            pAllParamColorTheme.Controls.Add(BorderStartParamColor);
            pAllParamColorTheme.Location = new Point(1, 70);
            pAllParamColorTheme.Name = "pAllParamColorTheme";
            pAllParamColorTheme.Size = new Size(554, 350);
            pAllParamColorTheme.TabIndex = 2;
            // 
            // BorderEndParamColor
            // 
            BorderEndParamColor.AutoSize = true;
            BorderEndParamColor.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            BorderEndParamColor.ForeColor = Color.White;
            BorderEndParamColor.Location = new Point(262, 49);
            BorderEndParamColor.Name = "BorderEndParamColor";
            BorderEndParamColor.Size = new Size(24, 32);
            BorderEndParamColor.TabIndex = 1;
            BorderEndParamColor.Text = "_";
            // 
            // BorderStartParamColor
            // 
            BorderStartParamColor.AutoSize = true;
            BorderStartParamColor.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            BorderStartParamColor.ForeColor = Color.White;
            BorderStartParamColor.Location = new Point(262, -18);
            BorderStartParamColor.Name = "BorderStartParamColor";
            BorderStartParamColor.Size = new Size(24, 32);
            BorderStartParamColor.TabIndex = 0;
            BorderStartParamColor.Text = "_";
            // 
            // lDescription
            // 
            lDescription.AutoSize = true;
            lDescription.BackColor = Color.DimGray;
            lDescription.BorderStyle = BorderStyle.FixedSingle;
            lDescription.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lDescription.ForeColor = Color.White;
            lDescription.Location = new Point(794, 12);
            lDescription.Name = "lDescription";
            lDescription.Size = new Size(101, 17);
            lDescription.TabIndex = 2;
            lDescription.Text = "Описание темы";
            // 
            // bCreateNewTheme
            // 
            bCreateNewTheme.BackColor = Color.AntiqueWhite;
            bCreateNewTheme.Location = new Point(36, 450);
            bCreateNewTheme.Name = "bCreateNewTheme";
            bCreateNewTheme.Size = new Size(128, 23);
            bCreateNewTheme.TabIndex = 3;
            bCreateNewTheme.Text = "Создать новую Тему";
            bCreateNewTheme.UseVisualStyleBackColor = false;
            bCreateNewTheme.Click += BCreateNewTheme_Click;
            // 
            // pMiniPanel
            // 
            pMiniPanel.BackColor = Color.FromArgb(45, 72, 59);
            pMiniPanel.BorderStyle = BorderStyle.Fixed3D;
            pMiniPanel.Controls.Add(ButtonDeleteTheme);
            pMiniPanel.Controls.Add(pmpAllButtons);
            pMiniPanel.Controls.Add(bmpButtonBack);
            pMiniPanel.Controls.Add(lmpName);
            pMiniPanel.Location = new Point(794, 39);
            pMiniPanel.Name = "pMiniPanel";
            pMiniPanel.Size = new Size(204, 146);
            pMiniPanel.TabIndex = 4;
            // 
            // ButtonDeleteTheme
            // 
            ButtonDeleteTheme.BackgroundImage = (Image)resources.GetObject("ButtonDeleteTheme.BackgroundImage");
            ButtonDeleteTheme.BackgroundImageLayout = ImageLayout.Zoom;
            ButtonDeleteTheme.Location = new Point(3, 0);
            ButtonDeleteTheme.Name = "ButtonDeleteTheme";
            ButtonDeleteTheme.Size = new Size(21, 23);
            ButtonDeleteTheme.TabIndex = 9;
            ButtonDeleteTheme.Click += ButtonDeleteTheme_Click;
            // 
            // pmpAllButtons
            // 
            pmpAllButtons.BackColor = Color.FromArgb(35, 62, 49);
            pmpAllButtons.BorderStyle = BorderStyle.FixedSingle;
            pmpAllButtons.Controls.Add(bSetActiveTheme);
            pmpAllButtons.Controls.Add(bpmpChangedThemeInfo);
            pmpAllButtons.Controls.Add(bmpSaveChangeTheme);
            pmpAllButtons.Location = new Point(1, 23);
            pmpAllButtons.Name = "pmpAllButtons";
            pmpAllButtons.Size = new Size(198, 116);
            pmpAllButtons.TabIndex = 8;
            // 
            // bSetActiveTheme
            // 
            bSetActiveTheme.BackColor = Color.FromArgb(255, 160, 137);
            bSetActiveTheme.Cursor = Cursors.Hand;
            bSetActiveTheme.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            bSetActiveTheme.Location = new Point(5, 3);
            bSetActiveTheme.Name = "bSetActiveTheme";
            bSetActiveTheme.Size = new Size(110, 23);
            bSetActiveTheme.TabIndex = 7;
            bSetActiveTheme.Text = "Установить тему";
            bSetActiveTheme.TextAlign = ContentAlignment.TopCenter;
            bSetActiveTheme.UseVisualStyleBackColor = false;
            // 
            // bpmpChangedThemeInfo
            // 
            bpmpChangedThemeInfo.BackColor = Color.FromArgb(255, 160, 137);
            bpmpChangedThemeInfo.Cursor = Cursors.Hand;
            bpmpChangedThemeInfo.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            bpmpChangedThemeInfo.Location = new Point(5, 31);
            bpmpChangedThemeInfo.Name = "bpmpChangedThemeInfo";
            bpmpChangedThemeInfo.Size = new Size(187, 23);
            bpmpChangedThemeInfo.TabIndex = 1;
            bpmpChangedThemeInfo.Text = "Изменить информацию темы";
            bpmpChangedThemeInfo.UseVisualStyleBackColor = false;
            bpmpChangedThemeInfo.Click += BpmpChangedThemeInfo_Click;
            // 
            // bmpSaveChangeTheme
            // 
            bmpSaveChangeTheme.BackColor = Color.FromArgb(255, 160, 137);
            bmpSaveChangeTheme.Cursor = Cursors.Hand;
            bmpSaveChangeTheme.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            bmpSaveChangeTheme.Location = new Point(5, 59);
            bmpSaveChangeTheme.Name = "bmpSaveChangeTheme";
            bmpSaveChangeTheme.Size = new Size(167, 23);
            bmpSaveChangeTheme.TabIndex = 4;
            bmpSaveChangeTheme.Text = "Менеджер цветов темы";
            bmpSaveChangeTheme.UseVisualStyleBackColor = false;
            // 
            // bmpButtonBack
            // 
            bmpButtonBack.BackColor = Color.FromArgb(45, 72, 59);
            bmpButtonBack.BackgroundImage = (Image)resources.GetObject("bmpButtonBack.BackgroundImage");
            bmpButtonBack.BackgroundImageLayout = ImageLayout.Zoom;
            bmpButtonBack.Cursor = Cursors.Hand;
            bmpButtonBack.FlatStyle = FlatStyle.Flat;
            bmpButtonBack.ForeColor = Color.FromArgb(45, 72, 59);
            bmpButtonBack.Location = new Point(177, 0);
            bmpButtonBack.Name = "bmpButtonBack";
            bmpButtonBack.Size = new Size(23, 23);
            bmpButtonBack.TabIndex = 2;
            bmpButtonBack.UseVisualStyleBackColor = false;
            // 
            // lmpName
            // 
            lmpName.AutoEllipsis = true;
            lmpName.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lmpName.ForeColor = SystemColors.Info;
            lmpName.Location = new Point(28, 4);
            lmpName.Name = "lmpName";
            lmpName.Size = new Size(153, 15);
            lmpName.TabIndex = 0;
            lmpName.Text = "Действия темы N";
            lmpName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FormThemesEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(55, 62, 79);
            ClientSize = new Size(1007, 476);
            Controls.Add(pMiniPanel);
            Controls.Add(bCreateNewTheme);
            Controls.Add(lDescription);
            Controls.Add(pAllRegisteredThemes);
            Controls.Add(pAllRedactorParamColorTheme);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormThemesEditor";
            Text = "Менеджер тем программы";
            FormClosed += Themes_FormClosed;
            pAllRegisteredThemes.ResumeLayout(false);
            pAllRedactorParamColorTheme.ResumeLayout(false);
            ToolPanelParamColor.ResumeLayout(false);
            ToolPanelParamColor.PerformLayout();
            MultiSearchPanel.ResumeLayout(false);
            pAllParamColorTheme.ResumeLayout(false);
            pAllParamColorTheme.PerformLayout();
            pMiniPanel.ResumeLayout(false);
            pmpAllButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pAllRegisteredThemes;
        private Panel pAllRedactorParamColorTheme;
        private Button bCreateNewTheme;
        public Label lDescription;
        private Label lmpName;
        private Button bmpButtonBack;
        public Panel pMiniPanel;
        private Button bSetActiveTheme;
        private Button bpmpChangedThemeInfo;
        private Button bmpSaveChangeTheme;
        public VScrollBar vSBRegisteredThemes;
        public Panel pmpAllButtons;
        public Panel pAllElementsRegThemes;
        public Panel pAllParamColorTheme;
        protected ColorDialog ColorCreator;
        private GUI.IELImageButton ButtonDeleteTheme;
        private Label BorderEndParamColor;
        private Label BorderStartParamColor;
        private VScrollBar ScrollBarParamColor;
        private Panel ToolPanelParamColor;
        private GUI.IELImageButton ButtonCloseChangeParamColor;
        private Label NameTheme;
        private TextBox SearchParamColorTextBox;
        private GUI.IELImageButton ButtonSearchParamColor;
        private Panel MultiSearchPanel;
        private GUI.IELImageButton ButtonLeftMultiSearchParamColor;
        private GUI.IELImageButton ButtonRightMultiSearchParamColor;
        private Label MultiSearchCounter;
    }
}