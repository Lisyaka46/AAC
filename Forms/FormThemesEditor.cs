using AAC.Classes;
using AAC.GUI;
using static AAC.Classes.AnimationDL.Animate.AnimFormule;
using static AAC.Classes.MainTheme;
using static AAC.Startcs;

namespace AAC.Forms
{
    public partial class FormThemesEditor : Form
    {
        /// <summary>
        /// Массив визуализационных объектов тем программы
        /// </summary>
        private List<IELTheme> MassThemePanel { get; set; }

        /// <summary>
        /// Массив визуализационных объектов параметров цвета темы
        /// </summary>
        private IELParamColorTheme[] ParamColorTheme { get; set; }

        /// <summary>
        /// Тема над которой производятся действия
        /// </summary>
        private Theme? ThemeAction { get; set; }

        /// <summary>
        /// Тема над которой производятся действие изменение параметров цвета
        /// </summary>
        private Theme? ThemeChangeParamColor { get; set; }

        /// <summary>
        /// Активно ли состояние изменения параметров цвета в теме
        /// </summary>
        private bool ActiveChangeParamColorTheme { get; set; }

        /// <summary>
        /// Состояние активности мини-панели действий над темой
        /// </summary>
        private bool ActiveMiniPanel { get; set; }

        /// <summary>
        /// Счётчик скролл-бара параметров цвета темы
        /// </summary>
        private CounterScrollBar CounterParamColorTheme { get; }

        /// <summary>
        /// Счётчик скролл-бара тем
        /// </summary>
        private CounterScrollBar CounterTheme { get; }

        /// <summary>
        /// Массив поиска индексов элементов
        /// </summary>
        private int[]? MassMultiSearchIndex { get; set; }

        /// <summary>
        /// Активный индекс поиска элементов
        /// </summary>
        private int MultiSearchActiveIndex { get; set; }

