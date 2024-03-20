using AAC.GUI;
using static AAC.Classes.AnimationDL.Animate.AnimColor;
using static AAC.Startcs;

namespace AAC.Forms.Dialogs
{
    public partial class DialogCreateLabel : Form
    {
        /// <summary>
        /// Кэш сохранённого текста действия ярлыка
        /// </summary>
        private string[] CacheActionText { get; set; }

        /// <summary>
        /// Кэш старого индекса активного стиля действия
        /// </summary>
        private int CacheSelectedIndex { get; set; }

        /// <summary>
        /// Генерируемый объект
        /// </summary>
        private InfoLabelAccess? GenerateLabelInfo { get; set; }

        public DialogCreateLabel()
        {
            InitializeComponent();
            GenerateLabelInfo = null;
            CacheActionText = new string[cbStyleLabel.Items.Count];
            CacheSelectedIndex = 0;
            MaximumSize = Size;
            MinimumSize = Size;
            cbPriorityLabel.Checked = false;
            cbStyleLabel.SelectedIndex = CacheSelectedIndex;
            tbPreviewAction.ReadOnly = true;
            bCancel.Click += (sender, e) => Close();
            bComplete.Click += (sender, e) =>
            {
                GenerateLabelInfo = GenerateInfoObject();
                if (GenerateLabelInfo.HasValue) Close();
                else
                {
                    ConstAnimColor RedInstall = new(Color.DarkRed, Color.White, 4);
                    if (tbNameLabel.TextLength == 0)
                    {
                        RedInstall.AnimInit(tbNameLabel, AnimStyleColor.BackColor);
                    }
                    if (tbPreviewAction.TextLength == 0)
                    {
                        RedInstall.AnimInit(tbPreviewAction, AnimStyleColor.BackColor);
                    }
                }
            };
            cbPriorityLabel.CheckedChanged += (sender, e) => ChangeImagePreviewLabel(cbStyleLabel.SelectedIndex, cbPriorityLabel.Checked);
            cbStyleLabel.SelectedIndexChanged += (sender, e) =>
            {
                CacheActionText[CacheSelectedIndex] = tbPreviewAction.Text;
                tbPreviewAction.Text = CacheActionText[cbStyleLabel.SelectedIndex];
                CacheSelectedIndex = cbStyleLabel.SelectedIndex;
                ChangeImagePreviewLabel(cbStyleLabel.SelectedIndex, cbPriorityLabel.Checked);
                ChangedElement(cbStyleLabel.SelectedIndex);
            };
            ChangeImagePreviewLabel(cbStyleLabel.SelectedIndex, cbPriorityLabel.Checked);
        }

        /// <summary>
        /// Сгенерировать объект ярлыка через предоставляемую форму создания
        /// </summary>
        /// <param name="ApplicationDialog">Форма в которой открывается диалоговое окно</param>
        /// <param name="Parent">Панель парента, где будет находиться объект ярлыка</param>
        /// <returns>Настроенный объект ярлыка <b>с нулевым индексом позиции</b></returns>
        public IELLabelAccess? GenerateLabelAccess(Form ApplicationDialog, Panel? Parent)
        {
            ShowDialog(ApplicationDialog);
            if (GenerateLabelInfo.HasValue)
                return new IELLabelAccess(GenerateLabelInfo.Value, Parent, cbPriorityLabel.Checked ? TypeLabel.Priority : TypeLabel.Default, 0);
            else return null;
        }

        private void ChangedElement(int index)
        {
            bCreateActionLabel.Text = index switch
            {
                0 => "Задать директорию",
                1 => "Задать команду",
                2 => "Задать ссылку",
                _ => string.Empty
            };
            switch (index)
            {
                case 0:
                    tbPreviewAction.ReadOnly = true;
                    tbPreviewAction.MaxLength = 32767;
                    tbPreviewAction.Cursor = Cursors.No;
                    break;
                case 1:
                case 2:
                    tbPreviewAction.ReadOnly = false;
                    tbPreviewAction.MaxLength = App.MainForm.tbInput.MaxLength;
                    tbPreviewAction.Cursor = Cursors.IBeam;
                    break;
            }
        }

        /// <summary>
        /// Задать элементам превью картинку по директории
        /// </summary>
        /// <param name="Directory">Директория картинки</param>
        private void ChangePreviewImage(string Directory)
        {
            if (File.Exists(Directory))
            {
                pbPreviewIconLabelNormal.ImageLocation = Directory;
                pbPreviewIconLabelMinimal.ImageLocation = Directory;
                pbPreviewIconLabelNormal.Image = Image.FromFile(Directory);
                pbPreviewIconLabelMinimal.Image = Image.FromFile(Directory);
                pbPreviewIconLabelNormal.Refresh();
                pbPreviewIconLabelMinimal.Refresh();
            }
            else ObjLog.LOGTextAppend($"Директория для создания иконки ярлыка не верна: {Directory}");
        }

        private void CreateActionLabelActivate(object sender, EventArgs e)
        {
            switch (bCreateActionLabel.Text)
            {
                case "Задать директорию":
                    DialogResult result = BrowserDialogFolder.ShowDialog(this);
                    if (result == DialogResult.OK) tbPreviewAction.Text = BrowserDialogFolder.SelectedPath;
                    break;
                case "Задать команду":
                case "Открытие ссылки":
                    ConstAnimColor constAnimColor = new(Color.Green, Color.White, 7);
                    constAnimColor.AnimInit(tbPreviewAction, AnimStyleColor.BackColor);
                    break;
            }
        }

        private InfoLabelAccess? GenerateInfoObject()
        {
            if (tbNameLabel.TextLength == 0 || tbPreviewAction.Text.Length == 0) return null;
            return new InfoLabelAccess(tbNameLabel.Text, pbPreviewIconLabelNormal.ImageLocation,
                (InfoLabelAccess.TypeActionLabel)cbStyleLabel.SelectedIndex, tbPreviewAction.Text);
        }

        private void ChangeImagePreviewLabel(int Index, bool Priority)
        {
            string PriorityState = Priority ? "King" : string.Empty;
            string DirectoryImage = Index switch
            {
                0 => $"{Directory.GetCurrentDirectory()}\\Data\\Image\\Label\\Folder{PriorityState}.png",
                1 => $"{Directory.GetCurrentDirectory()}\\Data\\Image\\Label\\Console{PriorityState}.png",
                2 => $"{Directory.GetCurrentDirectory()}\\Data\\Image\\Label\\Brower{PriorityState}.png",
                _ => string.Empty
            };
            ChangePreviewImage(DirectoryImage);
        }
    }
}
