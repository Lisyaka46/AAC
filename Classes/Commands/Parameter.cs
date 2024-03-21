using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Classes.Commands
{
    /// <summary>
    /// Параметер команды
    /// </summary>
    /// <remarks>
    /// Инициализировать объект параметра команды
    /// </remarks>
    /// <param name="NameParameter">Имя параметра</param>
    /// <param name="value">Значение параметра</param>
    public class Parameter(string NameParameter, string value)
    {
        /// <summary>
        /// Имя параметра команды
        /// </summary>
        public readonly string Name = NameParameter;

        /// <summary>
        /// Значение параметра команды
        /// </summary>
        public string Value { get; set; } = value;
    }
}
