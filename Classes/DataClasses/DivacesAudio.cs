using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Classes.DataClasses
{
    /// <summary>
    /// Описание объекта управления девайсами звука
    /// </summary>
    public class DivacesAudio
    {
        /// <summary>
        /// Список доступных аудио-девайсов
        /// </summary>
        private readonly MMDeviceEnumerator DivacesEnum;

        /// <summary>
        /// Активное устройство воспроизводящее звук
        /// </summary>
        public MMDevice ActiveDevice { get; private set; }

        /// <summary>
        /// Инициализировать объект управления девайсами звука
        /// </summary>
        public DivacesAudio()
        {
            DivacesEnum = new();
            ActiveDevice = DivacesEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        }

        /// <summary>
        /// Изменить активное устройство аудиовывода
        /// </summary>
        /// <returns>Активный девайс воспроизведения звука</returns>
        public void UpdateActivateDivaceAudioOutput()
        {
            ActiveDevice = DivacesEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        }
    }
}
