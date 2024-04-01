using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAC.Forms;
using AAC.Forms.Dialogs;

namespace AAC.Classes
{
    public class Applications()
    {
        /// <summary>
        /// Основная форма
        /// </summary>
        public MainApplication MainForm = null;

        /// <summary>
        /// Форма описания всех команд зарегестрируемых в проект
        /// </summary>
        public FormExplanationCommands InformationCommand = null;

        /// <summary>
        /// Форма визуализации данных журнала
        /// </summary>
        public LogForm Log = null;

        /// <summary>
        /// Форма стартовой анимации
        /// </summary>
        public FormAnimateStart Starting = null;

        /// <summary>
        /// Диалоговое окно изменения кастомных gif анимаций на главной форме
        /// </summary>
        public DialogCustomImage DialogCustomImage_Form = null;

        /// <summary>
        /// Форма благодарностей
        /// </summary>
        public DialogThanks Thanks = null;

        /// <summary>
        /// Главная форма настроек
        /// </summary>
        public FormMainSettings WindowSettings = null;

        /// <summary>
        /// Форма редактирования тем
        /// </summary>
        public FormThemesEditor ThemesCreated = null;

        /// <summary>
        /// Диалоговое окно создания ярлыка CLR
        /// </summary>
        public DialogCreateLabel DialogCreateLabel = null;
    }
}
