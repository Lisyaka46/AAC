using AAC.Classes;
using System.Xml.Linq;
using static AAC.Classes.AnimationDL.Animate.AnimFormule;
using static AAC.Forms_Functions;
using Buffer = AAC.Classes.Buffer;

namespace AAC.GUI
{
    public partial class IELBuffer : UserControl
    {
        /// <summary>
        /// Визуализационный объект буфера
        /// </summary>
        /// <param name="Length">Максимальное кол-во всетимых объектов</param>
        public class ListElementBuffer(int Length)
        {
            /// <summary>
            /// Делегат события добваления элемента в буфер
            /// </summary>
            /// <param name="Parent">Панель в которой должен обязательно находиться элемент</param>
            /// <param name="Text">Текст элемента</param>
            /// <param name="Index">Индексированная позиция элемента в панели</param>
            /// <returns>Настроенный объект буфера</returns>
            public delegate Label EventGenerateLabelAdd(Panel Parent, string Text, int Index);

            /// <summary>
            /// Событие добавления объекта в буфер
            /// </summary>
            public event EventGenerateLabelAdd? GenerateLabel;

            /// <summary>
            /// Массив объектов буфера
            /// </summary>
            private readonly List<Label> Elements = [];

            /// <summary>
            /// Максимальное количество вместимости буфера
            /// </summary>
            public readonly int Length = Length;

            /// <summary>
            /// Количество находимых элементов в буфере
            /// </summary>
            public int Count => Elements.Count;

            /// <summary>
            /// Узнать текст объекта буфера по индексу
            /// </summary>
            /// <param name="key">Индекс объекта буфера</param>
            /// <returns>Текст объекта буфера</returns>
            public string this[Index key]
            {
                get => Elements[key].Text;
            }

            /// <summary>
            /// Добавить объект в буфер
            /// </summary>
            /// <param name="Parent">Панель в которой находится объект</param>
            /// <param name="Text">Текст объекта буфера</param>
            public void Add(Panel Parent, string Text)
            {
                if (GenerateLabel == null) return;
                if (Elements.Count < Length)
                {
                    Label label = GenerateLabel.Invoke(Parent, Text, Elements.Count);
                    label.MouseEnter += (sender, e) => label.BackColor = ColorWhile.SetOffsetColor(label.BackColor, 30);
                    label.MouseLeave += (sender, e) => label.BackColor = ColorWhile.SetOffsetColor(label.BackColor, -30);
                    label.Click += (sender, e) => TypeCommand.ReadDefaultConsoleCommand(label.Text).ExecuteCommand(false).Summarize();
                    Elements.Add(label);
                }
                else
                {
                    for (int i = 1; i < Elements.Count; i++) Elements[i - 1].Text = Elements[i].Text;
                    Elements[^1].Text = Text;
                }
            }

            /// <summary>
            /// Удалить все объекты из буфера
            /// </summary>
            public void DeleteAll()
            {
                for (int i = 0; i < Elements.Count; i++) Elements[i].Dispose();
                Elements.Clear();
            }
        }

        /// <summary>
        /// Буфер визуализационного объекта
        /// </summary>
        private Buffer BufferData;

        /// <summary>
        /// Счётчик скролл-бара
        /// </summary>
        private CounterScrollBar CounterScroll { get; }

        /// <summary>
        /// Массив визуализационных элементов буфера
        /// </summary>
        public ListElementBuffer ElementsBuffer { get; private set; }

