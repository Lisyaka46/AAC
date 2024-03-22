using System.Data;
using static AAC.Classes.AnimationDL.Animate.AnimColor;
using static AAC.Startcs;

namespace AAC
{
    public partial class FormExplanationCommands : Form
    {
        private class Info
        {
            /// <summary>
            /// Активный индекс элемента
            /// </summary>
            public int Position { get; set; }

            /// <summary>
            /// Массив имён команд
            /// </summary>
            public string[] MassCommand { get; }

            /// <summary>
            /// Массив описаний команд
            /// </summary>
            public string[] MassExplanationCommand { get; }

            /// <summary>
            /// Активное состояние предоставления информации о командах
            /// </summary>
            public StateDescriptionInfo ActivateStateInfo { get; }

            /// <summary>
            /// Инициализировать объект информации
            /// </summary>
            /// <param name="State">Состояние пояснения списка команд</param>
            public Info(StateDescriptionInfo State)
            {
                ActivateStateInfo = State;
                Position = 0;
                if (State == StateDescriptionInfo.DefaultCommand)
                {
                    MassCommand = MainData.MainCommandData.MassConsoleCommand.Select(i => i.WritingCommandName()).ToArray();
                    MassExplanationCommand = MainData.MainCommandData.MassConsoleCommand.Select(i => i.Explanation).ToArray();
                }
                else if (State == StateDescriptionInfo.VoiceCommand)
                {
                    List<string> PhrasesOneCommand = [];
                    string OnePhrases = "| ";
                    MassExplanationCommand = MainData.MainCommandData.MassVoiceCommand.Select(i => i.ExplanationCommand).ToArray();
                    for (int i = 0; i < MainData.MainCommandData.MassVoiceCommand.Length; i++)
                    {
                        foreach (string Element in MainData.MainCommandData.MassVoiceCommand[i].Phrases)
                        {
                            OnePhrases += Element[1..] + " | ";
                        }
                        PhrasesOneCommand.Add(OnePhrases);
                        OnePhrases = "| ";
                    }
                    MassCommand = [.. PhrasesOneCommand];
                }
                else
                {
                    MassCommand = [];
                    MassExplanationCommand = [];
                }
            }
        }

        /// <summary>
        /// Объект информации пояснения команд
        /// </summary>
        private Info ObjInfo { get; set; }

