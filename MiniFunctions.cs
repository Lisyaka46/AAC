using AAC.Classes;
using Microsoft.Win32;
using System.Diagnostics;
using static AAC.Classes.AnimationDL.Animate.AnimFormule;
using static AAC.Startcs;

namespace AAC
{
    public static class MiniFunctions
    {

        /// <summary>
        /// Цифры в текст
        /// </summary>
        /// <param name="InputNumber">Число больше нуля переводимое в текст</param>
        /// <param name="RoundUpTen">Округлять до десяти (False?/True)</param>
        /// <returns>string: Текст цыфр</returns>
        public static string StringNumberConvert(int InputNumber, bool RoundUpTen = false)
        {
            string OutputText = "?";
            string[] NumberText10 = ["Ноль", "Один", "Два", "Три", "Четыре", "Пять", "Шесть", "Семь", "Восемь", "Девять", "Десять"];
            string[] NumberTextPre11 = ["Один", "Две", "Три", "Четыр", "Пят", "Шест", "Сем", "Восем", "Девят"];
            string[] NumberTextStart100 = ["Двадцать", "Тридцать", "Сорок", "Пятьдесят", "Шестьдесят", "Семьдесят", "Восемьдесят", "Девяносто"];
            if (InputNumber > -1)
            {
                if (InputNumber < 11)
                    OutputText = NumberText10[InputNumber];
                else if (InputNumber >= 11 && InputNumber < 20)
                    OutputText = NumberTextPre11[InputNumber % 11] + "надцать";
                else if (InputNumber >= 20 && InputNumber < 100)
                {
                    OutputText = NumberTextStart100[InputNumber / 10 - 2];
                    if (InputNumber % 10 > 0 && !RoundUpTen)
                        OutputText += " " + NumberText10[InputNumber % 10];
                }
                else
                    return OutputText;
            }
            return OutputText;
        }

        /// <summary>
        /// Выполнить команду shell по индификатору indicator
        /// </summary>
        /// <param name="indicator"> Названия выполняющего программного действия shell</param>
        public static void ShellGUID(string indicator)
        {
            string id_ = string.Empty;
            switch (indicator)
            {
                case "DesktopVisualTrue":
                    id_ = "3080F90D-D7AD-11D9-BD98-0000947B0257";
                    break;
                case "CommandPanelWin":
                    id_ = "21EC2020-3AEA-1069-A2DD-08002B30309D";
                    break;
                case "Test":
                    id_ = "BB06C0E4-D293-4f75-8A90-CB05B6477EEE";
                    break;
                case null:
                    return;
            }
            Process.Start("explorer.exe", @"shell:::{" + id_ + @"}");
            MainData.MainMP3.PlaySound("Complete");
        }

