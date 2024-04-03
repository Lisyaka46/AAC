using AAC.Classes;
using AAC.Classes.Commands;
using AAC.Classes.DataClasses;
using AAC.GUI;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static AAC.Classes.AnimationDL.Animate.AnimColor;
using static AAC.Classes.AnimationDL.Animate.AnimFormule;
using static AAC.Classes.AnimationDL.Animate.AnimText;
using static AAC.Classes.Data;
using static AAC.Classes.DataClasses.SettingsData;
using static AAC.Classes.MainTheme;
using static AAC.Forms_Functions;
using static AAC.Startcs;
using System.Reflection;

namespace AAC
{
    public partial class MainApplication : Form
    {

        /// <summary>
        /// Класс объекта флагов формы
        /// </summary>
        private class Flags()
        {
            /// <summary>
            /// Статус активности объекта пояснения
            /// </summary>
            public Flag Information { get; } = new(false);

            /// <summary>
            /// Статус активности взаимодействия с мини-панелью с помощью клавиатуры
            /// </summary>
            public Flag PAC_PanelAltActivate { get; } = new(false);

            /// <summary>
            /// Статус активности главной формы
            /// </summary>
            public Flag FormActivity { get; } = new(true);

            /// <summary>
            /// Статус перемещения окна формы
            /// </summary>
            public Flag MovingApplication { get; } = new(false);

            /// <summary>
            /// Статус активности мини-панели главной формы
            /// </summary>
            public Flag PAC_PanelActivate { get; } = new(false);

            /// <summary>
            /// Статус активности клавиш на клавиатуре
            /// </summary>
            public Flag KeyActivity { get; } = new(false);

            /// <summary>
            /// Статус активности буфера в консольной строке
            /// </summary>
            public Flag BufferConsole { get; } = new(false);

            /// <summary>
            /// Статус активности панели подсказок к командам консоли
            /// </summary>
            public Flag ActiveHitPanelConsole { get; } = new(false);

            /// <summary>
            /// Статус активности панели ярлыков
            /// </summary>
            public Flag ActiveExplorerLabel { get; } = new(false);

            /// <summary>
            /// Статус активности мини-панели настроек
            /// </summary>
            public Flag ActiveSettingsMiniPanel { get; } = new(false);

            /// <summary>
            /// Статус активности переключения кнопок в подсказках к командам
            /// </summary>
            public Flag ActiveIndexSwitchingHitPanel { get; } = new(false);
        }

        private readonly Flags FormFlags = new();

        /// <summary>
        /// Путь к файлу ярлыков
        /// </summary>
        public const string ObjLabelFilePath = $"Data\\Info\\Label.r1";

        /// <summary>
        /// Активная панель в PAC
        /// </summary>
        private Panel ActivatePACPanelPage { get; set; }

        private Point StartCursorMovingtbOutput { get; set; }

        //
        private int IndexConsoleReadBuffer = -1;

        /// <summary>
        /// Сохранённая позиция окна для анимации сворачивания/разворачивания
        /// </summary>
        private Point SavePositionAnimateWindow { get; set; }

        /// <summary>
        /// Состояние окна главной формы
        /// </summary>
        public StateAnimateWindow StateAnimWindow { get; private set; }

        /// <summary>
        /// подсказки к командам
        /// </summary>
        private readonly List<IELHitCommand> LabelHitCommand;

        /// <summary>
        /// Массив ярлыков
        /// </summary>
        private readonly ListLabel<IELLabelAccess> LabelAccess;

        /// <summary>
        /// Индекс активного объекта ярлыка в PAC
        /// </summary>
        private int? IndexActiveLabelHitCommand;

        /// <summary>
        /// Индекс активного объекта ярлыка в PAC
        /// </summary>
        private int? PAC_IndexIELLabelAccess;

        /// <summary>
        /// Кэш активной панели перед активацией действий над ярлыками
        /// </summary>
        private Panel? PAC_PanelActivityCache;

        /// <summary>
        /// Счётчик скролл-бара ярлыков
        /// </summary>
        private readonly CounterScrollBar ScrollLabels;

        /// <summary>
        /// Массив нажатых клавиш
        /// </summary>
        private readonly List<Keys> ControlKey;

        /// <summary>
        /// Поток изменения видимости приложения
        /// </summary>
        private Thread? ThreadWindow { get; set; }