        public FormExplanationCommands()
        {
            InitializeComponent();
            ObjInfo = new(StateDescriptionInfo.DefaultCommand);
            cbCommands.Items.Clear();
            cbCommands.Items.AddRange(ObjInfo.MassCommand);
            //if (MainData.MainCommandData.SoftCommandData.NamesCommand.Length == 0)
            //{
            bSoftCommand.Hide();
            lInfoComplete.Location = new(244, 14);
            //}
            MaximumSize = Size;
            MinimumSize = Size;
            lInfoComplete.ForeColor = SystemColors.ControlText;
            bSoftCommand.ForeColor = Color.DarkGray;
            bAllVoiceCommand.ForeColor = Color.DarkGray;
            bDefaultCommand.ForeColor = Color.White;
            lSearchText.ForeColor = Color.Black;
            UpdateInformationCommands();
        }
        public void UpdateInformationCommands()
        {
            tbSearch.Text = "Поиск элемента";
            tbSearch.TextAlign = HorizontalAlignment.Center;
            tbSearch.ForeColor = Color.DimGray;

            switch (ObjInfo.ActivateStateInfo)
            {
                case StateDescriptionInfo.DefaultCommand:
                    cbCommands.Text = cbCommands.Items[ObjInfo.Position].ToString();
                    lPositionInformationCommand.Text = $"{ObjInfo.Position + 1}/{ObjInfo.MassCommand.Length}";
                    lExplanation.Text = $"<System command>\n{ObjInfo.MassExplanationCommand[ObjInfo.Position]}";
                    INameCommand.Text = $"{ObjInfo.Position + 1}. {MainData.MainCommandData.MassConsoleCommand[ObjInfo.Position].WritingCommandName()}";
                    lExampleWriteCommand.Text = MainData.MainCommandData.MassConsoleCommand[ObjInfo.Position].WritingCommandAll();
                    break;
                case StateDescriptionInfo.VoiceCommand:
                    cbCommands.Text = cbCommands.Items[ObjInfo.Position].ToString();
                    lPositionInformationCommand.Text = $"{ObjInfo.Position + 1}/{ObjInfo.MassCommand.Length}";
                    lExplanation.Text = $"<Voice command>\n{ObjInfo.MassExplanationCommand[ObjInfo.Position]}";
                    INameCommand.Text = $"{ObjInfo.Position + 1}. {ObjInfo.MassCommand[ObjInfo.Position]}";
                    lExampleWriteCommand.Text = "Сказать одну из предложенных фраз";
                    break;
                case StateDescriptionInfo.SoftCommand:
                    cbCommands.Text = cbCommands.Items[ObjInfo.Position].ToString();
                    lPositionInformationCommand.Text = $"{ObjInfo.Position + 1}/{ObjInfo.MassCommand.Length}";
                    lExplanation.Text = $"<Soft command>\n{ObjInfo.MassExplanationCommand[ObjInfo.Position]}";
                    INameCommand.Text = $"{ObjInfo.Position + 1}. {ObjInfo.MassCommand[ObjInfo.Position]}";
                    lExampleWriteCommand.Text = ObjInfo.MassCommand[ObjInfo.Position];
                    break;
            }
            return;
            /*
            lPositionInformationCommand.Text = $"{Info.Position + 1}/{Info.ListActive.Count}";
            lExplanation.Text = Info.ActivateStateInfo == StateDescriptionInfo.SoftCommand ? "<Soft>\n" : "<System>\n";
            lExplanation.Text += Info.Position >= Info.ExplanationListActive.Count || Info.ExplanationListActive[Info.Position].Length == 0 ?
                "Нет описания" : Info.ExplanationListActive[Info.Position];

            if (Info.ListActive.Count > 0)
                INameCommand.Text = $"{Info.Position + 1}. {Info.ListActive[Info.Position][0].ToString().ToUpper()}{Info.ListActive[Info.Position][1..].Replace("_", " ")}";
            else
                INameCommand.Text = $"Команды Отсутствуют";
            cbCommands.Items.Clear();
            foreach (string listElement in Info.ListActive)
                cbCommands.Items.Add(listElement);
            cbCommands.Text = cbCommands.Items[Info.Position].ToString();

            if (Info.ActivateStateInfo == StateDescriptionInfo.DefaultCommand) lExampleWriteCommand.Text = $"Ввести команду в строке: {CommandInfo.ConsoleCommands[Info.Position].TeamRegCommand}";
            else if (Info.ActivateStateInfo == StateDescriptionInfo.VoiceCommand) lExampleWriteCommand.Text = "Сказать одну из приведённых фраз";
            else if (Info.ActivateStateInfo == StateDescriptionInfo.SoftCommand)
                lExampleWriteCommand.Text = $"Написать в консоли: {Info.ListActive[Info.Position]} или сказать фразу";
            */
        }
        private void ButtonMouseMove(object sender, MouseEventArgs e)
        {
            Button Element = (Button)sender;
            Element.ForeColor = Color.LightSkyBlue;
        }
        private void ButtonMouseLeave(object sender, EventArgs e)
        {
            if (ObjInfo.ActivateStateInfo == StateDescriptionInfo.DefaultCommand) bDefaultCommand.ForeColor = Color.White;
            else if (ObjInfo.ActivateStateInfo == StateDescriptionInfo.VoiceCommand) bAllVoiceCommand.ForeColor = Color.White;
            else if (ObjInfo.ActivateStateInfo == StateDescriptionInfo.SoftCommand) bSoftCommand.ForeColor = Color.White;
        }
        private void ButtonListChange_Click(object sender, EventArgs e)
        {
            Button Element = (Button)sender;
            if (ObjInfo.Position == 0)
                ObjInfo.Position = Element.Name.Equals("bListDown") ? ObjInfo.Position + 1 : ObjInfo.MassCommand.Length - 1;
            else if (ObjInfo.Position < ObjInfo.MassCommand.Length - 1)
                ObjInfo.Position = Element.Name.Equals("bListDown") ? ObjInfo.Position + 1 : ObjInfo.Position - 1;
            else if (ObjInfo.Position == ObjInfo.MassCommand.Length - 1)
                ObjInfo.Position = Element.Name.Equals("bListDown") ? 0 : ObjInfo.Position - 1;
            UpdateInformationCommands();
        }
        private void ApplicationInfoCommandCLR_FormClosing(object sender, FormClosingEventArgs e)
        {
            App.MainForm.UnfoldingApplication(null, null);
            App.InformationCommand = null;
        }
        private void ApplicationInfoCommandCLR_Deactivate(object sender, EventArgs e)
        {
            App.MainForm.UnfoldingApplication(null, null);
        }

