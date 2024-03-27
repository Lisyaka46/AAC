using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static AAC.Startcs;
using AAC.Classes.DataClasses;
using Microsoft.Speech.Recognition;

namespace AAC.Classes.Commands
{
    /// <summary>
    /// Консольная команда
    /// </summary>
    public class VoiceCommand
    {
        /// <summary>
        /// Фразы команды
        /// </summary>
        public readonly string[] Phrases;

        /// <summary>
        /// Описание голосовой команды
        /// </summary>
        public readonly string Explanation;

        /// <summary>
        /// Делегат события выполнения команды
        /// </summary>
        /// <param name="ParametersValue">Параметры команды</param>
        /// <returns>Итог выполнения команды</returns>
        public delegate Task<CommandStateResult> ExecuteCom();

        /// <summary>
        /// Действие которое выполняет команда
        /// </summary>
        private event ExecuteCom Execute;

        /// <summary>
        /// Инициализировать объект голосовой команды
        /// </summary>
        /// <param name="Phrases">Имя</param>
        /// <param name="id">Индификатор</param>
        /// <param name="parameters">Параметры команды</param>
        public VoiceCommand(string[] Phrases, string? Explanation, ExecuteCom Execute)
        {
            this.Phrases = [.. Phrases.Select((i) => "" + i)];
            this.Explanation = Explanation ?? "Нет описания";
            this.Execute = Execute;
        }

        /// <summary>
        /// Инициализировать голосовую команду
        /// </summary>
        /// <returns>Итог выполнения голосовой команды</returns>
        public async Task<CommandStateResult> ExecuteCommand() => await Execute.Invoke();

        /// <summary>
        /// Поиск голосовой команды по её фразе
        /// </summary>
        /// <param name="Phrase">Фраза</param>
        /// <returns>Голосовая команда</returns>
        public static VoiceCommand? SearchVoiceCommand(VoiceCommand[] ArrayVoiceCommand, string Phrase) => ArrayVoiceCommand.FirstOrDefault((i) => i.Phrases.Contains(Phrase));
    }
}
