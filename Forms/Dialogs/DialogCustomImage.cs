using static AAC.Classes.AnimationDL.Animate.AnimColor;
using static AAC.Startcs;

namespace AAC
{
    public partial class DialogCustomImage : Form
    {
        public DialogCustomImage()
        {
            InitializeComponent();
            lErrorInstallImage.ForeColor = Color.Black;
            cbListCustomImage.Items.Clear();
            pbVisibleImage.ImageLocation = string.Empty;
            ObjLog.LOGTextAppend("Программа изучает добавленные GIF анимации");
            foreach (string element in Directory.GetFiles($"{Directory.GetCurrentDirectory()}\\Data\\Image\\Custom\\"))
            {
                if (Path.GetExtension(element).Equals(".gif"))
                    cbListCustomImage.Items.Add(element.Replace($"{Directory.GetCurrentDirectory()}\\Data\\Image\\Custom\\", string.Empty));
            }
            cbListCustomImage.Text = cbListCustomImage.Items[0].ToString();
            tbDirectoryImageFile.Text = App.MainForm.pbCustom.ImageLocation;
        }
        private void BComplete_Click(object sender, EventArgs e)
        {
            ObjLog.LOGTextAppend("Была нажата кнопка установки GIF анимации");
            App.MainForm.pbCustom.ImageLocation = pbVisibleImage.ImageLocation;
            Close();
        }
        private void BComplete_MouseEnter(object sender, EventArgs e)
        {
            bComplete.BackColor = Color.FromArgb(0, 120, 0);
        }
        private void BComplete_MouseLeave(object sender, EventArgs e)
        {
            bComplete.BackColor = Color.FromArgb(0, 84, 0);
        }

        private void BCancel_MouseEnter(object sender, EventArgs e)
        {
            bCancel.BackColor = Color.FromArgb(150, 0, 0);
        }
        private void BCancel_MouseLeave(object sender, EventArgs e)
        {
            bCancel.BackColor = Color.FromArgb(120, 0, 0);
        }
        private void BCancel_Click(object sender, EventArgs e)
        {
            ObjLog.LOGTextAppend("Была нажата кнопка отмены установки GIF анимации");
            Close();
        }
        private void CbListCustomImage_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists($"{Directory.GetCurrentDirectory()}\\Data\\Image\\Custom\\{cbListCustomImage.Text}"))
            {
                bComplete.Cursor = Cursors.Hand;
                pbVisibleImage.Image = Image.FromFile($"{Directory.GetCurrentDirectory()}\\Data\\Image\\Custom\\{cbListCustomImage.Text}");
            }
            else
            {
                bComplete.Cursor = Cursors.No;
                lErrorInstallImage.Text = "файл не найден";
                LErrorInstallImage_Click(null, null);
                pbVisibleImage.ImageLocation = string.Empty;
                pbVisibleImage.Image?.Dispose();
                pbVisibleImage.Refresh();
            }
        }

        private void LErrorInstallImage_Click(object sender, EventArgs e)
        {
            ConstAnimColor constAnim = new(Color.Red, Color.Black, 4);
            constAnim.AnimInit(lErrorInstallImage, AnimStyleColor.ForeColor);
        }
    }
}
