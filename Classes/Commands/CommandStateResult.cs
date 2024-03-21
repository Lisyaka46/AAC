using static AAC.Classes.AnimationDL.Animate.AnimText;
using static AAC.Startcs;

namespace AAC.Classes.Commands
{
    /// <summary>
    /// Конечные результаты выполнения команды
    /// </summary>
    public enum ResultStateCommand
    {
        /// <summary>
        /// Команда не выполнилась
        /// </summary>
        Failed = 0,

        /// <summary>
        /// Команда выполнилась успешно
        /// </summary>
        Complete = 1
    }

    /// <summary>
    /// Объект итогового состояния выполнения команды
    /// </summary>
    public class CommandStateResult
    {
        /// <summary>
        /// Итоговое состояние команды
        /// </summary>
        public readonly ResultStateCommand State;

        /// <summary>
        /// Сообщение в LOG
        /// </summary>
        public readonly string LOGMassage;

        /// <summary>
        /// Сообщение в консольную строку
        /// </summary>
        public readonly string Massage;

        /// <summary>
        /// Успешный итог выполнения команды
        /// </summary>
        public static CommandStateResult Completed => new(ResultStateCommand.Complete, string.Empty, string.Empty);

        /// <summary>
        /// Инициализировать объект итога выполнения команды
        /// </summary>
        /// <param name="ResultState">Итоговое состояние выполнения</param>
        /// <param name="Massage">Сообщение в консольную строку</param>
        /// <param name="Massage_log">Сообщение в LOG</param>
        internal CommandStateResult(ResultStateCommand ResultState, string Massage, string Massage_log)
        {
            State = ResultState;
            this.Massage = Massage;
            LOGMassage = Massage_log;
        }

        /// <summary>
        /// Выполнить обычные действия подведения итога команды
        /// </summary>
        public void Summarize()
        {
            if (LOGMassage.Length > 0) ObjLog.LOGTextAppend(LOGMassage);
            if (State == ResultStateCommand.Failed)
            {
                MainData.MainMP3.PlaySound("Error");
                //ConstAnimMove ConstantFormule = new(15, 15, 10);
                //ConstantFormule.InitAnimFormule(App.MainForm.tbOutput, //!! Formules.Sinusoid, new ConstAnimMove(App.MainForm.tbOutput.Location.Y), AnimationStyle.XY);
            }
            if (Massage.Length > 0) new Instr_AnimText(App.MainForm.tbOutput, Massage).AnimInit(true);
            if (App.MainForm.WindowState == FormWindowState.Normal) App.MainForm.LComplete_Click(null, null);
            //else if (MainData.Flags.FormActivity == Data.BooleanFlags.False && State == ResultStateCommand.Complete) MainData.MainMP3.PlaySound("Complete");
        }
    }
}
