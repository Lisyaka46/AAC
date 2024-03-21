using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AAC.Classes.DataClasses.SettingsData;
using AAC.Classes.DataClasses.SpecialColorClasses;

namespace AAC.Classes.DataClasses
{
    /// <summary>
    /// Параметры для управления специальными цветами
    /// </summary>
    /// <remarks>
    /// Инициализировать объект управления специальными цветами
    /// </remarks>
    public class SpecialColor(SettingsBoolParameter ParamRGB, SettingsBoolParameter ParamRGBCC, SettingsBoolParameter ParamSC, Color Acient)
    {
        /// <summary>
        /// Объект специального цвета RGB
        /// </summary>
        public readonly ColorRGB RGB = new(ParamRGB);

        /// <summary>
        /// Объект специального цвета RGBCC
        /// </summary>
        public readonly ColorRGBCC RGBCC = new(ParamRGBCC);

        /// <summary>
        /// Объект специального цвета SC
        /// </summary>
        public readonly ColorSC SC = new(ParamSC, Acient);

    }
}
