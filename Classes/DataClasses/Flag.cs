using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Classes.DataClasses
{
    /// <summary>
    /// Статус обычного Флага
    /// </summary>
    public enum StatusFlags
    {

        /// <summary>
        /// Не активен
        /// </summary>
        NotActive = 0,

        /// <summary>
        /// Активен
        /// </summary>
        Active = 1,

        /// <summary>
        /// Состояние спящего режима или состояние проверки точного состояния
        /// </summary>
        Sleep = 2
    }

    /// <summary>
    /// Статус Флага диалогового окна
    /// </summary>
    public enum DialogWindowStatus
    {

        /// <summary>
        /// Отмена
        /// </summary>
        Cancel = 0,

        /// <summary>
        /// Одобрено
        /// </summary>
        Ok = 1
    }

    /// <summary>
    /// Статус булевого Флага
    /// </summary>
    public enum BooleanFlags
    {

        /// <summary>
        /// Отрицание Флага
        /// </summary>
        False = 0,

        /// <summary>
        /// Одобрение Флага
        /// </summary>
        True = 1
    }

    /// <summary>
    /// Описание структуры объекта флага
    /// </summary>
    /// <remarks>
    /// Инициализировать объект флага
    /// </remarks>
    /// <param name="Value">Стартовое значение</param>
    public class Flag(bool Value)
    {
        /// <summary>
        /// Делегат события изменения состояния флага
        /// </summary>
        /// <param name="SetBool">Состояние изменённого флага</param>
        public delegate void EventChangeStateFlag(bool SetBool);

        /// <summary>
        /// Событие изменения состояния флага
        /// </summary>
        public event EventChangeStateFlag? ChangeStateFlag;

        /// <summary>
        /// Ресурсное значение флага
        /// </summary>
        private bool _Value = Value;

        /// <summary>
        /// Видимое значение флага
        /// </summary>
        public bool Value
        {
            get => _Value;
            set
            {
                _Value = value;
                ChangeStateFlag?.Invoke(_Value);
            }
        }
    }
}
