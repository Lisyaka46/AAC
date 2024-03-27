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
    /// Специальный цвет RGBCC
    /// </summary>
    /// <remarks>
    /// Инициализировать объект управления специальным цветом RGBCC
    /// </remarks>
    /// <param name="SettringsRGBCC">Параметр который управляет обновлениями специального цвета</param>
    public class ColorRGBCC(SettingsBoolParameter SettringsRGBCC)
    {
        /// <summary>
        /// Список элементов под воздействием постоянного изменения цвета с помощью курсора
        /// </summary>
        private List<dynamic> RGBCCLabel { get; set; } = [];

        /// <summary>
        /// Текущий цвет объекта специального цвета
        /// </summary>
        public Color RealyColor { get; set; } = Color.FromArgb(0, 0, 0);

        /// <summary>
        /// Объект управляющий обновлениями
        /// </summary>
        private SettingsData.SettingsBoolParameter Parameter { get; } = SettringsRGBCC;

        /// <summary>
        /// Добавить элемент под контроль специального цвета
        /// </summary>
        /// <param name="Element">Добавляемый элемент</param>
        /// <exception cref="ArgumentException">Ошибка при которой элемент не имеет свойство ForeColor</exception>
        public void AddElement(dynamic Element)
        {
            try { Element.ForeColor = RealyColor; RGBCCLabel.Add(Element); }
            catch { throw new ArgumentException("Argument invalid {Element}-try-.ForeColor ADD(RGBCC)"); }
        }

        /// <summary>
        /// Постоянный цикл обновления RGBCC текста
        /// </summary>
        public async void StartUpdate()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (Parameter.Value && Apps.MainForm.WindowState == FormWindowState.Normal)
                    {
                        RealyColor = Color.FromArgb(
                            Convert.ToInt32(Math.Abs((Math.Atan(Cursor.Position.X) - Cursor.Position.Y) / 5) % 256d),
                            Convert.ToInt32(Math.Abs((Math.Cos(Cursor.Position.Y) - Cursor.Position.X) / 10) % 256d),
                            Convert.ToInt32(Math.Abs((1080d - Math.Cos(Cursor.Position.X)) * 2) % 256d)
                            );
                        RGBCCLabel.ForEach(c =>
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