        private void BCopyCommand_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lExampleWriteCommand.Text);
            ConstAnimColor constAnimColor = new(Color.White, Color.Black, 4);
            constAnimColor.AnimInit(lInfoComplete, AnimStyleColor.ForeColor);
        }

        private void ButtonMouseEnter(object sender, EventArgs e)
        {
            Button Element = (Button)sender;
            Element.ForeColor = Color.LightSkyBlue;
        }

        private void ButtonMouseDown(object sender, MouseEventArgs e)
        {
            MainData.MainMP3.PlaySound("ClickDown");
            Button Element = (Button)sender;
            Element.ForeColor = Color.White;
        }

        private void ButtonMouseUp(object sender, MouseEventArgs e)
        {
            MainData.MainMP3.PlaySound("ClickUp");
            Button Element = (Button)sender;
            Element.ForeColor = Color.LightSkyBlue;
        }

        private void TbSearch_Enter(object sender, EventArgs e)
        {
            if (tbSearch.Text.Equals("Поиск элемента"))
            {
                tbSearch.Text = string.Empty;
                tbSearch.TextAlign = HorizontalAlignment.Left;
                tbSearch.ForeColor = Color.DarkOrange;
            }
        }

        private void TbSearch_Leave(object sender, EventArgs e)
        {
            if (tbSearch.Text.Length == 0)
            {
                tbSearch.Text = "Поиск элемента";
                tbSearch.TextAlign = HorizontalAlignment.Center;
                tbSearch.ForeColor = Color.DimGray;
                cbCommands.Items.AddRange(ObjInfo.MassCommand);
                cbCommands.Text = cbCommands.Items[ObjInfo.Position].ToString();
            }
        }
        private void BSearch_Click(object sender, EventArgs e)
        {
            ConstAnimColor ConstColor;
            bool Add = false;
            cbCommands.Items.Clear();
            foreach (string listElement in ObjInfo.MassCommand)
            {
                if (tbSearch.Text.Length > 0 && listElement.Contains(tbSearch.Text))
                {
                    Add = true;
                    lSearchText.Text = "Элементы найдены";
                    cbCommands.Items.Add(listElement);
                }
            }
            if (Add)
            {
                ConstColor = new(Color.Green, Color.Black, 3);
                ConstColor.AnimInit(lSearchText, AnimStyleColor.ForeColor);
                cbCommands.Text = cbCommands.Items[0].ToString();
            }
            else if (!Add)
            {
                cbCommands.Items.AddRange(ObjInfo.MassCommand);
                lSearchText.Text = "Элементы не найдены";
                ConstColor = new(Color.Red, Color.Black, 3);
                ConstColor.AnimInit(lSearchText, AnimStyleColor.ForeColor);
            }
        }

        private void BSearchItem_Click(object sender, EventArgs e)
        {
            ConstAnimColor ConstColor;
            if (cbCommands.Items.Count > 0)
            {
                if (ObjInfo.MassCommand.Contains(cbCommands.Text))
                {
                    ObjInfo.Position = cbCommands.SelectedIndex;
                    lSearchText.Text = "Элемент активирован";
                    ConstColor = new(Color.Green, Color.Black, 3);
                    ConstColor.AnimInit(lSearchText, AnimStyleColor.ForeColor);
                }
                else
                {
                    lSearchText.Text = "Элемент не найден";
                    ConstColor = new(Color.Red, Color.Black, 3);
                    ConstColor.AnimInit(lSearchText, AnimStyleColor.ForeColor);
                }
                UpdateInformationCommands();
            }
        }

        private void TbSearch_TextChanged(object sender, EventArgs e)
        {
            bool Add = false;
            cbCommands.Items.Clear();
            foreach (string listElement in ObjInfo.MassCommand)
            {
                if (tbSearch.Text.Length > 0 && listElement.Contains(tbSearch.Text))
                {
                    Add = true;
                    cbCommands.Items.Add(listElement);
                }
            }
            if (Add)
                cbCommands.Text = cbCommands.Items[0].ToString();
            else if (!Add)
            {
                cbCommands.Items.AddRange(ObjInfo.MassCommand);
            }
        }

        private void LInfoComplete_Click(object sender, EventArgs e)
        {
            ConstAnimColor constAnimColor = new(Color.White, Color.Black, 4);
            constAnimColor.AnimInit(lInfoComplete, AnimStyleColor.ForeColor);
        }

        private void DefaultCommandActivate(object sender, EventArgs e)
        {
            ObjInfo = new(StateDescriptionInfo.DefaultCommand);
            cbCommands.Items.Clear();
            cbCommands.Items.AddRange(ObjInfo.MassCommand);
            UpdateInformationCommands();
        }

        private void AllVoiceCommandActivate(object sender, EventArgs e)
        {
            ObjInfo = new(StateDescriptionInfo.VoiceCommand);
            cbCommands.Items.Clear();
            cbCommands.Items.AddRange(ObjInfo.MassCommand);
            UpdateInformationCommands();
        }

        private void SoftCommandActivate(object sender, EventArgs e)
        {
            ObjInfo = new(StateDescriptionInfo.SoftCommand);
            cbCommands.Items.Clear();
            cbCommands.Items.AddRange(ObjInfo.MassCommand);
            UpdateInformationCommands();
        }

        /// <summary>
        /// Состояния предоставления информации о командах
        /// </summary>
        public enum StateDescriptionInfo
        {
            DefaultCommand = 0,
            VoiceCommand = 1,
            SoftCommand = 2
        }
    }
}
