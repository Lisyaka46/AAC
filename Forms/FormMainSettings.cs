using AAC.GUI;
using System;
using static AAC.Classes.AnimationDL.Animate.AnimColor;
using static AAC.Classes.AnimationDL.Animate.AnimFormule;
using static AAC.Startcs;

namespace AAC.Forms
{
    /// <summary>
    /// Все страницы формы Settings
    /// </summary>
    public enum SettingsPage
    {
        /// <summary>
        /// Форма отключена, Нет доступа к странице
        /// </summary>
        Null = 0,

        /// <summary>
        /// Главная страница (Главное меню)
        /// </summary>
        MainMenu = 1,

        /// <summary>
        /// Страница управление цветами (Палитра)
        /// </summary>
        Colored = 2,

        /// <summary>
        /// Страница PAC (Мини-панель)
        /// </summary>
        PAC = 3,

        /// <summary>
        /// Страница прочих параметров (Прочее..)
        /// </summary>
        Other = 4,
    }

    public partial class FormMainSettings : Form
    {
        /// <summary>
        /// Обновлять позицию изображения автора
        /// </summary>
        private bool UpdatePositionImageAutor { get; set; }

        /// <summary>
        /// Сохранённая позиция курсора при нажатии на иконку автора программы
        /// </summary>
        private Point SavePositionCursorStart { get; set; }

        /// <summary>
        /// Сохранённое смещение курсора от иконки
        /// </summary>
        private Point SavePositionOffsetCursorStart { get; set; }

        /// <summary>
        /// Активная панель настроек
        /// </summary>
        private Panel ActivePanel { get; set; }

        /// <summary>
        /// Все страницы настроек
        /// </summary>
        private readonly List<Panel> Page;

        /// <summary>
        /// Все Tool кнопки привязанные к страницам
        /// </summary>
        private readonly List<Button> ToolButtons;

        /// <summary>
        /// Статус активной страницы в форме Settings
        /// </summary>
        public SettingsPage ActiveSettingsPage { get; set; } = SettingsPage.MainMenu;

        /// <summary>
        /// Клавиши которые нельзя использовать для фиксации переключения Alt режима в PAC
        /// </summary>
        private readonly Keys[] NotBindingKeysAltModePAC =
        [
            Keys.Escape, Keys.Oem3, Keys.Capital, Keys.LWin, Keys.Apps,
            Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0,
        ];

