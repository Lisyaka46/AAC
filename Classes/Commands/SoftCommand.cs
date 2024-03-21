namespace AAC.Classes.Commands
{
    /// <summary>
    /// Команда Soft
    /// </summary>
    public class SoftCommand
    {
        /// <summary>
        /// Имя файла откуда прочиталась команда Soft
        /// </summary>
        public string NameFile { get; }

        /// <summary>
        /// Имя команды Soft
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Параметр Voice - голосовая команда
        /// </summary>
        public string Voice { get; set; }

        /// <summary>
        /// Параметр Print - Имитация вывода данных
        /// </summary>
        public string Print { get; set; }

        /// <summary>
        /// Параметр Explanation - Описание команды Soft
        /// </summary>
        public string Explanation { get; set; }

        /// <summary>
        /// Параметр SoundComplete - Воспроизведение сигнала об успешно выполненой команде
        /// </summary>
        public bool SoundComplete { get; set; }

        /// <summary>
        /// Параметр Action - Логика действий в команде
        /// </summary>
        public SoftAction Action { get; set; }

        /// <summary>
        /// Класс логики действий в команде Soft
        /// </summary>
        public class SoftAction
        {
            /// <summary>
            /// Имя системной команды
            /// </summary>
            public List<string> Name { get; set; }

            /// <summary>
            /// Параметры для системной команды
            /// </summary>
            public List<string> Parameteres { get; set; }

            /// <summary>
            /// Инициализировать пустую логику действий команды Soft
            /// </summary>
            public SoftAction()
            {
                Name = [];
                Parameteres = [];
            }
        }

        /// <summary>
        /// Инициализировать пустую инструкцию команды Soft
        /// </summary>
        /// <param name="nameFile">Имя файла откуда прочиталась Soft команда</param>
        public SoftCommand(string nameFile)
        {
            Name = string.Empty;
            Voice = string.Empty;
            Print = string.Empty;
            Explanation = string.Empty;
            SoundComplete = true;
            Action = new SoftAction();
            NameFile = nameFile;
        }

        /// <summary>
        /// Пустой объект Soft команды
        /// </summary>
        public static SoftCommand Empty { get; } = new(string.Empty);
    }
}