        public FormThemesEditor()
        {
            InitializeComponent();
            ActiveChangeParamColorTheme = false;
            MultiSearchActiveIndex = -1;
            MassThemePanel = [];

            foreach (Theme theme in MainData.MainThemeData.MassTheme)
            {
                MassThemePanel.Add(new(theme, pAllElementsRegThemes, new(5, MassThemePanel.Count > 0 ? 26 * (MassThemePanel.Count + 1) + 4 : 5)));
                MassThemePanel[^1].ActionLeftClick += (theme) => { MiniPanelDetect(theme); };

                MassThemePanel[^1].DescriptionEvent += (DescriptionText) =>
                {
                    if (lDescription.Visible && DescriptionText == null) lDescription.Hide();
                    else if (DescriptionText != null)
                    {
                        lDescription.Text = DescriptionText;
                        lDescription.Show();
                    }
                };
            }

            ParamColorTheme = new IELParamColorTheme[MainData.MainThemeData.MassInfoParameters.Length];
            for (int i = 0; i < MainData.MainThemeData.MassInfoParameters.Length; i++)
            {
                ParamColorTheme[i] = new(MainData.MainThemeData.MassInfoParameters[i], pAllParamColorTheme, null, i)
                {
                    Location = new(5, i == 0 ? 3 : 70 * i + 3)
                };
                ParamColorTheme[i].BringToFront();
                ParamColorTheme[i].ParamChangeColor += (Index) =>
                {
                    if (ColorCreator.ShowDialog(this) == DialogResult.OK)
                    {
                        ParamColorTheme[Index].SetParamColor(ColorCreator.Color);
                        if (ThemeChangeParamColor.HasValue)
                        {
                            if (ThemeChangeParamColor.Value.Name.Equals(MainData.MainThemeData.ActivateTheme.Name))
                            {
                                MainData.MainThemeData.ActivateTheme.ObjColors[Index].ElColor = ColorCreator.Color;
                                UpdateAllFormsInThemeIndex(Index);
                            }
                        }
                        return ColorCreator.Color;
                    }
                    return null;
                };
            }
            pAllRedactorParamColorTheme.Location = new(-560, pAllRedactorParamColorTheme.Location.Y);
            pAllParamColorTheme.Size = new(pAllParamColorTheme.Width, 70 * ParamColorTheme.Length + 3);
            UpdateInfo();

            MultiSearchPanel.Size = new(MultiSearchPanel.Width, 0);

            CounterTheme = new(MassThemePanel.Count, 9);

            pMiniPanel.Size = new(pMiniPanel.Width, 0);
            ActiveMiniPanel = false;

            Size = new(808, 515);
            MaximumSize = Size;
            MinimumSize = Size;
            bool CloseMiniPanel = true;

            CounterParamColorTheme = new(ParamColorTheme.Length, 5);
            ScrollBarParamColor.Maximum = CounterParamColorTheme.MaxValue;
            ScrollBarParamColor.LargeChange = CounterParamColorTheme.TrafficShare;
            ScrollBarParamColor.Value = CounterParamColorTheme.Value;
            BorderStartParamColor.Location = new(pAllParamColorTheme.Width / 2 + BorderStartParamColor.Width / 2, -27);
            BorderEndParamColor.Location = new(pAllParamColorTheme.Width / 2 + BorderEndParamColor.Width / 2, ParamColorTheme[^1].Location.Y + ParamColorTheme[^1].Height - 27);
            pAllParamColorTheme.MouseWheel += (sender, e) =>
            {
                if (MassMultiSearchIndex != null) MultiSearchDisactivate();
                ScrollParamColor(e.Delta < 0 ? CounterParamColorTheme.Down() : CounterParamColorTheme.Up());
                ScrollBarParamColor.Value = CounterParamColorTheme.Value;
            };
            ScrollBarParamColor.ValueChanged += (sender, e) =>
            {
                if (MassMultiSearchIndex != null) MultiSearchDisactivate();
                ScrollParamColor(ScrollBarParamColor.Value);
                CounterParamColorTheme.Value = ScrollBarParamColor.Value;
            };

            pAllRegisteredThemes.MouseWheel += (sender, e) =>
            {
                if (MainData.Flags.MiniPanelpMiniPanelActive == Data.BooleanFlags.True)
                {
                    if (e.Delta < 0 && vSBRegisteredThemes.Value != vSBRegisteredThemes.Maximum)
                    {
                        pMiniPanel.Location = new(pMiniPanel.Location.X, pMiniPanel.Location.Y - vSBRegisteredThemes.LargeChange);
                        if (pMiniPanel.Location.Y < pAllRegisteredThemes.Location.Y + 5)
                            DeactivateMiniPanel();
                    }
                    else if (e.Delta > 0 && vSBRegisteredThemes.Value != vSBRegisteredThemes.Minimum)
                    {
                        pMiniPanel.Location = new(pMiniPanel.Location.X, pMiniPanel.Location.Y + vSBRegisteredThemes.LargeChange);
                        if (pMiniPanel.Location.Y > pAllRegisteredThemes.Location.Y + pAllRegisteredThemes.Size.Height - 5)
                            DeactivateMiniPanel();
                    }
                    CloseMiniPanel = false;
                }
                MiniFunctions.UpdateVScrollBar(vSBRegisteredThemes, e.Delta);
            };
            vSBRegisteredThemes.ValueChanged += (sender, e) =>
            {
                if (!CloseMiniPanel) CloseMiniPanel = !CloseMiniPanel;
                else if (CloseMiniPanel && MainData.Flags.MiniPanelpMiniPanelActive == Data.BooleanFlags.True) DeactivateMiniPanel();
                MiniFunctions.MoveElementinVScrollBar(pAllElementsRegThemes, vSBRegisteredThemes);
            };

            bmpButtonBack.Click += (sender, e) => { DeactivateMiniPanel(); };
            bmpSaveChangeTheme.Click += (sender, e) =>
            {
                if (ThemeChangeParamColor != null && (!ThemeAction?.Name.Equals(ThemeChangeParamColor?.Name) ?? true)) ColorManagerChange();
                else ColorManagerDetect();
            };
            ButtonCloseChangeParamColor.Click += (sender, e) => { ColorManagerDetect(); };
            ButtonSearchParamColor.Click += (sender, e) =>
            {
                int? SearchIndex = SearchParamColor(SearchParamColorTextBox.Text);
                if (SearchIndex != null)
                {
                    ScrollBarParamColor.Value = (int)SearchIndex > CounterParamColorTheme.CountVisibleElements ?
                    (int)SearchIndex - CounterParamColorTheme.CountVisibleElements : 0;
                    ScrollSearchDetect((int)SearchIndex);
                }
                else
                {
                    MassMultiSearchIndex = MultiSearchParamColor(SearchParamColorTextBox.Text);
                    if (MassMultiSearchIndex != null) MultiSearchActivate();
                    else
                    {
                        ConstAnimMove Anim = new(SearchParamColorTextBox.Location.X, SearchParamColorTextBox.Location.X, 7);
                        Anim.InitAnimFormule(SearchParamColorTextBox,
                            Formules.QuickSinusoid, new ConstAnimMove(SearchParamColorTextBox.Location.Y), AnimationStyle.XY);
                    }
                }
                SearchParamColorTextBox.Text = "Поиск параметров";
                SearchParamColorTextBox.TextAlign = HorizontalAlignment.Center;
            };
            ButtonRightMultiSearchParamColor.MouseClick += (sender, e) => { MultiSearchDetectUp(); };
            ButtonLeftMultiSearchParamColor.MouseClick += (sender, e) => { MultiSearchDetectDown(); };
            MultiSearchCounter.Click += (sender, e) => { MultiSearchDisactivate(); };
            bSetActiveTheme.Click += (sender, e) => { SetActiveTheme(); };

            pMiniPanel.Size = new(pMiniPanel.Width, 0);
            lDescription.Hide();
            Task.Run(WhileCursorPosition);
        }