        /// <summary>
        /// Инициализация главной формы программы
        /// </summary>
        public MainApplication()
        {
            InitializeComponent();
            PAC_Buffer.InitializeNewBuffer((int)MainData.Settings.Buffer_Count_Elements.Value);
            //ResHit = new();

            /* Создание отслеживания флагов */
            if ((bool)MainData.Settings.Developer_Mode)
            {
                Array.ForEach(FormFlags.GetType().GetProperties(), (i) =>
                {
                    if (FormFlags.GetType().GetProperty(i.Name)?.GetValue(FormFlags, null) is Flag flag) SetEventFlag(flag, clbDeveloperFlags, clbDeveloperFlags.Items.Add(i.Name));
                    else throw new Exception($"Объект {i.Name} является нулевым объектом при привидении типов {i.PropertyType} => {typeof(Flag)}");
                });
            }

            StateAnimWindow = StateAnimateWindow.Hide;
            if (Screen.PrimaryScreen != null)
                SavePositionAnimateWindow = new(Screen.PrimaryScreen.Bounds.Width / 2 - Size.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - Size.Height / 2);
            else SavePositionAnimateWindow = new(0, 0);
            ControlKey = [];
            LabelHitCommand = [];

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



            UpdateTheme(MainData.MainThemeData.ActivityTheme);
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

            // Кнопка "RE"
            bRebootApplication.MouseClick += (sender, e) =>
            {
                lInformationCursor.Hide();
                ConsoleCommand.ReadConsoleCommand(MainData.MainCommandData.MassConsoleCommand, "reboot");
            };
            bRebootApplication.MouseHover += (sender, e) => ActivateLabelInfo("Перезагружает программу");
            bRebootApplication.MouseLeave += (sender, e) => DisactivateLabelInfo();


            pDeveloper.MouseWheel += (sender, e) => { MiniFunctions.UpdateVScrollBar(Apps.MainForm.sbhDebeloper, e.Delta); };
            sbhDebeloper.ValueChanged += (sender, e) => { MiniFunctions.MoveElementinVScrollBar(pDeveloperElements, sbhDebeloper, 3); };

            //pmpBufferCommandButtons.MouseWheel += (sender, e) => { MiniFunctions.UpdateVScrollBar(App.MainForm.PAC_sbBuffer, e.Delta); };
            //PAC_sbBuffer.ValueChanged += (sender, e) => { MiniFunctions.MoveElementinVScrollBar(pmpBufferCommandButtons, PAC_sbBuffer, 23); };
            CgangeAllButtonAltMode();
            pMiniPanelOutput.PreviewKeyDown += (sender, e) => AltMiniPanelDetect(e.KeyCode);

            bMinimizedApplication.Click += (sender, e) => FoldingMoveApplication();
            Activated += (sender, e) =>
            {
                if (Apps.MainForm.StateAnimWindow == StateAnimateWindow.Hide) UnfoldingMoveApplication();
                else UnfoldingOpacityApplication();
            };

            // Кнопка в PAC "Редактор цветовых палитр"
            PAC_bColoredTheme.ActivateButton += (KeyActivity) =>
            {
                PAC_Disactivate();
                ConsoleCommand.ReadConsoleCommand(MainData.MainCommandData.MassConsoleCommand, "colored");
            };

            // Кнопка в PAC "Изменить цвет текста"
            PAC_bSetColor.ActivateButton += (KeyActivity) =>
            {
                PAC_Disactivate();
                //TypeCommand.ReadDefaultConsoleCommand("color").ExecuteCommand(false);
            };

            // Кнопка в PAC "Очистить текст из консоли"
            PAC_bClearConsole.ActivateButton += (KeyActivity) =>
            {
                PAC_Disactivate();
                ConsoleCommand.ReadConsoleCommand(MainData.MainCommandData.MassConsoleCommand, "clear");
            };

            // Кнопка в PAC "Журнал сообщений"
            PAC_bLogMessage.ActivateButton += (KeyActivity) =>
            {
                PAC_Disactivate();
                ConsoleCommand.ReadConsoleCommand(MainData.MainCommandData.MassConsoleCommand, "log");
            };

            //
            PAC_bClearBuffer.ActivateButton += (KeyActivity) =>
            {
                ClearBufferCommand();
            };

            // Кнопка в PAC "Командный буфер..."
            PAC_bCommandBuffer.ActivateButton += (KeyActivity) =>
            {
                LmpExecuteCommands_Click();
            };

            PAC_bBackBuffer.ActivateButton += (KeyActivity) =>
            {
                MiniFunctions.OpenNewMiniMenu(pmpMain, ActivatePACPanelPage, 0, DirectionsParties.Left);
                ActivatePACPanelPage = pmpMain;
            };

            // Кнопка в PAC "Проводник"
            PAC_bConductor.ActivateButton += (KeyActivity) =>
            {
                MiniFunctions.OpenNewMiniMenu(pmpExplorer, ActivatePACPanelPage, 0);
                ActivatePACPanelPage = pmpExplorer;
            };

            // Кнопка в PAC <Conductor> "Назад"
            PAC_bBackConductor.ActivateButton += (KeyActivity) =>
            {
                MiniFunctions.OpenNewMiniMenu(pmpMain, ActivatePACPanelPage, 0, DirectionsParties.Left);
                ActivatePACPanelPage = pmpMain;
            };

            // Кнопка в PAC "Активный проводник"
            PAC_bActiveExplorer.ActivateButton += (KeyActivity) =>
            {
                PAC_Disactivate();
                Process.Start("explorer.exe", Directory.GetCurrentDirectory());
            };

            // Кнопка в PAC "Главный проводник"
            PAC_bMainExplorer.ActivateButton += (KeyActivity) =>
            {
                PAC_Disactivate();
                Process.Start("explorer.exe");
            };

            // Кнопка в PAC "Активировать ярлык"
            PAC_bActivateLabel.ActivateButton += (KeyActivity) =>
            {
                if (PAC_IndexIELLabelAccess.HasValue)
                {
                    LabelAccess[PAC_IndexIELLabelAccess.Value].IELLabelAccess_ActivateLabel();
                    PAC_Disactivate();
                }
            };

            // Кнопка в PAC "Активировать ярлык"
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

            // Функция события для PSettings
            void EventPSettings()
            {
                if (!FormFlags.ActiveSettingsMiniPanel.Value) PanelSettingsActivate();
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
            Opacity = 0d;

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

            // Самый нижний элемент панели Developer для настройки VScrollBar
            MiniFunctions.CalculationDownElementScrollBar(clbDeveloperFlags, pDeveloperElements, pDeveloper, sbhDebeloper, 10, false);

            lActiveitedSoftCommand.Text = "Soft-Команды отключены";
            //lActiveitedSoftCommand.Text = $"+{MainData.MainCommandData.SoftCommandData.NamesCommand.Length} Soft-{MiniFunctions.LogisticText(MainData.MainCommandData.SoftCommandData.NamesCommand.Length, "Команда")}";

            if ((bool)MainData.Settings.Developer_Mode) ObjLog.LOGTextAppend("Объявлено что программу запустил разработчик");
            else pDeveloper.Hide();

            CapsLock_Info.Image = Control.IsKeyLocked(Keys.CapsLock) ?
                        Image.FromFile(@"Data\Image\Up-A.gif") : Image.FromFile(@"Data\Image\Down-a.gif");
        }

        /// <summary>
        /// Сгенерировать и отсортировать объект ярлыка
        /// </summary>
        public void GenerateLabel()
        {
            Apps.DialogCreateLabel?.Dispose();
            Apps.DialogCreateLabel = new();
            IELLabelAccess? GenerateLabelAccess = Apps.DialogCreateLabel.GenerateLabelAccess(this, pAllVisualLabel);
            if (GenerateLabelAccess != null)
            {
                Color MP = MainData.MainThemeData.ActivityTheme.Palette[7];
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
        /// Создать событие изменение флага в данной форме
        /// <code>Element.SetItemChecked(Index, Value);</code>
        /// </summary>
        /// <param name="flag">Флаг формы</param>
        /// <param name="Element">Элемент содержащий визуализирующий флаг</param>
        /// <param name="Index">Индекс объекта визуализации флага</param>
        private static void SetEventFlag(Flag flag, CheckedListBox Element, int Index)
        {
            flag.ChangeStateFlag += (Value) => Element.SetItemChecked(Index, Value);
        }

        public void LComplete_Click(object sender, EventArgs e)
        {
            ConstAnimColor constAnim = new(Color.White, MainData.MainThemeData.ActivityTheme.Palette[3], 6);
            constAnim.AnimInit(lComplete, AnimStyleColor.ForeColor);
        }

        public void LActiveitedSoftCommand_Click(object sender, EventArgs e)
        {
            ConstAnimColor constAnim = new(Color.White, MainData.MainThemeData.ActivityTheme.Palette[3], 2);
            constAnim.AnimInit(lActiveitedSoftCommand, AnimStyleColor.ForeColor);
        }

        private void Application_Deactivate(object sender, EventArgs e)
        {
            ObjLog.LOGTextAppend("Главная форма не активна");
            FormFlags.FormActivity.Value = false;
            FoldingOpacityApplication();
        }

        /// <summary>
        /// Создать анимацию развёртывания или свёртывания панели ввода команды
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
            ObjLog.LOGTextAppend("Был нажат микрофон");
            if (MainData.Flags.AudioCommand == StatusFlags.Active)
            {
                ObjLog.LOGTextAppend("Было обработано исключение на неполноценное выключение голосовых команд");
                MainData.InputVoiceDevice.LimitationSpeech = true;
                MainData.Flags.AudioCommand = StatusFlags.Sleep;
            }
            else if (MainData.Flags.AudioCommand == StatusFlags.Sleep)
            {
                ObjLog.LOGTextAppend("Было обработано исключение на ПОЛНОЕ отключение голосовых команд");
                MainData.InputVoiceDevice.Diactivate();
                MainData.Flags.AudioCommand = StatusFlags.NotActive;
            }
            else if (MainData.Flags.AudioCommand == StatusFlags.NotActive)
            {
                ObjLog.LOGTextAppend("Было обработано исключение на выход из полного отключения голосовых команд");
                MainData.InputVoiceDevice.Activate();
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
            if (ControlKey.Count == 0) FormFlags.KeyActivity.Value = false;
            lDeveloper_PressEndKey.Text = $"PressEndKey: <{(ControlKey.Count > 0 ? ControlKey[^1].ToString().Trim() : string.Empty)}>";
            lDeveloper_ControlKey.Text = $"ControlKeys: <{string.Join(", ", ControlKey.AsEnumerable())}>";
            switch (e.KeyCode)
            {
                // Очистка консольной строки
                case Keys.Escape:
                    if (FormFlags.ActiveIndexSwitchingHitPanel.Value && IndexActiveLabelHitCommand.HasValue)
                    {
                        FormFlags.ActiveIndexSwitchingHitPanel.Value = false;
                        Color MP = MainData.MainThemeData.ActivityTheme.Palette[8];
                        LabelHitCommand[IndexActiveLabelHitCommand.Value].BackColor = Color.FromArgb(
                        MP.R + (MP.R <= 150 ? 55 : -55),
                        MP.G + (MP.G <= 150 ? 55 : -55),
                        MP.B + (MP.B <= 150 ? 55 : -55));
                        IndexActiveLabelHitCommand = null;
                        return;
                    }
                    AnimationDL.StopAnimate(AnimationDL.StyleAnimateObj.AnimText, tbInput.Name);
                    if (tbInput.Size.Width > 322) TbInputChangeLineText();
                    tbInput.Text = string.Empty;
                    DiactivateConsoleBuffer();
                    break;

                // Переключения буфера команд в консольной строке
                case Keys.Up:
                case Keys.Down:
                    if ((ControlKey.Contains(Keys.ControlKey) && !FormFlags.ActiveIndexSwitchingHitPanel.Value) || FormFlags.ActiveIndexSwitchingHitPanel.Value)
                    {
                        if (!FormFlags.ActiveIndexSwitchingHitPanel.Value) FormFlags.ActiveIndexSwitchingHitPanel.Value = true;
                        if (IndexActiveLabelHitCommand.HasValue)
                        {
                            LabelHitCommand[IndexActiveLabelHitCommand.Value].DiactivateColor();
                        }
                        if (e.KeyCode == Keys.Up)
                        {
                            if (!IndexActiveLabelHitCommand.HasValue) IndexActiveLabelHitCommand = LabelHitCommand.Count - 1;
                            else IndexActiveLabelHitCommand = IndexActiveLabelHitCommand == 0 ? LabelHitCommand.Count - 1 : IndexActiveLabelHitCommand - 1;
                        }
                        else if (e.KeyCode == Keys.Down)
                        {
                            if (!IndexActiveLabelHitCommand.HasValue) IndexActiveLabelHitCommand = 0;
                            else IndexActiveLabelHitCommand = IndexActiveLabelHitCommand == LabelHitCommand.Count - 1 ? 0 : IndexActiveLabelHitCommand + 1;
                        }
                        if (IndexActiveLabelHitCommand.HasValue)
                        {
                            LabelHitCommand[IndexActiveLabelHitCommand.Value].ActivateColor();
                        }
                    }
                    else
                    {
                        if (PAC_Buffer.ElementsBuffer.Count > 0)
                        {
                            if (!FormFlags.BufferConsole.Value) ActivateConsoleBuffer();
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
                            ObjLog.LOGTextAppend($"Индекс читаемой команды в консоли: {IndexConsoleReadBuffer}");
                            string NameActiveCommand = PAC_Buffer.ElementsBuffer[IndexConsoleReadBuffer];
                            lCountActiveBufferCommand.Text = $"{IndexConsoleReadBuffer + 1}\n{PAC_Buffer.ElementsBuffer.Count}";
                            tbInput.Text = NameActiveCommand;
                            tbInput.SelectionStart = tbInput.TextLength;

                            if (NameActiveCommand.Length >= 42 || (NameActiveCommand.Length < 42 && tbInput.Size.Width > 322)) TbInputChangeLineText();
                        }
                    }
                    break;

                // Активация команды
                case Keys.Enter:
                    if (FormFlags.ActiveIndexSwitchingHitPanel.Value && IndexActiveLabelHitCommand.HasValue)
                    {
                        FormFlags.ActiveIndexSwitchingHitPanel.Value = false;
                        ObjLog.LOGTextAppend(IndexActiveLabelHitCommand.Value.ToString() + ", " + LabelHitCommand.Count.ToString());
                        string Text = LabelHitCommand[IndexActiveLabelHitCommand.Value].Text;
                        tbInput.Text = Text;
                        IndexActiveLabelHitCommand = null;
                        return;
                    }
                    DiactivateConsoleBuffer();
                    ActivateConsoleCommand(null, null);
                    break;

                // Визуализация большого/маленького регистра печати
                case Keys.Capital:
                case Keys.ShiftKey:
                    CapsLock_Info.Image = Image.FromFile(@$"Data\Image\{(IsKeyLocked(Keys.CapsLock) ? "Up-A" : "Down-a")}.gif");
                    break;

                // Активация последней команды в "подсказках к командам" в консольной строке
                case Keys.D0:
                    if (ControlKey.Contains(Keys.ControlKey) && FormFlags.ActiveHitPanelConsole.Value)
                    {
                        tbInput.Text = LabelHitCommand[^1].Tag?.ToString() ?? string.Empty;
                        HitPanelDiactivate();
                        tbInput.SelectionStart = tbInput.TextLength;
                    }
                    break;

                // Активация определённой команды в "подсказках к командам" в консольной строке
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    if (ControlKey.Contains(Keys.ControlKey) && FormFlags.ActiveHitPanelConsole.Value)
                    {
                        int SaveNumber = e.KeyCode.ToString()[1] - 48;
                        if (LabelHitCommand.Count >= SaveNumber)
                        {
                            tbInput.Text = LabelHitCommand[SaveNumber - 1].Tag?.ToString() ?? string.Empty;
                            HitPanelDiactivate();
                            tbInput.SelectionStart = tbInput.TextLength;
                        }
                    }
                    break;
                default:
                    if (tbInput.TextLength > 0) IndexingHitCommands(Regex.Escape(tbInput.Text.Replace(' ', '_')));
                    else HitPanelDiactivate();
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
            if (FormFlags.KeyActivity.Value) return;
            ConstAnimMove constAnim = new(-14, 2, 10);

            FormFlags.KeyActivity.Value = true;
            MainData.MainMP3.PlaySound("ClickDown");
            if (e.KeyCode == Keys.Capital || e.KeyCode == Keys.ShiftKey)
                CapsLock_Info.Image = !IsKeyLocked(Keys.CapsLock) ? Image.FromFile(@"Data\Image\Up-A.gif") : Image.FromFile(@"Data\Image\Down-a.gif");
            else if (((e.KeyCode == Keys.Right && tbInput.SelectionStart == tbInput.TextLength) || e.KeyCode == Keys.Return) && FormFlags.BufferConsole.Value)
            {
                constAnim.Reverse().InitAnimFormule(lCountActiveBufferCommand, Formules.QuickTransition, null, AnimationStyle.XY);
                FormFlags.BufferConsole.Value = false;
            }
            else if (e.KeyCode == Keys.Back && FormFlags.BufferConsole.Value)
            {
                constAnim.Reverse().InitAnimFormule(lCountActiveBufferCommand, Formules.QuickTransition, null, AnimationStyle.XY);
                FormFlags.BufferConsole.Value = false;
            }
            else if (e.KeyCode == Keys.Return && !FormFlags.ActiveIndexSwitchingHitPanel.Value)
            {
                tbInput.Text = tbInput.Text.Replace("\n", string.Empty);
                tbInput.SelectionStart = tbInput.TextLength;
            }
            else if (e.KeyCode == Keys.Apps)
            {
                FormFlags.KeyActivity.Value = false;
                PAC_Activate();
            }

        }

        public void DeveloperPanelClick(object sender, EventArgs e)
        {
            if (FormFlags.FormActivity.Value)
            {
                ObjLog.LOGTextAppend("Была нажата боковая панель разработчика");
                if ((bool)MainData.Settings.Developer_Mode)
                {
                    if (FormFlags.PAC_PanelActivate.Value) PAC_Disactivate();
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
            ObjLog.LOGTextAppend("Была активирована ссылка на разработчика программы");
            Process.Start(new ProcessStartInfo("https://vk.com/l1s8vr9al") { UseShellExecute = true });
            FoldingMoveApplication();
        }

        private void PbCustom_DoubleClick(object sender, EventArgs e)
        {
            ObjLog.LOGTextAppend($"Вызвано диалоговое окно по установке кастомного GIF");
            Apps.DialogCustomImage_Form = new DialogCustomImage();
            Apps.DialogCustomImage_Form.ShowDialog();
        }

        private void TbInput_TextChanged(object sender, EventArgs e)
        {
            if ((tbInput.TextLength >= 42 && tbInput.Size.Width == 322) || (tbInput.TextLength <= 41 && tbInput.Size.Width > 322)) TbInputChangeLineText();
            if (tbInput.TextLength == tbInput.MaxLength || lMaxLangthCommand.Location.Y > 515)
            {
                ConstAnimMove ConstFormule = new(lMaxLangthCommand.Location.Y, tbInput.TextLength == tbInput.MaxLength ? 524 : 515, 10);
                new ConstAnimMove(lMaxLangthCommand.Location.X).InitAnimFormule(lMaxLangthCommand, Formules.QuickTransition, ConstFormule, AnimationStyle.XY);
            }
            if (FormFlags.BufferConsole.Value || !(bool)MainData.Settings.Hit_Panel) return;
            if (tbInput.TextLength == 0)
            {
                HitPanelDiactivate();
                return;
            }
        }

        //
        private void IndexingHitCommands(string Text)
        {
            string[] HitCommandText = [.. MainData.MainCommandData.MassConsoleCommand.Select((i) => { return i.WritingCommandName(); })];
            HitCommandText = [.. HitCommandText.Where((i) => { return i.Contains(Text, StringComparison.CurrentCultureIgnoreCase); })];
            int MaxWigth = 0;
            ObjLog.LOGTextAppend($"{LabelHitCommand.Count} - {HitCommandText.Length}");
            if (LabelHitCommand.Count < HitCommandText.Length)
            {
                for (int i = 0; i < HitCommandText.Length; i++)
                {
                    if (i < LabelHitCommand.Count)
                    {
                        LabelHitCommand[i].ElementText = HitCommandText[i];
                        if (LabelHitCommand[i].Width > MaxWigth) MaxWigth = LabelHitCommand[i].Width;
                        LabelHitCommand[i].Visible = true;
                    }
                    else
                    {
                        IELHitCommand Hit = new(pHitCommandConsole, MainData.MainThemeData.ActivityTheme.Palette[8].ToKnownColor(), LabelHitCommand.Count + 1, HitCommandText[i])
                        {
                            Location = new(2, LabelHitCommand.Count == 0 ? 2 : 2 + (LabelHitCommand.Count * 18) + 2 * LabelHitCommand.Count),
                            Visible = true
                        };
                        Hit.ClickActivateElement += () =>
                        {
                            tbInput.Text = Hit.ElementText;
                            tbInput.SelectionStart = tbInput.TextLength;
                            HitPanelDiactivate();
                        };
                        LabelHitCommand.Add(Hit);
                        if (LabelHitCommand[^1].Width > MaxWigth) MaxWigth = LabelHitCommand[^1].Width;
                    }
                }
            }
            else if (LabelHitCommand.Count > HitCommandText.Length)
            {
                for (int i = 0; i < LabelHitCommand.Count; i++)
                {
                    if (i < HitCommandText.Length)
                    {
                        LabelHitCommand[i].ElementText = HitCommandText[i];
                        if (LabelHitCommand[i].Width > MaxWigth) MaxWigth = LabelHitCommand[i].Width;
                        LabelHitCommand[i].Visible = true;
                    }
                    else LabelHitCommand[i].Visible = false;
                }
            }
            else
            {
                for (int i = 0; i < LabelHitCommand.Count; i++)
                {
                    LabelHitCommand[i].ElementText = HitCommandText[i];
                    if (LabelHitCommand[i].Width > MaxWigth) MaxWigth = LabelHitCommand[i].Width;
                    LabelHitCommand[i].Visible = true;
                }
            }
            if (HitCommandText.Length > 0)
            {
                FormFlags.ActiveHitPanelConsole.Value = true;
                IELHitCommand hit = LabelHitCommand[HitCommandText.Length - 1];
                pHitCommandConsole.Width = MaxWigth + 6;
                pHitCommandConsole.Height = hit.Location.Y + hit.Height + 3;
                pHitCommandConsole.Location = new(tbInput.Location.X + tbInput.Width - MaxWigth - 9, tbInput.Location.Y - pHitCommandConsole.Height);
            }
            else HitPanelDiactivate();
            ObjLog.LOGTextAppend($"{LabelHitCommand.Count} - {HitCommandText.Length} !");
        }

        /// <summary>
        /// Диактивировать панель подсказок к командам
        /// </summary>
        private void HitPanelDiactivate()
        {
            if (!FormFlags.ActiveHitPanelConsole.Value) return;
            FormFlags.ActiveHitPanelConsole.Value = false;
            pHitCommandConsole.Width = 0;
            pHitCommandConsole.Height = 0;
            pHitCommandConsole.Location = new(tbInput.Location.X + tbInput.Width, tbInput.Location.Y);
        }

        /// <summary>
        /// Открыть мини-панель настроек
        /// </summary>
        private void PanelSettingsActivate()
        {
            FormFlags.ActiveSettingsMiniPanel.Value = true;
            lSettingsVolume.Text = "Загрузка...";
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
        /// Закрыть мини-панель настроек
        /// </summary>
        private void PanelSettingsDiactivate()
        {
            FormFlags.ActiveSettingsMiniPanel.Value = false;
            tbOutput.Focus();
            pbSttingsName.Cursor = Cursors.Hand;
            pSettings.Cursor = Cursors.Hand;

            ConstAnimMove ConstantFormulePanel = new(pSettings.Location.X, 0 - pSettings.Width + 13, 12);
            ConstAnimMove ConstantFormuleText = new(pbSttingsName.Location.Y, pSettings.Height / 2 - pbSttingsName.Height / 2, 12);
            ConstantFormulePanel.InitAnimFormule(pSettings, Formules.QuickTransition, new ConstAnimMove(pSettings.Location.Y), AnimationStyle.XY);
            new ConstAnimMove(pbSttingsName.Location.X).InitAnimFormule(pbSttingsName, Formules.QuickTransition, ConstantFormuleText, AnimationStyle.XY);
        }

        /// <summary>
        /// Активировать описание элемента
        /// </summary>
        /// <param name="Text">Текст описания</param>
        private void ActivateLabelInfo(string Text)
        {
            FormFlags.Information.Value = true;
            lInformationCursor.Text = Text;
            lInformationCursor.Show();
        }

        /// <summary>
        /// Диактивировать описание элемента
        /// </summary>
        private void DisactivateLabelInfo()
        {
            FormFlags.Information.Value = false;
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
            new ConstAnimMove(Apps.MainForm.pICON.Location.X, 15, 8).InitAnimFormule(Apps.MainForm.pICON, Formules.QuickTransition, new(Apps.MainForm.pICON.Location.Y), AnimationStyle.XY);
            Location = new(Location.X, Location.Y + 150);
            WindowState = FormWindowState.Normal;
            Show();
            if (!Focused) Activate();
        }

        /// <summary>
        /// Обновить изображение кнопки голосовых команд
        /// </summary>
        /// <param name="flag">Статус параметра голосовых команд</param>
        /// <param name="MouseEnter">Касается ли курсор объекта</param>
        public void VoiceButtonImageUpdate(StatusFlags flag, bool MouseEnter)
        {
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
            pbVoiceButton.Refresh();
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
            if (FormFlags.FormActivity.Value)
            {
                ConstAnimMove ConstantFormule = new(lWhat.Size.Width, 120, 15);
                ConstantFormule.InitAnimFormule(lWhat, Formules.QuickTransition, new ConstAnimMove(lWhat.Size.Height), AnimationStyle.Size);
            }
        }

        private void BWhatInformation_MouseLeave(object sender, EventArgs e)
        {
            if (FormFlags.FormActivity.Value)
            {
                ConstAnimMove ConstantFormule = new(lWhat.Size.Width, 0, 15);
                ConstantFormule.InitAnimFormule(lWhat, Formules.QuickTransition, new ConstAnimMove(lWhat.Size.Height), AnimationStyle.Size);
            }
        }

        private void BWhatInformation_MouseClick(object sender, MouseEventArgs e)
        {
            BWhatInformation_MouseLeave(null, null);
            ConsoleCommand.ReadConsoleCommand(MainData.MainCommandData.MassConsoleCommand, "help");
        }

        private void UpperPanelActivate(object sender, EventArgs e)
        {
            if (FormFlags.FormActivity.Value)
            {
                ConstAnimMove ConstantFormule = new(pUppingMenu.Location.Y, -1, 9);
                new ConstAnimMove(pUppingMenu.Location.X).InitAnimFormule(pUppingMenu, Formules.QuickTransition, ConstantFormule, AnimationStyle.XY);
                if (FormFlags.PAC_PanelActivate.Value) PAC_Disactivate(false);
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
            Apps.Thanks = null;
            Apps.MainForm.FoldingOpacityApplication();
            FormFlags.FormActivity.Value = false;
            Apps.Thanks = new();
            Thread.Sleep(100);
            Apps.Thanks.Show();
        }

        public void BCloseMainApplication(object sender, EventArgs e)
        {
            Apps.Log?.Close();
            Apps.InformationCommand?.Close();
            Apps.WindowSettings?.Close();
            Apps.ThemesCreated?.Close();
            Environment.Exit(0);
        }

        /// <summary>
        /// Воспроизвести анимацию развёртывания или активации окна главной формы
        /// </summary>
        public void UnfoldingOpacityApplication()
        {
            if (ThreadWindow?.IsAlive ?? false) ThreadWindow.Interrupt();
            StateAnimWindow = StateAnimateWindow.OpacityHidePlus;
            ThreadWindow = new(async () =>
            {
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
                if (StateAnimWindow == StateAnimateWindow.OpacityHidePlus)
                {
                    FormFlags.FormActivity.Value = true;
                    WindowState = FormWindowState.Normal;
                    Opacity = 1d;
                    StateAnimWindow = StateAnimateWindow.Active;
                }
            });
            ThreadWindow.Start();
        }

        /// <summary>
        /// Воспроизвести анимацию развёртывания или активации окна главной формы
        /// </summary>
        public void UnfoldingMoveApplication()
        {
            if (ThreadWindow?.IsAlive ?? false) ThreadWindow.Interrupt();
            StateAnimWindow = StateAnimateWindow.MoveHidePlus;
            ThreadWindow = new(async () =>
            {
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
                if (StateAnimWindow == StateAnimateWindow.MoveHidePlus)
                {
                    FormFlags.FormActivity.Value = true;
                    WindowState = FormWindowState.Normal;
                    Opacity = 1d;
                    StateAnimWindow = StateAnimateWindow.Active;
                }
            });
            ThreadWindow.Start();
        }

        /// <summary>
        /// Воспроизвести анимацию сворачивания окна главной формы
        /// </summary>
        public void FoldingMoveApplication()
        {
            if (ThreadWindow?.IsAlive ?? false) ThreadWindow.Interrupt();
            StateAnimWindow = StateAnimateWindow.MoveHideMinus;
            ThreadWindow = new(async () =>
            {
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
                    FormFlags.FormActivity.Value = false;
                    WindowState = FormWindowState.Minimized;
                    StateAnimWindow = StateAnimateWindow.Hide;
                }
            });
            ThreadWindow.Start();
        }

        /// <summary>
        /// Воспроизвести анимацию дизактивации окна главной формы
        /// </summary>
        public void FoldingOpacityApplication()
        {
            if (ThreadWindow?.IsAlive ?? false) ThreadWindow.Interrupt();
            StateAnimWindow = StateAnimateWindow.OpacityHideMinus;
            ThreadWindow = new(async () =>
            {
                int i = 0;
                while (Opacity > 0.45d && StateAnimWindow == StateAnimateWindow.OpacityHideMinus)
                {
                    await Task.Run(() =>
                    {
                        Opacity -= 0.009d;
                        if (i++ % 6 == 0) Thread.Sleep(1);
                        Update();
                    });
                }
                if (StateAnimWindow == StateAnimateWindow.OpacityHideMinus)
                {
                    Opacity = 0.45d;
                    FormFlags.FormActivity.Value = false;
                    StateAnimWindow = StateAnimateWindow.HalfHide;
                }
            });
            ThreadWindow.Start();
        }

        /// <summary>
        /// Активировать изменение позиции окна главной формы
        /// </summary>
        /// <param name="sender">Объект события</param>
        /// <param name="e">Событие</param>
        private async void MovingApplicationActivate(object sender, MouseEventArgs e)
        {
            Point StartCursor = Cursor.Position, StartLocation = Location;
            int MPX, MPY;
            FormFlags.MovingApplication.Value = true;
            while (FormFlags.MovingApplication.Value)
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
        /// Диактивировать изменение позиции окна главной формы
        /// </summary>
        /// <param name="sender">Объект события</param>
        /// <param name="e">Событие</param>
        private void MovingApplicationDiactivate(object sender, MouseEventArgs e)
        {
            FormFlags.MovingApplication.Value = false;
            SavePositionAnimateWindow = Location;
        }

        public void BSettings_Click(object sender, EventArgs e)
        {
            ObjLog.LOGTextAppend("Была активирована форма настроек");
            Apps.WindowSettings = new();
            Apps.WindowSettings.ShowDialog();
        }

        public void TbOutput_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && (Math.Abs(StartCursorMovingtbOutput.X - Cursor.Position.X) < 15 || FormFlags.PAC_PanelActivate.Value))
            {
                PAC_Activate();
            }
            else if (e.Button == MouseButtons.Left && FormFlags.PAC_PanelActivate.Value)
            {
                PAC_Disactivate();
            }
        }

        /// <summary>
        /// Активировать мини-панель быстрых действий
        /// </summary>
        /// <param name="LabelAccessDetect">PAC активируется с помощью LabelAccess</param>
        private void PAC_Activate(bool LabelAccessDetect = false)
        {
            const int OffsetX = 5, OffsetY = -30, WidthA = 212, HeightA = 221;
            int XPos = Cursor.Position.X - Apps.MainForm.Location.X + OffsetX, YPos = Cursor.Position.Y - Apps.MainForm.Location.Y + OffsetY;
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

            if (!FormFlags.PAC_PanelActivate.Value)
            {
                pMiniPanelOutput.Location = new(ConstX, ConstY);
                pMiniPanelOutput.Size = new(0, 0);
                pMiniPanelOutput.Focus();
                FormFlags.PAC_PanelActivate.Value = true;
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
        /// Диактивировать мини панель консоли
        /// </summary>
        /// <param name="FocusMode">Перенаправлять ли фокус на другой элемент</param>
        private void PAC_Disactivate(bool FocusMode = true)
        {
            FormFlags.PAC_PanelActivate.Value = false;
            ActivatePACPanelPage = pmpMain;
            if (FocusMode) tbInput.Focus();
            ConstAnimMove ConstantFormuleW = new(pMiniPanelOutput.Size.Width, 0, 10);
            ConstAnimMove ConstantFormuleH = new(pMiniPanelOutput.Size.Height, 0, 10);
            ConstantFormuleW.InitAnimFormule(pMiniPanelOutput, Formules.QuickTransition, ConstantFormuleH, AnimationStyle.Size);

            if (FormFlags.PAC_PanelAltActivate.Value && (bool)MainData.Settings.Alt_Diactivate_PAC)
            {
                FormFlags.PAC_PanelAltActivate.Value = false;
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
                    lSettingsVolume.Text = $"Громкость: {tbSettingsVolume.Value}";
                }
            }
            catch { }
        }

        public void LmpExecuteCommands_Click()
        {
            PAC_bClearBuffer.Location = new(82, PAC_Buffer.ElementsBuffer.Count > 0 ? 3 : -29);
            //PAC_sbBuffer.Visible = MainData.Settings.BufferCommand.Count > 0;
            //PAC_sbBuffer.Location = new(189, PAC_sbBuffer.Location.Y);
            Apps.MainForm.PAC_lBufferCount.Text = $"{PAC_Buffer.ElementsBuffer.Count}/{PAC_Buffer.ElementsBuffer.Length}";
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
            Apps.MainForm.PAC_lBufferCount.Text = $"0/{PAC_Buffer.ElementsBuffer.Length}";
            new ConstAnimMove(PAC_bClearBuffer.Location.X)
                .InitAnimFormule(PAC_bClearBuffer, Formules.QuickTransition, new(PAC_bClearBuffer.Location.Y, -26, 7), AnimationStyle.XY);
        }

        private void TbInputDisactivate(object sender, EventArgs e)
        {
            if (FormFlags.BufferConsole.Value)
            {
                FormFlags.BufferConsole.Value = false;
                ConstAnimMove ConstFormule = new(lCountActiveBufferCommand.Location.X, -14, 7);
                ConstFormule.InitAnimFormule(lCountActiveBufferCommand, Formules.QuickTransition, new ConstAnimMove(lCountActiveBufferCommand.Location.Y), AnimationStyle.XY);
            }
        }

        private void TbNameColorParamContains_TextChanged(object sender, EventArgs e)
        {
            lDeveloper_ContainsTextInColorParam.Text = $"CTICP: {ThemeInfoParameters.Select(i => i.Name).Contains(tbNameColorParamContains.Text)}";
        }

        private async void TbOutput_MouseDown(object sender, MouseEventArgs e)
        {
            StartCursorMovingtbOutput = Cursor.Position;
            if (!FormFlags.PAC_PanelActivate.Value && pDeveloper.Location.X >= 760)
            {
                Point StartTbOutPutPoint = tbOutput.Location;
                Size StartTbOutPutSize = tbOutput.Size;
                if (StartTbOutPutSize.Width == 771) pLabelExplorer.Location = new(604, pLabelExplorer.Location.Y);
                int Height = tbOutput.Size.Height, Y = tbOutput.Location.Y;
                MainData.Flags.ActiveMovingMainConsole = BooleanFlags.True;
                int StartActivePanelExplorerX = Apps.MainForm.pLabelExplorer.Location.X;
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
                        if (StartCursorMovingtbOutput.X - Cursor.Position.X <= 0 && FormFlags.ActiveExplorerLabel.Value)
                            Apps.MainForm.pLabelExplorer.Location = new(StartActivePanelExplorerX + (StartCursorMovingtbOutput.X - Cursor.Position.X) / 8, Apps.MainForm.pLabelExplorer.Location.Y);
                    });
                }
                if (StartCursorMovingtbOutput.X - Cursor.Position.X >= 55)
                {
                    ConstFormuleW = new(tbOutput.Size.Width, 685, 7);
                    pAllLabelExplorerEndX = 704;

                    if (!FormFlags.ActiveExplorerLabel.Value)
                    {
                        pAllVisualLabel.Location = new(pAllVisualLabel.Location.X, pLabelExplorer.Height);
                        ConstPanelExplorer = new(pAllVisualLabel.Location.Y, 14 - 56 * ScrollLabels.Value, 17);
                        new ConstAnimMove(pAllVisualLabel.Location.X).InitAnimFormule(pAllVisualLabel, Formules.QuickTransition, ConstPanelExplorer, AnimationStyle.XY);
                    }

                    FormFlags.ActiveExplorerLabel.Value = true;
                }
                else if (StartCursorMovingtbOutput.X - Cursor.Position.X <= -130)
                {
                    ConstFormuleW = new(tbOutput.Size.Width, 771, 7);
                    if (FormFlags.ActiveExplorerLabel.Value)
                    {
                        FormFlags.ActiveExplorerLabel.Value = false;
                    }
                    else
                    {
                        ObjLog.LOGTextAppend($"Была распознана очистка консоли <tbOutput> (Смещением консоли)");
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
            MainData.Flags.ActiveMovingMainConsole = BooleanFlags.False;
        }

        /// <summary>
        /// Обновление отображаемой информации для разработчиков в панели формы
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

        private void ActivateLOG(object sender, EventArgs e) => ConsoleCommand.ReadConsoleCommand(MainData.MainCommandData.MassConsoleCommand, "log");

        /// <summary>
        /// Активация команды через консоль
        /// </summary>
        /// <param name="sender">Объект</param>
        /// <param name="e">Событие</param>
        public void ActivateConsoleCommand(object sender, EventArgs e)
        {
            string text = (tbInput.Text ?? string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty);
            if (tbInput.Size.Width > 322) TbInputChangeLineText();
            PAC_Buffer.AddNewElement(text);
            ConsoleCommand.ReadConsoleCommand(MainData.MainCommandData.MassConsoleCommand, text, tbInput);
        }

        /// <summary>
        /// Создать анимацию активации/диактивации буфера в консоли
        /// </summary>
        /// <param name="Activate">Состояние на которое изменяется буфер в консоли</param>
        void ConsoleBufferAnimate(bool Activate)
        {
            if ((Activate && !FormFlags.BufferConsole.Value) || (!Activate && FormFlags.BufferConsole.Value))
            {
                ConstAnimMove ConstFormule = new(lCountActiveBufferCommand.Location.X, Activate ? 2 : -14, 7);
                ConstFormule.InitAnimFormule(lCountActiveBufferCommand, Formules.QuickTransition, new ConstAnimMove(lCountActiveBufferCommand.Location.Y), AnimationStyle.XY);
                FormFlags.BufferConsole.Value = Activate;
                IndexConsoleReadBuffer = 0;
            }
        }

        /// <summary>
        /// Включить буфер команд в консоли
        /// </summary>
        public void ActivateConsoleBuffer() => ConsoleBufferAnimate(true);

        /// <summary>
        /// Выключить буфер команд в консоли
        /// </summary>
        public void DiactivateConsoleBuffer() => ConsoleBufferAnimate(false);

        /// <summary>
        /// Обновляет информацию в аудио мини-панели настроек
        /// </summary>
        /// <param name="Animation">Воспроизводится ли анимация или нет</param>
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
                lSettingsVolume.Text = $"Громкость: {tbSettingsVolume.Value}";
                if (!MainData.Divaces.ActiveDevice.AudioEndpointVolume.Mute)
                    bSettingsMute.BackgroundImage = Image.FromFile($"{Directory.GetCurrentDirectory()}\\Data\\Image\\Sound.png");
                else bSettingsMute.BackgroundImage = Image.FromFile($"{Directory.GetCurrentDirectory()}\\Data\\Image\\Mute.png");
            });
        }

        private void Change_MicrophoneActivate(object sender, EventArgs e) => SettingsData.SetParamOption(nameof(MainData.Settings.Activation_Microphone), cbSettingsVoice.Checked ? "1" : "0");

        /// <summary>
        /// Обновить Alt состояние всех кнопок PAC
        /// </summary>
        private void CgangeAllButtonAltMode()
        {
            PAC_bSetColor.AltIndexActivity = FormFlags.PAC_PanelAltActivate.Value;
            PAC_bClearConsole.AltIndexActivity = FormFlags.PAC_PanelAltActivate.Value;
            PAC_bColoredTheme.AltIndexActivity = FormFlags.PAC_PanelAltActivate.Value;
            PAC_bLogMessage.AltIndexActivity = FormFlags.PAC_PanelAltActivate.Value;
            PAC_bCommandBuffer.AltIndexActivity = FormFlags.PAC_PanelAltActivate.Value;
            PAC_bConductor.AltIndexActivity = FormFlags.PAC_PanelAltActivate.Value;

            PAC_bBackBuffer.AltIndexActivity = FormFlags.PAC_PanelAltActivate.Value;
            PAC_bClearBuffer.AltIndexActivity = FormFlags.PAC_PanelAltActivate.Value;

            PAC_bBackConductor.AltIndexActivity = FormFlags.PAC_PanelAltActivate.Value;
            PAC_bActiveExplorer.AltIndexActivity = FormFlags.PAC_PanelAltActivate.Value;
            PAC_bMainExplorer.AltIndexActivity = FormFlags.PAC_PanelAltActivate.Value;

            PAC_bActivateLabel.AltIndexActivity = FormFlags.PAC_PanelAltActivate.Value;
            PAC_bDeleteLabel.AltIndexActivity = FormFlags.PAC_PanelAltActivate.Value;
        }

        private void AltMiniPanelDetect(Keys key)
        {
            if (key == Keys.Escape) PAC_Disactivate();
            else if ((bool)MainData.Settings.Alt_OrientationLR_PAC && key == (Keys)MainData.Settings.HC_Alt_Activate_PAC.Value)
            {
                FormFlags.PAC_PanelAltActivate.Value = key != (Keys)MainData.Settings.HC_Alt_Diactivate_PAC.Value || !FormFlags.PAC_PanelAltActivate.Value;
                CgangeAllButtonAltMode();
            }
            else if ((bool)MainData.Settings.Alt_OrientationLR_PAC && key == (Keys)MainData.Settings.HC_Alt_Diactivate_PAC.Value)
            {
                FormFlags.PAC_PanelAltActivate.Value = key == (Keys)MainData.Settings.HC_Alt_Activate_PAC.Value && !FormFlags.PAC_PanelAltActivate.Value;
                CgangeAllButtonAltMode();
            }
            else if (!(bool)MainData.Settings.Alt_OrientationLR_PAC && key == Keys.Menu)
            {
                FormFlags.PAC_PanelAltActivate.Value = !FormFlags.PAC_PanelAltActivate.Value;
                CgangeAllButtonAltMode();
            }
            else if (FormFlags.PAC_PanelAltActivate.Value)
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
                ConstAnimX.InitAnimFormule(Apps.MainForm, Formules.QuickTransition, ConstAnimY, AnimationStyle.XY);
            }
        }

        /// <summary>
        /// Постоянный цикл обновления информации
        /// </summary>
        public async Task AlwaysUpdateWindow()
        {
            ObjLog.LOGTextAppend("Была Вызвана функция автоматического обновления данных");

            int AllTick = -1, PointVolume;
            MainData.Divaces.UpdateActivateDivaceAudioOutput();
            lData.Text = DateTime.Now.ToString("dd/MM/yyyy");
            object LockPositionVizVol = new();
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (Apps.MainForm.WindowState == FormWindowState.Normal)
                    {
                        AllTick = ++AllTick % 200;
                        PointVolume = (int)(MainData.Divaces.ActiveDevice.AudioMeterInformation.MasterPeakValue * 100);

                        // Скорость обновления
                        lDeveloper_AllTick.Text = $"AT: {AllTick}";

                        lTime.Text = DateTime.Now.ToString("HH:mm:ss");
                        if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0) lData.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        lLanguageName.Text = InputLanguage.CurrentInputLanguage.Culture.Name.Equals("en-US") ? "EN" : "RU";

                        if (FormFlags.Information.Value) lInformationCursor.Location =
                            new(Cursor.Position.X - Location.X, Cursor.Position.Y - Location.Y - 57);

                        // Обновление позиции визуализатора музыки
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

                        // Обновление информации в панели разработчика
                        if ((bool)MainData.Settings.Developer_Mode && MainData.Flags.PanelDeveloper == BooleanFlags.True)
                            Apps.MainForm.DeveloperPanelInformation();

                        if (AllTick % 10 == 0)
                        {
                            await Task.Run(() =>
                            {
                                Internet_Info.ImageLocation = $@"Data\Image\Internet{(DLLMethods.InternetCheckConnection("https://ya.ru", 1, 0) ? "Activate" : "Disactivate")}.png";
                                Internet_Info.Refresh();
                            });

                            // Обновление активного аудио-девайса
                            if (FormFlags.ActiveSettingsMiniPanel.Value)
                            {
                                MainData.Divaces.UpdateActivateDivaceAudioOutput();
                                UpdateInformationAudioDevice(true);
                            }
                        }
                    }

                    Thread.Sleep(20);
                }
            });
        }

