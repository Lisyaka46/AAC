using static AAC.Startcs;

namespace AAC
{
    public partial class FormAnimateStart : Form
    {
        public FormAnimateStart()
        {
            InitializeComponent();
            Shown += async (sender, e) =>
            {
                MainData.MainMP3.PlaySound("StartingSound");
                await Task.Run(() =>
                {
                    while (Opacity > 0d)
                    {
                        Opacity -= 0.009d;
                        Thread.Sleep(1);
                    }
                    Opacity = 0d;
                    Visible = false;
                    Close();
                });
            };
        }
    }
}
