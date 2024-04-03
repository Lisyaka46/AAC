using AAC.Classes.Commands;
using System.Data;
using System.Data.Common;
using System.Linq;
using static AAC.Classes.AnimationDL.Animate.AnimColor;
using static AAC.Startcs;

namespace AAC
{
    public partial class FormExplanationCommands : Form
    {
        /// <summary>
        /// Перечисление состояний чтения данных команд
        /// </summary>
        private enum StateReadInfoCommand
        {
            DefaultCommand = 0,
            VoiceCommand = 1,
        }

        /// <summary>
        /// Индекс чтения данных команды
        /// </summary>
        private int Index;

        private StateReadInfoCommand StateReadInfo;

        private object[] Commands;

        private int[] IndexSearch;

        public FormExplanationCommands()
        {
            InitializeComponent();
            IndexSearch = [];
            cbCommands.Items.Clear();
            lbSeachCommands.Items.Clear();
            StateReadInfo = StateReadInfoCommand.DefaultCommand;
            Commands = MainData.MainCommandData.MassConsoleCommand;
            cbCommands.Items.AddRange([.. Commands.Select((i) => (i as ConsoleCommand)?.WritingCommandName() ?? "???")]);
            bSoftCommand.Hide();
            MaximumSize = Size;
            MinimumSize = Size;
            lInfoComplete.ForeColor = SystemColors.ControlText;
            bSoftCommand.ForeColor = Color.DarkGray;
            bAllVoiceCommand.ForeColor = Color.DarkGray;
            bDefaultCommand.ForeColor = Color.White;
            bItemUpdate.Click += (sender, e) =>
            {
                Index = cbCommands.SelectedIndex;
                UpdateInformationCommands();
            };
            bDefaultCommand.Click += (sender, e) =>
            {
                cbCommands.Items.Clear();
                Commands = MainData.MainCommandData.MassConsoleCommand;
                cbCommands.Items.AddRange([.. Commands.Select((i) => (i as ConsoleCommand)?.WritingCommandName() ?? "???")]);
                StateReadInfo = StateReadInfoCommand.DefaultCommand;
                Index = 0;
                UpdateInformationCommands();
            };
            bAllVoiceCommand.Click += (sender, e) =>
            {
                cbCommands.Items.Clear();
                Commands = MainData.MainCommandData.MassVoiceCommand;
                cbCommands.Items.AddRange([.. Commands.Select((i) => string.Join(", ", (i as VoiceCommand)?.Phrases ?? ["???"]))]);
                StateReadInfo = StateReadInfoCommand.VoiceCommand;
                Index = 0;
                UpdateInformationCommands();
            };
            bListDown.Click += (sender, e) =>
            {
                if (Index == 0) return;
                Index--;
                UpdateInformationCommands();
            };
            bListUp.Click += (sender, e) =>
            {
                if (Index == Commands.Length - 1) return;
                Index++;
                UpdateInformationCommands();
            };
            bCopyCommand.Click += (sender, e) =>
            {
                Clipboard.SetText(lExampleWriteCommand.Text);
                ConstAnimColor constAnimColor = new(Color.Green, Color.White, 4);
                constAnimColor.AnimInit(lExampleWriteCommand, AnimStyleColor.ForeColor);
            };
            tbSearch.Enter += (sender, e) =>
            {
                if (tbSearch.TextAlign == HorizontalAlignment.Center)
                {
                    tbSearch.Text = string.Empty;
                    tbSearch.TextAlign = HorizontalAlignment.Left;
                    tbSearch.ForeColor = Color.DarkOrange;
                }
            };
            tbSearch.Leave += (sender, e) =>
            {
                if (tbSearch.TextLength == 0)
                {
                    tbSearch.Text = "Поиск элемента";
                    tbSearch.TextAlign = HorizontalAlignment.Center;
                    tbSearch.ForeColor = Color.DimGray;
                }
            };
            bClearSearch.Click += (sender, e) =>
            {
                if (lbSeachCommands.Items.Count > 0)
                {
                    lbSeachCommands.Items.Clear();
                    IndexSearch = [];
                }
                tbSearch.Text = "Поиск элемента";
                tbSearch.TextAlign = HorizontalAlignment.Center;
                tbSearch.ForeColor = Color.DimGray;
            };
            bSearch.Click += (sender, e) =>
            {
                if (tbSearch.TextAlign == HorizontalAlignment.Center) return;
                lbSeachCommands.Items.Clear();
                int k = 0;
                Array.ForEach(Commands, (i) =>
                {
                    if (StateReadInfo == StateReadInfoCommand.DefaultCommand)
                    {
                        if (i is ConsoleCommand Ccommand)
                        {
                            if (Ccommand.WritingCommandName().Contains(tbSearch.Text, StringComparison.CurrentCultureIgnoreCase))
                            {
                                IndexSearch = [.. IndexSearch.Append(k)];
                                lbSeachCommands.Items.Add(Ccommand.WritingCommandName());
                            }
                        }
                    }
                    else if (StateReadInfo == StateReadInfoCommand.VoiceCommand)
                    {
                        if (i is VoiceCommand Vcommand)
                        {
                            if (Vcommand.Phrases.Any((i) => i.Contains(tbSearch.Text, StringComparison.CurrentCultureIgnoreCase)))
                            {
                                IndexSearch = [.. IndexSearch.Append(k)];
                                lbSeachCommands.Items.Add(string.Join(", ", Vcommand.Phrases));
                            }
                        }
                    }
                    k++;
                });
            };
            lbSeachCommands.SelectedIndexChanged += (sender, e) =>
            {
                if (IndexSearch[lbSeachCommands.SelectedIndex] != Index)
                {
                    Index = IndexSearch[lbSeachCommands.SelectedIndex];
                    UpdateInformationCommands();
                }
            };
            UpdateInformationCommands();
        }


