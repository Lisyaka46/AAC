﻿using System;
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
    /// <param name="AbsolutlyParameter">Обязателен или не обязателен</param>
    public class Parameter(string NameParameter, bool AbsolutlyParameter)
    {
        /// <summary>
        /// Имя параметра команды
        /// </summary>
        public readonly string Name = NameParameter;

        /// <summary>
        /// Значение параметра команды
        /// </summary>
        public readonly bool Absolutly = AbsolutlyParameter;
    }
}
