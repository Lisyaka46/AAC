using AAC.Classes;
using AAC.GUI;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static AAC.Classes.AnimationDL.Animate.AnimColor;
using static AAC.Classes.AnimationDL.Animate.AnimFormule;
using static AAC.Classes.AnimationDL.Animate.AnimText;
using static AAC.Classes.MainTheme;
using static AAC.Data;
using static AAC.Forms_Functions;
using static AAC.Startcs;
using AAC.Classes.Commands;

namespace AAC
{
    public partial class MainApplication : Form
    {
        /// <summary>
        /// ���� ����������� �������� �������
        /// </summary>
        /// <typeparam name="T">������� �������</typeparam>
        public class ListLabel<T> : List<T> where T : IELLabelAccess
        {
            /// <summary>
            /// �������� � ������������� ����������� �����
            /// </summary>
            /// <param name="Element">����������� ������ �����</param>
            public new void Add(T Element)
            {
                int NumStatus = (int)Element.Status;
                if (Count > 0)
                {
                    if (NumStatus == 0 && this[^1].Status == TypeLabel.System || NumStatus == 2)
                    {
                        Element.SetLocationIndex(Count);
                        base.Add(Element);
                    }
                    else
                    {
                        int i = 0;
                        while (NumStatus >= (int)this[i].Status && i < Count) i++;
                        Element.SetLocationIndex(i);
                        Insert(i++, Element);
                        for (; i < Count; i++) this[i].SetLocationIndex(i);
                    }
                }
                else base.Add(Element);
            }

            /// <summary>
            /// ������� ������ ������ �� �������
            /// </summary>
            /// <param name="Index">������ ������� ������</param>
            public new void RemoveAt(int Index)
            {
                this[Index].Dispose();
                base.RemoveAt(Index);
            }
        }

        /// <summary>
        /// ����� ������� ������ �����
        /// </summary>
        private class Flags
        {
            /// <summary>
            /// ������ ���������� ������� ���������
            /// </summary>
            public static Flag Information { get; } = new(false);

            /// <summary>
            /// ������ ���������� �������������� � ����-������� � ������� ����������
            /// </summary>
            public static Flag PAC_PanelAltActivate { get; } = new(false);

            /// <summary>
            /// ������ ���������� ������� �����
            /// </summary>
            public static Flag FormActivity { get; } = new(true);

            /// <summary>
            /// ������ ����������� ���� �����
            /// </summary>
            public static Flag MovingApplication { get; } = new(false);

            /// <summary>
            /// ������ ���������� ����-������ ������� �����
            /// </summary>
            public static Flag PAC_PanelActivate { get; } = new(false);

            /// <summary>
            /// ������ ���������� ������ �� ����������
            /// </summary>
            public static Flag KeyActivity { get; } = new(false);

            /// <summary>
            /// ������ ���������� ������ � ���������� ������
            /// </summary>
            public static Flag BufferConsole { get; } = new(false);

            /// <summary>
            /// ������ ���������� ������ ��������� � �������� �������
            /// </summary>
            public static Flag ActiveHitPanelConsole { get; } = new(false);

            /// <summary>
            /// ������ ���������� ������ �������
            /// </summary>
            public static Flag ActiveExplorerLabel { get; } = new(false);

            /// <summary>
            /// ������ ���������� ����-������ ��������
            /// </summary>
            public static Flag ActiveSettingsMiniPanel { get; } = new(false);
        }

        /// <summary>
        /// ���� � ����� �������
        /// </summary>
        public const string ObjLabelFilePath = $"Data\\Info\\Label.r1";

        private object ResHit { get; set; }

        /// <summary>
        /// �������� ������ � PAC
        /// </summary>
        private Panel ActivatePACPanelPage { get; set; }

        private Point StartCursorMovingtbOutput { get; set; }

        //
        public int ChangeLengthBuffer { get; set; }

        //
        private int IndexConsoleReadBuffer = -1;

        /// <summary>
        /// ���������� ������� ���� ��� �������� ������������/��������������
        /// </summary>
        private Point SavePositionAnimateWindow { get; set; }

        /// <summary>
        /// ��������� ���� ������� �����
        /// </summary>
        private StateAnimateWindow StateAnimWindow { get; set; }

        /// <summary>
        /// ��������� � ��������
        /// </summary>
        public List<Label> HitCommandConsole { get; private set; }

        /// <summary>
        /// ������ �������
        /// </summary>
        private ListLabel<IELLabelAccess> LabelAccess;

        /// <summary>
        /// ������ ��������� ������� ������ � PAC
        /// </summary>
        private int? PAC_IndexIELLabelAccess;

        /// <summary>
        /// ��� �������� ������ ����� ���������� �������� ��� ��������
        /// </summary>
        private Panel? PAC_PanelActivityCache;

        /// <summary>
        /// ������� ������-���� �������
        /// </summary>
        private CounterScrollBar ScrollLabels { get; set; }


        /// <summary>
        /// ������ ������� ������
        /// </summary>
        private List<Keys> ControlKey { get; set; }

