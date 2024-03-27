using AAC.GUI;
using static AAC.Classes.AnimationDL.Animate.AnimFormule;
using static AAC.Startcs;
using AAC.Classes;
using System.Reflection.Metadata.Ecma335;
using System.Diagnostics;
using MMC20;

namespace AAC
{
    public partial class LogForm : Form
    {
        /// <summary>
        /// Массив объектов журнала
        /// </summary>
        private List<IELLogElement> LogElements { get; set; }

        /// <summary>
        /// Счётчик скроллбара журнала
        /// </summary>
        private CounterScrollBar ScrollLogElements { get; set; }

        public LogForm()
        {
            InitializeComponent();
            lActiveDir.Text = $"D: <{Directory.GetCurrentDirectory()}>";
            MaximumSize = new(1200, 500);
            MinimumSize = new(1200, 500);
            LogElements = [];
            ScrollLogElements = new(LogElements.Count, 4, LogElements.Count - 4);
            for (int i = 0; i < ObjLog.MassLogElements.Count; i++) AppendLogElement(ObjLog.MassLogElements[i].Text, i);
            ScrollLogElements.Value = ScrollLogElements.MaxValue;
            vsbScrollLogElement.Maximum = ScrollLogElements.MaxValue;
            vsbScrollLogElement.Minimum = 0;
            vsbScrollLogElement.Value = vsbScrollLogElement.Maximum;
            pAllLogElements.Size = new(pAllLogElements.Width, (108 + 8) * LogElements.Count);
            //pIndormationObject.Dispose();
            pAllLogElements.MouseWheel += (sender, e) =>
            {
                if (e.Delta > 0 && ScrollLogElements.Value > 0) ScrollLogElements.Value--;
                else if (e.Delta < 0 && ScrollLogElements.Value < ScrollLogElements.MaxValue) ScrollLogElements.Value++;
                vsbScrollLogElement.Value = ScrollLogElements.Value;
            };
            ObjLog.ResizeMoveLogElements += MovingLogElements;
            ObjLog.AddLogELement += AppendLogElement;
            //ObjLog.ResizeMoveLogElements += ResizeMassLogElements;
            ConstAnimMove Const = new(pAllLogElements.Location.Y, PositionScroll(ScrollLogElements.MaxValue), 10);
            new ConstAnimMove(pAllLogElements.Location.X).InitAnimFormule(pAllLogElements, Formules.QuickTransition, Const, AnimationStyle.XY);
            lScrollValue.Text = $"{ScrollLogElements.Value}/{ScrollLogElements.MaxValue} ({LogElements.Count})";

        }

        private void AppendLogElement(string Text, int Index)
        {
            LogElements.Add(new(new(569, 100), pAllLogElements, Text, Index));
            LogElements[^1].ClickActionButton += () =>
            {
                // тут что будет выполняться при нажатии на кнопку отдельно на каждом объекте журнала
            };
            pAllLogElements.Size = new(pAllLogElements.Width, (108 + 8) * LogElements.Count);
            if (LogElements.Count > ScrollLogElements.CountVisibleElements)
            {
                ScrollLogElements.MaxUp(1);
                vsbScrollLogElement.Maximum++;
                vsbScrollLogElement.Value = vsbScrollLogElement.Maximum;
            }
        }

        private void MovingLogElements(string Text)
        {
            for (int i = 0; i < LogElements.Count - 1; i++) LogElements[i].ObjText = LogElements[i + 1].ObjText;
            LogElements[^1].ObjText = Text;
            if (vsbScrollLogElement.Value == vsbScrollLogElement.Maximum)
            {
                pAllLogElements.Location = new(pAllLogElements.Location.X, PositionScroll(ScrollLogElements.MaxValue - 1));
                ScrollLogElementValueChanged(null, null);
            }
            else vsbScrollLogElement.Value = vsbScrollLogElement.Maximum;
        }

        /// <summary>
        /// Расчитать позицию по индексу скролла
        /// </summary>
        /// <param name="IndexScroll">Индекс скролл-бара</param>
        /// <returns>Расчитанное число позиции</returns>
        private static int PositionScroll(int IndexScroll) => 0 - (IndexScroll * 108);

        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ObjLog.AddLogELement -= AppendLogElement;
            ObjLog.ResizeMoveLogElements -= MovingLogElements;
            //ObjLog.ResizeMoveLogElements -= ResizeMassLogElements;
        }

        /// <summary>
        /// Событие изменения значения скролл-бара напрямую
        /// </summary>
        /// <param name="sender">объект создавший событие</param>
        /// <param name="e">Объект самого события</param>
        private void ScrollLogElementValueChanged(object sender, EventArgs e)
        {
            ScrollLogElements.Value = vsbScrollLogElement.Value;
            ConstAnimMove Const = new(pAllLogElements.Location.Y, PositionScroll(ScrollLogElements.Value), 10);
            new ConstAnimMove(pAllLogElements.Location.X).InitAnimFormule(pAllLogElements, Formules.QuickTransition, Const, AnimationStyle.XY);
            lScrollValue.Text = $"{ScrollLogElements.Value}/{ScrollLogElements.MaxValue} ({LogElements.Count})";
        }
    }
}
