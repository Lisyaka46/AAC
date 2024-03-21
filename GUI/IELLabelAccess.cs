using AAC.Classes;
using System.Diagnostics;

namespace AAC.GUI
{

    public partial class IELLabelAccess : UserControl
    {
        /// <summary>
        /// Индекс изменения цвета (Активация/Дизактивация)
        /// </summary>
        private int ColorIndexChange { get; } = 50;

        /// <summary>
        /// Объект информации ярлыка
        /// </summary>
        public InfoLabelAccess InfoLabel { get; }

        /// <summary>
        /// Вид ярлыка
        /// </summary>
        public TypeLabel Status { get; }

        /// <summary>
        /// Количественный индекс
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Делегат события нажатия правой кнопкой мыши по объекту ярлыка
        /// </summary>
        /// <param name="info">Информационный объект ярлыка</param>
        public delegate void EventRightClickMouse(InfoLabelAccess? info, int index);

        /// <summary>
        /// Событие нажатия правой нопкой мыши по объекту ярлыка
        /// </summary>
        public event EventRightClickMouse? RightClickMouse;

        /// <summary>
        /// Инициализировать визуализационный объект ярлыка
        /// </summary>
        /// <param name="LabelInfo">Информация о ярлыке</param>
        /// <param name="Parent">Паль в которой может находиться объект</param>
        /// <param name="status">Статус ярлыка</param>
        /// <param name="Index">Индивидуальный номер ярлыка</param>
        /// <param name="Hide">Скрыть элемент из области видимости объекта</param>
        public IELLabelAccess(InfoLabelAccess LabelInfo, Panel? Parent, TypeLabel status = TypeLabel.Default, int Index = 0)
        {
            InitializeComponent();
            if (Parent != null) this.Parent = Parent;
            Status = status;
            this.Index = Index;
            SetLocationIndex(Index);
            InfoLabel = LabelInfo;
            NameLabel.Text = LabelInfo.Name;
            IconElement.Image = LabelInfo.IconElement(status);
            BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// Установить позицию объекта по индексу
        /// </summary>
        /// <param name="Index">Индекс позиции объекта</param>
        public void SetLocationIndex(int Index)
        {
            this.Index = Index;
            Location = new(10, 3 + 56 * Index);
            NumLabel.Text = (Index + 1).ToString();
        }

        /// <summary>
        /// Активировать ярлык
        /// </summary>
        /// <exception cref="NotImplementedException">Исключение нулевой активации</exception>
        public void IELLabelAccess_ActivateLabel()
        {
            if (InfoLabel.Action == InfoLabelAccess.TypeActionLabel.InitializeCommand)
            {
                ConsoleCommand Command = TypeCommand.ReadDefaultConsoleCommand(InfoLabel.Text);
                Command.ExecuteCommand(false).Summarize();
            }
            else if (InfoLabel.Action == InfoLabelAccess.TypeActionLabel.OpenDirectoryElement) Process.Start("explorer.exe", InfoLabel.Text);
            else if (InfoLabel.Action == InfoLabelAccess.TypeActionLabel.OpenLinkBrower)
            {
                try { Process.Start(new ProcessStartInfo(InfoLabel.Text) { UseShellExecute = true }); }
                catch
                {
                    new TypeCommand.StateResult(TypeCommand.ResultStateCommand.Failed,
                        $"Failed activate link \"{InfoLabel.Text}\"", $"Не удалось открыть ссылку {InfoLabel.Text}").Summarize();
                }
            }
        }

        /// <summary>
        /// Инициализировать пустой визуализационный объект ярлыка
        /// </summary>
        public IELLabelAccess()
        {
            InitializeComponent();
            Status = TypeLabel.Default;
            Index = 0;
            InfoLabel = new("???", null, InfoLabelAccess.TypeActionLabel.OpenDirectoryElement, string.Empty);
            BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// Событие входа курсора в область элемент изображения
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект события</param>
        private void IconElement_MouseEnter(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(
                BackColor.R <= 255 - ColorIndexChange ?
                BackColor.R + ColorIndexChange : 255 - BackColor.R + ColorIndexChange - 255 - ColorIndexChange,
                BackColor.G <= 255 - ColorIndexChange ?
                BackColor.G + ColorIndexChange : 255 - BackColor.G + ColorIndexChange - 255 - ColorIndexChange,
                BackColor.B <= 255 - ColorIndexChange ?
                BackColor.B + ColorIndexChange : 255 - BackColor.B + ColorIndexChange - 255 - ColorIndexChange
                );
        }

        /// <summary>
        /// Событие выхода курсора из области элемента изображения
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект события</param>
        private void IconElement_MouseLeave(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(
                BackColor.R >= ColorIndexChange ? BackColor.R - ColorIndexChange : 0,
                BackColor.G >= ColorIndexChange ? BackColor.G - ColorIndexChange : 0,
                BackColor.B >= ColorIndexChange ? BackColor.B - ColorIndexChange : 0
                );
        }

        /// <summary>
        /// Событие щелчка мыши по элементу
        /// </summary>
        /// <param name="sender">Объект создавший событие</param>
        /// <param name="e">Объект события</param>
        private void IconElement_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) IELLabelAccess_ActivateLabel();
            else if (e.Button == MouseButtons.Right) RightClickMouse?.Invoke(InfoLabel, Index);
        }
    }