        /// <summary>
        /// ������������� ������� ����� ���������
        /// </summary>
        public MainApplication()
        {
            InitializeComponent();
            PAC_Buffer.InitializeNewBuffer((int)MainData.Settings.Buffer_Count_Elements.Value);
            lHit.Dispose();
            ResHit = new();

            /* �������� ������������ ������ */ if ((bool)MainData.Settings.Developer_Mode)
            {
                SetEventFlag(Flags.Information, clbDeveloperFlags, clbDeveloperFlags.Items.Add(nameof(Flags.Information)));

                SetEventFlag(Flags.PAC_PanelActivate, clbDeveloperFlags, clbDeveloperFlags.Items.Add(nameof(Flags.PAC_PanelActivate)));

                SetEventFlag(Flags.PAC_PanelAltActivate, clbDeveloperFlags, clbDeveloperFlags.Items.Add(nameof(Flags.PAC_PanelAltActivate)));

                SetEventFlag(Flags.FormActivity, clbDeveloperFlags, clbDeveloperFlags.Items.Add(nameof(Flags.FormActivity)));

                SetEventFlag(Flags.MovingApplication, clbDeveloperFlags, clbDeveloperFlags.Items.Add(nameof(Flags.MovingApplication)));

                SetEventFlag(Flags.KeyActivity, clbDeveloperFlags, clbDeveloperFlags.Items.Add(nameof(Flags.KeyActivity)));

                SetEventFlag(Flags.BufferConsole, clbDeveloperFlags, clbDeveloperFlags.Items.Add(nameof(Flags.BufferConsole)));

                SetEventFlag(Flags.ActiveHitPanelConsole, clbDeveloperFlags, clbDeveloperFlags.Items.Add(nameof(Flags.ActiveHitPanelConsole)));

                SetEventFlag(Flags.ActiveExplorerLabel, clbDeveloperFlags, clbDeveloperFlags.Items.Add(nameof(Flags.ActiveExplorerLabel)));

                SetEventFlag(Flags.ActiveSettingsMiniPanel, clbDeveloperFlags, clbDeveloperFlags.Items.Add(nameof(Flags.ActiveSettingsMiniPanel)));
            }
            
            ChangeLengthBuffer = -1;
            StateAnimWindow = StateAnimateWindow.Hide;
            if (Screen.PrimaryScreen != null)
                SavePositionAnimateWindow = new(Screen.PrimaryScreen.Bounds.Width / 2 - Size.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - Size.Height / 2);
            else SavePositionAnimateWindow = new(0, 0);
            ControlKey = [];
            HitCommandConsole = [];

            MainData.AllSpecialColor.RGB.AddElement(lDeveloper_RGB_Text);
            MainData.AllSpecialColor.RGB.AddElement(lTitleApplication);
            MainData.AllSpecialColor.RGBCC.AddElement(lDeveloper_RGBTextCC);
            MainData.AllSpecialColor.SC.AddElement(lDeveloper_TextSC);

            ActivatePACPanelPage = pmpMain;

            LabelAccess =
            [
                new(new("New Label", null, InfoLabelAccess.TypeActionLabel.InitializeCommand, "new_label"), pAllVisualLabel, TypeLabel.System, 0),
                new(new("Main Dir", null, InfoLabelAccess.TypeActionLabel.OpenDirectoryElement, Directory.GetCurrentDirectory()), pAllVisualLabel, TypeLabel.System, 1),
            ];
            if (File.Exists(ObjLabelFilePath)) ReadPanelsLabel(File.ReadAllText(ObjLabelFilePath), pAllVisualLabel, ref LabelAccess);
            else File.Create(ObjLabelFilePath);

            ScrollLabels = new(LabelAccess.Count, 8);
            pAllVisualLabel.MouseWheel += (sender, e) =>
            {
                if (ScrollLabels.Value <= ScrollLabels.MaxValue)
                {
                    if (e.Delta < 0 && ScrollLabels.Value < ScrollLabels.MaxValue) ScrollLabels.Value++;
                    else if (e.Delta > 0 && ScrollLabels.Value > 0) ScrollLabels.Value--;
                    ConstAnimMove ConstPanelExplorer = new(pAllVisualLabel.Location.Y, 14 - 56 * ScrollLabels.Value, 17);
                    new ConstAnimMove(pAllVisualLabel.Location.X).InitAnimFormule(pAllVisualLabel, Formules.QuickTransition, ConstPanelExplorer, AnimationStyle.XY);
                }
            };
            pAllVisualLabel.Size = new(pAllVisualLabel.Size.Width, 6 + 56 * LabelAccess.Count);
            lBorderLabelDown.Location = new(lBorderLabelDown.Location.X, pAllVisualLabel.Height - 25);
            lBorderLabelUp.SendToBack();
            lBorderLabelDown.SendToBack();



            UpdateTheme(MainData.MainThemeData.ActivateTheme);
            if ((bool)MainData.Settings.Activation_Microphone)
            {
                MainData.Flags.AudioCommand = StatusFlags.Active;
                //MainData.InputVoiceDevice.RecordInput.RecognizeAsync();
            }
            else MainData.Flags.AudioCommand = StatusFlags.NotActive;
            VoiceButtonImageUpdate(MainData.Flags.AudioCommand, false);
            if ((int)MainData.Settings.Font_Size_Console_Text.Value >= 7 && (int)MainData.Settings.Font_Size_Console_Text.Value <= 40)
                tbOutput.Font = new(tbOutput.Font.Name, Convert.ToInt32(MainData.Settings.Font_Size_Console_Text.Value));
            else new Instr_AnimText(tbOutput, $">>> Fail install int parameter \"FontSize\": {MainData.Settings.Font_Size_Console_Text.Value} (7 - 40)").AnimInit(true);

            // ������ "RE"
            bRebootApplication.MouseClick += (sender, e) =>
            {
                lInformationCursor.Hide();
                ConsoleCommand.ReadDefaultConsoleCommand("reboot").ExecuteCommand(false);
            };
            bRebootApplication.MouseHover += (sender, e) => ActivateLabelInfo("������������� ���������");
            bRebootApplication.MouseLeave += (sender, e) => DisactivateLabelInfo();


            pDeveloper.MouseWheel += (sender, e) => { MiniFunctions.UpdateVScrollBar(App.MainForm.sbhDebeloper, e.Delta); };
            sbhDebeloper.ValueChanged += (sender, e) => { MiniFunctions.MoveElementinVScrollBar(pDeveloperElements, sbhDebeloper, 3); };

            //pmpBufferCommandButtons.MouseWheel += (sender, e) => { MiniFunctions.UpdateVScrollBar(App.MainForm.PAC_sbBuffer, e.Delta); };
            //PAC_sbBuffer.ValueChanged += (sender, e) => { MiniFunctions.MoveElementinVScrollBar(pmpBufferCommandButtons, PAC_sbBuffer, 23); };
            CgangeAllButtonAltMode();
            pMiniPanelOutput.PreviewKeyDown += (sender, e) => AltMiniPanelDetect(e.KeyCode);

            // ������ � PAC "�������� �������� ������"
            PAC_bColoredTheme.ActivateButton += (KeyActivity) =>
            {
                PAC_Disactivate();
                ConsoleCommand.ReadDefaultConsoleCommand("colored").ExecuteCommand(false);
            };

            // ������ � PAC "�������� ���� ������"
            PAC_bSetColor.ActivateButton += (KeyActivity) =>
            {
                PAC_Disactivate();
                //TypeCommand.ReadDefaultConsoleCommand("color").ExecuteCommand(false);
            };

            // ������ � PAC "�������� ����� �� �������"
            PAC_bClearConsole.ActivateButton += (KeyActivity) =>
            {
                PAC_Disactivate();
                ConsoleCommand.ReadDefaultConsoleCommand("clear").ExecuteCommand(false);
            };

            // ������ � PAC "������ ���������"
            PAC_bLogMessage.ActivateButton += (KeyActivity) =>
            {
                PAC_Disactivate();
                ConsoleCommand.ReadDefaultConsoleCommand("log").ExecuteCommand(false);
            };

            //
            PAC_bClearBuffer.ActivateButton += (KeyActivity) =>
            {
                ClearBufferCommand();
            };

            // ������ � PAC "��������� �����..."
            PAC_bCommandBuffer.ActivateButton += (KeyActivity) =>
            {
                LmpExecuteCommands_Click();
            };

            PAC_bBackBuffer.ActivateButton += (KeyActivity) =>
            {
                MiniFunctions.OpenNewMiniMenu(pmpMain, ActivatePACPanelPage, 0, DirectionsParties.Left);
                ActivatePACPanelPage = pmpMain;
            };

            // ������ � PAC "���������"
            PAC_bConductor.ActivateButton += (KeyActivity) =>
            {
                MiniFunctions.OpenNewMiniMenu(pmpExplorer, ActivatePACPanelPage, 0);
                ActivatePACPanelPage = pmpExplorer;
            };

            // ������ � PAC <Conductor> "�����"
            PAC_bBackConductor.ActivateButton += (KeyActivity) =>
            {
                MiniFunctions.OpenNewMiniMenu(pmpMain, ActivatePACPanelPage, 0, DirectionsParties.Left);
                ActivatePACPanelPage = pmpMain;
            };

            // ������ � PAC "�������� ���������"
            PAC_bActiveExplorer.ActivateButton += (KeyActivity) =>
            {
                PAC_Disactivate();
                Process.Start("explorer.exe", Directory.GetCurrentDirectory());
            };

            // ������ � PAC "������� ���������"
            PAC_bMainExplorer.ActivateButton += (KeyActivity) =>
            {
                PAC_Disactivate();
                Process.Start("explorer.exe");
            };

            // ������ � PAC "������������ �����"
            PAC_bActivateLabel.ActivateButton += (KeyActivity) =>
            {
                if (PAC_IndexIELLabelAccess.HasValue)
                {
                    LabelAccess[PAC_IndexIELLabelAccess.Value].IELLabelAccess_ActivateLabel();
                    PAC_Disactivate();
                }
            };

            // ������ � PAC "������������ �����"
            PAC_bDeleteLabel.ActivateButton += (KeyActivity) =>
            {
                if (PAC_IndexIELLabelAccess.HasValue)
                {
                    ConstAnimMove ConstantFormule;
                    LabelAccess.RemoveAt(PAC_IndexIELLabelAccess.Value);
                    if (ScrollLabels.MaxValue > 0)
                    {
                        ScrollLabels.MaxDown(1);
                        ConstantFormule = new(pAllVisualLabel.Location.Y, 14 - 56 * ScrollLabels.Value, 17);
                        new ConstAnimMove(pAllVisualLabel.Location.X).InitAnimFormule(pAllVisualLabel, Formules.QuickTransition, ConstantFormule, AnimationStyle.XY);
                    }
                    for (int i = PAC_IndexIELLabelAccess.Value; i < LabelAccess.Count; i++) LabelAccess[i].SetLocationIndex(i);
                    ConstantFormule = new(lBorderLabelDown.Location.Y, lBorderLabelDown.Location.Y - 56, 4);
                    new ConstAnimMove(lBorderLabelDown.Location.X).InitAnimFormule(lBorderLabelDown, Formules.QuickTransition, ConstantFormule, AnimationStyle.XY);
                    PAC_Disactivate();
                }
            };

            // ������� ������� ��� PSettings
            void EventPSettings()
            {
                if (!Flags.ActiveSettingsMiniPanel.Value) PanelSettingsActivate();
                else PanelSettingsDiactivate();
            }
            bSettingsMute.Cursor = Cursors.Hand;
            pSettings.Click += (sender, e) => EventPSettings();
            pbSttingsName.Click += (sender, e) => EventPSettings();


            pMiniPanelOutput.Size = new(0, 0);

            lComplete.ForeColor = Color.Black;

            pHitCommandConsole.Size = new(pHitCommandConsole.Width, 16);
            pHitCommandConsole.Location = new(pHitCommandConsole.Location.X, tbInput.Location.Y);

            pDeveloper.Size = new(318, 493);
            pDeveloper.Location = new(792, 5);

            sbhDebeloper.Size = new(sbhDebeloper.Size.Width, pDeveloper.Size.Height);

            pUppingMenu.Location = new(pUppingMenu.Location.X, -22);

            //Flags.PAC_PanelAltActivate.Value = true;
            //lmpClearBuffer.Location = new(58, 1);
            //AltMiniPanelDetect(null, new(Keys.Menu));

            lInformationCursor.Hide();
            tbOutput.Size = new(771, 467);
            pMainConsole.Size = new(816, pMainConsole.Size.Height);

            pLabelExplorer.Location = new(622, 6);

            Size = new(815, 577);
            MaximumSize = Size;
            MinimumSize = Size;

            lWhat.Size = new(20, 17);
            InputPanel.Size = new(InputPanel.Size.Width, 34);
            lCountActiveBufferCommand.Location = new(-14, 505);
            if (DateTime.Now.ToString("dd/MM").Equals("19.04")) // 19.04
            {
                lActiveitedSoftCommand.Location = new(108, 9);
                pbHappyBigthday.Location = new(81, 2);
            }

            lMaxLangthCommand.Location = new(lMaxLangthCommand.Location.X, 514);

            pSettings.Location = new(0 - pSettings.Width + 13, pSettings.Location.Y);
            pbSttingsName.Location = new(pbSttingsName.Location.X, pSettings.Height / 2 - pbSttingsName.Height / 2);

            pbIndicatorMusic1.Location = new(pbIndicatorMusic1.Location.X, pbIndicatorMusic1.Location.Y - 34);

            pICON.Location = new(-500, pICON.Location.Y);

            // ����� ������ ������� ������ Developer ��� ��������� VScrollBar
            MiniFunctions.CalculationDownElementScrollBar(clbDeveloperFlags, pDeveloperElements, pDeveloper, sbhDebeloper, 10, false);

            lActiveitedSoftCommand.Text = "Soft-������� ���������";
            //lActiveitedSoftCommand.Text = $"+{MainData.MainCommandData.SoftCommandData.NamesCommand.Length} Soft-{MiniFunctions.LogisticText(MainData.MainCommandData.SoftCommandData.NamesCommand.Length, "�������")}";

            if ((bool)MainData.Settings.Developer_Mode) ObjLog.LOGTextAppend("��������� ��� ��������� �������� �����������");
            else pDeveloper.Hide();
        }

        /// <summary>
        /// ������������� � ������������� ������ ������
        /// </summary>
        public void GenerateLabel()
        {
            App.Create.DialogCreateLabel?.Dispose();
            App.Create.DialogCreateLabel = new();
            IELLabelAccess? GenerateLabelAccess = App.Create.DialogCreateLabel.GenerateLabelAccess(this, pAllVisualLabel);
            if (GenerateLabelAccess != null)
            {
                Color MP = MainData.MainThemeData.ActivateTheme.ObjColors[7].ElColor;
                byte MPR = (byte)(MP.R + (MP.R <= 150 ? 55 : -55));
                GenerateLabelAccess.BackColor = Color.FromArgb(MPR, MPR, MPR);
                GenerateLabelAccess.BringToFront();
                GenerateLabelAccess.RightClickMouse += (info, Index) =>
                {
                    PAC_IndexIELLabelAccess = Index;
                    PAC_Activate(true);
                };
                LabelAccess.Add(GenerateLabelAccess);

                pAllVisualLabel.Size = new(pAllVisualLabel.Size.Width, 6 + 56 * LabelAccess.Count);
                lBorderLabelDown.Location = new(lBorderLabelDown.Location.X, pAllVisualLabel.Height - 25);
                lBorderLabelDown.SendToBack();
                ScrollLabels.MaxUp(1);
            }
        }

        /// <summary>
        /// ������� ������� ��������� ����� � ������ �����
        /// <code>Element.SetItemChecked(Index, Value);</code>
        /// </summary>
        /// <param name="flag">���� �����</param>
        /// <param name="Element">������� ���������� ��������������� ����</param>
        /// <param name="Index">������ ������� ������������ �����</param>
        private static void SetEventFlag(Flag flag, CheckedListBox Element, int Index)
        {
            flag.ChangeStateFlag += (Value) => Element.SetItemChecked(Index, Value);
        }

        public void LComplete_Click(object sender, EventArgs e)
        {
            ConstAnimColor constAnim = new(Color.White, MainData.MainThemeData.ActivateTheme.ObjColors[3].ElColor, 6);
            constAnim.AnimInit(lComplete, AnimStyleColor.ForeColor);
        }

        public void LActiveitedSoftCommand_Click(object sender, EventArgs e)
        {
            ConstAnimColor constAnim = new(Color.White, MainData.MainThemeData.ActivateTheme.ObjColors[3].ElColor, 2);
            constAnim.AnimInit(lActiveitedSoftCommand, AnimStyleColor.ForeColor);
        }

