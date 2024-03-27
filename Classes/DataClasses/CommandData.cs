using AAC.Classes.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Classes.DataClasses
{
    /// <summary>
    /// Класс данных команд
    /// </summary>
    public class CommandData
    {
        /// <summary>
        /// Массив консольных команд
        /// </summary>
        public readonly ConsoleCommand[] MassConsoleCommand;

        /// <summary>
        /// Массив голосовых команд
        /// </summary>
        public readonly VoiceCommand[] MassVoiceCommand;

        /// <summary>
        /// Инициализировать объект данных команд
        /// </summary>
        /// <param name="MCC">Консольные команды</param>
        /// <param name="MVC">Голосовые команды</param>
        internal CommandData(ConsoleCommand[] MCC, VoiceCommand[] MVC)
        {
            MassConsoleCommand = MCC;
            MassVoiceCommand = MVC;
        }
    }
}
