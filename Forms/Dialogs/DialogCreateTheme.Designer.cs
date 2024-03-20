namespace AAC.Forms.Dialogs
{
    partial class DialogCreateTheme
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogCreateTheme));
            tbNameTheme = new TextBox();
            tbNameFileTheme = new TextBox();
            lFileIndex = new Label();
            cbMainThemeDelegat = new ComboBox();
            lImportTheme = new Label();
            tbDesctiprion = new TextBox();
            label2 = new Label();
            pCreateTheme = new Panel();
            pbIconTheme = new PictureBox();
            lInfo = new Label();
            cbDirectorySaveFile = new ComboBox();
            bCancel = new Button();
            bComplete = new Button();
            ChangeIconTheme = new OpenFileDialog();
            pCreateTheme.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbIconTheme).BeginInit();
            SuspendLayout();
            // 
            // tbNameTheme
            // 
            tbNameTheme.BackColor = Color.Silver;
            tbNameTheme.BorderStyle = BorderStyle.FixedSingle;
            tbNameTheme.Location = new Point(63, 3);
            tbNameTheme.MaxLength = 128;
            tbNameTheme.Name = "tbNameTheme";
            tbNameTheme.Size = new Size(195, 23);
            tbNameTheme.TabIndex = 2;
            tbNameTheme.Tag = "Имя темы отображаемое в редакторе";
            tbNameTheme.MouseLeave += Mouse_Leave;
            tbNameTheme.MouseHover += MouseHoverDescriptionTB;
            // 
            // tbNameFileTheme
            // 
            tbNameFileTheme.BackColor = Color.DarkSeaGreen;
            tbNameFileTheme.Location = new Point(158, 162);
            tbNameFileTheme.MaxLength = 100;
            tbNameFileTheme.Name = "tbNameFileTheme";
            tbNameFileTheme.Size = new Size(146, 23);
            tbNameFileTheme.TabIndex = 4;
            // 
            // lFileIndex
            // 
            lFileIndex.AutoSize = true;
            lFileIndex.Location = new Point(304, 167);
            lFileIndex.Name = "lFileIndex";
            lFileIndex.Size = new Size(49, 15);
            lFileIndex.TabIndex = 5;
            lFileIndex.Text = "._theme";
            // 
            // cbMainThemeDelegat
            // 
            cbMainThemeDelegat.BackColor = Color.DarkSeaGreen;
            cbMainThemeDelegat.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMainThemeDelegat.FormattingEnabled = true;
            cbMainThemeDelegat.Location = new Point(49, 111);
            cbMainThemeDelegat.Name = "cbMainThemeDelegat";
            cbMainThemeDelegat.Size = new Size(262, 23);
            cbMainThemeDelegat.TabIndex = 7;
            // 
            // lImportTheme
            // 
            lImportTheme.AutoSize = true;
            lImportTheme.Font = new Font("Verdana", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lImportTheme.Location = new Point(117, 96);
            lImportTheme.Name = "lImportTheme";
            lImportTheme.Size = new Size(114, 14);
            lImportTheme.TabIndex = 8;
            lImportTheme.Text = "Основная тема:";
            // 
            // tbDesctiprion
            // 
            tbDesctiprion.BackColor = Color.Silver;
            tbDesctiprion.BorderStyle = BorderStyle.FixedSingle;
            tbDesctiprion.Location = new Point(63, 32);
            tbDesctiprion.MaxLength = 128;
            tbDesctiprion.Name = "tbDesctiprion";
            tbDesctiprion.Size = new Size(195, 23);
            tbDesctiprion.TabIndex = 12;
            tbDesctiprion.Tag = "Описание темы";
            tbDesctiprion.MouseLeave += Mouse_Leave;
            tbDesctiprion.MouseHover += MouseHoverDescriptionTB;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Verdana", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(6, 145);
            label2.Name = "label2";
            label2.Size = new Size(221, 14);
            label2.TabIndex = 11;
            label2.Text = "Имя и директория файла темы:";
            // 
            // pCreateTheme
            // 
            pCreateTheme.BackColor = Color.Gray;
            pCreateTheme.BorderStyle = BorderStyle.FixedSingle;
            pCreateTheme.Controls.Add(pbIconTheme);
            pCreateTheme.Controls.Add(tbNameTheme);
            pCreateTheme.Controls.Add(tbDesctiprion);
            pCreateTheme.Location = new Point(48, 13);
            pCreateTheme.Name = "pCreateTheme";
            pCreateTheme.Size = new Size(265, 65);
            pCreateTheme.TabIndex = 13;
            // 
            // pbIconTheme
            // 
            pbIconTheme.InitialImage = (Image)resources.GetObject("pbIconTheme.InitialImage");
            pbIconTheme.Location = new Point(6, 4);
            pbIconTheme.Name = "pbIconTheme";
            pbIconTheme.Size = new Size(51, 51);
            pbIconTheme.SizeMode = PictureBoxSizeMode.Zoom;
            pbIconTheme.TabIndex = 13;
            pbIconTheme.TabStop = false;
            pbIconTheme.Tag = "Иконка темы";
            pbIconTheme.DoubleClick += PbIconTheme_DoubleClick;
            pbIconTheme.MouseLeave += Mouse_Leave;
            pbIconTheme.MouseHover += MouseHoverDescriptionPB;
            // 
            // lInfo
            // 
            lInfo.AutoSize = true;
            lInfo.BackColor = Color.DarkGray;
            lInfo.BorderStyle = BorderStyle.FixedSingle;
            lInfo.Location = new Point(12, 201);
            lInfo.Name = "lInfo";
            lInfo.Size = new Size(58, 17);
            lInfo.TabIndex = 14;
            lInfo.Text = "Опиание";
            // 
            // cbDirectorySaveFile
            // 
            cbDirectorySaveFile.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDirectorySaveFile.FormattingEnabled = true;
            cbDirectorySaveFile.Location = new Point(7, 162);
            cbDirectorySaveFile.Name = "cbDirectorySaveFile";
            cbDirectorySaveFile.Size = new Size(145, 23);
            cbDirectorySaveFile.TabIndex = 15;
            // 
            // bCancel
            // 
            bCancel.Location = new Point(77, 200);
            bCancel.Name = "bCancel";
            bCancel.Size = new Size(75, 23);
            bCancel.TabIndex = 16;
            bCancel.Text = "Отмена";
            bCancel.UseVisualStyleBackColor = true;
            bCancel.Click += Cancel;
            // 
            // bComplete
            // 
            bComplete.Location = new Point(194, 200);
            bComplete.Name = "bComplete";
            bComplete.Size = new Size(75, 23);
            bComplete.TabIndex = 17;
            bComplete.Text = "Создать";
            bComplete.UseVisualStyleBackColor = true;
            bComplete.Click += Complete;
            // 
            // ChangeIconTheme
            // 
            ChangeIconTheme.Filter = "Image Theme|*.ico;*.png";
            ChangeIconTheme.FileOk += ChangeIconTheme_FileOk;
            // 
            // DialogCreateTheme
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DarkSeaGreen;
            ClientSize = new Size(362, 227);
            Controls.Add(bComplete);
            Controls.Add(bCancel);
            Controls.Add(cbDirectorySaveFile);
            Controls.Add(lInfo);
            Controls.Add(pCreateTheme);
            Controls.Add(lImportTheme);
            Controls.Add(cbMainThemeDelegat);
            Controls.Add(tbNameFileTheme);
            Controls.Add(label2);
            Controls.Add(lFileIndex);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(378, 266);
            MinimizeBox = false;
            MinimumSize = new Size(378, 266);
            Name = "DialogCreateTheme";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Настройки создаваемой темы";
            pCreateTheme.ResumeLayout(false);
            pCreateTheme.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbIconTheme).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox tbNameTheme;
        private TextBox tbNameFileTheme;
        private Label lFileIndex;
        private ComboBox cbMainThemeDelegat;
        private Label lImportTheme;
        private TextBox tbDesctiprion;
        private Label label2;
        private Panel pCreateTheme;
        private Label lInfo;
        private PictureBox pbIconTheme;
        private ComboBox cbDirectorySaveFile;
        private Button bCancel;
        private Button bComplete;
        public OpenFileDialog ChangeIconTheme;
    }
}