        private void Application_Deactivate(object sender, EventArgs e)
        {
            ObjLog.LOGTextAppend("������� ����� �� �������");
            Flags.FormActivity.Value = false;
            FoldingApplication(null, null);
        }

        /// <summary>
        /// ������� �������� ������������ ��� ���������� ������ ����� �������
        /// </summary>
        private void TbInputChangeLineText()
        {
            bool tbTextLength = tbInput.TextLength >= 42;
            ConstAnimMove ConstantFormule = new(tbInput.Size.Width, tbTextLength ? 447 : 322, 13);
            ConstantFormule.InitAnimFormule(tbInput, Formules.QuickTransition, new(tbInput.Size.Height), AnimationStyle.Size);

            ConstantFormule = new(InputPanel.Location.X, tbTextLength ? 465 : 340, 13);
            ConstantFormule.InitAnimFormule(InputPanel, Formules.QuickTransition, new(InputPanel.Location.Y), AnimationStyle.XY);
        }

        public void VoiceInfo_Click(object sender, EventArgs e)
        {
            ObjLog.LOGTextAppend("��� ����� ��������");
            if (MainData.Flags.AudioCommand == StatusFlags.Active)
            {
                ObjLog.LOGTextAppend("���� ���������� ���������� �� ������������� ���������� ��������� ������");
                MainData.Flags.AudioCommand = StatusFlags.Sleep;
            }
            else if (MainData.Flags.AudioCommand == StatusFlags.Sleep)
            {
                ObjLog.LOGTextAppend("���� ���������� ���������� �� ������ ���������� ��������� ������");
                //MainData.InputVoiceDevice.RecordInput.RecognizeAsyncStop();
                //MainData.InputVoiceDevice.RecordInput.UnloadAllGrammars();
                MainData.Flags.AudioCommand = StatusFlags.NotActive;
            }
            else if (MainData.Flags.AudioCommand == StatusFlags.NotActive)
            {
                ObjLog.LOGTextAppend("���� ���������� ���������� �� ����� �� ������� ���������� ��������� ������");
                //MainData.InputVoiceDevice.RecordInput.RecognizeAsync();
                MainData.Flags.AudioCommand = StatusFlags.Active;
            }
            VoiceButtonImageUpdate(MainData.Flags.AudioCommand, true);
        }

        private void TbOutput_TextChanged(object sender, EventArgs e)
        {
            if (tbOutput.Text.Count(x => x == '\n') >= Math.Round(tbOutput.Height / tbOutput.Font.Size / 1.6))
                tbOutput.Text = MiniFunctions.UpdateLineOutput(tbOutput.Text);
        }

        private void TbInput_KeyUp(object sender, KeyEventArgs e)
        {
            ControlKey.Remove(e.KeyCode);
            if (ControlKey.Count == 0) Flags.KeyActivity.Value = false;
            lDeveloper_PressEndKey.Text = $"PressEndKey: <{(ControlKey.Count > 0 ? ControlKey[^1].ToString().Trim() : string.Empty)}>";
            lDeveloper_ControlKey.Text = $"ControlKeys: <{string.Join(", ", ControlKey.AsEnumerable())}>";
            switch (e.KeyCode)
            {
                // ������� ���������� ������
                case Keys.Escape:
                    AnimationDL.StopAnimate(AnimationDL.StyleAnimateObj.AnimText, tbInput.Name);
                    if (tbInput.Size.Width > 322) TbInputChangeLineText();
                    tbInput.Text = string.Empty;
                    DiactivateConsoleBuffer();
                    break;

                // ������������ ������ ������ � ���������� ������
                case Keys.Up:
                case Keys.Down:
                    if (PAC_Buffer.ElementsBuffer.Count > 0)
                    {
                        if (!Flags.BufferConsole.Value) ActivateConsoleBuffer();
                        else
                        {
                            if (e.KeyCode == Keys.Up)
                            {
                                if (IndexConsoleReadBuffer == 0) IndexConsoleReadBuffer = PAC_Buffer.ElementsBuffer.Count - 1;
                                else IndexConsoleReadBuffer--;
                            }
                            else if (e.KeyCode == Keys.Down)
                            {
                                if (IndexConsoleReadBuffer == PAC_Buffer.ElementsBuffer.Count - 1) IndexConsoleReadBuffer = 0;
                                else IndexConsoleReadBuffer++;
                            }
                        }
                        ObjLog.LOGTextAppend($"������ �������� ������� � �������: {IndexConsoleReadBuffer}");
                        string NameActiveCommand = PAC_Buffer.ElementsBuffer[IndexConsoleReadBuffer];
                        lCountActiveBufferCommand.Text = $"{IndexConsoleReadBuffer + 1}\n{PAC_Buffer.ElementsBuffer.Count}";
                        tbInput.Text = NameActiveCommand;
                        tbInput.SelectionStart = tbInput.TextLength;

                        if (NameActiveCommand.Length >= 42 || (NameActiveCommand.Length < 42 && tbInput.Size.Width > 322)) TbInputChangeLineText();
                    }
                    break;

                // ��������� �������
                case Keys.Enter:
                    DiactivateConsoleBuffer();
                    ActivateConsoleCommand(null, null);
                    break;

                // ������������ ��������/���������� �������� ������
                case Keys.Capital:
                case Keys.ShiftKey:
                    CapsLock_Info.Image = Image.FromFile(@$"Data\Image\{(IsKeyLocked(Keys.CapsLock) ? "Up-A" : "Down-a")}.gif");
                    break;

                // ��������� ��������� ������� � "���������� � ��������" � ���������� ������
                case Keys.D0:
                    if (ControlKey.Contains(Keys.ControlKey) && Flags.ActiveHitPanelConsole.Value)
                    {
                        tbInput.Text = HitCommandConsole[^1].Tag?.ToString() ?? string.Empty;
                        HitPanelDiactivate();
                        tbInput.SelectionStart = tbInput.TextLength;
                    }
                    break;

                // ��������� ����������� ������� � "���������� � ��������" � ���������� ������
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    if (ControlKey.Contains(Keys.ControlKey) && Flags.ActiveHitPanelConsole.Value)
                    {
                        int SaveNumber = e.KeyCode.ToString()[1] - 48;
                        if (HitCommandConsole.Count >= SaveNumber)
                        {
                            tbInput.Text = HitCommandConsole[SaveNumber - 1].Tag?.ToString() ?? string.Empty;
                            HitPanelDiactivate();
                            tbInput.SelectionStart = tbInput.TextLength;
                        }
                    }
                    break;
            }
        }

        private void TbInput_KeyDown(object sender, KeyEventArgs e)
        {
            lDeveloper_PressEndKey.Text = $"PressEndKey: <{e.KeyCode.ToString().Trim()}>";
            if (!ControlKey.Contains(e.KeyCode))
            {
                ControlKey.Add(e.KeyCode);
                lDeveloper_ControlKey.Text = $"ControlKeys: <{string.Join(", ", ControlKey.AsEnumerable())}>";
            }
            if (Flags.KeyActivity.Value) return;
            ConstAnimMove constAnim = new(-14, 2, 10);

            Flags.KeyActivity.Value = true;
            MainData.MainMP3.PlaySound("ClickDown");
            if (e.KeyCode == Keys.Capital || e.KeyCode == Keys.ShiftKey)
                CapsLock_Info.Image = !IsKeyLocked(Keys.CapsLock) ? Image.FromFile(@"Data\Image\Up-A.gif") : Image.FromFile(@"Data\Image\Down-a.gif");
            else if (((e.KeyCode == Keys.Right && tbInput.SelectionStart == tbInput.TextLength) || e.KeyCode == Keys.Return) && Flags.BufferConsole.Value)
            {
                constAnim.Reverse().InitAnimFormule(lCountActiveBufferCommand, Formules.QuickTransition, null, AnimationStyle.XY);
                Flags.BufferConsole.Value = false;
            }
            else if (e.KeyCode == Keys.Back && Flags.BufferConsole.Value)
            {
                constAnim.Reverse().InitAnimFormule(lCountActiveBufferCommand, Formules.QuickTransition, null, AnimationStyle.XY);
                Flags.BufferConsole.Value = false;
            }
            else if (e.KeyCode == Keys.Return)
            {
                tbInput.Text = tbInput.Text.Replace("\n", string.Empty);
                tbInput.SelectionStart = tbInput.TextLength;
            }
            else if (e.KeyCode == Keys.Apps)
            {
                Flags.KeyActivity.Value = false;
                PAC_Activate();
            }

        }

        public void DeveloperPanelClick(object sender, EventArgs e)
        {
            if (Flags.FormActivity.Value)
            {
                ObjLog.LOGTextAppend("���� ������ ������� ������ ������������");
                if ((bool)MainData.Settings.Developer_Mode)
                {
                    if (Flags.PAC_PanelActivate.Value) PAC_Disactivate();
                    (int, int) Coords = MainData.Flags.PanelDeveloper == BooleanFlags.False ? (792, 498) : (498, 792);
                    MainData.Flags.PanelDeveloper = MainData.Flags.PanelDeveloper == BooleanFlags.True ? BooleanFlags.False : BooleanFlags.True;
                    ConstAnimMove ConstantFormule = new(Coords.Item1, Coords.Item2, 10);
                    ConstantFormule.InitAnimFormule(pDeveloper, Formules.QuickTransition, new ConstAnimMove(pDeveloper.Location.Y), AnimationStyle.XY);
                    pDeveloper.BringToFront();
                    pDeveloper.Focus();
                }
            }
        }

        private void AutorContact_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ObjLog.LOGTextAppend("���� ������������ ������ �� ������������ ���������");
            Process.Start(new ProcessStartInfo("https://vk.com/l1s8vr9al") { UseShellExecute = true });
            FoldingMoveApplication(null, null);
        }

        private void PbCustom_DoubleClick(object sender, EventArgs e)
        {
            ObjLog.LOGTextAppend($"������� ���������� ���� �� ��������� ���������� GIF");
            App.DialogCustomImage_Form = new DialogCustomImage();
            App.DialogCustomImage_Form.ShowDialog();
        }

