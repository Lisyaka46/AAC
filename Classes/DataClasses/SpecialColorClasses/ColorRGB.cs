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
    /// Специальный цвет RGB
    /// </summary>
    /// <remarks>
    /// Инициализировать объект управления специальным цветом RGB
    /// </remarks>
    /// <param name="SettringsRGB">Параметр который управляет обновлениями специального цвета</param>
    public class ColorRGB(SettingsBoolParameter SettringsRGB)
    {
        /// <summary>
        /// Лист элементов под воздействием постоянного изменения цвета
        /// </summary>
        private readonly List<dynamic> RGBLabel = [];

        /// <summary>
        /// Текущий цвет специального цвета
        /// </summary>
        public Color RealyColor { get; private set; } = Color.FromArgb(255, 0, 0);

        /// <summary>
        /// Объект управляющий обновлениями
        /// </summary>
        private readonly SettingsData.SettingsBoolParameter Parameter = SettringsRGB;

        /// <summary>
        /// Добавить элемент под контроль специального цвета
        /// </summary>
        /// <param name="Element">Добавляемый элемент</param>
        /// <exception cref="ArgumentException">Ошибка при которой элемент не имеет свойство ForeColor</exception>
        public void AddElement(dynamic Element)
        {
            try { Element.ForeColor = RealyColor; RGBLabel.Add(Element); }
            catch { throw new ArgumentException("Argument invalid {Element}-try-.ForeColor ADD(RGB)"); }
        }

        /// <summary>
        /// Постоянный цикл обновления RGB текста
        /// </summary>
        public async void StartUpdate()
        {
            int i = 0;
            await Task.Run(() =>
            {
                while (true)
                {
                    if (Parameter.Value && Apps.MainForm.WindowState == FormWindowState.Normal)
                    {
                        switch (i)
                        {
                            case 0:
                                RealyColor = Color.FromArgb(RealyColor.R - 1, RealyColor.G + 1, RealyColor.B);
                                break;
                            case 1:
                                RealyColor = Color.FromArgb(RealyColor.R, RealyColor.G - 1, RealyColor.B + 1);
                                break;
                            case 2:
                                RealyColor = Color.FromArgb(RealyColor.R + 1, RealyColor.G, RealyColor.B - 1);
                                break;
                        }
                        if (RealyColor.R == 255 || RealyColor.G == 255 || RealyColor.B == 255) i = (i + 1) % 3;
                        RGBLabel.ForEach(c =>
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
