using WMPLib;

namespace AAC.Classes.DataClasses
{
    /// <summary>
    /// Класс девайса воспроизводящего медиафайлы
    /// </summary>
    /// <remarks>
    /// Инициализировать объект даты воспроизведения Mp3 файлов
    /// </remarks>
    public class MP3()
    {
        private const string Dir = $"Data/Sound";

        /// <summary>
        /// Плеер для звуковых файлов
        /// </summary>
        private WindowsMediaPlayer audio = new()
        {
            URL = string.Empty,
        };

        /// <summary>
        /// Воспроизвести звук
        /// </summary>
        /// <param name="NameSound">Путь к звуковому файлу</param>
        public void PlaySound(string NameSound)
        {
            NameSound = $"{Dir}/{NameSound.Replace(".mp3", string.Empty)}.mp3";
            if (audio.URL.Equals(NameSound)) audio.controls.play();
            else
            {
                if (File.Exists(NameSound))
                {
                    audio.currentMedia = audio.newMedia(NameSound);
                    audio.controls.play();
                }
                else throw new Exception($"Объект воспроизведения звука не найден: <..{NameSound}>");
            }
        }

        /// <summary>
        /// Диструктор объекта звуков
        /// </summary>
        ~MP3()
        {
            audio.close();
        }
    }
}
