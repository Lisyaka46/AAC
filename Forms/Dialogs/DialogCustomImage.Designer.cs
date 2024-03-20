namespace AAC
{
    partial class DialogCustomImage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogCustomImage));
            cbListCustomImage = new ComboBox();
            pbVisibleImage = new PictureBox();
            bComplete = new Button();
            bCancel = new Button();
            lErrorInstallImage = new Label();
            pImage = new Panel();
            bCheckImageInternet = new Button();
            tbDirectoryImageFile = new TextBox();
            bGetImageFile = new Button();
            BrowserDialogImage = new FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)pbVisibleImage).BeginInit();
            pImage.SuspendLayout();
            SuspendLayout();
            // 
            // cbListCustomImage
            // 
            cbListCustomImage.BackColor = Color.Black;
            cbListCustomImage.ForeColor = Color.Bisque;
            cbListCustomImage.FormattingEnabled = true;
            cbListCustomImage.Location = new Point(3, 9);
            cbListCustomImage.Name = "cbListCustomImage";
            cbListCustomImage.Size = new Size(234, 23);
            cbListCustomImage.TabIndex = 0;
            cbListCustomImage.TextChanged += CbListCustomImage_TextChanged;
            // 
            // pbVisibleImage
            // 
            pbVisibleImage.BorderStyle = BorderStyle.Fixed3D;
            pbVisibleImage.ErrorImage = (Image)resources.GetObject("pbVisibleImage.ErrorImage");
            pbVisibleImage.InitialImage = (Image)resources.GetObject("pbVisibleImage.InitialImage");
            pbVisibleImage.Location = new Point(417, 3);
            pbVisibleImage.Name = "pbVisibleImage";
            pbVisibleImage.Size = new Size(34, 34);
            pbVisibleImage.SizeMode = PictureBoxSizeMode.Zoom;
            pbVisibleImage.TabIndex = 1;
            pbVisibleImage.TabStop = false;
            // 
            // bComplete
            // 
            bComplete.BackColor = Color.FromArgb(0, 84, 0);
            bComplete.Cursor = Cursors.Hand;
            bComplete.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            bComplete.Location = new Point(242, 131);
            bComplete.Name = "bComplete";
            bComplete.Size = new Size(81, 23);
            bComplete.TabIndex = 3;
            bComplete.Text = "Установить";
            bComplete.UseVisualStyleBackColor = false;
            bComplete.Click += BComplete_Click;
            bComplete.MouseEnter += BComplete_MouseEnter;
            bComplete.MouseLeave += BComplete_MouseLeave;
            // 
            // bCancel
            // 
            bCancel.BackColor = Color.FromArgb(120, 0, 0);
            bCancel.Cursor = Cursors.Hand;
            bCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            bCancel.Location = new Point(162, 131);
            bCancel.Name = "bCancel";
            bCancel.Size = new Size(58, 23);
            bCancel.TabIndex = 4;
            bCancel.Text = "Отмена";
            bCancel.UseVisualStyleBackColor = false;
            bCancel.Click += BCancel_Click;
            bCancel.MouseEnter += BCancel_MouseEnter;
            bCancel.MouseLeave += BCancel_MouseLeave;
            // 
            // lErrorInstallImage
            // 
            lErrorInstallImage.AutoSize = true;
            lErrorInstallImage.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lErrorInstallImage.ForeColor = Color.White;
            lErrorInstallImage.Location = new Point(1, 48);
            lErrorInstallImage.Name = "lErrorInstallImage";
            lErrorInstallImage.Size = new Size(100, 15);
            lErrorInstallImage.TabIndex = 5;
            lErrorInstallImage.Text = "Файл не найден";
            lErrorInstallImage.TextAlign = ContentAlignment.MiddleLeft;
            lErrorInstallImage.Click += LErrorInstallImage_Click;
            // 
            // pImage
            // 
            pImage.BorderStyle = BorderStyle.FixedSingle;
            pImage.Controls.Add(bCheckImageInternet);
            pImage.Controls.Add(lErrorInstallImage);
            pImage.Controls.Add(tbDirectoryImageFile);
            pImage.Controls.Add(bGetImageFile);
            pImage.Controls.Add(pbVisibleImage);
            pImage.Controls.Add(cbListCustomImage);
            pImage.Location = new Point(5, 6);
            pImage.Name = "pImage";
            pImage.Size = new Size(456, 120);
            pImage.TabIndex = 6;
            // 
            // bCheckImageInternet
            // 
            bCheckImageInternet.BackColor = Color.Black;
            bCheckImageInternet.ForeColor = Color.MistyRose;
            bCheckImageInternet.Location = new Point(258, 91);
            bCheckImageInternet.Name = "bCheckImageInternet";
            bCheckImageInternet.Size = new Size(192, 24);
            bCheckImageInternet.TabIndex = 7;
            bCheckImageInternet.Text = "Проверить ссылку в интернете";
            bCheckImageInternet.UseVisualStyleBackColor = false;
            // 
            // tbDirectoryImageFile
            // 
            tbDirectoryImageFile.BackColor = Color.FromArgb(12, 30, 6);
            tbDirectoryImageFile.Font = new Font("Tahoma", 9F, FontStyle.Bold, GraphicsUnit.Point);
            tbDirectoryImageFile.ForeColor = SystemColors.Info;
            tbDirectoryImageFile.Location = new Point(6, 66);
            tbDirectoryImageFile.Name = "tbDirectoryImageFile";
            tbDirectoryImageFile.ReadOnly = true;
            tbDirectoryImageFile.Size = new Size(443, 22);
            tbDirectoryImageFile.TabIndex = 3;
            tbDirectoryImageFile.Text = "C:\\";
            // 
            // bGetImageFile
            // 
            bGetImageFile.BackColor = Color.Black;
            bGetImageFile.Location = new Point(243, 7);
            bGetImageFile.Name = "bGetImageFile";
            bGetImageFile.Size = new Size(169, 27);
            bGetImageFile.TabIndex = 2;
            bGetImageFile.Text = "Выбрать своё изображение";
            bGetImageFile.UseVisualStyleBackColor = false;
            // 
            // DialogCustomImage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(467, 158);
            ControlBox = false;
            Controls.Add(pImage);
            Controls.Add(bComplete);
            Controls.Add(bCancel);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximumSize = new Size(483, 197);
            MinimumSize = new Size(483, 197);
            Name = "DialogCustomImage";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Custom Image";
            ((System.ComponentModel.ISupportInitialize)pbVisibleImage).EndInit();
            pImage.ResumeLayout(false);
            pImage.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ComboBox cbListCustomImage;
        private PictureBox pbVisibleImage;
        private Button bComplete;
        private Button bCancel;
        private Label lErrorInstallImage;
        private Panel pImage;
        private Button bGetImageFile;
        private TextBox tbDirectoryImageFile;
        private FolderBrowserDialog BrowserDialogImage;
        private Button bCheckImageInternet;
    }
}