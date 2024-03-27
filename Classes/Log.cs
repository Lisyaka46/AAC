using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Classes
{
    /// <summary>
    /// Описание объекта журнала
    /// </summary>
    /// <remarks>
    /// Инициализировать объект журнала
    /// </remarks>
    /// <param name="MaxLength">Максимальная вместимость журнала</param>
    public class Log(int MaxLength)
    {
        /// <summary>
        /// Инициализировать объект журнала
        /// </summary>
        /// <param name="Text">Текст объекта журнала</param>
        public class ObjLogInfo(string Text)
        {
            /// <summary>
            /// Текст объекта журнала
            /// </summary>
            public string Text { get; } = $"{DateTime.Now:dd/MM/yyyy} {DateTime.Now:HH/mm/ss/fff} >> {Text}";

            /// <summary>
            /// Время создания объекта журнала
            /// </summary>
            public DateTime DateTime { get; } = DateTime.Now;
        }

        /// <summary>
        /// Массив объектов журнала
        /// </summary>
        public List<ObjLogInfo> MassLogElements { get; private set; } = [];

        /// <summary>
        /// Максимальная вместимость объектов журнала
        /// </summary>
        public int MaxLength { get; } = MaxLength;

        /// <summary>
        /// Делегат события добавления объекта в журнал
        /// </summary>
        /// <param name="Text">Добавляемые данные</param>
        /// <param name="index">Индексированая позиция объекта журнала</param>
        public delegate void AppendLogElement(string Text, int index);

        /// <summary>
        /// Делегат события смещения объектов журнала
        /// </summary>
        /// <param name="Text">Добавляемые данные</param>
        public delegate void MovingLogElements(string Text);

        /// <summary>
        /// Событие добавления объекта в журнал
        /// </summary>
        public event AppendLogElement? AddLogELement;

        /// <summary>
        /// Событие смещения объектов журнала
        /// </summary>
        public event MovingLogElements? ResizeMoveLogElements;

        /// <summary>
        /// Создать объект журнала
        /// </summary>
        /// <param name="Text">Добавляемый текст</param>
        public void LOGTextAppend(string Text)
        {
            if (MassLogElements.Count == MaxLength) ResizeMoveLogElements?.Invoke(MassLogElements[^1].Text);
            else if (MassLogElements.Count < MaxLength)
            {
                MassLogElements.Add(new(Text));
                AddLogELement?.Invoke(MassLogElements[^1].Text, MassLogElements.Count - 1);
            }
        }
    }
}
