using IWshRuntimeLibrary;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static AAC.Classes.AnimationDL.Animate.AnimText;
using static AAC.MiniFunctions;
using static AAC.Startcs;
using AAC.Classes.DataClasses;
using static AAC.Classes.Commands.ConsoleCommand;
using System.CodeDom.Compiler;

namespace AAC.Classes.Commands
{
    /// <summary>
    /// Консольная команда
    /// </summary>
    public partial class ConsoleCommand
    {
        /// <summary>
        /// Делегат события выполнения команды
        /// </summary>
        /// <param name="ParametersValue">Параметры команды</param>
        /// <returns>Итог выполнения команды</returns>
        public delegate Task<CommandStateResult> ExecuteCom(string[] ParametersValue);

        /// <summary>
        /// Имя команды
        /// </summary>
        private readonly string Name;

        /// <summary>
        /// Описание консольной команды
        /// </summary>
        public readonly string Explanation;

        /// <summary>
        /// Параметры команды
        /// </summary>
        public readonly Parameter[]? Parameters;

        /// <summary>
        /// Действие которое выполняет команда
        /// </summary>
        private event ExecuteCom Execute;

        /// <summary>
        /// Инициализировать объект консольной команды
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <param name="Parameters">Параметры команды</param>
        public ConsoleCommand(string Name, Parameter[]? Parameters, string? Explanation, ExecuteCom? Execute)
        {
            this.Name = Name;
            this.Parameters = Parameters;
            this.Explanation = Explanation ?? "Нет описания";
            Execute ??= (parameters) => { return Task.FromResult(CommandStateResult.Completed); };
            this.Execute = Execute;

        }

        /// <summary>
        /// Сгенерировать пропись команды
        /// </summary>
        /// <returns>Строка прописи команды</returns>
        public string WritingCommandAll()
        {
            string Output = WritingCommandName();
            if (Parameters?.Length > 0) Output += $"* <{string.Join(", <", Parameters.Select(i => $"{i.Name}{(i.Absolutly ? string.Empty : "?")}>"))}";
            return Output;
        }

        /// <summary>
        /// Сгенерировать пропись команды
        /// </summary>
        /// <returns>Строка прописи команды</returns>
        public string WritingCommandName() => $"{char.ToUpper(Name[0])}{Name[1..].ToString().Replace("_", " ") ?? string.Empty}";

        /// <summary>
        /// Сгенерировать пропись команды
        /// </summary>
        /// <returns>Строка прописи команды</returns>
        public string[] WritingCommandParameters()
        {
            List<string> Output = [];
            if (Parameters?.Length > 0)
            {
                IEnumerable<string> ParameterNames = Parameters.Select(I => I.Name + ">");
                Output.Add($"{string.Join(", <", ParameterNames)}");
            }
            return [.. Output];
        }

        /// <summary>
        /// Прочитать команду из консоли
        /// </summary>
        /// <param name="TextCommand">Читаемая команда</param>
        /// <returns>Объект консольной команды</returns>
        public static void ReadConsoleCommand(ConsoleCommand[] ConsoleCommands, string TextCommand, TextBox? ConsoleText = null)
        {
            ObjLog.LOGTextAppend($"Начинаю читать команду <{TextCommand}>");
            List<string> Parameters = [];
            while (TextCommand.Length > 0)
            {
                if (TextCommand[^1] == ' ') TextCommand = TextCommand.Remove(TextCommand.Length - 1);
                else if (TextCommand.Contains("  ")) TextCommand = TextCommand.Replace("  ", " ");
                else break;
            }
            if (TextCommand.Contains('*') && TextCommand[^1] != '*') // command* param1, param2, param3 ...
            {
                if (TextCommand[TextCommand.IndexOf('*') + 1] != ' ') TextCommand = TextCommand.Replace("*", "* ");
                TextCommand = TextCommand[0..TextCommand.IndexOf('*')].Replace(" ", "_").ToLower() + TextCommand[TextCommand.IndexOf('*')..];
                Parameters = RegexParameterCommand().Matches(TextCommand).Select(i => i.Value[2..]).ToList();
                TextCommand = RegexSortCommand().Match(TextCommand).Value.ToString().Replace("*", string.Empty).Replace(" ", string.Empty);
            }
            else // command
            {
                TextCommand = TextCommand.Replace(" ", "_").Replace("*", string.Empty).ToLower();
            }
            ConsoleCommand? SearchCommand = ConsoleCommands.SingleOrDefault(i => i.Name.Equals(TextCommand));
            CommandStateResult ResultState;
            if (SearchCommand == null)
            {
                ResultState = new CommandStateResult(Commands.ResultState.Failed, $"Invalid command \"{TextCommand}\"", $"Команда \"{TextCommand}\" не найдена");
            }
            else
            {
                ResultState = SearchCommand.ExecuteCommand([.. Parameters]).Result;
            }
            if (ConsoleText != null) ConsoleText.Text = string.Empty;
            ResultState.Summarize();
        }

        /// <summary>
        /// Создать выполнение команды
        /// </summary>
        public async Task<CommandStateResult> ExecuteCommand(string[]? parameters)
        {
            int LengthParam = 0;
            if (Parameters != null) Array.ForEach(Parameters, (i) => { if (i.Absolutly) LengthParam++; });
            if (parameters?.Length >= LengthParam)
            {
                ObjLog.LOGTextAppend($"Выполнение команды:\n<{Name}>");
                Apps.MainForm.lDeveloper_ParametersCommand.Text = $"PC: <{string.Join(", ", parameters.AsEnumerable())}>";
                return await Execute.Invoke(parameters);
            }
            else return new CommandStateResult(ResultState.Failed,
                $"There are not enough parameters to execute the \"{Name}\" command",
                $"Недостаточно параметров для выполнения команды \"{Name}\"");
        }

        [GeneratedRegex(@"( |\*|,)([^,]|,,)+")]
        private static partial Regex RegexParameterCommand();
        [GeneratedRegex(@"\b[^\*~!@#$<>,.\/\\?|'"";:`%^&*()\[\]{} \-=+]+\* ?")]
        private static partial Regex RegexSortCommand();
    }
}