        private void TbInput_TextChanged(object sender, EventArgs e)
        {
            if ((tbInput.TextLength >= 42 && tbInput.Size.Width == 322) || (tbInput.TextLength <= 41 && tbInput.Size.Width > 322)) TbInputChangeLineText();
            if (tbInput.TextLength == tbInput.MaxLength || lMaxLangthCommand.Location.Y > 515)
            {
                ConstAnimMove ConstFormule = new(lMaxLangthCommand.Location.Y, tbInput.TextLength == tbInput.MaxLength ? 524 : 515, 10);
                new ConstAnimMove(lMaxLangthCommand.Location.X).InitAnimFormule(lMaxLangthCommand, Formules.QuickTransition, ConstFormule, AnimationStyle.XY);
            }
            if (Flags.BufferConsole.Value || !(bool)MainData.Settings.Hit_Panel) return;
            if (tbInput.TextLength == 0)
            {
                HitPanelDiactivate();
                return;
            }

            for (int i = 0; i < HitCommandConsole.Count; i++)
            {
                HitCommandConsole[i].Dispose();
            }
            HitCommandConsole.Clear();

            int MaxWigth = 0;
            string RegularString = Regex.Escape(tbInput.Text.Replace(' ', '_'));
            Match match;
            for (int i = 0; i < MainData.MainCommandData.MassConsoleCommand.Length; i++)
            {
                match = Regex.Match(MainData.MainCommandData.MassConsoleCommand[i].Name, @$"\b{RegularString}.*");
                if (match.Value.Length > 0)
                {
                    lock (this)
                    {
                        Color MP = MainData.MainThemeData.ActivateTheme.ObjColors[8].ElColor;
                        Label Hit = new()
                        {
                            Parent = pHitCommandConsole,
                            Location = new(3, HitCommandConsole.Count == 0 ? 2 : (HitCommandConsole.Count * 18) + 2),
                            AutoSize = true,
                            Text = $"{HitCommandConsole.Count + 1}. {match.Value}",
                            Font = new(Font, FontStyle.Bold),
                            Tag = match.Value,
                            ForeColor = Color.White,
                            BackColor = Color.FromArgb(MP.R + (MP.R <= 150 ? 55 : -55), MP.G + (MP.G <= 150 ? 55 : -55), MP.B + (MP.B <= 150 ? 55 : -55)),
                        };
                        Hit.Click += (sender, e) =>
                        {
                            tbInput.Text = Hit.Tag.ToString() ?? string.Empty;
                            tbInput.SelectionStart = tbInput.TextLength;
                            HitPanelDiactivate();
                        };
                        Hit.MouseEnter += (sender, e) =>
                        {
                            Hit.BackColor = Color.FromArgb(
                                Hit.BackColor.R + (Hit.BackColor.R <= 150 ? 55 : -55),
                                Hit.BackColor.G + (Hit.BackColor.G <= 150 ? 55 : -55),
                                Hit.BackColor.B + (Hit.BackColor.B <= 150 ? 55 : -55));
                        };
                        Hit.MouseLeave += (sender, e) =>
                        {
                            Hit.BackColor = Color.FromArgb(
                                MP.R + (MP.R <= 150 ? 55 : -55),
                                MP.G + (MP.G <= 150 ? 55 : -55),
                                MP.B + (MP.B <= 150 ? 55 : -55));
                        };
                        HitCommandConsole.Add(Hit);
                        if (HitCommandConsole[^1].Size.Width > MaxWigth) MaxWigth = HitCommandConsole[^1].Size.Width;
                    }
                }
            }
            if (HitCommandConsole.Count > 0) HitPanelActivate(MaxWigth);
            else HitPanelDiactivate();
        }

        /// <summary>
        /// ������������ ������ ��������� � ��������
        /// </summary>
        private void HitPanelActivate(int MaxWigth)
        {
            if (!Flags.ActiveHitPanelConsole.Value)
            {
                pHitCommandConsole.Size = new(MaxWigth + 9, 0);
                pHitCommandConsole.Location = new(tbInput.Location.X + tbInput.Size.Width - MaxWigth - 9, tbInput.Location.Y + tbInput.Size.Height + 13);
                Flags.ActiveHitPanelConsole.Value = true;
            }
            int Offset = pHitCommandConsole.Location.Y + HitCommandConsole[^1].Location.Y + HitCommandConsole[^1].Size.Height;
            ConstAnimMove ConstClosePanelHitY = new(pHitCommandConsole.Location.Y, Offset + (tbInput.Location.Y - Offset) - (HitCommandConsole[^1].Location.Y + HitCommandConsole[^1].Size.Height) - 3, 6);
            ConstAnimMove ConstClosePanelHitH = new(pHitCommandConsole.Size.Height, HitCommandConsole[^1].Location.Y + HitCommandConsole[^1].Size.Height + 3, 6);
            Instr_GroupAnimFormule GroupClose = new(Formules.QuickTransition, null, ConstClosePanelHitY, null, ConstClosePanelHitH);
            GroupClose.InitGroupAnimation(pHitCommandConsole);
        }

        /// <summary>
        /// �������������� ������ ��������� � ��������
        /// </summary>
        private void HitPanelDiactivate()
        {
            if (!Flags.ActiveHitPanelConsole.Value) return;
            Flags.ActiveHitPanelConsole.Value = false;
            ConstAnimMove ConstClosePanelHitY = new(pHitCommandConsole.Location.Y, pHitCommandConsole.Location.Y + pHitCommandConsole.Size.Height, 6);
            ConstAnimMove ConstClosePanelHitH = new(pHitCommandConsole.Size.Height, 0, 6);
            Instr_GroupAnimFormule GroupClose = new(Formules.QuickTransition, null, ConstClosePanelHitY, null, ConstClosePanelHitH);
            GroupClose.InitGroupAnimation(pHitCommandConsole);
            for (int i = 0; i < HitCommandConsole.Count; i++)
            {
                HitCommandConsole[i].Dispose();
                HitCommandConsole.RemoveAt(i);
            }
        }

        /// <summary>
        /// ������� ����-������ ��������
        /// </summary>
        private void PanelSettingsActivate()
        {
            Flags.ActiveSettingsMiniPanel.Value = true;
            lSettingsVolume.Text = "��������...";
            pSettingsVolumeDivace.Size = new(144, 24);
            UpdateInformationAudioDevice(false);
            pSettings.Focus();
            pbSttingsName.Cursor = Cursors.Default;
            pSettings.Cursor = Cursors.Default;

            ConstAnimMove ConstantFormulePanel = new(pSettings.Location.X, -5, 12);
            ConstAnimMove ConstantFormuleText = new(pbSttingsName.Location.Y, 0, 12);
            ConstantFormulePanel.InitAnimFormule(pSettings, Formules.QuickTransition, new ConstAnimMove(pSettings.Location.Y), AnimationStyle.XY);
            new ConstAnimMove(pbSttingsName.Location.X).InitAnimFormule(pbSttingsName, Formules.QuickTransition, ConstantFormuleText, AnimationStyle.XY);
        }

        /// <summary>
        /// ������� ����-������ ��������
        /// </summary>
        private void PanelSettingsDiactivate()
        {
            Flags.ActiveSettingsMiniPanel.Value = false;
            tbOutput.Focus();
            pbSttingsName.Cursor = Cursors.Hand;
            pSettings.Cursor = Cursors.Hand;

            ConstAnimMove ConstantFormulePanel = new(pSettings.Location.X, 0 - pSettings.Width + 13, 12);
            ConstAnimMove ConstantFormuleText = new(pbSttingsName.Location.Y, pSettings.Height / 2 - pbSttingsName.Height / 2, 12);
            ConstantFormulePanel.InitAnimFormule(pSettings, Formules.QuickTransition, new ConstAnimMove(pSettings.Location.Y), AnimationStyle.XY);
            new ConstAnimMove(pbSttingsName.Location.X).InitAnimFormule(pbSttingsName, Formules.QuickTransition, ConstantFormuleText, AnimationStyle.XY);
        }

        /// <summary>
        /// ������������ �������� ��������
        /// </summary>
        /// <param name="Text">����� ��������</param>
        private void ActivateLabelInfo(string Text)
        {
            Flags.Information.Value = true;
            lInformationCursor.Text = Text;
            lInformationCursor.Show();
        }

        /// <summary>
        /// �������������� �������� ��������
        /// </summary>
        private void DisactivateLabelInfo()
        {
            Flags.Information.Value = false;
            lInformationCursor.Text = string.Empty;
            lInformationCursor.Hide();
        }

        private void Settings_MouseHover(object sender, EventArgs e)
        {
            ActivateLabelInfo("Text");
        }

        private void Settings_MouseLeave(object sender, EventArgs e) => DisactivateLabelInfo();

        private void ApplicationCLR_Shown(object sender, EventArgs e)
        {
            Location = new(Location.X, Location.Y + 150);
            WindowState = FormWindowState.Normal;
            Show();
            if (!Focused) Activate();
        }

        /// <summary>
        /// �������� ����������� ������ ��������� ������
        /// </summary>
        /// <param name="flag">������ ��������� ��������� ������</param>
        /// <param name="MouseEnter">�������� �� ������ �������</param>
        private void VoiceButtonImageUpdate(StatusFlags flag, bool MouseEnter)
        {
            if (!Flags.FormActivity.Value) return;
            string DirectoryImage = @"Data\Image\Micro\Micro";
            DirectoryImage += flag switch
            {
                StatusFlags.Active => "Activate",
                StatusFlags.Sleep => "Sleeping",
                StatusFlags.NotActive => "Disactivate",
                _ => string.Empty
            };
            DirectoryImage += $"{(MouseEnter ? "Yes" : "Not")}Mouse.png";
            pbVoiceButton.ImageLocation = DirectoryImage;
            pbVoiceButton.Image = Image.FromFile(DirectoryImage);
        }

        private void VoiceButtonUpdateMouseEnter(object sender, EventArgs e)
        {
            VoiceButtonImageUpdate(MainData.Flags.AudioCommand, true);
        }

        private void Voice_Info_MouseLeave(object sender, EventArgs e)
        {
            VoiceButtonImageUpdate(MainData.Flags.AudioCommand, false);
        }

        private void BWhatInformation_MouseEnter(object sender, EventArgs e)
        {
            if (Flags.FormActivity.Value)
            {
                ConstAnimMove ConstantFormule = new(lWhat.Size.Width, 120, 15);
                ConstantFormule.InitAnimFormule(lWhat, Formules.QuickTransition, new ConstAnimMove(lWhat.Size.Height), AnimationStyle.Size);
            }
        }

        private void BWhatInformation_MouseLeave(object sender, EventArgs e)
        {
            if (Flags.FormActivity.Value)
            {
                ConstAnimMove ConstantFormule = new(lWhat.Size.Width, 0, 15);
                ConstantFormule.InitAnimFormule(lWhat, Formules.QuickTransition, new ConstAnimMove(lWhat.Size.Height), AnimationStyle.Size);
            }
        }

        private void BWhatInformation_MouseClick(object sender, MouseEventArgs e)
        {
            BWhatInformation_MouseLeave(null, null);
            ConsoleCommand.ReadDefaultConsoleCommand("help").ExecuteCommand(false);
        }

        private void UpperPanelActivate(object sender, EventArgs e)
        {
            if (Flags.FormActivity.Value)
            {
                ConstAnimMove ConstantFormule = new(pUppingMenu.Location.Y, -1, 9);
                new ConstAnimMove(pUppingMenu.Location.X).InitAnimFormule(pUppingMenu, Formules.QuickTransition, ConstantFormule, AnimationStyle.XY);
                if (Flags.PAC_PanelActivate.Value) PAC_Disactivate(false);
                pUppingMenu.Focus();
            }
        }