    /// <summary>
    /// Структура данных о ярлыке
    /// </summary>
    /// <remarks>
    /// Инициализировать объект информации о ярлыке
    /// </remarks>
    /// <param name="NameLabel">Имя ярлыка</param>
    /// <param name="Dir">Директория иконки ярлыка</param>
    /// <param name="Action">Статус активации ярлыка</param>
    /// <param name="Text">Текст действия ярлыка</param>
    public readonly struct InfoLabelAccess(string NameLabel, string? Dir, InfoLabelAccess.TypeActionLabel Action, string Text)
    {
        /// <summary>
        /// Имя ярлыка
        /// </summary>
        public string Name { get; } = NameLabel;

        /// <summary>
        /// Директория иконки
        /// </summary>
        private string? IconDirectory { get; } = Dir;

        /// <summary>
        /// Создать иконку по директории элемента
        /// </summary>
        /// <param name="status">Вид ярлыка</param>
        /// <returns>Карта изображения (Может отличаться и быть присвоена зарезервированая программой)</returns>
        public Image IconElement(TypeLabel status)
        {
            string FileTypeName = string.Empty;
            if (status == TypeLabel.Priority) FileTypeName = "King";
            else if (status == TypeLabel.System) FileTypeName = "System";

            if (Action == TypeActionLabel.OpenDirectoryElement)
                return Image.FromFile(IconDirectory ?? $"{Directory.GetCurrentDirectory()}\\Data\\Image\\Label\\Folder{FileTypeName}.png");
            else if (Action == TypeActionLabel.InitializeCommand)
                return Image.FromFile(IconDirectory ?? $"{Directory.GetCurrentDirectory()}\\Data\\Image\\Label\\Console{FileTypeName}.png");
            else if (Action == TypeActionLabel.OpenLinkBrower)
                return Image.FromFile(IconDirectory ?? $"{Directory.GetCurrentDirectory()}\\Data\\Image\\Label\\Brower{FileTypeName}.png");
            else return Image.FromFile($"{Directory.GetCurrentDirectory()}\\Data\\Image\\Error.png");
        }

        /// <summary>
        /// Вид действия ярлыка
        /// </summary>
        public TypeActionLabel Action { get; } = Action;

        /// <summary>
        /// Текст действия
        /// </summary>
        public string Text { get; } = Text;


        /// <summary>
        /// Перечисление состояний выполнения действий ярлыка
        /// </summary>
        public enum TypeActionLabel
        {
            /// <summary>
            /// Активация елемента по его директории (Файл/Папка)
            /// </summary>
            OpenDirectoryElement = 0,

            /// <summary>
            /// Выполнение одной команды в программе
            /// </summary>
            InitializeCommand = 1,

            /// <summary>
            /// Открытие ссылки в браузере
            /// </summary>
            OpenLinkBrower = 2,
        }
    }

    /// <summary>
    /// Перечисление видов ярлыков
    /// </summary>
    public enum TypeLabel
    {
        /// <summary>
        /// Системный ярлык
        /// </summary>
        System = 0,

        /// <summary>
        /// Приоритетный ярлык
        /// </summary>
        Priority = 1,

        /// <summary>
        /// Обычный ярлык
        /// </summary>
        Default = 2,
    }
}
