using System.CodeDom;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static AAC.Forms_Functions;
using AAC.Classes;
using static AAC.Classes.AnimationDL.Animate.AnimFormule;

namespace AAC
{
    public static partial class Startcs
    {
        /// <summary>
        /// Объект управления объектами журнала
        /// </summary>
        public static readonly Log ObjLog = new(50);

        /// <summary>
        /// Объект главной даты программы
        /// </summary>
        public static readonly Data MainData = new();

        /// <summary>
        /// Класс всех форм
        /// </summary>
        public static readonly Applications Apps = new();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Starting();
        }

        private static void Starting()
        {
            try
            {
                Apps.Starting = new();
                MainData.MainMP3.PlaySound("StartingSound");
                /*Task.Run(() =>
                {
                    MainData.MainMP3.PlaySound("StartingSound");
                    Application.Run(Apps.Starting);
                });*/

                Apps.MainForm = new();
                Task.Run(() =>
                {
                    while (Apps.Starting.Opacity > 0d)
                    {
                        Apps.Starting.Opacity -= 0.009d;
                        Thread.Sleep(1);
                    }
                    Apps.Starting.Opacity = 0d;
                    Apps.Starting.Visible = false;
                    Apps.Starting.Close();
                });

                ObjLog.LOGTextAppend("Программа активируется");

                Task.Run(Apps.MainForm.AlwaysUpdateWindow);
                MainData.AllSpecialColor.RGB.StartUpdate();
                MainData.AllSpecialColor.RGBCC.StartUpdate();
                MainData.AllSpecialColor.SC.StartUpdate();
                Apps.MainForm.LActiveitedSoftCommand_Click(null, null);

                Application.Run(Apps.MainForm);
            }
            catch (Exception ex)
            {
                StackFrame[] Frames = new StackTrace(ex, true).GetFrames();
                ObjLog.LOGTextAppend($"** Исключение старта программы");
                for (int i = 0; i < Frames.Length; i++)
                {
                    ObjLog.LOGTextAppend($"{i}. Файл {Frames[i].GetFileName() ?? "??"} <{Frames[i].GetFileLineNumber()}/{Frames[i].GetFileColumnNumber()}>" +
                        $"\nText: {Frames[i]}");
                }
                Apps.Log = new();
                Application.Run(Apps.Log);
            }
        }
    }
}