        /// <summary>
        /// Функция постоянного обновления параметров в форме
        /// </summary>
        public async void WhileCursorPosition()
        {
            await Task.Run(() =>
            {
                while (this != null)
                {
                    if (lDescription.Visible)
                        lDescription.Location = new(Cursor.Position.X - Location.X - 4, Cursor.Position.Y - Location.Y - 52);
                }
            });
        }

        /// <summary>
        /// Функция активации минипанели
        /// </summary>
        private void MiniPanelDetect(Theme? theme)
        {
            if (theme == null)
            {
                if (ActiveMiniPanel) DeactivateMiniPanel();
                return;
            }
            if (MassMultiSearchIndex != null) MultiSearchDisactivate();

            // Создание позиции для перемещения
            Point LocationSet()
            {
                const int OffsetX = -2, OffsetY = -23;
                int X = Cursor.Position.X - Location.X + OffsetX, Y = Cursor.Position.Y - Location.Y + OffsetY;
                return new(
                    X + pMiniPanel.Width < Width - 16 ? X : Width - pMiniPanel.Width - 16,
                    Y + pMiniPanel.Height < Height ? Y : Height - pMiniPanel.Height);
            }
            Point RelativeLocation = LocationSet();
            ThemeAction = theme;

            if (!ActiveMiniPanel)
            {
                ActiveMiniPanel = true;
                pMiniPanel.Location = new(RelativeLocation.X, RelativeLocation.Y);
                ConstAnimMove Anim = new(0, 146, 13);
                new ConstAnimMove(pMiniPanel.Width).InitAnimFormule(pMiniPanel, Formules.QuickTransition, Anim, AnimationStyle.Size);
            }
            else
            {
                ConstAnimMove AnimX, AnimY;
                AnimX = new(pMiniPanel.Location.X, RelativeLocation.X, 10);
                AnimY = new(pMiniPanel.Location.Y, RelativeLocation.Y, 10);
                AnimX.InitAnimFormule(pMiniPanel, Formules.QuickTransition, AnimY, AnimationStyle.XY);
            }
            lmpName.Text = $"Действия темы {theme.Value.Name}";
            if (theme.Value.Name.Equals("Default")) ButtonDeleteTheme.BackgroundImage = Image.FromFile($"{Directory.GetCurrentDirectory()}/Data/Image/DisactiveTrashCan.png");
            else ButtonDeleteTheme.BackgroundImage = Image.FromFile($"{Directory.GetCurrentDirectory()}/Data/Image/ActiveTrashCan.png");
            ButtonDeleteTheme.Enabled = !theme.Value.Name.Equals("Default");
            bSetActiveTheme.BackColor = theme.Value.Name.Equals(MainData.MainThemeData.ActivateTheme.Name) ? Color.FromArgb(255, 120, 97) : Color.FromArgb(255, 160, 137);
            if (ThemeChangeParamColor == null) bmpSaveChangeTheme.Text = "Менеджер цветов темы";
            else if (theme.Value.Name.Equals(ThemeChangeParamColor.Value.Name)) bmpSaveChangeTheme.Text = "Сохранить палитру цветов";
            else bmpSaveChangeTheme.Text = "Переназначить тему";
        }

        /// <summary>
        /// Деактивировать мини-панель с помощью обратной анимации
        /// </summary>
        public void DeactivateMiniPanel()
        {
            ActiveMiniPanel = false;
            ConstAnimMove ConstantFormule = new(pMiniPanel.Height, 0, 9);
            new ConstAnimMove(pMiniPanel.Width).InitAnimFormule(pMiniPanel, Formules.QuickTransition, ConstantFormule, AnimationStyle.Size);
            MainData.Flags.MiniPanelpMiniPanelActive = Data.BooleanFlags.False;
        }

