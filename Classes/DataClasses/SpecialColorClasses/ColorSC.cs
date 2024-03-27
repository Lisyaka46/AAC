using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AAC.Classes.DataClasses.SettingsData;
using static AAC.Startcs;

namespace AAC.Classes.DataClasses.SpecialColorClasses
{
    /// <summary>
    /// Специальный цвет SC
    /// </summary>
    /// <remarks>
    /// Инициализировать объект специального цвета SC
    /// </remarks>
    /// <param name="ParamSC">Параметр от которого будет зависеть специальный цвет</param>
    /// <param name="Actient">Статический акцент специального цвета</param>
    public class ColorSC(SettingsBoolParameter ParamSC, Color Actient)
    {
        /// <summary>
        /// Список элементов под воздействием постоянного изменения цвета с помощью звука
        /// </summary>
        private readonly List<dynamic> SCLabel = [];

        /// <summary>
        /// Текущий цвет объекта специального цвета
        /// </summary>
        public Color RealyColor { get; set; }

        /// <summary>
        /// Статический акцент специального цвета
        /// </summary>
        public Color ActientColorSC { get; set; } = Actient;

        /// <summary>
        /// Параметр от которого зависит специальный цвет
        /// </summary>
        private readonly SettingsBoolParameter Parameter = ParamSC;

        /// <summary>
        /// Добавить элемент под контроль специального цвета
        /// </summary>
        /// <param name="Element">Добавляемый элемент</param>
        /// <exception cref="ArgumentException">Ошибка при которой элемент не имеет свойство ForeColor</exception>
        public void AddElement(dynamic Element)
        {
            try { Element.ForeColor = RealyColor; SCLabel.Add(Element); }
            catch { throw new ArgumentException("Argument invalid {Element}-try-.ForeColor ADD(SC)"); }
        }

        /// <summary>
        /// Обновить цвет SC цвета
        /// </summary>
        public async void StartUpdate()
        {
            MMDeviceEnumerator Device = new();
            float PointVolume;
            //int FullAudio255, ColorRatio, MoveIndicatorOffset1, MoveIndicatorOffset2;
            int R, G, B;
            await Task.Run(() =>
            {
                while (true)
                {
                    if (Parameter.Value && Apps.MainForm.WindowState == FormWindowState.Normal)
                    {
                        PointVolume = (int)(Device.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).AudioMeterInformation.MasterPeakValue * 100);
                        R = (int)(2.55f * PointVolume + ActientColorSC.R);
                        G = (int)(2.55f * PointVolume + ActientColorSC.G);
                        B = (int)(2.55f * PointVolume + ActientColorSC.B);
                        RealyColor = Color.FromArgb(
                            R > 255 ? 255 - (R - 255) : R,
                            G > 255 ? 255 - (G - 255) : G,
                            B > 255 ? 255 - (B - 255) : B);
                        SCLabel.ForEach(c =>
                        {
                            try { c.ForeColor = RealyColor; }
                            catch { }
                        });
                    }
                    Thread.Sleep(10);
                }
            });
        }
    }
}