        private void UpperPanelDiactivate(object sender, EventArgs e)
        {
            ConstAnimMove ConstantFormule = new(pUppingMenu.Location.Y, -22, 9);
            new ConstAnimMove(pUppingMenu.Location.X).InitAnimFormule(pUppingMenu, Formules.QuickTransition, ConstantFormule, AnimationStyle.XY);
        }

        private void ActivateFormThanks(object sender, EventArgs e)
        {
            App.Thanks = null;
            App.MainForm.FoldingApplication(null, null);
            Flags.FormActivity.Value = false;
            App.Thanks = new();
            Thread.Sleep(100);
            App.Thanks.Show();
        }

        public void BCloseMainApplication(object sender, EventArgs e)
        {
            App.Log?.Close();
            App.InformationCommand?.Close();
            App.Settings.WindowSettings?.Close();
            App.Settings.ThemesCreated?.Close();
            Environment.Exit(0);
        }

        /// <summary>
        /// ������������� �������� ������������ ��� ��������� ���� ������� �����
        /// </summary>
        /// <param name="sender">������ �������</param>
        /// <param name="e">�������</param>
        public async void UnfoldingApplication(object sender, EventArgs e)
        {
            if (StateAnimWindow != StateAnimateWindow.Hide && StateAnimWindow != StateAnimateWindow.HalfHide) return;

            ObjLog.LOGTextAppend("������� ����� �������");
            App.MainForm.WindowState = FormWindowState.Normal;
            Flags.FormActivity.Value = true;

            if (StateAnimWindow == StateAnimateWindow.Hide)
            {
                StateAnimWindow = StateAnimateWindow.MoveHidePlus;
                int i = 0;
                while (Opacity < 1d && StateAnimWindow == StateAnimateWindow.MoveHidePlus)
                {
                    await Task.Run(() =>
                    {
                        Opacity += 0.02d;
                        Location = new(Location.X, Location.Y > SavePositionAnimateWindow.Y ? Location.Y - 3 : SavePositionAnimateWindow.Y);
                        if (i++ % 9 == 0) Thread.Sleep(1);
                        Update();
                    });
                }
            }
            else if (StateAnimWindow == StateAnimateWindow.HalfHide)
            {
                StateAnimWindow = StateAnimateWindow.OpacityHidePlus;
                int i = 0;
                while (Opacity < 1d && StateAnimWindow == StateAnimateWindow.OpacityHidePlus)
                {
                    await Task.Run(() =>
                    {
                        Opacity += 0.009d;
                        if (i++ % 6 == 0) Thread.Sleep(1);
                        Update();
                    });
                }
            }
            if (StateAnimWindow == StateAnimateWindow.OpacityHidePlus || StateAnimWindow == StateAnimateWindow.MoveHidePlus)
            {
                Opacity = 1d;
                StateAnimWindow = StateAnimateWindow.Active;
            }
        }

        /// <summary>
        /// ������������� �������� ������������ ���� ������� �����
        /// </summary>
        /// <param name="sender">������ �������</param>
        /// <param name="e">�������</param>
        public async void FoldingMoveApplication(object sender, EventArgs e)
        {
            Flags.FormActivity.Value = false;
            StateAnimWindow = StateAnimateWindow.MoveHideMinus;
            int i = 0;
            while (Opacity > 0.1d && StateAnimWindow == StateAnimateWindow.MoveHideMinus)
            {
                await Task.Run(() =>
                {
                    Opacity -= 0.02d;
                    Location = new(Location.X, Location.Y + 3);
                    if (i++ % 9 == 0) Thread.Sleep(1);
                    Update();
                });
            }
            if (StateAnimWindow == StateAnimateWindow.MoveHideMinus)
            {
                Opacity = 0d;
                App.MainForm.WindowState = FormWindowState.Minimized;
                StateAnimWindow = StateAnimateWindow.Hide;
            }
        }