        /// <summary>
        /// Функция скрола параметров цветов темы
        /// </summary>
        /// <param name="Index">Индекс скрола</param>
        private void ScrollParamColor(int Index)
        {
            if (ActiveMiniPanel) DeactivateMiniPanel();
            int Y = 70 - 70 / CounterParamColorTheme.TrafficShare * Index;
            ConstAnimMove ConstantFormule = new(pAllParamColorTheme.Location.Y, Y, 14);
            new ConstAnimMove(pAllParamColorTheme.Location.X).InitAnimFormule(pAllParamColorTheme,
                Formules.QuickTransition, ConstantFormule, AnimationStyle.XY);
        }

        //
        public void AppendNewTheme(Theme NewTheme)
        {
            MainData.MainThemeData.MassTheme.Add(NewTheme);
            MassThemePanel.Add(new(NewTheme, pAllElementsRegThemes, new(5, MassThemePanel.Count > 0 ? 26 * (MassThemePanel.Count + 1) + 4 : 5)));
            UpdateInfo();
            vSBRegisteredThemes.Value = vSBRegisteredThemes.Maximum;
        }

        public void UpdateInfo()
        {
            if (MassThemePanel.Count > 0)
                MiniFunctions.CalculationDownElementScrollBar(MassThemePanel[^1], pAllElementsRegThemes, pAllRegisteredThemes, vSBRegisteredThemes, 2);
            else
            {
                vSBRegisteredThemes.Hide();
                vSBRegisteredThemes.Maximum = 0;
            }
        }

        private void BCreateNewTheme_Click(object sender, EventArgs e)
        {
            Theme NEWTHEME;
            NEWTHEME = MainData.MainThemeData.ActivateTheme;
            App.Create.DialogCreateTheme = new(NEWTHEME);
            App.Create.DialogCreateTheme.ShowDialog();
        }

