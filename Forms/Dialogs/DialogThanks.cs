using AAC.Classes;
using static AAC.Classes.AnimationDL.Animate.AnimFormule;
using static AAC.Startcs;

namespace AAC
{
    public partial class DialogThanks : Form
    {
        public static Point ChangeFormSize { get; set; } = Screen.PrimaryScreen != null ? new(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height) : new(10, 10);

        public static bool Fast { get; set; } = false;

        public DialogThanks()
        {
            InitializeComponent();
            Size = new Size(ChangeFormSize.X, ChangeFormSize.Y);

            lGlobalProName.Location = new(lGlobalProName.Size.Height - 5, lGlobalName.Size.Height - lGlobalProName.Size.Height);
            pThanksCreators.Location = new((ChangeFormSize.X / 2) - (pThanksCreators.Size.Width / 2), ChangeFormSize.Y + 5);
            lGlobalName.Location = new((ChangeFormSize.X / 2) - (lGlobalName.Size.Width / 2), 5);
            pThanksCreators.Size = new(691, Size.Height - 100);
            pLabelsThanks.Location = new(pLabelsThanks.Location.X, pThanksCreators.Size.Height + 100);
            //SpecialColor.RGBLabel.Add(lThanksThis);
        }

        public void CloseForm(object sender, EventArgs e)
        {
            Apps.MainForm.UnfoldingOpacityApplication();
            AnimationDL.StopAnimate(AnimationDL.StyleAnimateObj.AnimFormule, lGlobalProName.Name);
            AnimationDL.StopAnimate(AnimationDL.StyleAnimateObj.AnimFormule, pThanksCreators.Name);
            Close();
        }

        private void DialogThanks_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                Fast = false;
            }
        }

        private void DialogThanks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Apps.Thanks = null;
                CloseForm(null, null);
            }
            else if (e.KeyCode == Keys.Space)
            {
                Fast = true;
            }
        }

        private async void DialogThanks_Shown(object sender, EventArgs e)
        {
            Activate();

            lGlobalName.Location = new Point(4, lGlobalName.Location.Y);
            await Task.Run(() =>
            {
                Thread.Sleep(1000);
                ConstAnimMove TaskConstantFormule = new(lGlobalProName.Location.X, 4 + lGlobalName.Size.Width + 3, 12);
                TaskConstantFormule.InitAnimFormule(lGlobalProName, Formules.QuickTransition, new ConstAnimMove(lGlobalProName.Location.Y), AnimationStyle.XY);
            });

            ConstAnimMove ConstantFormule = new(pThanksCreators.Location.Y, Size.Height - pThanksCreators.Size.Height, 45);
            new ConstAnimMove(pThanksCreators.Location.X).InitAnimFormule(pThanksCreators, Formules.QuickTransition, ConstantFormule, AnimationStyle.XY);

            await Task.Run(() =>
            {
                try
                {
                    Thread.Sleep(100);
                    int i = 0;
                    while (pLabelsThanks.Location.Y + pLabelsThanks.Size.Height > lThanksThis.Size.Height)
                    {
                        i++;
                        pLabelsThanks.Location = new Point(pLabelsThanks.Location.X, Apps.Thanks.pThanksCreators.Size.Height + 5 - i);
                        pKvadre.Location = new(151 - (i / 7), 57);
                        lKvadreINFO.Location = new(-80 + (i / 5), 116);
                        lIcons.Location = new(3 + (i / 19), 7);
                        lMayorIconsINFO.Location = new(46 - (i / 12), 32);
                        pb1.Location = new(40 - (i / 3), 60 + (i / 5));
                        lHellyHellINFO2.Location = new(34 + (i / 23), 227);
                        Thread.Sleep(Fast ? 10 : 100);
                    }
                    ConstAnimMove ConstantFormule = new(pThanksCreators.Location.Y, Size.Height, 20);
                    new ConstAnimMove(pThanksCreators.Location.X).InitAnimFormule(pThanksCreators, Formules.QuickTransition, ConstantFormule, AnimationStyle.XY);

                    Thread.Sleep(700);
                }
                catch { }
                CloseForm(null, null);
            });
        }
    }
}
