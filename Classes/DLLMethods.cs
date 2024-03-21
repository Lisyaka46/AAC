using System;
using System.Collections.Generic;
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
    }
}