        public IELBuffer()
        {
            InitializeComponent();
            BufferData = new();
            ElementsBuffer = new(BufferData.Length);
            ElementsBuffer.GenerateLabel += GenerateLabelBuffer;
            CounterScroll = new(0, 6);
            ScrollBar.ValueChanged += (sender, e) =>
            {
                CounterScroll.Value = ScrollBar.Value;
                new ConstAnimMove(pElements.Location.X)
                    .InitAnimFormule(pElements, Formules.QuickTransition, new(pElements.Location.Y, -1 - (30 * ScrollBar.Value), 17), AnimationStyle.XY);
            };
            ScrollBar.Location = new(Width, 0);
            ScrollBar.Value = 0;
            ScrollBar.Maximum = 0;
            pElements.MouseWheel += (sender, e) =>
            {
                if (e.Delta < 0 && CounterScroll.Value < CounterScroll.MaxValue) CounterScroll.Value++;
                else if (e.Delta > 0 && CounterScroll.Value > 0) CounterScroll.Value--;
                ScrollBar.Value = CounterScroll.Value;
            };
            SizeChanged += (sender, e) =>
            {
                ScrollBar.Location = new(Width, 0);
                ScrollBar.Size = new(17, Height);
                pElements.Size = new(Width - 19, Height);
                lInfoZeroCommandBuffer.Location = new(pElements.Width / 2 - lInfoZeroCommandBuffer.Width / 2, 9);
            };
        }

        /// <summary>
        /// Инициализировать новый объект буфера
        /// </summary>
        /// <param name="MaxElements">Максимальное кол-во вместимых объектов</param>
        public void InitializeNewBuffer(int MaxElements = 50)
        {
            BufferData = new(Math.Clamp(MaxElements, 4, 80));
            ElementsBuffer = new(BufferData.Length);
        }

        /// <summary>
        /// Сгенерировать объект команды
        /// </summary>
        /// <param name="Parent">Панель где находится объект</param>
        /// <param name="Text">текст команды</param>
        /// <param name="Index">Индекс позиции</param>
        /// <returns>Объект команды буфера</returns>
        private static Label GenerateLabelBuffer(Panel Parent, string Text, int Index)
        {
            return new()
            {
                Parent = Parent,
                Text = Text,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Location = new(9, 30 * Index + 7),
                Size = new(Parent.Width - 18, 23),
                AutoEllipsis = true,
                ForeColor = Color.White,
                BackColor = Parent.BackColor,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new("Segoe UI Semibold", 9.75f, FontStyle.Bold),
                Cursor = Cursors.Hand,
            };
        }

        /// <summary>
        /// Добавить новый объект и настроить буфер
        /// </summary>
        /// <param name="Element">Текст команды</param>
        public void AddNewElement(string Element)
        {
            bool Append = BufferData.Add(Element);
            if (Append)
            {
                ElementsBuffer.Add(pElements, Element);
                if (BufferData.Count > CounterScroll.CountVisibleElements)
                {
                    ScrollBar.Location = new(Width - 19, 0);
                    pElements.Size = new(Width - 19, pElements.Height + 30);
                    CounterScroll.MaxUp(1);
                    ScrollBar.Value = 0;
                    ScrollBar.Maximum++;
                }
                if (lInfoZeroCommandBuffer.Visible) lInfoZeroCommandBuffer.Visible = false;
            }
        }

        /// <summary>
        /// Удалить все объекты буввера команд
        /// </summary>
        public void DeleteAll()
        {
            if (BufferData.Count > CounterScroll.CountVisibleElements)
                new ConstAnimMove(ScrollBar.Location.X, Width, 10)
                    .InitAnimFormule(ScrollBar, Formules.QuickTransition, new(ScrollBar.Location.Y), AnimationStyle.XY);
            ScrollBar.Maximum = 0;
            pElements.Location = new(-1, -1);
            pElements.Size = new(Width - 19, Height);
            lInfoZeroCommandBuffer.Visible = true;
            new ConstAnimMove(lInfoZeroCommandBuffer.Location.X)
                .InitAnimFormule(lInfoZeroCommandBuffer, Formules.QuickTransition, new ConstAnimMove(-33, 9, 10), AnimationStyle.XY);
            BufferData.DeleteAll();
            ElementsBuffer.DeleteAll();
            CounterScroll.MaxDown(CounterScroll.MaxValue);
        }
    }
}
