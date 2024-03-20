using AAC.Classes;
using System.ComponentModel;
using static AAC.Forms.FormThemesEditor;
using static AAC.Startcs;

namespace AAC.Forms.Dialogs
{
    public partial class DialogCreateTheme : Form
    {
        public MainTheme.Theme CreateNewTheme;
        public string NewImageLocation;
        public bool ChangeInfo = false;
        public DialogCreateTheme(MainTheme.Theme CreateTheme, bool Change = false)
        {
            ChangeInfo = Change;
            CreateNewTheme = CreateTheme;
            NewImageLocation = CreateTheme.IconDirectory;
            InitializeComponent();
            lInfo.Hide();
            tbNameTheme.Text = CreateTheme.Name;
            tbNameFileTheme.Text = !ChangeInfo ? Directory.EnumerateFiles($"{Directory.GetCurrentDirectory()}\\Data\\Theme\\").Contains($"FileTheme_{CreateTheme.Name}._theme") ?
                $"FileTheme_{CreateTheme.Name}+" : $"FileTheme_{CreateTheme.Name}" : Path.GetFileName(CreateTheme.FileDirectory)?.Replace("._theme", string.Empty);
            tbDesctiprion.Text = !ChangeInfo ? "Описание..." : CreateTheme.Description;
            pbIconTheme.ImageLocation = NewImageLocation;
            if (NewImageLocation.Contains("http:") || NewImageLocation.Contains("https:"))
                pbIconTheme.Refresh();
            else pbIconTheme.Image = Image.FromFile(CreateNewTheme.IconDirectory);
            Task.Run(LocationChange);
            cbMainThemeDelegat.Items.Clear();
            cbDirectorySaveFile.Items.Clear();
            if (!ChangeInfo)
            {
                foreach (MainTheme.Theme Element in MainData.MainThemeData.MassTheme)
                    cbMainThemeDelegat.Items.Add(Element.Name);
                int IndexTheme = MainData.MainThemeData.MassTheme.IndexOf(MainData.MainThemeData.ActivateTheme);
                cbMainThemeDelegat.Text = cbMainThemeDelegat.Items[IndexTheme + 1].ToString();
                cbDirectorySaveFile.Items.Add($"{Directory.GetCurrentDirectory()}\\Data\\\\Theme\\\\");
                cbDirectorySaveFile.Text = cbDirectorySaveFile.Items[0].ToString();
            }
            else
            {
                Text = "Изменение информации для темы";
                cbMainThemeDelegat.Items.Add("Custom");
                cbMainThemeDelegat.Text = cbMainThemeDelegat.Items[0].ToString();
                cbDirectorySaveFile.Items.Add((Path.GetDirectoryName(CreateTheme.FileDirectory) + "\\")?.Replace("\\", "\\\\"));
            }
            cbDirectorySaveFile.Text = cbDirectorySaveFile.Items[0].ToString();
            if (ChangeInfo)
            {
                bComplete.Text = "Изменить";
                tbNameFileTheme.ReadOnly = true;
            }
        }

        private void MouseHoverDescriptionPB(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            lInfo.Text = pb.Tag.ToString();
            lInfo.Show();
        }

        private void MouseHoverDescriptionTB(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            lInfo.Text = tb.Tag.ToString();
            lInfo.Show();
        }

        private void Mouse_Leave(object sender, EventArgs e)
        {
            lInfo.Hide();
        }

        private void LocationChange()
        {
            try
            {
                while (this != null)
                    if (lInfo.Visible) lInfo.Location =
                            new Point(Cursor.Position.X - Location.X - 3, Cursor.Position.Y - Location.Y - 50);
            }
            catch { }
        }

        private void Complete(object sender, EventArgs e)
        {
            List<char> NotSymbolNameFile = ['\\', '/', ':', '*', '?', '"', '<', '>', '|' ];
            if (!ChangeInfo)
            {
                foreach (char Symbol in NotSymbolNameFile)
                {
                    if (tbNameFileTheme.Text.Contains(Symbol))
                    {
                        //Instr_AnimInstruction Animate = new(tbNameFileTheme, CommandInfo.AnimationFollow["Shake"], null, (1, 0), AnimMove.AnimationStyle.XY);
                        //Animate.AnimInit(new(158, 162), new(158, 162));
                        return;
                    }
                }
                string DirectoryFileTheme = cbDirectorySaveFile.Text + tbNameFileTheme.Text + "._theme";
                if (File.Exists(DirectoryFileTheme))
                {
                    //AnimMove.Instr_AnimInstruction Animate = new(tbNameFileTheme, CommandInfo.AnimationFollow["Shake"], null, (1, 0), AnimMove.AnimationStyle.XY);
                    //Animate.AnimInit(new(158, 162), new(158, 162));
                    return;
                }
                if (!cbMainThemeDelegat.Text.Equals("Default"))
                {
                    foreach (MainTheme.Theme Element in MainData.MainThemeData.MassTheme)
                    {
                        if (Element.Name.Equals(cbMainThemeDelegat.Text))
                        {
                            CreateNewTheme = new(MainData.MainThemeData.MassInfoParameters, Element, null, tbNameTheme.Text, tbDesctiprion.Text, pbIconTheme.ImageLocation);
                            break;
                        }
                    }
                }
                else
                {
                    CreateNewTheme = new(MainData.MainThemeData.MassInfoParameters, MainData.MainThemeData.Default, null, tbNameTheme.Text, tbDesctiprion.Text, pbIconTheme.ImageLocation);
                }
                //App.Settings.ThemesCreated.AppendNewTheme(ElementIlustrationTheme);
                CreateNewFileTheme(CreateNewTheme, DirectoryFileTheme);
            }
            else
            {
                CreateNewTheme = new(MainData.MainThemeData.MassInfoParameters, CreateNewTheme, null, tbNameTheme.Text, tbDesctiprion.Text, pbIconTheme.ImageLocation);

                /*ThemePanel Element = App.Settings.ThemesCreated.MassThemePanel[MainData.MainThemeData.MassTheme.IndexOf(CreateNewTheme)];

                Element.IconPanelTheme.ImageLocation = CreateNewTheme.IconDirectory;
                Element.IconPanelTheme.Refresh();

                Element.LabelNameTheme.Text = tbNameTheme.Text;
                Element.LabelDescriptionTheme.Tag = tbDesctiprion.Text;
                */
                App.Settings.ThemesCreated.SaveThemeInFile(CreateNewTheme);
            }
            Close();
        }

        private void Cancel(object sender, EventArgs e)
        {
            Close();
        }

        private void PbIconTheme_DoubleClick(object sender, EventArgs e)
        {
            ChangeIconTheme.Title = "Изменение иконки темы";
            ChangeIconTheme.InitialDirectory = Directory.GetCurrentDirectory();
            ChangeIconTheme.Multiselect = false;
            ChangeIconTheme.CheckFileExists = true;
            ChangeIconTheme.ShowDialog(this);
        }

        private void ChangeIconTheme_FileOk(object sender, CancelEventArgs e)
        {
            pbIconTheme.Image = Image.FromFile(ChangeIconTheme.FileName);
            pbIconTheme.ImageLocation = ChangeIconTheme.FileName;
        }
    }
}
