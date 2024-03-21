namespace AAC.Classes
{
    /// <summary>
    /// Буфер консольных команд
    /// </summary>
    /// <remarks>
    /// Инициализировать новый буфер команд
    /// </remarks>
    /// <param name="CountBuffer">Количество сохраняемых команд в буфер</param>
    public class Buffer(int CountBuffer = 50)
    {
        /// <summary>
        /// Массив элементов буфера
        /// </summary>
        private string[] BufferElements = new string[Math.Clamp(CountBuffer, 4, 80)];

        /// <summary>
        /// Количество добавленных команд
        /// </summary>
        public int Count { get; private set; } = 0;

        /// <summary>
        /// Общее количество мест в буфере
        /// </summary>
        public int Length => BufferElements.Length;

        /// <summary>
        /// Индексатор буфера элементов
        /// </summary>
        /// <param name="key">Индекс читаемого элемента</param>
        /// <returns>Прочитанный текст элемента</returns>
        /// <exception cref="IndexOutOfRangeException">Исключение выхода индекса за границы буфера</exception>
        public string this[Index key]
        {
            get
            {
                if (key.Value < Length) return BufferElements[key];
                else throw new IndexOutOfRangeException($"Индекс ({key}) вышел за рамки буфера ({Length})");
            }
            private set
            {
                if (key.Value < Length) BufferElements[key] = value;
                else throw new IndexOutOfRangeException($"Индекс ({key}) вышел за рамки буфера ({Length})");
            }
        }

        /// <summary>
        /// Удалить <b>все</b> элементы буфера
        /// </summary>
        public void DeleteAll()
        {
            if (Count > 0)
            {
                BufferElements = new string[BufferElements.Length];
                Count = 0;
            }
        }

        /// <summary>
        /// Добавить элемент в буфер <b></b>
        /// </summary>
        /// <remarks>
        /// При переполнении самый первый элемент удаляется и добавляется текущий
        /// </remarks>
        /// <param name="Text">Текст элемента буфера</param>
        public bool Add(string Text)
        {
            if (BufferElements.Contains(Text)) return false;
            if (Count < BufferElements.Length - 1) this[++Count] = Text;
            else
            {
                BufferElements = [..BufferElements.Skip(1)];
                this[^1] = Text;
            }
            return true;
        }
    }
}
