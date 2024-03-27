using AAC.Classes.Commands;
using Microsoft.Speech.Recognition;
using static AAC.Classes.AnimationDL.Animate.AnimFormule;
using static AAC.Startcs;

namespace AAC.Classes.DataClasses
{
    /// <summary>
    /// Класс голосового девайса разпознающего речь
    /// </summary>
    public class InputVoiceCommandDevice
    {
        /// <summary>
        /// Глобальный дивайс ввода голосовых команд
        /// </summary>
        private readonly SpeechRecognitionEngine RecordInput;

        /// <summary>
        /// Коэффициент точности распознавания голосовых фраз
        /// </summary>
        public static readonly float FactorAccuracyVoice = 0.68f;

        /// <summary>
        /// Встроенные голосовые фразы 
        /// </summary>
        private readonly Choices DefaultChoicesProgramm;

        /// <summary>
        /// Использование ограничения распознавания голоса
        /// </summary>
        /// <remarks>
        /// Если активен данный параметр, <b>будут распозначаться команды только те, </b><c>которые встроены в сам объект распозначания голоса</c>
        /// </remarks>
        public bool LimitationSpeech = false;

        /// <summary>
        /// Встроенные команды которые выполняются в ограниченном режиме
        /// </summary>
        private readonly VoiceCommand[] LimitationVoiceCommand;

        /// <summary>
        /// Обновить данные фраз
        /// </summary>
        public void UpdateAllGrammars()
        {
            Apps.MainForm.pbLoadingIndicator.Location = new(788, Apps.MainForm.pbLoadingIndicator.Location.Y);
            bool Activated = RecordInput.AudioState == AudioState.Silence;
            if (Activated) RecordInput.RecognizeAsyncStop();
            RecordInput.UnloadAllGrammars();

            GrammarBuilder grammarBuilder = new()
            {
                Culture = new System.Globalization.CultureInfo("ru-RU"),
            };
            grammarBuilder.Append(DefaultChoicesProgramm);
            Grammar grammar = new(grammarBuilder);
            RecordInput.LoadGrammarAsync(grammar);
            if (Activated) RecordInput.RecognizeAsync(RecognizeMode.Multiple);
        }

        /// <summary>
        /// Выключить распознование
        /// </summary>
        public void Diactivate()
        {
            if (!LimitationSpeech) LimitationSpeech = true;
            else
            {
                RecordInput.RecognizeAsyncStop();
                LimitationSpeech = false;
            }
        }

        /// <summary>
        /// Включить распознавание
        /// </summary>
        public void Activate()
        {
            LimitationSpeech = false;
            UpdateAllGrammars();
            RecordInput.RecognizeAsync(RecognizeMode.Multiple);
        }

        /// <summary>
        /// Инициализировать объект девайса распознавающего голос
        /// </summary>
        /// <param name="DefaultVoiceCommand">Встроенные фразы</param>
        /// <param name="ActivateDevice">Активировать ли распознавание</param>
        public InputVoiceCommandDevice(CommandData Data, SettingsData.SettingsBoolParameter ActivateDevice) // https://ok.ru/video/215884041856
        {
            RecordInput = new();
            LimitationVoiceCommand =
            [
                new VoiceCommand(["включи голосовые команды"], "Включает голосовые команды при их отключённом состоянии", () =>
                {
                    if (MainData.Flags.AudioCommand == StatusFlags.Sleep)
                    {
                        Apps.MainForm.VoiceButtonImageUpdate(StatusFlags.Active, false);
                        MainData.Flags.AudioCommand = StatusFlags.Active;
                        Activate();
                        if (Apps.MainForm.StateAnimWindow != StateAnimateWindow.Active) MainData.MainMP3.PlaySound("Complete");
                    }
                    return Task.FromResult(CommandStateResult.Completed);
                })
            ];
            string[] strings = [];
            Array.ForEach(LimitationVoiceCommand, (i) => strings = [.. strings.Concat(i.Phrases)]);
            Array.ForEach(Data.MassVoiceCommand, (i) => strings = [.. strings.Concat(i.Phrases)]);
            DefaultChoicesProgramm = new(strings);

            GrammarBuilder bulder = new();
            bulder.Append(DefaultChoicesProgramm);
            GrammarBuilder grammarBuilder = new()
            {
                Culture = new System.Globalization.CultureInfo("ru-RU"),
            };
            grammarBuilder.Append(DefaultChoicesProgramm);
            Grammar grammar = new(grammarBuilder);
            RecordInput.LoadGrammar(grammar);
            RecordInput.SetInputToDefaultAudioDevice();
            RecordInput.LoadGrammarCompleted += (sender, e) =>
            {
                new ConstAnimMove(Apps.MainForm.pbLoadingIndicator.Location.X, 813, 7).InitAnimFormule
                (Apps.MainForm.pbLoadingIndicator, Formules.QuickTransition, new(Apps.MainForm.pbLoadingIndicator.Location.Y), AnimationStyle.XY);
            };
            RecordInput.SpeechRecognized += (sender, e) =>
            {
                if (e.Result.Confidence < FactorAccuracyVoice) return;
                Task.Run(async () =>
                {
                    VoiceCommand? command = VoiceCommand.SearchVoiceCommand(LimitationSpeech ? LimitationVoiceCommand : Data.MassVoiceCommand, e.Result.Text);
                    if (command != null) (await command.ExecuteCommand()).Summarize();
                });
            };
            if ((bool)ActivateDevice) RecordInput.RecognizeAsync(RecognizeMode.Multiple);
        }
    }
}