        public FormMainSettings()
        {
            InitializeComponent();
            UpdatePositionImageAutor = true;

            ActivePanel = pMainMenu;
            bToolMainMenu.BackColor = Color.FromArgb(170, 240, 209);

            MainColorDialog.Color = MainData.AllSpecialColor.SC.ActientColorSC;

            Page =
            [pMainMenu, pColored, pPAC, pOther];

            ToolButtons =
            [bToolMainMenu, bToolColored, bToolPAC, bToolOther];

            Size = new(800, 450);


            if (App.MainForm.ChangeLengthBuffer == -1) App.MainForm.ChangeLengthBuffer = (int)MainData.Settings.Buffer_Count_Elements.Value;
            tbOtherCountBuffer.Value = App.MainForm.ChangeLengthBuffer;
            lOtherWarningCountBuffer.Size = new(App.MainForm.ChangeLengthBuffer == (int)MainData.Settings.Buffer_Count_Elements.Value ? 0 : 291, 18);
            lOtherCountBufferPreview.Text = tbOtherCountBuffer.Value.ToString();

            cbHitPanel.BoolParameter = MainData.Settings.Hit_Panel;
            cbMovingBorderScreenForm.BoolParameter = MainData.Settings.Moving_Border_Screen_Form;
            cbAltDiactivatePAC.BoolParameter = MainData.Settings.Alt_Diactivate_PAC;

            pKeyAltOrientation.Size = new(pKeyAltOrientation.Width, MainData.Settings.Alt_OrientationLR_PAC.Value ? 44 : 0);
            pPAC_Parameters.Size = new(pPAC_Parameters.Width, MainData.Settings.Alt_OrientationLR_PAC.Value ? 129 : 86);
            cbAllSpecialColorActivate.BoolParameter = MainData.Settings.All_SpecialColor_Activate;
            cbAllSpecialColorActivate.CheckedChanged += (State) =>
            {
                ConstAnimMove
                W = new(pSpecialColorParameter.Width, State ? 90 : 0, 10),
                H = new(pSpecialColorParameter.Height, State ? 79 : 0, 10);
                W.InitAnimFormule(pSpecialColorParameter, Formules.QuickTransition, H, AnimationStyle.Size);
            };
            pSpecialColorParameter.Size = new(cbAllSpecialColorActivate.Checked ? 90 : 0, cbAllSpecialColorActivate.Checked ? 79 : 0);
            cbSpecialColorRGB.BoolParameter = MainData.Settings.SpecialColor_RGB;
            cbSpecialColorRGBCC.BoolParameter = MainData.Settings.SpecialColor_RGBCC;
            cbSpecialColorSC.BoolParameter = MainData.Settings.SpecialColor_SC;

            cbAltOrientationPAC.BoolParameter = MainData.Settings.Alt_OrientationLR_PAC;
            cbAltOrientationPAC.CheckedChanged += (State) =>
            {
                ConstAnimMove
                W = new(pKeyAltOrientation.Width, State ? 241 : 0, 10),
                H = new(pKeyAltOrientation.Height, State ? 44 : 0, 10);
                W.InitAnimFormule(pKeyAltOrientation, Formules.QuickTransition, H, AnimationStyle.Size);

                new ConstAnimMove(pPAC_Parameters.Width)
                .InitAnimFormule(pPAC_Parameters, Formules.QuickTransition, new ConstAnimMove(pPAC_Parameters.Height, State ? 129 : 86, 10), AnimationStyle.Size);
            };
            tbPAC_KeyDiactivate.Text = MainData.Settings.HC_Alt_Diactivate_PAC.Value.ToString();
            tbPAC_KeyActivate.Text = MainData.Settings.HC_Alt_Activate_PAC.Value.ToString();
            tbPAC_KeyDiactivate.KeyDown += (Sender, e) =>
            {
                if (NotBindingKeysAltModePAC.Contains(e.KeyCode))
                {
                    new ConstAnimColor(Color.DarkRed, Color.FromArgb(159, 226, 229), 6).AnimInit(tbPAC_KeyDiactivate, AnimStyleColor.BackColor);
                    return;
                }
                tbPAC_KeyDiactivate.Text = e.KeyCode.ToString();
                MainData.Settings.HC_Alt_Diactivate_PAC.Value = e.KeyCode;
                MainData.Settings.SetParamOption(MainData.Settings.HC_Alt_Diactivate_PAC.Name, tbPAC_KeyDiactivate.Text);
                pPAC.Focus();
            };
            tbPAC_KeyActivate.KeyDown += (Sender, e) =>
            {
                if (NotBindingKeysAltModePAC.Contains(e.KeyCode))
                {
                    new ConstAnimColor(Color.DarkRed, Color.FromArgb(159, 226, 229), 6).AnimInit(tbPAC_KeyActivate, AnimStyleColor.BackColor);
                    return;
                }
                tbPAC_KeyActivate.Text = e.KeyCode.ToString();
                MainData.Settings.HC_Alt_Activate_PAC.Value = e.KeyCode;
                MainData.Settings.SetParamOption(MainData.Settings.HC_Alt_Activate_PAC.Name, tbPAC_KeyActivate.Text);
                pPAC.Focus();
            };



            lMainSoftCountAppend.Text = "Soft команды отключены";
            //lMainSoftCountAppend.Text = $"Добавлено {MainData.MainCommandData.SoftCommandData.Length} Soft команд";
            pExampleAcentColor.BackColor = MainData.AllSpecialColor.SC.ActientColorSC;
        }

        private void ReSize(object sender, EventArgs e)
        {
            Size SizedPanel = new(Size.Width - 16, Size.Height - 69);

            if (ActiveSettingsPage != SettingsPage.MainMenu) pMainMenu.Location = new(Size.Width + 2, pMainMenu.Location.Y);
            if (ActiveSettingsPage != SettingsPage.Colored) pColored.Location = new(Size.Width + 2, pColored.Location.Y);
            if (ActiveSettingsPage != SettingsPage.Other) pOther.Location = new(Size.Width + 2, pOther.Location.Y);
            if (ActiveSettingsPage != SettingsPage.PAC) pPAC.Location = new(Size.Width + 2, pPAC.Location.Y);

            foreach (Panel Element in Page)
            {
                Element.Location = ReSizePanelToolPanel(Element);
                Element.Size = SizedPanel;
            }

            pToolButton.Size = new(Size.Width - 16, pToolButton.Size.Height);
            bToolOther.Location = new(pToolButton.Size.Width - bToolOther.Size.Width - 2, bToolOther.Location.Y);

            lMainVersion.Location = new(lMainVersion.Location.X, Page[0].Size.Height - lMainVersion.Size.Height);
            lMainSoftCountAppend.Location = new(lMainSoftCountAppend.Location.X, Page[0].Size.Height - lMainSoftCountAppend.Size.Height - 2);
            pIconCollection.Location = new(Page[0].Size.Width - pIconCollection.Size.Width - 7, pIconCollection.Location.Y);
        }

        /// <summary>
        /// Переместить страницу, панель настроек на координаты сопоставимые с Tool
        /// </summary>
        /// <param name="Element">Изменяемая панель</param>
        public Point ReSizePanelToolPanel(Panel Element)
        {
            return new(Element.Location.X, pToolButton.Size.Height);
        }

