using AAC.Classes;
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
        public static TypeCommand.ConsoleCommand[] ReadConsoleCommandDataBase()
        {
            /*Описание всех консольных, встроенных команд*/ {
                return
                [
                    new TypeCommand.ConsoleCommand("settings", 1, null, "Открывает глобальные настройки"),
                    new TypeCommand.ConsoleCommand("clear", 2, null, "Очистка выводимых данных"),
                    new TypeCommand.ConsoleCommand("print", 3, [new TypeCommand.Parameter("Text", string.Empty)], "Вывод текста на экран"),
                    new TypeCommand.ConsoleCommand("reboot", 4, null, "Перезагружает программу"),
                    new TypeCommand.ConsoleCommand("close", 5, null, "Закрывает программу"),
                    new TypeCommand.ConsoleCommand("close_process", 6, [new TypeCommand.Parameter("Process", string.Empty)], "Завершает работу данного процесса"),

                    new TypeCommand.ConsoleCommand("cmd", 7,
                    [
                        new TypeCommand.Parameter("C1", string.Empty), new TypeCommand.Parameter("C2", string.Empty),
                        new TypeCommand.Parameter("C3", string.Empty), new TypeCommand.Parameter("C4", string.Empty),
                        new TypeCommand.Parameter("C5", string.Empty), new TypeCommand.Parameter("C6", string.Empty)
                    ], "Открывает командную строку Windows с текущей директорией программы"),

                    new TypeCommand.ConsoleCommand("emptytrash", 8, null, "Очищает корзину"),
                    new TypeCommand.ConsoleCommand("color", 10, null, ""),
                    new TypeCommand.ConsoleCommand("font_size", 11, [new TypeCommand.Parameter("Size", string.Empty)], "Изменяет размер выводимого текста в консоли"),
                    new TypeCommand.ConsoleCommand("open_link", 12, [new TypeCommand.Parameter("Link", string.Empty)], "Открывает в браузере заданную ссылку"),
                    new TypeCommand.ConsoleCommand("open_directory", 13, [new TypeCommand.Parameter("Directory", string.Empty)], "Открывает заданную директорию в проводнике"),
                    new TypeCommand.ConsoleCommand("open_file", 14, [new TypeCommand.Parameter("File", string.Empty)], "Открывает файл по его заданной директории"),
                    new TypeCommand.ConsoleCommand("create_label", 15, [new TypeCommand.Parameter("Name", string.Empty), new TypeCommand.Parameter("Directory", string.Empty)], "Создаёт ярлык на рабочем столе с заданным именем на директорию файла"),
                    new TypeCommand.ConsoleCommand("help", 16, null, "Открывает окно описания всех команд доступных в программе"),
                    new TypeCommand.ConsoleCommand("windows_bat", 17, [new TypeCommand.Parameter("Name", string.Empty)], "Активирует BAT файл с заданным именем в определённой директории"),
                    new TypeCommand.ConsoleCommand("test", 18, null, "Тестирует программный код для отладки (Выполняйте если только знаете что делаете)"),
                    new TypeCommand.ConsoleCommand("log", 21, null, "Открывает окно журнала сообщений программы"),
                    new TypeCommand.ConsoleCommand("save_log", 22, null, "Сохраняет сообщения журнала в отдельный файл TXT"),
                    new TypeCommand.ConsoleCommand("delete_log", 23, [new TypeCommand.Parameter("Name", string.Empty)], "Удаляет отдельный файл сообщений при указании его имени. При отсутствии имени будет предложено удалить все файлы сообщений"),
                    new TypeCommand.ConsoleCommand("pythagorean_three", 24, [new TypeCommand.Parameter("BaseLength", string.Empty), new TypeCommand.Parameter("Catheter I", string.Empty)], "Вычисляет пифогорову тройку по формуле комплекстного числа заданной длинны основания и заданного катета представляемого комплекстным числом"),
                    new TypeCommand.ConsoleCommand("colored", 25, null, "Открывает окно редактора цветовых палитр"),
                    new TypeCommand.ConsoleCommand("new_label", 26, null, "Открывает окно редактора создания нового ярлыка программы"),
                ];
            }
        }

        /// <summary>
        /// Прочитать базу данных консольных команд и создать массив консольных команд
        /// </summary>
        /// <returns>Массив консольных команд</returns>
        public static TypeCommand.VoiceCommand[] ReadVoiceCommandDataBase()
        {
            return [];
        }
    }
}