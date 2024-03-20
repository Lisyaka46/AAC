using static AAC.Startcs;

namespace AAC
{
    public partial class FormAnimateStart : Form
    {
        public FormAnimateStart()
        {
            InitializeComponent();
        }

        private void FormAnimateStart_Shown(object sender, EventArgs e)
        {
            MainData.MainMP3.PlaySound("StartingFile");
        }
    }
}