        /// <summary>
        /// ������������� �������� ������������ ���� ������� �����
        /// </summary>
        /// <param name="sender">������ �������</param>
        /// <param name="e">�������</param>
        public async void FoldingApplication(object sender, EventArgs e)
        {
            Flags.FormActivity.Value = false;
            StateAnimWindow = StateAnimateWindow.OpacityHideMinus;
            int i = 0;
            while (Opacity > 0.45d && StateAnimWindow == StateAnimateWindow.OpacityHideMinus)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        Opacity -= 0.009d;
                        if (i++ % 6 == 0) Thread.Sleep(1);
                        Update();
                    }
                    catch
                    {
                        
                    }
                });
            }
            if (StateAnimWindow == StateAnimateWindow.OpacityHideMinus)
            {
                Opacity = 0.45d;
                StateAnimWindow = StateAnimateWindow.HalfHide;
            }
        }

        /// <summary>
        /// ������������ ��������� ������� ���� ������� �����
        /// </summary>
        /// <param name="sender">������ �������</param>
        /// <param name="e">�������</param>
        private async void MovingApplicationActivate(object sender, MouseEventArgs e)
        {
            Point StartCursor = Cursor.Position, StartLocation = Location;
            int MPX, MPY;
            Flags.MovingApplication.Value = true;
            while (Flags.MovingApplication.Value)
            {
                await Task.Run(() =>
                {
                    MPX = StartLocation.X + (Cursor.Position.X - StartCursor.X);
                    MPY = StartLocation.Y + (Cursor.Position.Y - StartCursor.Y);

                    if ((!MainData.Settings.Moving_Border_Screen_Form.Value && MPX >= 0 && MPX + Size.Width <= Screen.PrimaryScreen?.Bounds.Width) ||
                    MainData.Settings.Moving_Border_Screen_Form.Value)
                        Location = new(MPX, Location.Y);
                    if ((!MainData.Settings.Moving_Border_Screen_Form.Value && MPY >= 0 && MPY + Size.Height <= Screen.PrimaryScreen?.Bounds.Height) ||
                    MainData.Settings.Moving_Border_Screen_Form.Value)
                        Location = new(Location.X, MPY);
                });
            }
        }

        /// <summary>
        /// �������������� ��������� ������� ���� ������� �����
        /// </summary>
        /// <param name="sender">������ �������</param>
        /// <param name="e">�������</param>
        private void MovingApplicationDiactivate(object sender, MouseEventArgs e)
        {
            Flags.MovingApplication.Value = false;
            SavePositionAnimateWindow = Location;
        }

        public void BSettings_Click(object sender, EventArgs e)
        {
            ObjLog.LOGTextAppend("���� ������������ ����� ��������");
            App.Settings.WindowSettings = new();
            App.Settings.WindowSettings.ShowDialog();
        }

        public void TbOutput_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (Math.Abs(StartCursorMovingtbOutput.X - Cursor.Position.X) < 15 || Flags.PAC_PanelActivate.Value))
            {
                PAC_Activate();
            }
            else if (e.Button == MouseButtons.Left && Flags.PAC_PanelActivate.Value)
            {
                PAC_Disactivate();
            }
        }

        /// <summary>
        /// ������������ ����-������ ������� ��������
        /// </summary>
        /// <param name="LabelAccessDetect">PAC ������������ � ������� LabelAccess</param>
        private void PAC_Activate(bool LabelAccessDetect = false)
        {
            const int OffsetX = 5, OffsetY = -30, WidthA = 212, HeightA = 221;
            int XPos = Cursor.Position.X - App.MainForm.Location.X + OffsetX, YPos = Cursor.Position.Y - App.MainForm.Location.Y + OffsetY;
            int ConstX, ConstY;
            if (pDeveloper.Location.X <= tbOutput.Location.X + tbOutput.Size.Width)
            {
                if (XPos + WidthA <= pDeveloper.Location.X) ConstX = XPos;
                else ConstX = pDeveloper.Location.X - WidthA;
            }
            else
            {
                if (XPos + WidthA <= tbOutput.Location.X + tbOutput.Size.Width ||
                    (LabelAccessDetect && XPos + WidthA <= pLabelExplorer.Location.X + 17)) ConstX = XPos;
                else
                {
                    if (!LabelAccessDetect) ConstX = tbOutput.Location.X + tbOutput.Size.Width - WidthA;
                    else ConstX = pLabelExplorer.Location.X + 17 - WidthA;
                }
            }
            if (YPos + HeightA <= tbOutput.Location.Y + tbOutput.Size.Height ||
                (LabelAccessDetect && YPos + HeightA <= pLabelExplorer.Location.Y + pLabelExplorer.Size.Height)) ConstY = YPos;
            else
            {
                if (!LabelAccessDetect) ConstY = tbOutput.Location.Y + tbOutput.Size.Height - HeightA;
                else ConstY = pLabelExplorer.Location.Y + pLabelExplorer.Size.Height - HeightA;
            }

            if (!Flags.PAC_PanelActivate.Value)
            {
                pMiniPanelOutput.Location = new(ConstX, ConstY);
                pMiniPanelOutput.Size = new(0, 0);
                pMiniPanelOutput.Focus();
                Flags.PAC_PanelActivate.Value = true;
                if (!LabelAccessDetect)
                {
                    pmpMain.Location = new(0, 0);
                    pmpMain.BringToFront();
                    ActivatePACPanelPage = pmpMain;
                }
                else
                {
                    pmpLabel.Location = new(0, 0);
                    pmpLabel.BringToFront();
                    ActivatePACPanelPage = pmpLabel;
                    PAC_PanelActivityCache = pmpMain;
                }
            }
            else
            {
                if (!ActivatePACPanelPage.Name.Equals(pmpLabel.Name) && LabelAccessDetect)
                {
                    MiniFunctions.OpenNewMiniMenu(pmpLabel, ActivatePACPanelPage, 0);
                    PAC_PanelActivityCache = ActivatePACPanelPage;
                    ActivatePACPanelPage = pmpLabel;
                }
                else if (ActivatePACPanelPage.Name.Equals(pmpLabel.Name) && !LabelAccessDetect && PAC_PanelActivityCache != null)
                {
                    MiniFunctions.OpenNewMiniMenu(PAC_PanelActivityCache, pmpLabel, 0, DirectionsParties.Left);
                    ActivatePACPanelPage = PAC_PanelActivityCache;
                    PAC_IndexIELLabelAccess = null;
                    PAC_PanelActivityCache = null;
                }
            }

            if (pMiniPanelOutput.Width < WidthA || pMiniPanelOutput.Height < 211)
            {
                ConstAnimMove ConstantFormuleX = new(pMiniPanelOutput.Location.X, ConstX, 12);
                ConstAnimMove ConstantFormuleY = new(pMiniPanelOutput.Location.Y, ConstY, 12);
                ConstAnimMove ConstantFormuleW = new(pMiniPanelOutput.Width, WidthA, 12);
                ConstAnimMove ConstantFormuleH = new(pMiniPanelOutput.Height, HeightA, 12);
                Instr_GroupAnimFormule GR = new(Formules.QuickTransition, ConstantFormuleX, ConstantFormuleY, ConstantFormuleW, ConstantFormuleH);
                GR.InitGroupAnimation(pMiniPanelOutput);
            }
            else
            {
                ConstAnimMove ConstantFormuleX = new(pMiniPanelOutput.Location.X, ConstX, 12);
                ConstAnimMove ConstantFormuleY = new(pMiniPanelOutput.Location.Y, ConstY, 12);
                ConstantFormuleX.InitAnimFormule(pMiniPanelOutput, Formules.QuickTransition, ConstantFormuleY, AnimationStyle.XY);
            }
        }

        /// <summary>
        /// �������������� ���� ������ �������
        /// </summary>
        /// <param name="FocusMode">�������������� �� ����� �� ������ �������</param>
        private void PAC_Disactivate(bool FocusMode = true)
        {
            Flags.PAC_PanelActivate.Value = false;
            ActivatePACPanelPage = pmpMain;
            if (FocusMode) tbInput.Focus();
            ConstAnimMove ConstantFormuleW = new(pMiniPanelOutput.Size.Width, 0, 10);
            ConstAnimMove ConstantFormuleH = new(pMiniPanelOutput.Size.Height, 0, 10);
            ConstantFormuleW.InitAnimFormule(pMiniPanelOutput, Formules.QuickTransition, ConstantFormuleH, AnimationStyle.Size);

            if (Flags.PAC_PanelAltActivate.Value && (bool)MainData.Settings.Alt_Diactivate_PAC)
            {
                Flags.PAC_PanelAltActivate.Value = false;
                CgangeAllButtonAltMode();
            }
            if (ActivatePACPanelPage.Name.Equals(pmpLabel.Name))
            {
                PAC_IndexIELLabelAccess = null;
                PAC_PanelActivityCache = null;
            }
        }

        private void UpdateDeveloperInfoChar(object sender, EventArgs e)
        {
            lDeveloper_InformationChar.Text = $"IC: {(tbCheckSymbol.Text.Length > 0 ? Convert.ToChar(tbCheckSymbol.Text) : 0)}";
        }

        private void BSettingsMute_Click(object sender, EventArgs e)
        {
            try
            {
                if (MainData.Divaces.ActiveDevice != null)
                {
                    if (MainData.Divaces.ActiveDevice.AudioEndpointVolume.Mute) bSettingsMute.BackgroundImage = Image.FromFile($"{Directory.GetCurrentDirectory()}\\Data\\Image\\Sound.png");
                    else bSettingsMute.BackgroundImage = Image.FromFile($"{Directory.GetCurrentDirectory()}\\Data\\Image\\Mute.png");
                    MainData.Divaces.ActiveDevice.AudioEndpointVolume.Mute = !MainData.Divaces.ActiveDevice.AudioEndpointVolume.Mute;
                }
            }
            catch { }
        }

        private void TbSettingsVolume_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (MainData.Divaces.ActiveDevice != null)
                {
                    MainData.Divaces.ActiveDevice.AudioEndpointVolume.MasterVolumeLevelScalar = tbSettingsVolume.Value / 100f;
                    lSettingsVolume.Text = $"���������: {tbSettingsVolume.Value}";
                }
            }
            catch { }
        }

        public void LmpExecuteCommands_Click()
        {
            PAC_bClearBuffer.Location = new(82, PAC_Buffer.ElementsBuffer.Count > 0 ? 3 : -29);
            //PAC_sbBuffer.Visible = MainData.Settings.BufferCommand.Count > 0;
            //PAC_sbBuffer.Location = new(189, PAC_sbBuffer.Location.Y);
            App.MainForm.PAC_lBufferCount.Text = $"{PAC_Buffer.ElementsBuffer.Count}/{PAC_Buffer.ElementsBuffer.Length}";
            if (PAC_Buffer.ElementsBuffer.Count == 0)
            {
                //lmpExecuteInfo.Show();
            }
            else
            {
                /*Label DownButton = MainData.Settings.BufferCommand[MainData.Settings.BufferCommand.Count - 1];
                lmpExecuteInfo.Hide();
                if (DownButton.Location.Y + DownButton.Size.Height + 23 > pmpBuffer.Height)
                {
                    PAC_sbBuffer.Minimum = 0;
                    int OffsetpDeveloper = DownButton.Location.Y + DownButton.Size.Height + 33 - pmpBuffer.Height;
                    PAC_sbBuffer.Maximum = OffsetpDeveloper;
                    PAC_sbBuffer.SmallChange = OffsetpDeveloper / 7;
                }
                else PAC_sbBuffer.Hide();
                */
            }
            ActivatePACPanelPage = pmpBuffer;
            MiniFunctions.OpenNewMiniMenu(pmpBuffer, pmpMain, 0);
        }

        private void ClearBufferCommand()
        {
            PAC_Buffer.DeleteAll();
            App.MainForm.PAC_lBufferCount.Text = $"0/{PAC_Buffer.ElementsBuffer.Length}";
            new ConstAnimMove(PAC_bClearBuffer.Location.X)
                .InitAnimFormule(PAC_bClearBuffer, Formules.QuickTransition, new(PAC_bClearBuffer.Location.Y, -26, 7), AnimationStyle.XY);
        }

        private void TbInputDisactivate(object sender, EventArgs e)
        {
            if (Flags.BufferConsole.Value)
            {
                Flags.BufferConsole.Value = false;
                ConstAnimMove ConstFormule = new(lCountActiveBufferCommand.Location.X, -14, 7);
                ConstFormule.InitAnimFormule(lCountActiveBufferCommand, Formules.QuickTransition, new ConstAnimMove(lCountActiveBufferCommand.Location.Y), AnimationStyle.XY);
            }
        }

        private void TbNameColorParamContains_TextChanged(object sender, EventArgs e)
        {
            lDeveloper_ContainsTextInColorParam.Text = $"CTICP: {MainData.MainThemeData.MassInfoParameters.Select(i => i.Name).Contains(tbNameColorParamContains.Text)}";
        }

        private async void TbOutput_MouseDown(object sender, MouseEventArgs e)
        {
            StartCursorMovingtbOutput = Cursor.Position;
            if (!Flags.PAC_PanelActivate.Value && pDeveloper.Location.X >= 760)
            {
                Point StartTbOutPutPoint = tbOutput.Location;
                Size StartTbOutPutSize = tbOutput.Size;
                if (StartTbOutPutSize.Width == 771) pLabelExplorer.Location = new(604, pLabelExplorer.Location.Y);
                int Height = tbOutput.Size.Height, Y = tbOutput.Location.Y;
                MainData.Flags.ActiveMovingMainConsole = Data.BooleanFlags.True;
                int StartActivePanelExplorerX = App.MainForm.pLabelExplorer.Location.X;
                int pAllLabelExplorerEndX = pLabelExplorer.Location.X;
                ConstAnimMove ConstFormuleW;
                ConstAnimMove ConstPanelExplorer;
                while (MainData.Flags.ActiveMovingMainConsole == BooleanFlags.True)
                {
                    await Task.Run(() =>
                    {
                        if (Cursor.Position.X < StartCursorMovingtbOutput.X)
                        {
                            tbOutput.Size = new(StartTbOutPutSize.Width - ((StartCursorMovingtbOutput.X - Cursor.Position.X) / (StartTbOutPutSize.Width == 771 ? 17 : 40)), Height);
                        }
                        else if (Cursor.Position.X > StartCursorMovingtbOutput.X)
                        {
                            tbOutput.Location = new(StartTbOutPutPoint.X + ((Cursor.Position.X - StartCursorMovingtbOutput.X) / 30), Y);
                            tbOutput.Size = new(StartTbOutPutSize.Width - ((Cursor.Position.X - StartCursorMovingtbOutput.X) / 30), Height);
                        }
                        if (StartCursorMovingtbOutput.X - Cursor.Position.X <= 0 && Flags.ActiveExplorerLabel.Value)
                            App.MainForm.pLabelExplorer.Location = new(StartActivePanelExplorerX + (StartCursorMovingtbOutput.X - Cursor.Position.X) / 8, App.MainForm.pLabelExplorer.Location.Y);
                    });
                }
                if (StartCursorMovingtbOutput.X - Cursor.Position.X >= 55)
                {
                    ConstFormuleW = new(tbOutput.Size.Width, 685, 7);
                    pAllLabelExplorerEndX = 704;

                    if (!Flags.ActiveExplorerLabel.Value)
                    {
                        pAllVisualLabel.Location = new(pAllVisualLabel.Location.X, pLabelExplorer.Height);
                        ConstPanelExplorer = new(pAllVisualLabel.Location.Y, 14 - 56 * ScrollLabels.Value, 17);
                        new ConstAnimMove(pAllVisualLabel.Location.X).InitAnimFormule(pAllVisualLabel, Formules.QuickTransition, ConstPanelExplorer, AnimationStyle.XY);
                    }

                    Flags.ActiveExplorerLabel.Value = true;
                }
                else if (StartCursorMovingtbOutput.X - Cursor.Position.X <= -130)
                {
                    ConstFormuleW = new(tbOutput.Size.Width, 771, 7);
                    if (Flags.ActiveExplorerLabel.Value)
                    {
                        Flags.ActiveExplorerLabel.Value = false;
                    }
                    else
                    {
                        ObjLog.LOGTextAppend($"���� ���������� ������� ������� <tbOutput> (��������� �������)");
                        AnimationDL.StopAnimate(AnimationDL.StyleAnimateObj.AnimText, "tbOutput");
                        tbOutput.Text = string.Empty;
                        LComplete_Click(null, null);
                    }
                }
                else
                {
                    ConstFormuleW = new(tbOutput.Size.Width, StartTbOutPutSize.Width, 7); // StartTbOutPutSize.Width
                }
                ConstPanelExplorer = new(pLabelExplorer.Location.X, pAllLabelExplorerEndX, 15);
                ConstPanelExplorer.InitAnimFormule(pLabelExplorer, Formules.QuickTransition, new ConstAnimMove(pLabelExplorer.Location.Y), AnimationStyle.XY);
                ConstAnimMove ConstFormuleX = new(tbOutput.Location.X, StartTbOutPutPoint.X, 10);
                Instr_GroupAnimFormule GroupAnim = new(Formules.QuickTransition, ConstFormuleX, new ConstAnimMove(Y), ConstFormuleW, new ConstAnimMove(Height));
                GroupAnim.InitGroupAnimation(tbOutput);
            }
        }

        private void DeactiveTbOutPut(object sender, MouseEventArgs e)
        {
            MainData.Flags.ActiveMovingMainConsole = Data.BooleanFlags.False;
        }

        /// <summary>
        /// ���������� ������������ ���������� ��� ������������� � ������ �����
        /// </summary>
        public void DeveloperPanelInformation()
        {
            lDeveloper_CursorX.Text = $"CX: {Cursor.Position.X}";
            lDeveloper_CursorY.Text = $"CY: {Cursor.Position.Y}";
            lDeveloper_FormCursorX.Text = $"FCX: {Cursor.Position.X - Location.X}";
            lDeveloper_FormCursorY.Text = $"FCY: {Cursor.Position.Y - Location.Y}";
            lDeveloper_ColorCursor.Text = "RGBCC: (" +
                $"R:{MainData.AllSpecialColor.RGBCC.RealyColor.R}," +
                $"G:{MainData.AllSpecialColor.RGBCC.RealyColor.G}," +
                $"B:{MainData.AllSpecialColor.RGBCC.RealyColor.B})";
            lDeveloper_ColorRGB.Text = "RGB (" +
                $"R:{MainData.AllSpecialColor.RGB.RealyColor.R}, " +
                $"G:{MainData.AllSpecialColor.RGB.RealyColor.G}, " +
                $"B:{MainData.AllSpecialColor.RGB.RealyColor.B})";
            lDeveloper_SongColor.Text = "SC (" +
                $"R:{MainData.AllSpecialColor.SC.RealyColor.R}, " +
                $"G:{MainData.AllSpecialColor.SC.RealyColor.G}, " +
                $"B:{MainData.AllSpecialColor.SC.RealyColor.B})";
            lDeveloper_SAW.Text = $"SAW: <{StateAnimWindow}>";
        }

        private void ActivateLOG(object sender, EventArgs e) => ConsoleCommand.ReadDefaultConsoleCommand("log").ExecuteCommand(false);

        /// <summary>
        /// ��������� ������� ����� �������
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">�������</param>
        public void ActivateConsoleCommand(object sender, EventArgs e)
        {
            TypeCommand.StateResult Result;
            string text = (tbInput.Text ?? string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty);
            if (tbInput.Size.Width > 322) TbInputChangeLineText();
            PAC_Buffer.AddNewElement(text);
            Result = ConsoleCommand.ReadDefaultConsoleCommand(text).ExecuteCommand(true);
            UpdateDeveloperLabel(lDeveloper_StyleCommand, Result.State.ToString());
            Result.Summarize();
        }

        /// <summary>
        /// ������� �������� ���������/����������� ������ � �������
        /// </summary>
        /// <param name="Activate">��������� �� ������� ���������� ����� � �������</param>
        void ConsoleBufferAnimate(bool Activate)
        {
            if ((Activate && !Flags.BufferConsole.Value) || (!Activate && Flags.BufferConsole.Value))
            {
                ConstAnimMove ConstFormule = new(lCountActiveBufferCommand.Location.X, Activate ? 2 : -14, 7);
                ConstFormule.InitAnimFormule(lCountActiveBufferCommand, Formules.QuickTransition, new ConstAnimMove(lCountActiveBufferCommand.Location.Y), AnimationStyle.XY);
                Flags.BufferConsole.Value = Activate;
                IndexConsoleReadBuffer = 0;
            }
        }

        /// <summary>
        /// �������� ����� ������ � �������
        /// </summary>
        public void ActivateConsoleBuffer() => ConsoleBufferAnimate(true);

        /// <summary>
        /// ��������� ����� ������ � �������
        /// </summary>
        public void DiactivateConsoleBuffer() => ConsoleBufferAnimate(false);

        public static void UpdateDeveloperLabel(Label DeveloperLabel, string TextUpdate)
        {
            if (DeveloperLabel.Tag != null)
                DeveloperLabel.Text = DeveloperLabel.Tag.ToString()?.Replace("?", TextUpdate);
            else DeveloperLabel.Text = TextUpdate;
        }

        /// <summary>
        /// ��������� ���������� � ����� ����-������ ��������
        /// </summary>
        /// <param name="Animation">��������������� �� �������� ��� ���</param>
        public async void UpdateInformationAudioDevice(bool Animation = true)
        {
            ConstAnimMove ConstAnim;
            if (!Animation)
            {
                ConstAnim = new(pSettingsVolumeDivace.Size.Height, 92, 30);
                new ConstAnimMove(pSettingsVolumeDivace.Width).InitAnimFormule(pSettingsVolumeDivace, Formules.QuickTransition, ConstAnim, AnimationStyle.Size);
            }
            await Task.Run(() =>
            {
                tbSettingsVolume.Value = (int)(MainData.Divaces.ActiveDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100f);
                if (Animation && !lNameActiveAudioDevice.Text.Equals(MainData.Divaces.ActiveDevice.DeviceFriendlyName))
                {
                    ConstAnim = new(lNameActiveAudioDevice.Location.X, lNameActiveAudioDevice.Location.X, 9);
                    ConstAnim.InitAnimFormule(lNameActiveAudioDevice, Formules.QuickSinusoid, new ConstAnimMove(lNameActiveAudioDevice.Location.Y), AnimationStyle.XY);
                }
                lNameActiveAudioDevice.Text = MainData.Divaces.ActiveDevice.DeviceFriendlyName;
                lSettingsVolume.Text = $"���������: {tbSettingsVolume.Value}";
                if (!MainData.Divaces.ActiveDevice.AudioEndpointVolume.Mute)
                    bSettingsMute.BackgroundImage = Image.FromFile($"{Directory.GetCurrentDirectory()}\\Data\\Image\\Sound.png");
                else bSettingsMute.BackgroundImage = Image.FromFile($"{Directory.GetCurrentDirectory()}\\Data\\Image\\Mute.png");
            });
        }

        private void Change_MicrophoneActivate(object sender, EventArgs e) => MainData.Settings.SetParamOption("Activation-Microphone", cbSettingsVoice.Checked ? "1" : "0");

        /// <summary>
        /// �������� Alt ��������� ���� ������ PAC
        /// </summary>
        private void CgangeAllButtonAltMode()
        {
            PAC_bSetColor.AltIndexActivity = Flags.PAC_PanelAltActivate.Value;
            PAC_bClearConsole.AltIndexActivity = Flags.PAC_PanelAltActivate.Value;
            PAC_bColoredTheme.AltIndexActivity = Flags.PAC_PanelAltActivate.Value;
            PAC_bLogMessage.AltIndexActivity = Flags.PAC_PanelAltActivate.Value;
            PAC_bCommandBuffer.AltIndexActivity = Flags.PAC_PanelAltActivate.Value;
            PAC_bConductor.AltIndexActivity = Flags.PAC_PanelAltActivate.Value;

            PAC_bBackBuffer.AltIndexActivity = Flags.PAC_PanelAltActivate.Value;
            PAC_bClearBuffer.AltIndexActivity = Flags.PAC_PanelAltActivate.Value;

            PAC_bBackConductor.AltIndexActivity = Flags.PAC_PanelAltActivate.Value;
            PAC_bActiveExplorer.AltIndexActivity = Flags.PAC_PanelAltActivate.Value;
            PAC_bMainExplorer.AltIndexActivity = Flags.PAC_PanelAltActivate.Value;

            PAC_bActivateLabel.AltIndexActivity = Flags.PAC_PanelAltActivate.Value;
            PAC_bDeleteLabel.AltIndexActivity = Flags.PAC_PanelAltActivate.Value;
        }

        private void AltMiniPanelDetect(Keys key)
        {
            if (key == Keys.Escape) PAC_Disactivate();
            else if ((bool)MainData.Settings.Alt_OrientationLR_PAC && key == (Keys)MainData.Settings.HC_Alt_Activate_PAC.Value)
            {
                Flags.PAC_PanelAltActivate.Value = key != (Keys)MainData.Settings.HC_Alt_Diactivate_PAC.Value || !Flags.PAC_PanelAltActivate.Value;
                CgangeAllButtonAltMode();
            }
            else if ((bool)MainData.Settings.Alt_OrientationLR_PAC && key == (Keys)MainData.Settings.HC_Alt_Diactivate_PAC.Value)
            {
                Flags.PAC_PanelAltActivate.Value = key == (Keys)MainData.Settings.HC_Alt_Activate_PAC.Value && !Flags.PAC_PanelAltActivate.Value;
                CgangeAllButtonAltMode();
            }
            else if (!(bool)MainData.Settings.Alt_OrientationLR_PAC && key == Keys.Menu)
            {
                Flags.PAC_PanelAltActivate.Value = !Flags.PAC_PanelAltActivate.Value;
                CgangeAllButtonAltMode();
            }
            else if (Flags.PAC_PanelAltActivate.Value)
            {
                switch (key)
                {
                    case Keys.Oem3:
                        if (ActivatePACPanelPage.Name.Equals(pmpBuffer.Name))
                        {
                            PAC_bBackBuffer.ActivateButtonMPanel(true);
                        }
                        else if (ActivatePACPanelPage.Name.Equals(pmpExplorer.Name))
                        {
                            PAC_bBackConductor.ActivateButtonMPanel(true);
                        }
                        break;

                    case Keys.D1:
                        if (ActivatePACPanelPage.Name.Equals(pmpMain.Name))
                        {
                            PAC_bSetColor.ActivateButtonMPanel(true);
                        }
                        else if (ActivatePACPanelPage.Name.Equals(pmpExplorer.Name))
                        {
                            PAC_bActiveExplorer.ActivateButtonMPanel(true);
                        }
                        else if (ActivatePACPanelPage.Name.Equals(pmpBuffer.Name) && PAC_Buffer.ElementsBuffer.Count > 0)
                        {
                            PAC_bClearBuffer.ActivateButtonMPanel(true);
                        }
                        else if (ActivatePACPanelPage.Name.Equals(pmpLabel.Name))
                        {
                            PAC_bActivateLabel.ActivateButtonMPanel(true);
                        }
                        break;

                    case Keys.D2:
                        if (ActivatePACPanelPage.Name.Equals(pmpMain.Name))
                        {
                            PAC_bClearConsole.ActivateButtonMPanel(true);
                        }
                        else if (ActivatePACPanelPage.Name.Equals(pmpExplorer.Name))
                        {
                            PAC_bMainExplorer.ActivateButtonMPanel(true);
                        }
                        else if (ActivatePACPanelPage.Name.Equals(pmpLabel.Name))
                        {
                            PAC_bDeleteLabel.ActivateButtonMPanel(true);
                        }
                        break;

                    case Keys.D3:
                        if (ActivatePACPanelPage.Name.Equals(pmpMain.Name))
                        {
                            PAC_bColoredTheme.ActivateButtonMPanel(true);
                        }
                        break;

                    case Keys.D4:
                        if (ActivatePACPanelPage.Name.Equals(pmpMain.Name))
                        {
                            PAC_bLogMessage.ActivateButtonMPanel(true);
                        }
                        break;

                    case Keys.D5:
                        if (ActivatePACPanelPage.Name.Equals(pmpMain.Name))
                        {
                            PAC_bCommandBuffer.ActivateButtonMPanel(true);
                        }
                        break;

                    case Keys.D6:
                        if (ActivatePACPanelPage.Name.Equals(pmpMain.Name))
                        {
                            PAC_bConductor.ActivateButtonMPanel(true);
                        }
                        break;
                }
            }
        }

        private void MovingFormSenterScreen(object sender, MouseEventArgs e)
        {
            if (Screen.PrimaryScreen != null)
            {
                ConstAnimMove ConstAnimX = new(Location.X, Screen.PrimaryScreen.Bounds.Width / 2 - Size.Width / 2, 10);
                ConstAnimMove ConstAnimY = new(Location.Y, Screen.PrimaryScreen.Bounds.Height / 2 - Size.Height / 2, 10);
                ConstAnimX.InitAnimFormule(App.MainForm, Formules.QuickTransition, ConstAnimY, AnimationStyle.XY);
            }
        }

        /// <summary>
        /// ���������� ���� ���������� ����������
        /// </summary>
        public async void AlwaysUpdateWindow()
        {
            ObjLog.LOGTextAppend("���� ������� ������� ��������������� ���������� ������");

            int AllTick = -1, PointVolume;
            MainData.Divaces.UpdateActivateDivaceAudioOutput();
            lData.Text = DateTime.Now.ToString("dd/MM/yyyy");
            object LockPositionVizVol = new();
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (App.MainForm.WindowState == FormWindowState.Normal)
                    {
                        AllTick = ++AllTick % 200;
                        PointVolume = (int)(MainData.Divaces.ActiveDevice.AudioMeterInformation.MasterPeakValue * 100);

                        // �������� ����������
                        lDeveloper_AllTick.Text = $"AT: {AllTick}";

                        lTime.Text = DateTime.Now.ToString("HH:mm:ss");
                        if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0) lData.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        lLanguageName.Text = InputLanguage.CurrentInputLanguage.Culture.Name.Equals("en-US") ? "EN" : "RU";

                        if (Flags.Information.Value) lInformationCursor.Location =
                            new(Cursor.Position.X - Location.X, Cursor.Position.Y - Location.Y - 57);

                        // ���������� ������� ������������� ������
                        lock (LockPositionVizVol)
                        {
                            if (PointVolume > 10)
                            {
                                pbIndicatorMusic1.Location = new(pbIndicatorMusic1.Location.X, 34 - (int)(0.17f * PointVolume));
                                pbIndicatorMusic2.Location = new(pbIndicatorMusic2.Location.X, (int)(0.17f * PointVolume) - 17);
                            }
                            else if (pbIndicatorMusic1.Location.Y != 34 && pbIndicatorMusic2.Location.Y != -17)
                            {
                                pbIndicatorMusic1.Location = new(pbIndicatorMusic1.Location.X, 34);
                                pbIndicatorMusic2.Location = new(pbIndicatorMusic2.Location.X, -17);
                            }
                        }

                        // ���������� ���������� � ������ ������������
                        if ((bool)MainData.Settings.Developer_Mode && MainData.Flags.PanelDeveloper == BooleanFlags.True)
                            App.MainForm.DeveloperPanelInformation();

                        if (AllTick % 10 == 0)
                        {
                            await Task.Run(() =>
                            {
                                Internet_Info.ImageLocation = $@"Data\Image\Internet{(InternetConnection ? "Activate" : "Disactivate")}.png";
                                Internet_Info.Refresh();
                            });

                            // ���������� ��������� �����-�������
                            if (Flags.ActiveSettingsMiniPanel.Value)
                            {
                                MainData.Divaces.UpdateActivateDivaceAudioOutput();
                                UpdateInformationAudioDevice(true);
                            }
                        }
                    }

                    Thread.Sleep(10);
                }
            });
        }

        /// <summary>
        /// �������� ��������� ���� ����������� � ���� �����
        /// </summary>
        /// <param name="theme">���� ��� ����������</param>
        public void UpdateTheme(Theme theme)
        {
            for (int i = 0; i < theme.ObjColors.Length; i++) UpdateThemeIndexElement(i);
        }

        /// <summary>
        /// �������� ���������� ������� ���� �������� ������
        /// </summary>
        /// <param name="Index">������ ������������ ��������</param>
        public void UpdateThemeIndexElement(int Index)
        {
            if (Index < 0 || Index >= MainData.MainThemeData.ActivateTheme.ObjColors.Length)
                throw new ArgumentOutOfRangeException(nameof(Index), $"������ {Index} ������������ �������� ��������� �� ��������� ������� ������ ����.");
            ThemeObjColor ObjMP = MainData.MainThemeData.ActivateTheme.ObjColors[Index];
            Color MP = ObjMP.ElColor;
            switch (Index)
            {
                // ���� ��� ������ � ������� ������
                case 0:
                    tbOutput.BackColor = MP;
                    break;

                // ���� ��� ����-�������
                case 1:
                    // ���� ��� ����-�������
                    pMiniPanelOutput.BackColor = MP;
                    pmpExplorer.BackColor = MP;
                    //pmpBufferCommandButtons.BackColor = MP;
                    pmpMain.BackColor = MP;
                    pmpBuffer.BackColor = MP;
                    break;

                // ���� ��� ������� ������
                case 2:
                    pUppingMenu.BackColor = MP;
                    break;

                // ���� ��� ���� � ������� �����
                case 3:
                    BackColor = MP;
                    pMainConsole.BackColor = MP;
                    lComplete.ForeColor = MP;
                    lActiveitedSoftCommand.ForeColor = MP;
                    lData.BackColor = MP;
                    lTime.BackColor = MP;
                    bWhatInformation.Refresh();
                    bRunCommand.Refresh();
                    bMinimizedApplication.Refresh();
                    bCloseApplication.Refresh();
                    tbInput.Refresh();
                    break;

                // ���� ��� ���� � ���������� ������
                case 4:
                    tbInput.BackColor = MP;
                    break;

                // ���� ���� �������������� ������
                case 5:
                    bWhatInformation.BackColor = MP;
                    break;

                // ���� ���� ����-������ ��������
                case 6:
                    pSettings.BackColor = MP;
                    break;

                // ���� ���� ������ �������
                case 7:
                    pLabelExplorer.BackColor = MP;
                    pAllVisualLabel.BackColor = MP;
                    foreach (IELLabelAccess Element in LabelAccess)
                    {
                        Element.BackColor = Color.FromArgb(MP.R + (MP.R <= 150 ? 55 : -55), MP.R + (MP.R <= 150 ? 55 : -55), MP.R + (MP.R <= 150 ? 55 : -55));
                        Element.Refresh();
                    }
                    break;

                // ���� ���� ������ ��������� � ��������
                case 8:
                    pHitCommandConsole.BackColor = MP;
                    if (HitCommandConsole.Count > 0)
                    {
                        foreach (Label Element in HitCommandConsole)
                        {
                            Element.BackColor = Color.FromArgb(MP.R + (MP.R <= 150 ? 55 : -55), MP.R + (MP.R <= 150 ? 55 : -55), MP.R + (MP.R <= 150 ? 55 : -55));
                            Element.Refresh();
                        }
                    }
                    break;

                // ���� ���� ������ ������������ ������� �����
                case 9:
                    bMinimizedApplication.BackColor = MP;
                    bMinimizedApplication.ForeColor = ColorWhile.InvColor(MP);
                    break;

                // ���� ���� ������ �������� ������� �����
                case 10:
                    bCloseApplication.BackColor = MP;
                    bCloseApplication.ForeColor = ColorWhile.InvColor(MP);
                    break;

                // ���� ������ �������
                case 11:
                    lTime.ForeColor = MP;
                    break;

                // ���� ������ ����
                case 12:
                    lData.ForeColor = MP;
                    break;

                // ���� ������ �����
                case 13:
                    lLanguageName.ForeColor = MP;
                    break;

                // ���� ������ �������
                case 14:
                    tbOutput.ForeColor = MP;
                    break;

                default:
                    throw new Exception($"{ObjMP.Name} ��� �������� {Index} �� ��������������� ��� ���������� {Name}");
            }
        }

        /// <summary>
        /// ��������� ����� ����� ��� ���������� ��������� ������
        /// </summary>
        /// <param name="TextFile">����� ����� ��� ������</param>
        /// <param name="Parent">������ � ������� ����� ��������� �������� �������</param>
        /// <param name="MassLabel">������ �� ���������� ���� ������������ ������� ������</param>
        private void ReadPanelsLabel(string TextFile, Panel? Parent, ref ListLabel<IELLabelAccess> MassLabel)
        {
            MatchCollection BlockLabel = _RegexBlockDetect().Matches(TextFile), OneLabelSort;
            string NameLabel;
            InfoLabelAccess.TypeActionLabel StyleAction;
            TypeLabel StyleLabel;
            for (int i = 0; i < BlockLabel.Count; i++)
            {
                OneLabelSort = _RegexBlockDetalDetect().Matches(BlockLabel[i].Value);
                if (OneLabelSort[0].Value.Length > 9 && OneLabelSort[1].Value.Length > 1 && OneLabelSort[2].Value.Length > 1) continue;

                NameLabel = OneLabelSort[0].Value;

                if (OneLabelSort[1].Value[0] == 'P') StyleLabel = TypeLabel.Priority;
                else if (OneLabelSort[1].Value[0] == 'D') StyleLabel = TypeLabel.Default;
                else continue;

                if (OneLabelSort[2].Value[0] == 'E') StyleAction = InfoLabelAccess.TypeActionLabel.OpenDirectoryElement;
                else if (OneLabelSort[2].Value[0] == 'C') StyleAction = InfoLabelAccess.TypeActionLabel.InitializeCommand;
                else if (OneLabelSort[2].Value[0] == 'L') StyleAction = InfoLabelAccess.TypeActionLabel.OpenLinkBrower;
                else continue;

                IELLabelAccess LabelAccess = new(new(NameLabel, null, StyleAction, OneLabelSort[3].Value), Parent, StyleLabel);
                LabelAccess.RightClickMouse += (info, Index) =>
                {
                    PAC_IndexIELLabelAccess = Index;
                    PAC_Activate(true);
                };
                MassLabel.Add(LabelAccess);
            }
        }

        private void TbInput_MouseClick(object sender, MouseEventArgs e)
        {
            if (Flags.PAC_PanelActivate.Value) PAC_Disactivate(false);
        }

        [GeneratedRegex(@"\b[^\?]+")]
        private static partial Regex _RegexBlockDetect();

        [GeneratedRegex(@"\b[^;]+")]
        private static partial Regex _RegexBlockDetalDetect();
    }

    /// <summary>
    /// ������������ ��������� ���� ������� �����
    /// </summary>
    public enum StateAnimateWindow
    {
        Active = 0,
        Hide = 2,
        OpacityHidePlus = 4,
        OpacityHideMinus = 3,
        MoveHidePlus = 6,
        MoveHideMinus = 5,
        HalfHide = 1,
    }
}