        /// <summary>
        /// Присутствуют ли буквы в Text
        /// </summary>
        /// <param name="Text">Проверяемый текст</param>
        /// <returns>true: Присутствуют символы | false: Только цифры</returns>
        public static bool Stringint(string Text)
        {
            for (int i = 0; i < Text.Length; i++)
            {
                if (Convert.ToChar(Text[i]) < '0' || Convert.ToChar(Text[i]) > '9')
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Удалить из построчного текста InputText одну строку
        /// </summary>
        /// <param name="InputText">Текст с отступами \n</param>
        /// <returns>string: Текст без одной строки сверху</returns>
        public static string UpdateLineOutput(string InputText)
        {
            bool WriteUpdate = false;
            string OutputText = string.Empty;
            for (int i = 0; i < InputText.Length; i++)
            {
                if (Convert.ToString(InputText[i]).Equals("\n") && !WriteUpdate)
                    WriteUpdate = true;
                else if (WriteUpdate)
                    OutputText += InputText[i];
            }
            return OutputText;
        }

        /// <summary>
        /// Активировать диалоговое окно подтверждения действия
        /// </summary>
        /// <param name="InfoAction">Текст выводимый сообщением об подтверждении</param>
        /// <param name="NameAction">Имя подтверждения действия</param>
        public static void ActivateActionDialog(string NameAction, string InfoAction)
        {
            MainData.MainMP3.PlaySound("Question");
            DialogResult Out = MessageBox.Show(InfoAction, NameAction, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button3);
            MainData.Flags.ResultConfirmationAction = Out == DialogResult.Cancel ? Data.DialogWindowStatus.Cancel : Data.DialogWindowStatus.Ok;
        }

        /// <summary>
        /// Воспроизвести цифры времени
        /// </summary>
        public static async void TimeTempus()
        {
            int hour = Convert.ToInt32(DateTime.Now.ToString("HH")), hour10 = hour % 10;
            int min = Convert.ToInt32(DateTime.Now.ToString("mm")), min10 = min % 10;
            await Task.Run(() =>
            {
                MainData.MainMP3.PlaySound($"Clock\\{StringNumberConvert(hour, true)}");
                if (hour >= 20)
                {
                    Thread.Sleep(500);
                    MainData.MainMP3.PlaySound($"Clock\\{StringNumberConvert(hour10)}");
                }
                Thread.Sleep(200);

                if (min == 1 || min == 2)
                {
                    Thread.Sleep(285);
                    MainData.MainMP3.PlaySound($"Clock\\{StringNumberConvert(min, true)}МИН");
                }
                else
                {
                    Thread.Sleep(285);
                    MainData.MainMP3.PlaySound($"Clock\\{StringNumberConvert(min, true)}");
                }
                if (min >= 20)
                {
                    Thread.Sleep(535);
                    if (min10 == 1 || min10 == 2)
                        MainData.MainMP3.PlaySound($"Clock\\{StringNumberConvert(min10)}МИН");
                    else
                        MainData.MainMP3.PlaySound($"Clock\\{StringNumberConvert(min10)}");
                }
            });
        }

        /// <summary>
        /// Произвести переход между панелями
        /// </summary>
        /// <param name="OpenPanel">Выезжающая панель</param>
        /// <param name="ClosePanel">Уезжающая панель</param>
        public static void OpenNewMiniMenu(Panel OpenPanel, Panel ClosePanel, int ConstY, DirectionsParties Direction = DirectionsParties.Right, int y = 9)
        {
            int Offset = 4;
            ConstAnimMove FormuleOpenPanel, FormuleClosePanel;
            if (Direction == DirectionsParties.Right)
                OpenPanel.Location = new(OpenPanel.Size.Width + Offset, ConstY);
            else OpenPanel.Location = new(-OpenPanel.Size.Width - Offset, ConstY);
            FormuleOpenPanel = new(OpenPanel.Location.X, 0, y);
            if (Direction == DirectionsParties.Right)
                FormuleClosePanel = new(ClosePanel.Location.X, -ClosePanel.Size.Width - Offset, y);
            else FormuleClosePanel = new(ClosePanel.Location.X, ClosePanel.Size.Width + Offset, y);
            OpenPanel.BringToFront();
            FormuleOpenPanel.InitAnimFormule(OpenPanel, Formules.QuickTransition, new ConstAnimMove(ConstY), AnimationStyle.XY);
            FormuleClosePanel.InitAnimFormule(ClosePanel, Formules.QuickTransition, new ConstAnimMove(ConstY), AnimationStyle.XY);
        }

        /// <summary>
        /// Обновить значения VScrollBar
        /// </summary>
        /// <param name="vScrollBar">Обновляемый скроллбар</param>
        /// <param name="Delta">Сторона изменения</param>
        public static void UpdateVScrollBar(VScrollBar vScrollBar, int Delta)
        {
            if (vScrollBar.Visible)
            {
                if (Delta > 0)
                    vScrollBar.Value = vScrollBar.Value - vScrollBar.SmallChange > vScrollBar.Minimum ?
                        vScrollBar.Value - vScrollBar.SmallChange : vScrollBar.Minimum;
                else if (Delta < 0)
                    vScrollBar.Value = vScrollBar.Value + vScrollBar.SmallChange < vScrollBar.Maximum ?
                        vScrollBar.Value + vScrollBar.SmallChange : vScrollBar.Maximum;
            }
        }

        /// <summary>
        /// Переместить MoveElement по значению VScrollBar
        /// </summary>
        /// <param name="vScrollBar">Обновляемый скроллбар</param>
        /// <param name="MoveElement">Движущийся элемент</param>
        public static void MoveElementinVScrollBar(Panel MoveElement, VScrollBar vScrollBar, int OffsetY = 0)
        {
            MoveElement.Location = new(MoveElement.Location.X, OffsetY - vScrollBar.Value);
        }

        /// <summary>
        /// Расчитать по данному элементу границы и интенсивность прокрутки скроллбара
        /// </summary>
        /// <param name="ElementEndScrollBar">Последний доступный элемент для прокрутки</param>
        /// <param name="ScrollPanel">Панель которую прокручивает скроллбар</param>
        /// <param name="ScrollEndPanel">Панель ограничивающая прокрутку</param>
        /// <param name="vScrollBar">Скроллбар для прокрутки</param>
        /// <param name="OffsetY">Оффсет прокрутки</param>
        /// <param name="ResizeScrollPanel">Изменять ли размеры прокручивающейся панели</param>
        public static void CalculationDownElementScrollBar(dynamic ElementEndScrollBar, Panel ScrollPanel, Panel ScrollEndPanel, VScrollBar vScrollBar, int OffsetY = 0, bool ResizeScrollPanel = true)
        {
            if (ElementEndScrollBar.Location.Y + ElementEndScrollBar.Size.Height > ScrollEndPanel.Size.Height)
            {
                vScrollBar.Minimum = 0;
                int Offset = ElementEndScrollBar.Location.Y + ElementEndScrollBar.Size.Height + OffsetY - ScrollEndPanel.Size.Height + 4;
                vScrollBar.Maximum = Offset;
                vScrollBar.SmallChange = ScrollPanel.Size.Height / 2;
                vScrollBar.Show();
                if (ResizeScrollPanel) ScrollPanel.Size = new(ScrollPanel.Size.Width, ElementEndScrollBar.Location.Y + ElementEndScrollBar.Size.Height + 4);
            }
            else vScrollBar.Hide();
        }
    }
}