        /// <summary>
        /// Обновить параметры темы относящиеся к этой форме
        /// </summary>
        /// <param name="theme">Тема для обновления</param>
        public void UpdateTheme(Theme theme)
        {
            for (int i = 0; i < theme.Palette.Length; i++) UpdateThemeIndexElement(i);
        }

        /// <summary>
        /// Обновить конкретный элемент темы учитывая индекс
        /// </summary>
        /// <param name="Index">Индекс обновляемого элемента</param>
        public void UpdateThemeIndexElement(int Index)
        {
            if (Index < 0 || Index >= MainData.MainThemeData.ActivityTheme.Palette.Length)
                throw new ArgumentOutOfRangeException(nameof(Index), $"Индекс {Index} обновляемого элемента находится за границами массива цветов темы.");
            Color MP = MainData.MainThemeData.ActivityTheme.Palette[Index];
            switch (Index)
            {
                // Цвет для вывода в консоль текста
                case 0:
                    tbOutput.BackColor = MP;
                    break;

                // Цвет для мини-панелей
                case 1:
                    // Цвет для мини-панелей
                    pMiniPanelOutput.BackColor = MP;
                    pmpExplorer.BackColor = MP;
                    //pmpBufferCommandButtons.BackColor = MP;
                    pmpMain.BackColor = MP;
                    pmpBuffer.BackColor = MP;
                    break;

                // Цвет для верхней панели
                case 2:
                    pUppingMenu.BackColor = MP;
                    break;

                // Цвет для фона в главной форме
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

                // Цвет для фона в консольной строке
                case 4:
                    tbInput.BackColor = MP;
                    break;

                // Цвет фона вопросительной кнопки
                case 5:
                    bWhatInformation.BackColor = MP;
                    break;

                // Цвет фона мини-панели настроек
                case 6:
                    pSettings.BackColor = MP;
                    break;

                // Цвет фона панели ярлыков
                case 7:
                    pLabelExplorer.BackColor = MP;
                    pAllVisualLabel.BackColor = MP;
                    foreach (IELLabelAccess Element in LabelAccess)
                    {
                        Element.BackColor = Color.FromArgb(MP.R + (MP.R <= 150 ? 55 : -55), MP.R + (MP.R <= 150 ? 55 : -55), MP.R + (MP.R <= 150 ? 55 : -55));
                        Element.Refresh();
                    }
                    break;

                // Цвет фона панели подсказок к командам
                case 8:
                    pHitCommandConsole.BackColor = MP;
                    LabelHitCommand.ForEach((i) => i.DiactivateColorComponent = MP);
                    break;

                // Цвет фона кнопки сворачивания главной формы
                case 9:
                    bMinimizedApplication.BackColor = MP;
                    bMinimizedApplication.ForeColor = ColorWhile.InvColor(MP);
                    break;

                // Цвет фона кнопки закрытия главной формы
                case 10:
                    bCloseApplication.BackColor = MP;
                    bCloseApplication.ForeColor = ColorWhile.InvColor(MP);
                    break;

                // Цвет текста времени
                case 11:
                    lTime.ForeColor = MP;
                    break;

                // Цвет текста даты
                case 12:
                    lData.ForeColor = MP;
                    break;

                // Цвет текста языка
                case 13:
                    lLanguageName.ForeColor = MP;
                    break;

                // Цвет текста консоли
                case 14:
                    tbOutput.ForeColor = MP;
                    break;

                default:
                    throw new Exception($"индекс {Index} не зарегестрирован под обновление {Name}");
            }
        }

        /// <summary>
        /// Прочитать текст файла для объявления элементов ярлыка
        /// </summary>
        /// <param name="TextFile">Текст файла для чтения</param>
        /// <param name="Parent">Панель в которой будет находятся элементы ярлыков</param>
        /// <param name="MassLabel">Ссылка на переменную куда записываются объекты ярлыка</param>
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
            if (FormFlags.PAC_PanelActivate.Value) PAC_Disactivate(false);
        }

        [GeneratedRegex(@"\b[^\?]+")]
        private static partial Regex _RegexBlockDetect();

        [GeneratedRegex(@"\b[^;]+")]
        private static partial Regex _RegexBlockDetalDetect();
    }

    /// <summary>
    /// Перечисление состояний окна главной формы
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