        private void ActivateMenu(object sender, EventArgs e)
        {
            Button Element = (Button)sender;
            int i = 0, y = (Size.Width * Size.Height) / 100000 + 4;
            foreach (Button For_Element in ToolButtons)
            {
                if (For_Element == Element)
                {
                    if (ActivePanel != Page[i])
                    {
                        int index = Page.IndexOf(ActivePanel);
                        ConstAnimColor constAnim = new(null, Color.FromArgb(164, 230, 235), 6);
                        constAnim.AnimInit(ToolButtons[index], AnimStyleColor.BackColor);
                        constAnim = new(null, Color.FromArgb(170, 240, 209), 6);
                        constAnim.AnimInit(Element, AnimStyleColor.BackColor);

                        new ConstAnimMove(ToolButtons[index].Location.X + (index > i ? -15 : 15), ToolButtons[index].Location.X, 6)
                            .InitAnimFormule(ToolButtons[index], Formules.QuickTransition, new ConstAnimMove(ToolButtons[index].Location.Y), AnimationStyle.XY);

                        ActiveSettingsPage = (SettingsPage)(i + 1);
                        Page[i].Location = new(Page[i].Location.X, ActivePanel.Location.Y);
                        MiniFunctions.OpenNewMiniMenu(Page[i], ActivePanel, pToolButton.Size.Height,
                            index > i ? DirectionsParties.Left : DirectionsParties.Right, y: y);
                        ActivePanel = Page[i];
                    }
                    ReSize(null, null);
                    break;
                }
                i++;
            }
        }

        private void FormCloseding(object sender, FormClosingEventArgs e)
        {
            ActiveSettingsPage = SettingsPage.Null;
            if (App.MainForm.ChangeLengthBuffer != -1)
            {
                MainData.Settings.SetParamOption("Count-Buffer", App.MainForm.ChangeLengthBuffer.ToString());
            }
        }

        private void TbCountBufferChanged(object sender, EventArgs e)
        {
            lOtherCountBufferPreview.Text = tbOtherCountBuffer.Value.ToString();
            App.MainForm.ChangeLengthBuffer = tbOtherCountBuffer.Value;
            if (lOtherWarningCountBuffer.Size.Width == 0)
            {
                ConstAnimMove ConstantFormule = new(0, 291, 10);
                ConstantFormule.InitAnimFormule(lOtherWarningCountBuffer, Formules.QuickTransition, new ConstAnimMove(lOtherWarningCountBuffer.Size.Height), AnimationStyle.Size);
            }

        }

        private void SettingsCLR_Shown(object sender, EventArgs e)
        {
            Random random = new(2);
            pbAutor.Location = new(random.Next(13, 82), random.Next(13, 82));
            MovingIconMain();
        }

        private void ChangeAcientColorSC(object sender, EventArgs e)
        {
            MainColorDialog.Color = pExampleAcentColor.BackColor;
            MainColorDialog.ShowDialog();
            MainData.Settings.SetParamOption("Gradient-SpecialColor-SC", MainColorDialog.Color);
            pExampleAcentColor.BackColor = MainColorDialog.Color;
            MainData.AllSpecialColor.SC.ActientColorSC = MainColorDialog.Color;
        }

        private async void MovingIconMain()
        {
            int min = -2, max = 2;
            Random R = new();
            int MX = R.Next(min, max), MY = R.Next(min, max);
            MX = MX == 0 ? 1 : MX;
            MY = MY == 0 ? -1 : MY;

            Point MewPositionMouse;

            while (true)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        if (UpdatePositionImageAutor)
                        {
                            pbAutor.Location = new(pbAutor.Location.X + MX, pbAutor.Location.Y + MY);
                            if (pbAutor.Location.X <= 3) MX = R.Next(0, max);
                            if (pbAutor.Location.X >= 92) MX = R.Next(min, 0);

                            if (pbAutor.Location.Y <= 3) MY = R.Next(0, max);
                            if (pbAutor.Location.Y >= 92) MY = R.Next(min, 0);

                            Thread.Sleep(7);
                        }
                        else
                        {
                            SavePositionCursorStart = new(Cursor.Position.X - Location.X - pIconCollection.Location.X, Cursor.Position.Y - Location.Y - pIconCollection.Location.Y);
                            MewPositionMouse = new(SavePositionCursorStart.X - SavePositionOffsetCursorStart.X, SavePositionCursorStart.Y - SavePositionOffsetCursorStart.Y);
                            if (MewPositionMouse.X >= 0 && MewPositionMouse.X + pbAutor.Width <= pIconCollection.Width) pbAutor.Location = new(MewPositionMouse.X, pbAutor.Location.Y);
                            if (MewPositionMouse.Y >= 0 && MewPositionMouse.Y + pbAutor.Height <= pIconCollection.Height) pbAutor.Location = new(pbAutor.Location.X, MewPositionMouse.Y);
                        }
                    }
                    catch { }
                });
            }
        }

        private void ActivateMoveMouseIconAutor(object sender, MouseEventArgs e)
        {
            SavePositionCursorStart = new(Cursor.Position.X - Location.X - pIconCollection.Location.X, Cursor.Position.Y - Location.Y - pIconCollection.Location.Y);
            SavePositionOffsetCursorStart = new(SavePositionCursorStart.X - pbAutor.Location.X, SavePositionCursorStart.Y - pbAutor.Location.Y);
            UpdatePositionImageAutor = false;
        }

        private void DiactivateMoveMouseIconAutor(object sender, MouseEventArgs e)
        {
            UpdatePositionImageAutor = true;
        }
    }
}