        private void UpdateInformationCommands()
        {
            tbSearch.Text = "Поиск элемента";
            tbSearch.TextAlign = HorizontalAlignment.Center;
            tbSearch.ForeColor = Color.DimGray;

            cbCommands.Text = cbCommands.Items[Index]?.ToString();
            lPositionInformationCommand.Text = $"{Index + 1}/{Commands.Length}";
            INameCommand.Text = $"{Index + 1}. {cbCommands.Items[Index]?.ToString()}";
            switch (StateReadInfo)
            {
                case StateReadInfoCommand.DefaultCommand:
                    ConsoleCommand? consoleCommand = Commands[Index] as ConsoleCommand;
                    lExplanation.Text = $"<System command>\n{consoleCommand?.Explanation ?? "???"}";
                    lExampleWriteCommand.Text = consoleCommand?.WritingCommandAll() ?? "???";
                    break;
                case StateReadInfoCommand.VoiceCommand:
                    VoiceCommand? voiceCommand = Commands[Index] as VoiceCommand;
                    lExplanation.Text = $"<Voice command>\n{voiceCommand?.Explanation ?? "???"}";
                    lExampleWriteCommand.Text = "Сказать одну из предложенных фраз";
                    lExampleWriteCommand.Text = cbCommands.Items[Index]?.ToString();
                    break;
                    /*case StateDescriptionInfo.SoftCommand:
                        cbCommands.Text = cbCommands.Items[ObjInfo.Position].ToString();
                        lPositionInformationCommand.Text = $"{ObjInfo.Position + 1}/{ObjInfo.MassCommand.Length}";
                        lExplanation.Text = $"<Soft command>\n{ObjInfo.MassExplanationCommand[ObjInfo.Position]}";
                        INameCommand.Text = $"{ObjInfo.Position + 1}. {ObjInfo.MassCommand[ObjInfo.Position]}";
                        lExampleWriteCommand.Text = ObjInfo.MassCommand[ObjInfo.Position];
                        break;
                    */
            }
        }
        private void ButtonMouseMove(object sender, MouseEventArgs e)
        {
            Button Element = (Button)sender;
            Element.ForeColor = Color.LightSkyBlue;
        }
        private void ButtonMouseLeave(object sender, EventArgs e)
        {
            if (StateReadInfo == StateReadInfoCommand.DefaultCommand) bDefaultCommand.ForeColor = Color.White;
            else if (StateReadInfo == StateReadInfoCommand.VoiceCommand) bAllVoiceCommand.ForeColor = Color.White;
            //else if (ObjInfo.ActivateStateInfo == StateDescriptionInfo.SoftCommand) bSoftCommand.ForeColor = Color.White;
        }
        private void ApplicationInfoCommand_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Apps.MainForm.StateAnimWindow == StateAnimateWindow.HalfHide) Apps.MainForm.UnfoldingOpacityApplication();
            else if (Apps.MainForm.StateAnimWindow == StateAnimateWindow.Hide) Apps.MainForm.UnfoldingMoveApplication();
            Apps.InformationCommand.Dispose();
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

        private void LInfoComplete_Click(object sender, EventArgs e)
        {
            ConstAnimColor constAnimColor = new(Color.White, Color.Black, 4);
            constAnimColor.AnimInit(lInfoComplete, AnimStyleColor.ForeColor);
        }
    }
}