        private void Themes_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ActiveChangeParamColorTheme && ThemeAction != null)
            {
                SaveThemeInFile(ThemeAction);
                if (ActiveChangeParamColorTheme) ActiveChangeParamColorTheme = false;
            }
            //foreach (ELIlustrationTheme Element in MassThemePanel) Element.MainPanelTheme.Dispose();
            MainData.Flags.MiniPanelpMiniPanelActive = Data.BooleanFlags.False;
        }

        /// <summary>
        /// Включить или выключить изменение цветовых параметров темы
        /// </summary>
        private void ColorManagerDetect()
        {
            if (ThemeAction == null) return;
            ThemeChangeParamColor = ActiveChangeParamColorTheme ? null : ThemeAction;
            if (ActiveChangeParamColorTheme && MassMultiSearchIndex != null) MultiSearchDisactivate();
            NameTheme.Text = ThemeChangeParamColor?.Name ?? string.Empty;
            for (int i = 0; i < ParamColorTheme.Length; i++)
                ParamColorTheme[i].SetParamColor(ActiveChangeParamColorTheme ? null : ThemeChangeParamColor?.ObjColors[i].ElColor);
            ConstAnimMove ConstantFormule = new(pAllRedactorParamColorTheme.Location.X, ActiveChangeParamColorTheme ? -560 : 209, 70);
            ConstantFormule.InitAnimFormule(pAllRedactorParamColorTheme, Formules.QuickTransition,
                new ConstAnimMove(pAllRedactorParamColorTheme.Location.Y), AnimationStyle.XY);
            ActiveChangeParamColorTheme = !ActiveChangeParamColorTheme;
            DeactivateMiniPanel();
        }

        /// <summary>
        /// Переназначить активную тему изменения цвета в теме
        /// </summary>
        private void ColorManagerChange()
        {
            if (!ActiveChangeParamColorTheme) return;
            ThemeChangeParamColor = ThemeAction;
            NameTheme.Text = ThemeChangeParamColor?.Name ?? string.Empty;
            for (int i = 0; i < ParamColorTheme.Length; i++) ParamColorTheme[i].SetParamColor(ThemeChangeParamColor?.ObjColors[i].ElColor);
            DeactivateMiniPanel();
        }

        /// <summary>
        /// Задать активную тему действующей
        /// </summary>
        private void SetActiveTheme()
        {
            if (ThemeAction.HasValue)
            {
                MainData.MainThemeData.ActivateTheme = ThemeAction.Value;
                ObjLog.LOGTextAppend($"CHANGE_THEME: \"{MainData.MainThemeData.ActivateTheme.Name}\" == \"{ThemeAction.Value.Name}\"");
                MainData.Settings.SetParamOption("Theme-Activate", ThemeAction.Value.Name);
                UpdateAllFormsInTheme();
                ThemeAction = null;
            }
            DeactivateMiniPanel();
        }

        /// <summary>
        /// Обновить персонализацию всех форм использующие темы
        /// </summary>
        private static void UpdateAllFormsInTheme()
        {
            App.MainForm.UpdateTheme(MainData.MainThemeData.ActivateTheme);
        }

        /// <summary>
        /// Обновить конкретный элемент в теме всех используемых формах по индексу
        /// </summary>
        /// <param name="Index">Индекс элемента</param>
        private static void UpdateAllFormsInThemeIndex(int Index)
        {
            App.MainForm.UpdateThemeIndexElement(Index);
        }

        /// <summary>
        /// Сохранить изменения в теме в файл текущей темы
        /// </summary>
        /// <param name="SaveTheme">Сохраняемая тема</param>
        /// <returns>Сохранилась ли тема или нет</returns>
        public bool SaveThemeInFile(Theme? SaveTheme)
        {
            if (SaveTheme == null) return false;
            if (SaveTheme.Value.FileDirectory?.Equals(string.Empty) ?? true) return false;
            Color LOGColor = SaveTheme.Value.ObjColors[0].ElColor;
            string NameFile = SaveTheme.Value.FileDirectory;
            ObjLog.LOGTextAppend($"Имя сохраняемой темы: {SaveTheme.Value.Name}");
            ObjLog.LOGTextAppend($"Директория файла: {NameFile}");
            ObjLog.LOGTextAppend($"Цвет BOCT [0]: R:{LOGColor.R}, G:{LOGColor.G}, B:{LOGColor.B}");
            return true;
        }

        /// <summary>
        /// Создать новый файл темы после создания
        /// </summary>
        /// <param name="CreateTheme">Тема из которой будут браться данные</param>
        /// <param name="NameFile">Имя создаваемого файла</param>
        public static void CreateNewFileTheme(Theme CreateTheme, string NameFile)
        {
            if (!NameFile.Contains("\\\\")) NameFile = NameFile.Replace("\\", "\\\\");
            Color? color;
            FileStream fs = new(NameFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            using StreamWriter FileWrite = new(fs);
            FileWrite.WriteLine("NAMEFILE:" + NameFile.Replace(";", "=;").Replace("=", "==") + ";");
            FileWrite.WriteLine("NAME:" + CreateTheme.Name.Replace(";", "=;").Replace("=", "==") + ";");
            FileWrite.WriteLine("DESCRIPTION:" + CreateTheme.Description.Replace(";", "=;").Replace("=", "==") + ";");
            FileWrite.WriteLine("ICON{" + CreateTheme.IconDirectory.Replace("\\", "\\\\").Replace(";", "=;").Replace("=", "==") + ";");
            FileWrite.WriteLine("THEME:\n");
            for (int i = 0; i < MainData.MainThemeData.MassInfoParameters.Length; i++)
            {
                color = CreateTheme.ObjColors[i].ElColor;
                if (color != null) FileWrite.WriteLine($"={color.Value.R};{color.Value.G};{color.Value.B};");
            }
            FileWrite.Write("}");
            FileWrite.Close();
        }

        private void BmpDeleteTheme_Click(object sender, EventArgs e)
        {
        }

        private void BpmpChangedThemeInfo_Click(object sender, EventArgs e)
        {
            //App.Create.DialogCreateTheme = new(ActiveThemePanelInForm.IlustrationTheme, true);
            //App.Create.DialogCreateTheme.ShowDialog();
        }

        private void ButtonDeleteTheme_Click(object sender, EventArgs e)
        {
            lmpName.Text = string.Empty;
        }

        /// <summary>
        /// Функция события активации панели ввода поиска параметра цвета
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект события</param>
        private void SearchEnter(object sender, EventArgs e)
        {
            SearchParamColorTextBox.Text = string.Empty;
            SearchParamColorTextBox.TextAlign = HorizontalAlignment.Left;
        }

        /// <summary>
        /// Функция события дизактивации поля ввода поиска
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект события</param>
        private void SearchLeave(object sender, EventArgs e)
        {
            if (SearchParamColorTextBox.TextLength == 0)
            {
                SearchParamColorTextBox.Text = "Поиск параметров";
                SearchParamColorTextBox.TextAlign = HorizontalAlignment.Center;
            }
        }

        /// <summary>
        /// Поиск явного параметра из массива цветов темы
        /// </summary>
        /// <param name="TextSearch">Текст поиска</param>
        /// <returns>Значение индекса</returns>
        private int? SearchParamColor(string TextSearch)
        {
            for (int i = 0; i < ParamColorTheme.Length; i++)
            {
                if (ParamColorTheme[i].InfoParameter?.Name.Equals(TextSearch) ?? false) return i;
            }
            return null;
        }

        /// <summary>
        /// Поиск по части имени параметра цвета темы
        /// </summary>
        /// <param name="TextSearch">текст поиска</param>
        /// <returns>Массив индексов параметров цвета темы</returns>
        private int[]? MultiSearchParamColor(string TextSearch)
        {
            List<int> ListIndexParamColor = [];
            for (int i = 0; i < ParamColorTheme.Length; i++)
            {
                if (ParamColorTheme[i].InfoParameter?.Name.Contains(TextSearch) ?? false) ListIndexParamColor.Add(i);
                if (ListIndexParamColor.Count >= 12) break;
            }
            if (ListIndexParamColor.Count == 0) return null;
            return [..ListIndexParamColor];
        }

        /// <summary>
        /// Увеличить индекс мульти-поиска вверх на 1
        /// </summary>
        private void MultiSearchDetectUp()
        {
            if (MassMultiSearchIndex != null)
            {
                if (MultiSearchActiveIndex < MassMultiSearchIndex.Length - 1) MultiSearchActiveIndex++;
                MultiSearchCounter.Text = $"{MultiSearchActiveIndex + 1}/{MassMultiSearchIndex.Length}";

                ScrollSearchDetect(MassMultiSearchIndex[MultiSearchActiveIndex]);
            }
            else MultiSearchDisactivate();
        }

        /// <summary>
        /// Уменьшить индекс мульти-поиска вниз на 1
        /// </summary>
        private void MultiSearchDetectDown()
        {
            if (MassMultiSearchIndex != null)
            {
                if (MultiSearchActiveIndex > 0) MultiSearchActiveIndex--;
                MultiSearchCounter.Text = $"{MultiSearchActiveIndex + 1}/{MassMultiSearchIndex.Length}";

                ScrollSearchDetect(MassMultiSearchIndex[MultiSearchActiveIndex]);
            }
            else MultiSearchDisactivate();
        }

        /// <summary>
        /// Закрыть мульти-поиск
        /// </summary>
        private void MultiSearchDisactivate()
        {
            MultiSearchActiveIndex = -1;
            MassMultiSearchIndex = null;
            ConstAnimMove ConstantFormule = new(MultiSearchPanel.Height, 0, 10);
            new ConstAnimMove(MultiSearchPanel.Width).InitAnimFormule(MultiSearchPanel,
                Formules.QuickTransition, ConstantFormule, AnimationStyle.Size);
        }

        /// <summary>
        /// Активировать мульти-поиск
        /// </summary>
        private void MultiSearchActivate()
        {
            if (MassMultiSearchIndex != null)
            {
                ConstAnimMove ConstantFormule = new(MultiSearchPanel.Height, 36, 10);
                new ConstAnimMove(MultiSearchPanel.Width).InitAnimFormule(MultiSearchPanel,
                    Formules.QuickTransition, ConstantFormule, AnimationStyle.Size);
                MultiSearchActiveIndex = 0;
                MultiSearchCounter.Text = $"{MultiSearchActiveIndex + 1}/{MassMultiSearchIndex.Length}";
                ScrollSearchDetect(MassMultiSearchIndex[MultiSearchActiveIndex]);
            }
        }

        /// <summary>
        /// Функция скролла к объекту параметра по индексу
        /// </summary>
        /// <param name="IndexSearch">Индекс параметра цвета</param>
        private void ScrollSearchDetect(int IndexSearch)
        {
            int IndexParameter = ParamColorTheme[IndexSearch].IndexParameter > CounterParamColorTheme.CountVisibleElements - 1 ?
                    ParamColorTheme[IndexSearch].IndexParameter - CounterParamColorTheme.CountVisibleElements + 1 : 0;
            ScrollParamColor(IndexParameter);
            ParamColorTheme[IndexSearch].SearchDetect();
        }
    }
}
