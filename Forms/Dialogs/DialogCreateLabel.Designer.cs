namespace AAC.Forms.Dialogs
{
    partial class DialogCreateLabel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogCreateLabel));
            tbNameLabel = new TextBox();
            cbStyleLabel = new ComboBox();
            pbPreviewIconLabelNormal = new PictureBox();
            pbPreviewIconLabelMinimal = new PictureBox();
            pPreview = new Panel();
            lPreview5050 = new Label();
            lPreview2121 = new Label();
            lNameLabelInfo = new Label();
            lStyleLabelInfo = new Label();
            bCreateActionLabel = new Button();
            tbPreviewAction = new TextBox();
            lActionLabelInfo = new Label();
            bComplete = new Button();
            bCancel = new Button();
            cbPriorityLabel = new CheckBox();
            BrowserDialogFolder = new FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)pbPreviewIconLabelNormal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbPreviewIconLabelMinimal).BeginInit();
            pPreview.SuspendLayout();
            SuspendLayout();
            // 
            // tbNameLabel
            // 
            tbNameLabel.BackColor = Color.White;
            tbNameLabel.Cursor = Cursors.IBeam;
            tbNameLabel.Location = new Point(125, 26);
            tbNameLabel.MaxLength = 9;
            tbNameLabel.Name = "tbNameLabel";
            tbNameLabel.Size = new Size(145, 23);
            tbNameLabel.TabIndex = 0;
            tbNameLabel.TextAlign = HorizontalAlignment.Center;
            // 
            // cbStyleLabel
            // 
            cbStyleLabel.BackColor = Color.White;
            cbStyleLabel.Cursor = Cursors.Hand;
            cbStyleLabel.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStyleLabel.FormattingEnabled = true;
            cbStyleLabel.Items.AddRange(new object[] { "Открытие директории", "Выполнение команды", "Открытие ссылки" });
            cbStyleLabel.Location = new Point(123, 78);
            cbStyleLabel.Name = "cbStyleLabel";
            cbStyleLabel.Size = new Size(147, 23);
            cbStyleLabel.TabIndex = 1;
            // 
            // pbPreviewIconLabelNormal
            // 
            pbPreviewIconLabelNormal.BorderStyle = BorderStyle.FixedSingle;
            pbPreviewIconLabelNormal.Cursor = Cursors.No;
            pbPreviewIconLabelNormal.Location = new Point(48, 26);
            pbPreviewIconLabelNormal.Name = "pbPreviewIconLabelNormal";
            pbPreviewIconLabelNormal.Size = new Size(40, 40);
            pbPreviewIconLabelNormal.SizeMode = PictureBoxSizeMode.Zoom;
            pbPreviewIconLabelNormal.TabIndex = 2;
            pbPreviewIconLabelNormal.TabStop = false;
            // 
            // pbPreviewIconLabelMinimal
            // 
            pbPreviewIconLabelMinimal.BorderStyle = BorderStyle.FixedSingle;
            pbPreviewIconLabelMinimal.Cursor = Cursors.No;
            pbPreviewIconLabelMinimal.Location = new Point(12, 45);
            pbPreviewIconLabelMinimal.Name = "pbPreviewIconLabelMinimal";
            pbPreviewIconLabelMinimal.Size = new Size(21, 21);
            pbPreviewIconLabelMinimal.SizeMode = PictureBoxSizeMode.Zoom;
            pbPreviewIconLabelMinimal.TabIndex = 3;
            pbPreviewIconLabelMinimal.TabStop = false;
            // 
            // pPreview
            // 
            pPreview.BorderStyle = BorderStyle.FixedSingle;
            pPreview.Controls.Add(lPreview5050);
            pPreview.Controls.Add(lPreview2121);
            pPreview.Controls.Add(pbPreviewIconLabelMinimal);
            pPreview.Controls.Add(pbPreviewIconLabelNormal);
            pPreview.Cursor = Cursors.Cross;
            pPreview.Location = new Point(12, 12);
            pPreview.Name = "pPreview";
            pPreview.Size = new Size(103, 86);
            pPreview.TabIndex = 4;
            // 
            // lPreview5050
            // 
            lPreview5050.AutoSize = true;
            lPreview5050.Font = new Font("Segoe UI", 7F);
            lPreview5050.Location = new Point(53, 11);
            lPreview5050.Name = "lPreview5050";
            lPreview5050.Size = new Size(30, 12);
            lPreview5050.TabIndex = 6;
            lPreview5050.Text = "40x40";
            // 
            // lPreview2121
            // 
            lPreview2121.AutoSize = true;
            lPreview2121.Font = new Font("Segoe UI", 7F);
            lPreview2121.Location = new Point(8, 31);
            lPreview2121.Name = "lPreview2121";
            lPreview2121.Size = new Size(30, 12);
            lPreview2121.TabIndex = 5;
            lPreview2121.Text = "21x21";
            // 
            // lNameLabelInfo
            // 
            lNameLabelInfo.AutoSize = true;
            lNameLabelInfo.Location = new Point(123, 10);
            lNameLabelInfo.Name = "lNameLabelInfo";
            lNameLabelInfo.Size = new Size(78, 15);
            lNameLabelInfo.TabIndex = 5;
            lNameLabelInfo.Text = "Имя ярлыка:";
            // 
            // lStyleLabelInfo
            // 
            lStyleLabelInfo.AutoSize = true;
            lStyleLabelInfo.Location = new Point(121, 62);
            lStyleLabelInfo.Name = "lStyleLabelInfo";
            lStyleLabelInfo.Size = new Size(139, 15);
            lStyleLabelInfo.TabIndex = 7;
            lStyleLabelInfo.Text = "Стиль действия ярлыка:";
            // 
            // bCreateActionLabel
            // 
            bCreateActionLabel.Cursor = Cursors.Hand;
            bCreateActionLabel.Location = new Point(131, 107);
            bCreateActionLabel.Name = "bCreateActionLabel";
            bCreateActionLabel.Size = new Size(130, 23);
            bCreateActionLabel.TabIndex = 8;
            bCreateActionLabel.Text = "Задать директорию";
            bCreateActionLabel.UseVisualStyleBackColor = true;
            bCreateActionLabel.Click += CreateActionLabelActivate;
            // 
            // tbPreviewAction
            // 
            tbPreviewAction.BackColor = Color.White;
            tbPreviewAction.Cursor = Cursors.No;
            tbPreviewAction.Location = new Point(12, 137);
            tbPreviewAction.Name = "tbPreviewAction";
            tbPreviewAction.Size = new Size(258, 23);
            tbPreviewAction.TabIndex = 9;
            // 
            // lActionLabelInfo
            // 
            lActionLabelInfo.AutoSize = true;
            lActionLabelInfo.Location = new Point(12, 121);
            lActionLabelInfo.Name = "lActionLabelInfo";
            lActionLabelInfo.Size = new Size(105, 15);
            lActionLabelInfo.TabIndex = 10;
            lActionLabelInfo.Text = "Действие ярлыка:";
            // 
            // bComplete
            // 
            bComplete.Cursor = Cursors.Hand;
            bComplete.Location = new Point(195, 184);
            bComplete.Name = "bComplete";
            bComplete.Size = new Size(75, 23);
            bComplete.TabIndex = 11;
            bComplete.Text = "Создать";
            bComplete.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            bCancel.Cursor = Cursors.Hand;
            bCancel.Location = new Point(112, 184);
            bCancel.Name = "bCancel";
            bCancel.Size = new Size(75, 23);
            bCancel.TabIndex = 12;
            bCancel.Text = "Отмена";
            bCancel.UseVisualStyleBackColor = true;
            // 
            // cbPriorityLabel
            // 
            cbPriorityLabel.AutoSize = true;
            cbPriorityLabel.Location = new Point(12, 163);
            cbPriorityLabel.Name = "cbPriorityLabel";
            cbPriorityLabel.Size = new Size(147, 19);
            cbPriorityLabel.TabIndex = 13;
            cbPriorityLabel.Text = "Приоритетный ярлык";
            cbPriorityLabel.UseVisualStyleBackColor = true;
            // 
            // BrowserDialogFolder
            // 
            BrowserDialogFolder.Description = "Выберите открываемую папку для ярлыка";
            BrowserDialogFolder.ShowNewFolderButton = false;
            // 
            // DialogCreateLabel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 215);
            Controls.Add(cbPriorityLabel);
            Controls.Add(bCancel);
            Controls.Add(bComplete);
            Controls.Add(tbPreviewAction);
            Controls.Add(bCreateActionLabel);
            Controls.Add(lNameLabelInfo);
            Controls.Add(pPreview);
            Controls.Add(cbStyleLabel);
            Controls.Add(tbNameLabel);
            Controls.Add(lStyleLabelInfo);
            Controls.Add(lActionLabelInfo);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "DialogCreateLabel";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Создание ярлыка";
            ((System.ComponentModel.ISupportInitialize)pbPreviewIconLabelNormal).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbPreviewIconLabelMinimal).EndInit();
            pPreview.ResumeLayout(false);
            pPreview.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbNameLabel;
        private ComboBox cbStyleLabel;
        private PictureBox pbPreviewIconLabelNormal;
        private PictureBox pbPreviewIconLabelMinimal;
        private Panel pPreview;
        private Label lPreview2121;
        private Label lPreview5050;
        private Label lNameLabelInfo;
        private Label lStyleLabelInfo;
        private Button bCreateActionLabel;
        private TextBox tbPreviewAction;
        private Label lActionLabelInfo;
        private Button bComplete;
        private Button bCancel;
        private CheckBox cbPriorityLabel;
        private FolderBrowserDialog BrowserDialogFolder;
    }
}