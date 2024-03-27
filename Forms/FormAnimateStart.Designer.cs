namespace AAC
{
    partial class FormAnimateStart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAnimateStart));
            pbStartingGif = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pbStartingGif).BeginInit();
            SuspendLayout();
            // 
            // pbStartingGif
            // 
            pbStartingGif.BackColor = Color.FromArgb(7, 25, 83);
            pbStartingGif.Image = (Image)resources.GetObject("pbStartingGif.Image");
            pbStartingGif.Location = new Point(12, 3);
            pbStartingGif.Name = "pbStartingGif";
            pbStartingGif.Size = new Size(79, 78);
            pbStartingGif.SizeMode = PictureBoxSizeMode.Zoom;
            pbStartingGif.TabIndex = 0;
            pbStartingGif.TabStop = false;
            // 
            // FormAnimateStart
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.FromArgb(7, 25, 83);
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(103, 84);
            ControlBox = false;
            Controls.Add(pbStartingGif);
            FormBorderStyle = FormBorderStyle.None;
            ImeMode = ImeMode.Off;
            MaximizeBox = false;
            MaximumSize = new Size(103, 84);
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(103, 84);
            Name = "FormAnimateStart";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            TopMost = true;
            TransparencyKey = Color.FromArgb(7, 25, 83);
            ((System.ComponentModel.ISupportInitialize)pbStartingGif).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pbStartingGif;
    }
}