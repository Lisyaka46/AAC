using AAC.Classes;
using AAC.Classes.Commands;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace AAC
{
    public static class Reading
    {
        /// <summary>
        /// Прочитать базу данных консольных команд и создать массив консольных команд
        /// </summary>
        /// <returns>Массив консольных команд</returns>
        public static ConsoleCommand[] ReadConsoleCommandDataBase()
        {
            /*Описание всех консольных, встроенных команд*/ {
                return
                [
                    new ConsoleCommand("settings", 1, null, "Открывает глобальные настройки"),
                    new ConsoleCommand("clear", 2, null, "Очистка выводимых данных"),
                    new ConsoleCommand("print", 3, [new Parameter("Text", string.Empty)], "Вывод текста на экран"),
                    new ConsoleCommand("reboot", 4, null, "Перезагружает программу"),
                    new ConsoleCommand("close", 5, null, "Закрывает программу"),
                    new ConsoleCommand("close_process", 6, [new Parameter("Process", string.Empty)], "Завершает работу данного процесса"),

                    new ConsoleCommand("cmd", 7,
                    [
                        new Parameter("C1", string.Empty), new Parameter("C2", string.Empty),
                        new Parameter("C3", string.Empty), new Parameter("C4", string.Empty),
                        new Parameter("C5", string.Empty), new Parameter("C6", string.Empty)
                    ], "Открывает командную строку Windows с текущей директорией программы"),

                    new ConsoleCommand("emptytrash", 8, null, "Очищает корзину"),
                    new ConsoleCommand("color", 10, null, ""),
                    new ConsoleCommand("font_size", 11, [new Parameter("Size", string.Empty)], "Изменяет размер выводимого текста в консоли"),
                    new ConsoleCommand("open_link", 12, [new Parameter("Link", string.Empty)], "Открывает в браузере заданную ссылку"),
                    new ConsoleCommand("open_directory", 13, [new Parameter("Directory", string.Empty)], "Открывает заданную директорию в проводнике"),
                    new ConsoleCommand("open_file", 14, [new Parameter("File", string.Empty)], "Открывает файл по его заданной директории"),
                    new ConsoleCommand("create_label", 15, [new Parameter("Name", string.Empty), new Parameter("Directory", string.Empty)], "Создаёт ярлык на рабочем столе с заданным именем на директорию файла"),
                    new ConsoleCommand("help", 16, null, "Открывает окно описания всех команд доступных в программе"),
                    new ConsoleCommand("windows_bat", 17, [new Parameter("Name", string.Empty)], "Активирует BAT файл с заданным именем в определённой директории"),
                    new ConsoleCommand("test", 18, null, "Тестирует программный код для отладки (Выполняйте если только знаете что делаете)"),
                    new ConsoleCommand("log", 21, null, "Открывает окно журнала сообщений программы"),
                    new ConsoleCommand("save_log", 22, null, "Сохраняет сообщения журнала в отдельный файл TXT"),
                    new ConsoleCommand("delete_log", 23, [new Parameter("Name", string.Empty)], "Удаляет отдельный файл сообщений при указании его имени. При отсутствии имени будет предложено удалить все файлы сообщений"),
                    new ConsoleCommand("pythagorean_three", 24, [new Parameter("BaseLength", string.Empty), new Parameter("Catheter I", string.Empty)], "Вычисляет пифогорову тройку по формуле комплекстного числа заданной длинны основания и заданного катета представляемого комплекстным числом"),
                    new ConsoleCommand("colored", 25, null, "Открывает окно редактора цветовых палитр"),
                    new ConsoleCommand("new_label", 26, null, "Открывает окно редактора создания нового ярлыка программы"),
                ];
            }
        }

        /// <summary>
        /// Прочитать базу данных консольных команд и создать массив консольных команд
        /// </summary>
        /// <returns>Массив консольных команд</returns>
        public static VoiceCommand[] ReadVoiceCommandDataBase()
        {
            return [];
        }
    }
}