using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using WMPLib;
using static AAC.Startcs;

namespace AAC.Classes.DataClasses
{
    /// <summary>
    /// Класс девайса воспроизводящего медиафайлы
    /// </summary>
    public class MP3
    {
        private const string Dir = $"Data/Sound";


        /// <summary>
        /// Количество каналов воспроизведения
        /// </summary>
        public readonly int CountChannelMP;

        /// <summary>
        /// Плеер для mp3 файлов
        /// </summary>
        private readonly List<WindowsMediaPlayer> DoMP3player;

        /// <summary>
        /// Свойство индекса канала плеера
        /// </summary>
        public int ActivityChannelMP { get; private set; }

        /// <summary>
        /// Инициализировать объект даты воспроизведения Mp3 файлов
        /// </summary>
        /// <param name="CountChannel">Количество доступных каналов</param>
        public MP3(int CountChannel)
        {
            CountChannelMP = CountChannel;
            DoMP3player = [];
            ActivityChannelMP = 0;
            for (int i = 0; i < CountChannelMP; i++) DoMP3player.Add(new());
        }

        /// <summary>
        /// Воспроизвести звук
        /// </summary>
        /// <param name="NameSound">Путь к звуковому файлу</param>
        public void PlaySound(string NameSound)
        {
            NameSound = NameSound.Replace(".mp3", string.Empty);
            if (File.Exists($"{Dir}/{NameSound}.mp3"))
            {
                try
                {
                    DoMP3player[ActivityChannelMP] = new()
                    {
                        URL = $"{Dir}/{NameSound}.mp3"
                    };
                    DoMP3player[ActivityChannelMP].controls.play();
                }
                catch { }
                ActivityChannelMP = (ActivityChannelMP + 1) % CountChannelMP;
            }
            else
            {
                ObjLog.LOGTextAppend($"Файл воспроизведения <..Data/Mp3/{NameSound}.mp3> не найден..");
            }
        }
    }
}
