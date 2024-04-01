using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AAC.Classes
{
    public static partial class DLLMethods
    {
        /// <summary>
        /// Блокировка экрана
        /// </summary>
        /// <returns>Состояние блокировки</returns>
        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool LockWorkStation();

        /// <summary>
        /// Перечистение флагов взаимодействия с корзиной
        /// </summary>
        public enum RecycleFlags : uint
        {
            SHERB_NOCONFIRMATION = 0x00000001,
            SHERB_NOPROGRESSUI = 0x00000002,
            SHERB_NOSOUND = 0x00000004
        }

        /// <summary>
        /// Очистка корзины
        /// </summary>
        /// <param name="hwnd">Код удаления всех файлов из корзины</param>
        /// <param name="pszRootPath">Директория Root папки корзины</param>
        /// <param name="dwFlags">Флаг удаления файлов</param>
        /// <returns>Ответный код действия</returns>
        [LibraryImport("Shell32.dll", StringMarshalling = StringMarshalling.Utf16)]
        public static partial uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

        /// <summary>
        /// Выполнить команду shell по индификатору indicator
        /// </summary>
        /// <param name="indicator"> Названия выполняющего программного действия shell</param>
        public static bool ShellGUID(string indicator)
        {
            string id_;
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
                default:
                    return false;
            }
            Process.Start("explorer.exe", @"shell:::{" + id_ + @"}");
            return true;
        }

        /// <summary>
        /// Узнать состояние подключения к интернету
        /// </summary>
        /// <param name="lpszUrl"></param>
        /// <param name="dwFlags"></param>
        /// <param name="dwReserved"></param>
        /// <returns>Bool состояние</returns>
        [LibraryImport("wininet.dll", EntryPoint = "InternetCheckConnectionW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool InternetCheckConnection(string lpszUrl, int dwFlags, int dwReserved);
    }
}
