using AAC.Classes;
using static AAC.Classes.AnimationDL.Animate.AnimColor;
using static AAC.Classes.MainTheme;
using static AAC.GUI.IELParamColorTheme.CustomEvents;

namespace AAC.GUI
{
    public partial class IELParamColorTheme : UserControl
    {
        /// <summary>
        /// Класс собственных событий объекта
        /// </summary>
        public static class CustomEvents
        {
            /// <summary>
            /// Делегат события активации изменения параметра цвета
            /// </summary>
            /// <param name="Index">Индекс ссылки параметра в теме</param>
            public delegate Color? EventChangeParamColor(int Index);
        }

        /// <summary>
        /// Событие изменения цвета параметра
        /// </summary>
        public event EventChangeParamColor? ParamChangeColor;

        /// <summary>
        /// Индекс ссылки к массиву цветов темы
        /// </summary>
        public int IndexParameter { get; }

        /// <summary>
        /// Параметр цвета темы
        /// </summary>
        public Color? ParamColor { get; private set; }

        /// <summary>
        /// Параметр информации о параметре цвета темы
        /// </summary>
        public ThemeInfoParameter? InfoParameter { get; }

        /// <summary>
        /// Инициализировать визуализационный объект цветового параметра темы
        /// </summary>
        /// <param name="infoParameter">Информационный параметр на который ссылается объект</param>
        /// <param name="parent">Панель в которой может находиться объект</param>
        /// <param name="color">Цвет параметра темы</param>
        /// <param name="Index">Индекс ссылка на элемент в массиве цветов темы</param>
        public IELParamColorTheme(ThemeInfoParameter? infoParameter, Panel? parent, Color? color, int Index = -1)
        {
            InitializeComponent();
            IndexParameter = Index;
            InfoParameter = infoParameter;
            ParamColor = color;
            if (parent != null) Parent = parent;
            ElementColor.BackColor = ParamColor ?? Color.Black;
            ElementColor.Text = ParamColor == null ? "Null" : string.Empty;
            if (infoParameter != null)
            {
                ElementName.Text = infoParameter.Name;
                Description.Text = infoParameter.Value;
            }
        }

        /// <summary>
        /// Инициализировать пустой объект цветового параметра темы
        /// </summary>
        public IELParamColorTheme()
        {
            InitializeComponent();
            IndexParameter = -1;
            ParamColor = null;
            ElementColor.BackColor = ParamColor ?? Color.Black;
            ElementColor.Text = ParamColor == null ? "Null" : string.Empty;
        }

        /// <summary>
        /// Задать параметр цвета объекту
        /// </summary>
        /// <param name="color">Цвет</param>
        public void SetParamColor(Color? color)
        {
            ParamColor = color;
            ElementColor.Text = color == null ? "Null" : string.Empty;
            ElementColor.BackColor = color == null ? Color.Black : (Color)color;
        }

        //
        public void SearchDetect()
        {
            ConstAnimColor constAnim = new(Color.White, Color.FromArgb(35, 42, 59), 3);
            constAnim.AnimInit(this, AnimStyleColor.BackColor);
        }

        /// <summary>
        /// Событие входа/выхода курсора в/из область(и) объекта канала цвета
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="e">Объект события</param>
        private void ElementColor_MouseDetectCurcor(object sender, EventArgs e)
        {
            bool i = ElementColor.Text.Equals("Null");
            ElementColor.BackColor = Other.InvertColor(ElementColor.BackColor);
            if (i) ElementColor.Text = "Null";
        }

        /// <summary>
        /// Событие двойного нажатия на объект канала объекта цвета
        /// </summary>
        /// <param name="sender">Объект вызвавший событие</param>
        /// <param name="e">Объект события</param>
        private void ElementColor_DoubleClick(object sender, EventArgs e)
        {
            ParamColor = ParamChangeColor?.Invoke(IndexParameter);
            if (ParamColor != null)
            {
                ElementColor.BackColor = (Color)ParamColor;
                if (ElementColor.Text.Equals("Null")) ElementColor.Text = string.Empty;
            }
        }
    }
}
