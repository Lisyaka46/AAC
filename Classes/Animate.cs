namespace AAC.Classes
{
    /// <summary>
	/// Главный класс анимации Daniil Lisov
	/// </summary>
    public class AnimationDL
    {

        /// <summary>
        /// Массив активных имён анимации по формуле
        /// </summary>
        private static List<Thread> ActiveElementsAnimationFormule { get; } = [];

        /// <summary>
        /// Массив активных имён анимации цвета
        /// </summary>
        private static List<Thread> ActiveElementsAnimationColor { get; } = [];

        /// <summary>
        /// Виды анимаций
        /// </summary>
        public enum StyleAnimateObj
        {
            /// <summary>
            /// Вид анимации по формулам
            /// </summary>
            AnimFormule = 0,

            /// <summary>
            /// Вид анимации цвета
            /// </summary>
            AnimColor = 1,

            /// <summary>
            /// Вид написации текста
            /// </summary>
            AnimText = 2
        }

        /// <summary>
        /// Остановить действующую анимацию
        /// </summary>
        /// <param name="style">Вид останавливаемой анимации</param>
        /// <param name="ObjName">Имя анимирующего объекта</param>
        public static void StopAnimate(StyleAnimateObj style, string ObjName)
        {
            static void ForRemoveThread(List<Thread> threads, string ObjName)
            {
                if (threads.Count == 0) return;
                foreach (Thread thread in threads)
                {
                    if ((thread.Name?.Equals(ObjName) ?? false) && thread.IsAlive)
                    {
                        try { thread.Interrupt(); }
                        catch { }
                        threads.Remove(thread);
                        break;
                    }
                }
            }

            switch (style)
            {
                case StyleAnimateObj.AnimFormule:
                    ForRemoveThread(ActiveElementsAnimationFormule, ObjName);
                    break;

                case StyleAnimateObj.AnimColor:
                    ForRemoveThread(ActiveElementsAnimationColor, ObjName);
                    break;

                case StyleAnimateObj.AnimText:
                    Animate.AnimText.RemoveAnimText(ObjName);
                    break;
            }
        }

        /// <summary>
        /// <b>Главный класс анимаций</b>
        /// </summary>
        public static class Animate
        {
            /// <summary>
            /// Класс объекта анимаций по формулам
            /// </summary>
            public static class AnimFormule
            {
                /// <summary>
                /// Класс объекта констант <b>сторон</b> анимаций по формулам X/Y/W/H
                /// </summary>
                public struct ConstAnimMove
                {
                    /// <summary>
                    /// Стартовая позиция анимации
                    /// </summary>
                    public float X0 { get; set; }

                    /// <summary>
                    /// Конечная позиция анимации
                    /// </summary>
                    public float X1 { get; set; }

                    /// <summary>
                    /// Коэффициент плавности
                    /// </summary>
                    public int Y { get; set; }

                    /// <summary>
                    /// Создать константы со своими значениями
                    /// </summary>
                    /// <param name="x0">Стартовая позиция</param>
                    /// <param name="x1">Конечная позиция</param>
                    /// <param name="y">Коэффициент плавности</param>
                    public ConstAnimMove(int x0, int x1, int y = 1)
                    {
                        X0 = x0;
                        X1 = x1;
                        Y = y;
                    }

                    /// <summary>
                    /// Создаёт фиксированное значение
                    /// </summary>
                    /// <param name="Pos">Позиция фиксированного значения</param>
                    public ConstAnimMove(int Pos)
                    {
                        X0 = Pos;
                        X1 = Pos;
                        Y = 1;
                    }

                    /// <summary>
                    /// Поменять значения
                    /// </summary>
                    public readonly ConstAnimMove Reverse() => new((int)X1, (int)X0, Y);

                    /// <summary>
                    /// Поменять значения с изменением параметра Y
                    /// </summary>
                    /// <param name="Y">Новый коэффициент плавности</param>
                    public readonly ConstAnimMove Reverse(int Y) => new((int)X1, (int)X0, Y);

                    /// <summary>
                    /// Обновить стартовое значение объекта констант
                    /// </summary>
                    /// <param name="X0">Новое стартовое значение объекта константы</param>
                    public readonly ConstAnimMove UpStart(int X0) => new(X0, (int)X1, Y);

                    /// <summary>
                    /// Инициализировать вторую сторону, инструкцию и создать анимацию учитывая первые и вторые константы
                    /// </summary>
                    /// <param name="obj">Анимированный объект</param>
                    /// <param name="ActiveFromule">Формула для анимации</param>
                    /// <param name="Y_H">Вторая константа Y (Первая X является ссылочной, текущей)</param>
                    /// <param name="AnimStyle">Стиль анимации</param>
                    public readonly void InitAnimFormule(dynamic obj, Formules ActiveFromule, ConstAnimMove? Y_H, AnimationStyle AnimStyle)
                    {
                        try
                        {
                            _ = obj.Location.X;
                            _ = obj.Size.Height;
                        }
                        catch { throw new ArgumentException("Объект анимации не потдерживает свойства Location|Size", nameof(obj)); }
                        Instr_AnimFormule Instruction = new(ActiveFromule, this, Y_H);
                        Instruction.InitUsualAnimation(obj, AnimStyle);
                    }
                }

                /// <summary>
                /// Класс объекта инструкции <b>обычной</b> анимаций по формулам
                /// </summary>
                public class Instr_AnimFormule
                {
                    /// <summary>
                    /// Погрешность расчётов анимации - смещение расчётов
                    /// </summary>
                    public const float Offset = 1f;

                    /// <summary>
                    /// Формула анимации
                    /// </summary>
                    public Formules ActivateFormule { get; }

                    /// <summary>
                    /// Константы для позиции X или Width
                    /// </summary>
                    public ConstAnimMove? ConstXW { get; private set; }

                    /// <summary>
                    /// Константы для позиции Y или Height
                    /// </summary>
                    public ConstAnimMove? ConstYH { get; private set; }

                    /// <summary>
                    /// Инициализировать инструкцию обычной анимации по формуле
                    /// </summary>
                    /// <param name="formule">Формула для создания анимации</param>
                    /// <param name="ConstXW">Константа позиции X или Width</param>
                    /// <param name="ConstYH">Константа позиции Y или Height</param>
                    /// <param name="animationStyle">Стиль анимации по формуле</param>
                    public Instr_AnimFormule(Formules formule, ConstAnimMove? ConstXW, ConstAnimMove? ConstYH)
                    {
                        ActivateFormule = formule;
                        this.ConstXW = ConstXW;
                        this.ConstYH = ConstYH;
                    }

                    /// <summary>
                    /// Создать обычную анимацию учитывая текущую инструкцию
                    /// </summary>
                    /// <param name="obj">Анимированный объект</param>
                    /// <param name="style">Стиль анимации</param>
                    public void InitUsualAnimation(dynamic obj, AnimationStyle style)
                    {
                        AnimFormuleInit ObjInit = AnimFormule(obj, style);
                        AnimFormuleInit.AnimInit(ObjInit);
                    }


                    /// <summary>
                    /// Создать обычную анимацию по формуле
                    /// </summary>
                    /// <param name="Object">Объект анимации</param>
                    /// <returns>List(Point): Инструкция для воспроизведения анимации</returns>
                    public AnimFormuleInit AnimFormule(dynamic Object, AnimationStyle style)
                    {
                        if ((int)style > 1) style = AnimationStyle.XY;
                        if (ConstXW.HasValue || ConstYH.HasValue)
                        {
                            List<Point> OutPut = [];
                            float XW0 = 0f, XW1 = 0f, YH0 = 0f, YH1 = 0f;
                            Func<float, float, float, bool> XWBool, YHBool;

                            if (!ConstXW.HasValue)
                            {
                                if (style == AnimationStyle.XY) ConstXW = new(Object.Location.X);
                                else if (style == AnimationStyle.Size) ConstXW = new(Object.Size.Width);
                            }
                            if (ConstXW.HasValue)
                            {
                                XW0 = ConstXW.Value.X0;
                                XW1 = ConstXW.Value.X1;
                            }

                            if (!ConstYH.HasValue)
                            {
                                if (style == AnimationStyle.XY) ConstYH = new(Object.Location.Y);
                                else if (style == AnimationStyle.Size) ConstYH = new(Object.Size.Height);
                            }
                            if (ConstYH.HasValue)
                            {
                                YH0 = ConstYH.Value.X0;
                                YH1 = ConstYH.Value.X1;
                            }

                            if (ConstXW.HasValue && ConstYH.HasValue)
                            {
                                if (XW0 > XW1) XWBool = (Ex0, Ex1, offset) => Ex0 > (Ex1 + offset);
                                else XWBool = (Ex0, Ex1, offset) => Ex0 < (Ex1 - offset);

                                if (YH0 > YH1) YHBool = (Ex0, Ex1, offset) => Ex0 > (Ex1 + offset);
                                else YHBool = (Ex0, Ex1, offset) => Ex0 < (Ex1 - offset);

                                switch (ActivateFormule)
                                {
                                    default:
                                        while (XWBool(XW0, XW1, Offset) || YHBool(YH0, YH1, Offset))
                                        {
                                            XW0 += AuxiliaryFunction.FormuleSwichUpdatePosition(XWBool(XW0, XW1, Offset), ActivateFormule, ConstXW.Value.UpStart((int)XW0));
                                            YH0 += AuxiliaryFunction.FormuleSwichUpdatePosition(YHBool(YH0, YH1, Offset), ActivateFormule, ConstYH.Value.UpStart((int)YH0));
                                            OutPut.Add(new((int)XW0, (int)YH0));
                                        }
                                        break;
                                }
                                OutPut.Add(new((int)XW1, (int)YH1));
                                if (style == AnimationStyle.XY) return new AnimFormuleInit(Object, OutPut, null, style);
                                else return new AnimFormuleInit(Object, null, OutPut, style);
                            }
                            else throw new Exception("Непредвиденная ошибка AAC_ANIMATE_CS-283");
                        }
                        else throw new Exception("Все константы при генерации обычной анимации не содержат в себе значения!");
                    }
                }

                /// <summary>
                /// Класс инструкции <b>групповой</b> анимаций по формулам
                /// </summary>
                public class Instr_GroupAnimFormule
                {
                    /// <summary>
                    /// Погрешность расчётов анимации - смещение расчётов
                    /// </summary>
                    public const float Offset = 1f;

                    /// <summary>
                    /// Формула генерации анимации
                    /// </summary>
                    public Formules Formule { get; }

                    /// <summary>
                    /// Константы для позиции X
                    /// </summary>
                    public ConstAnimMove? X { get; set; }

                    /// <summary>
                    /// Константы для позиции Y
                    /// </summary>
                    public ConstAnimMove? Y { get; set; }

                    /// <summary>
                    /// Константы для позиции W
                    /// </summary>
                    public ConstAnimMove? W { get; set; }

                    /// <summary>
                    /// Константы для позиции H
                    /// </summary>
                    public ConstAnimMove? H { get; set; }

                    /// <summary>
                    /// 
                    /// </summary>
                    /// <param name="formule">Формула для создания групповой анимации по формуле</param>
                    /// <param name="X">Отдельная константа X</param>
                    /// <param name="Y">Отдельная константа Y</param>
                    /// <param name="W">Отдельная константа Width</param>
                    /// <param name="H">Отдельная константа Height</param>
                    public Instr_GroupAnimFormule(Formules formule, ConstAnimMove? X, ConstAnimMove? Y, ConstAnimMove? W, ConstAnimMove? H)
                    {
                        Formule = formule;
                        this.X = X;
                        this.Y = Y;
                        this.W = W;
                        this.H = H;
                    }

                    /// <summary>
                    /// Создать групповую анимацию учитывая текущую инструкцию
                    /// </summary>
                    /// <param name="obj">Анимированный объект</param>
                    public void InitGroupAnimation(dynamic obj)
                    {
                        AnimFormuleInit Instruction = GroupAnimFormule(obj, Formule);
                        AnimFormuleInit.AnimInit(Instruction);
                    }

                    /// <summary>
                    /// Создать групповую анимацию по формуле
                    /// </summary>
                    /// <param name="Object">Элемент WinForms Потдерживающий позицию и размер</param>
                    /// <param name="style">Стиль анимации</param>
                    /// <returns>AnimFormuleInit: Объект инициализации анимации</returns>
                    private AnimFormuleInit GroupAnimFormule(dynamic Object, Formules style)
                    {
                        List<Point> OutPutLocation = [];
                        List<Point> OutPutSize = [];

                        if (!X.HasValue) X = new(Object.Location.X);
                        if (!Y.HasValue) Y = new(Object.Location.Y);

                        if (!W.HasValue) W = new(Object.Size.Width);
                        if (!H.HasValue) H = new(Object.Size.Height);

                        if (X.HasValue && Y.HasValue && W.HasValue && H.HasValue)
                        {
                            Func<float, float, float, bool> XBool, YBool, WBool, HBool;

                            float
                                Xx0 = X.Value.X0, Xx1 = X.Value.X1,
                                Yx0 = Y.Value.X0, Yx1 = Y.Value.X1,
                                Wx0 = W.Value.X0, Wx1 = W.Value.X1,
                                Hx0 = H.Value.X0, Hx1 = H.Value.X1;

                            if (Xx0 > Xx1) XBool = (Ex, Ey, offset) => Ex > Ey + offset;
                            else XBool = (Ex, Ey, offset) => Ex < Ey - offset;

                            if (Yx0 > Yx1) YBool = (Ex, Ey, offset) => Ex > Ey + offset;
                            else YBool = (Ex, Ey, offset) => Ex < Ey - offset;

                            if (Wx0 > Wx1) WBool = (Ex, Ey, offset) => Ex > Ey + offset;
                            else WBool = (Ex, Ey, offset) => Ex < Ey - offset;

                            if (Hx0 > Hx1) HBool = (Ex, Ey, offset) => Ex > Ey + offset;
                            else HBool = (Ex, Ey, offset) => Ex < Ey - offset;

                            while (XBool(Xx0, Xx1, Offset) || YBool(Yx0, Yx1, Offset) || WBool(Wx0, Wx1, Offset) || HBool(Hx0, Hx1, Offset))
                            {
                                Xx0 += AuxiliaryFunction.FormuleSwichUpdatePosition(XBool(Xx0, Xx1, Offset), style, X.Value.UpStart((int)Xx0));
                                Yx0 += AuxiliaryFunction.FormuleSwichUpdatePosition(YBool(Yx0, Yx1, Offset), style, Y.Value.UpStart((int)Yx0));

                                Wx0 += AuxiliaryFunction.FormuleSwichUpdatePosition(WBool(Wx0, Wx1, Offset), style, W.Value.UpStart((int)Wx0));
                                Hx0 += AuxiliaryFunction.FormuleSwichUpdatePosition(HBool(Hx0, Hx1, Offset), style, H.Value.UpStart((int)Hx0));

                                OutPutLocation.Add(new((int)Xx0, (int)Yx0));
                                OutPutSize.Add(new((int)Wx0, (int)Hx0));
                            }
                            OutPutLocation.Add(new((int)Xx1, (int)Yx1));
                            OutPutSize.Add(new((int)Wx1, (int)Hx1));
                            return new AnimFormuleInit(Object, OutPutLocation, OutPutSize, AnimationStyle.Group);
                        }
                        else throw new Exception("Некоторые константы при генерации анимации не содержат в себе значения ANIM-410");
                    }
                }

                /// <summary>
                /// Класс объекта инициализации обычных/групповых анимаций по формулам
                /// </summary>
                public struct AnimFormuleInit
                {
                    /// <summary>
                    /// Элемент который воспроизводит анимацию
                    /// </summary>
                    private dynamic AnimationElement { get; }

                    /// <summary>
                    /// Стиль обычной анимации
                    /// </summary>
                    private AnimationStyle AnimationStyle { get; }

                    /// <summary>
                    /// Инструкция позиций обычной анимации
                    /// </summary>
                    private Point[]? AnimationPointLocation { get; }

                    /// <summary>
                    /// Инструкция позиций обычной анимации
                    /// </summary>
                    private Point[]? AnimationPointSize { get; }

                    /// <summary>
                    /// Инициализировать объект инициализации анимации
                    /// </summary>
                    /// <param name="Object">Элемент WinForms Потдерживающий позицию и размер</param>
                    /// <param name="AnimPointLocation">Точечные позиции анимации изменения позиции</param>
                    /// <param name="AnimPointSize">Точечные позиции анимации изменения размера</param>
                    /// <param name="AnimStyle">Стиль анимации по формуле</param>
                    public AnimFormuleInit(dynamic Object, List<Point>? AnimPointLocation, List<Point>? AnimPointSize, AnimationStyle AnimStyle)
                    {
                        try
                        {
                            _ = Object.Location.X;
                            _ = Object.Size.Height;
                        }
                        catch { throw new ArgumentException("Объект анимации не потдерживает свойства Location|Size", nameof(Object)); }
                        AnimationElement = Object;
                        AnimationStyle = AnimStyle;
                        AnimationPointLocation = AnimPointLocation?.ToArray() ?? null;
                        AnimationPointSize = AnimPointSize?.ToArray() ?? null;
                    }

                    /// <summary>
                    /// Анимировать объект обычной/групповой анимацией по формуле
                    /// </summary>
                    public static void AnimInit(AnimFormuleInit ObjInit)
                    {
                        if (ObjInit.AnimationElement == null) throw new ArgumentNullException(nameof(ObjInit) + ".AnimationElement", "Анимационный элемент является нулевым объектом ANIM-464");
                        StopAnimate(StyleAnimateObj.AnimFormule, ObjInit.AnimationElement.Name);
                        ThreadStart Animating = new(() =>
                        {
                            try
                            {
                                switch (ObjInit.AnimationStyle)
                                {
                                    // Анимация позиции
                                    case AnimationStyle.XY:
                                        if (ObjInit.AnimationPointLocation != null)
                                        {
                                            for (int i = 0; i < ObjInit.AnimationPointLocation.Length; i++)
                                            {
                                                ObjInit.AnimationElement.Location = ObjInit.AnimationPointLocation[i];
                                                //ObjInit.AnimationElement.Update();
                                                Thread.Sleep(i % 3 / 2);
                                            }
                                            Array.Clear(ObjInit.AnimationPointLocation, 0, ObjInit.AnimationPointLocation.Length);
                                        }
                                        break;

                                    // Анимация размера
                                    case AnimationStyle.Size:
                                        if (ObjInit.AnimationPointSize != null)
                                        {
                                            for (int i = 0; i < ObjInit.AnimationPointSize.Length; i++)
                                            {
                                                ObjInit.AnimationElement.Size = (Size)ObjInit.AnimationPointSize[i];
                                                //ObjInit.AnimationElement.Update();
                                                Thread.Sleep(i % 3 / 2);
                                            }
                                            Array.Clear(ObjInit.AnimationPointSize, 0, ObjInit.AnimationPointSize.Length);
                                        }
                                        break;

                                    // Анимация позиции и размера
                                    case AnimationStyle.Group:
                                        if (ObjInit.AnimationPointLocation != null && ObjInit.AnimationPointSize != null)
                                        {
                                            for (int i = 0; i < Math.Max(ObjInit.AnimationPointLocation.Length, ObjInit.AnimationPointSize.Length); i++)
                                            {
                                                if (i < ObjInit.AnimationPointLocation.Length) ObjInit.AnimationElement.Location = ObjInit.AnimationPointLocation[i];
                                                else if (ObjInit.AnimationPointLocation.Length > 0) Array.Clear(ObjInit.AnimationPointLocation, 0, ObjInit.AnimationPointLocation.Length);
                                                //else ObjInit.AnimationElement.Location = ObjInit.AnimationPointLocation[^1];
                                                if (i < ObjInit.AnimationPointSize.Length) ObjInit.AnimationElement.Size = new Size(ObjInit.AnimationPointSize[i]);
                                                else if (ObjInit.AnimationPointSize.Length > 0) Array.Clear(ObjInit.AnimationPointSize, 0, ObjInit.AnimationPointSize.Length);
                                                //else ObjInit.AnimationElement.Size = new Size(ObjInit.AnimationPointSize[^1]);
                                                //ObjInit.AnimationElement.Update();
                                                Thread.Sleep(i % 3 / 2);
                                            }
                                        }
                                        else throw new Exception("В групповой анимации не может быть пустых массивов точечных позиций ANIM-512");
                                        break;

                                }
                            }
                            catch { }
                        });
                        Thread ThAnim = new(Animating)
                        { Name = ObjInit.AnimationElement.Name };
                        ActiveElementsAnimationFormule.Add(ThAnim);
                        ActiveElementsAnimationFormule[^1].Start();
                    }
                }

                /// <summary>
                /// Стили анимаций для элементов
                /// </summary>
                public enum AnimationStyle
                {
                    /// <summary>
                    /// Анимация формы элемента
                    /// </summary>
                    Size = 0,

                    /// <summary>
                    /// Анимация позиции элемента
                    /// </summary>
                    XY = 1,

                    /// <summary>
                    /// Групповая анимация
                    /// </summary>
                    Group = 2,
                }

                /// <summary>
                /// Ключевые формулы для анимаций по формулам
                /// </summary>
                public enum Formules
                {
                    /// <summary>
                    /// N0 = (x1 - x0) / y0
                    /// </summary>
                    QuickTransition = 0,

                    /// <summary>
                    /// N0 = (x1 - (x1 - 1 - x0)) / y0
                    /// </summary>
                    AcceleratingTransition = 1,

                    /// <summary>
                    /// QuickTransition 1N0 = ((x1 + y0 / 2) - x0) / y0
                    /// -------
                    /// Sinusoid 2N0 = sin(zN / y0) / zN * y0
                    /// </summary>
                    QuickSinusoid = 2,

                    /// <summary>
                    /// N0 = (x1 / (x1 - x0)) / y0
                    /// </summary>
                    FastReverseTransition = 4,
                }
            }

            /// <summary>
            /// Главный класс анимаций <b>Цвета</b>
            /// </summary>
            public static class AnimColor
            {
                /// <summary>
                /// Класс объекта константы анимации цвета
                /// </summary>
                public class ConstAnimColor
                {
                    /// <summary>
                    /// Стартовый цвет анимации
                    /// </summary>
                    public Color? StartColorAnimation { get; }

                    /// <summary>
                    /// Конечный цвет анимации
                    /// </summary>
                    public Color? EndColorAnimation { get; set; }

                    /// <summary>
                    /// Скорость цветовой анимации
                    /// </summary>
                    public byte SpeedAnimation { get; }

                    /// <summary>
                    /// Инициализировать объект константы
                    /// </summary>
                    /// <param name="StartColor">Стартовый цвет</param>
                    /// <param name="EndColor">Конечный цвет</param>
                    /// <param name="Speed">Скорость изменения цвета</param>
                    public ConstAnimColor(Color? StartColor, Color? EndColor, sbyte Speed)
                    {
                        StartColorAnimation = StartColor;
                        EndColorAnimation = EndColor;
                        SpeedAnimation = (byte)Math.Abs(Speed);
                    }

                    /// <summary>
                    /// Инициализировать анимацию с помощью константы анимации цвета
                    /// </summary>
                    /// <param name="Obj">Динамический объект анимации</param>
                    /// <param name="AnimStyle">Стиль анимации цвета</param>
                    /// <param name="AnimForeColor">Константа для создания сгруппированной анимации цвета</param>
                    public void AnimInit(dynamic Obj, AnimStyleColor AnimStyle, ConstAnimColor? AnimForeColor = null)
                    {
                        Instr_AnimColor Instr = new(Obj, AnimStyle, this, AnimForeColor);
                        Instr.AnimColorInit();
                    }
                }

                /// <summary>
                /// Класс объекта инструкции генерации анимации <b>Цвета</b>
                /// </summary>
                public class Instr_AnimColor
                {
                    /// <summary>
                    /// Константа отвечающая за обычную анимацию Back|Fore анимации цвета
                    /// </summary>
                    public ConstAnimColor ConstAnimOneColor { get; }

                    /// <summary>
                    /// Константа сгруппированной Fore анимации цвета
                    /// </summary>
                    public ConstAnimColor? ConstAnimTwoColor { get; }

                    /// <summary>
                    /// Объект анимации
                    /// </summary>
                    public dynamic AnimationElement { get; }

                    /// <summary>
                    /// Стиль анимации
                    /// </summary>
                    public AnimStyleColor AnimationStyle { get; }


                    /// <summary>
                    /// Инициализировать инструкцию для цветовой анимации
                    /// </summary>
                    /// <param name="Obj">Объект анимации</param>
                    /// <param name="StyleAnimate">Стиль анимации цвета</param>
                    /// <param name="AnimOneColor">Константы BackColor</param>
                    /// <param name="AnimTwoColor">Константы ForeColor</param>
                    public Instr_AnimColor(dynamic Obj, AnimStyleColor StyleAnimate, ConstAnimColor AnimOneColor, ConstAnimColor? AnimTwoColor)
                    {
                        try
                        {
                            _ = Obj.BackColor;
                            _ = Obj.ForeColor;
                        }
                        catch { throw new ArgumentException("Объект анимации не потдерживает свойства BackColor|ForeColor", nameof(Obj)); }
                        ConstAnimOneColor = AnimOneColor;
                        ConstAnimTwoColor = AnimTwoColor;
                        AnimationStyle = StyleAnimate;
                        AnimationElement = Obj;
                    }

                    /// <summary>
                    /// Анимировать объект анимацией цвета
                    /// </summary>
                    public void AnimColorInit()
                    {
                        if ((ConstAnimOneColor.StartColorAnimation == null && ConstAnimOneColor.EndColorAnimation == null)
                            || ConstAnimOneColor.SpeedAnimation == 0) return;
                        if (ConstAnimOneColor.StartColorAnimation.HasValue)
                        {
                            if (AnimationStyle == AnimStyleColor.BackColor)
                                AnimationElement.BackColor = ConstAnimOneColor.StartColorAnimation.Value;
                            else if (AnimationStyle == AnimStyleColor.ForeColor)
                                AnimationElement.ForeColor = ConstAnimOneColor.StartColorAnimation.Value;
                        }
                        if (!ConstAnimOneColor.EndColorAnimation.HasValue)
                        {
                            if (AnimationStyle == AnimStyleColor.BackColor)
                                ConstAnimOneColor.EndColorAnimation = AnimationElement.BackColor;
                            else if (AnimationStyle == AnimStyleColor.ForeColor)
                                ConstAnimOneColor.EndColorAnimation = AnimationElement.ForeColor;
                        }
                        if (!ConstAnimOneColor.EndColorAnimation.HasValue)
                            throw new Exception($"Элемент {nameof(ConstAnimOneColor.EndColorAnimation)} является нулевым объектом после определения цветовой анимации");
                        if (ConstAnimTwoColor != null)
                        {
                            if (ConstAnimTwoColor.StartColorAnimation.HasValue)
                                AnimationElement.ForeColor = ConstAnimTwoColor.StartColorAnimation.Value;
                            if (!ConstAnimTwoColor.EndColorAnimation.HasValue)
                                ConstAnimTwoColor.EndColorAnimation = AnimationElement.ForeColor;
                            if (!ConstAnimTwoColor.EndColorAnimation.HasValue)
                                throw new Exception($"Элемент {nameof(ConstAnimTwoColor.EndColorAnimation)} является нулевым объектом после определения цветовой анимации");
                        }
                        StopAnimate(StyleAnimateObj.AnimFormule, AnimationElement.Name);
                        ThreadStart Animating = new(() => { });
                        if (ConstAnimTwoColor == null)
                        {
                            switch (AnimationStyle)
                            {
                                case AnimStyleColor.BackColor:
                                    Animating = new(async () =>
                                    {
                                        await Task.Run(() =>
                                        {
                                            while (WhileAc())
                                            {
                                                AnimationElement.BackColor =
                                                    AnimColorChangeIndex(AnimationElement.BackColor,
                                                    ConstAnimOneColor.EndColorAnimation.Value,
                                                    ConstAnimOneColor.SpeedAnimation);
                                                Thread.Sleep(10);
                                            }
                                        });
                                    });
                                    break;

                                case AnimStyleColor.ForeColor:
                                    Animating = new(async () =>
                                    {
                                        await Task.Run(() =>
                                        {
                                            while (WhileAc())
                                            {
                                                AnimationElement.ForeColor =
                                                    AnimColorChangeIndex(AnimationElement.ForeColor,
                                                    ConstAnimOneColor.EndColorAnimation.Value,
                                                    ConstAnimOneColor.SpeedAnimation);
                                                Thread.Sleep(10);
                                            }
                                        });
                                    });
                                    break;
                            }
                        }
                        else if (ConstAnimTwoColor.EndColorAnimation.HasValue)
                        {
                            Animating = new(async () =>
                            {
                                await Task.Run(() =>
                                {
                                    while (WhileAc())
                                    {
                                        AnimationElement.BackColor =
                                            AnimColorChangeIndex(AnimationElement.BackColor,
                                            ConstAnimOneColor.EndColorAnimation.Value,
                                            ConstAnimOneColor.SpeedAnimation);
                                        AnimationElement.ForeColor =
                                            AnimColorChangeIndex(AnimationElement.ForeColor,
                                            ConstAnimTwoColor.EndColorAnimation.Value,
                                            ConstAnimTwoColor.SpeedAnimation);
                                        Thread.Sleep(10);
                                    }
                                });
                            });
                        }
                        Thread ThAnim = new(Animating)
                        { Name = AnimationElement.Name };
                        ActiveElementsAnimationColor.Add(ThAnim);
                        ActiveElementsAnimationColor[^1].Start();
                    }

                    /// <summary>
                    /// Можно ли изменять цвет или нет
                    /// </summary>
                    /// <returns>Bool: Можно ли изменять цвет или нет</returns>
                    private bool WhileAc()
                    {
                        Func<dynamic, Color?, bool> AnimationFunc;
                        if (AnimationStyle == AnimStyleColor.ForeColor)
                            AnimationFunc = (obj, color) =>
                            {
                                //Startcs.ObjLog.LOGTextAppend($"{}-{} ");
                                if (color.HasValue) return
                                    obj.ForeColor.R != color.Value.R ||
                                    obj.ForeColor.G != color.Value.G ||
                                    obj.ForeColor.B != color.Value.B;
                                return false;
                            };
                        else AnimationFunc = (obj, color) =>
                            {
                                if (color.HasValue) return
                                    obj.BackColor.R != color.Value.R ||
                                    obj.BackColor.G != color.Value.G ||
                                    obj.BackColor.B != color.Value.B;
                                return false;
                            };
                        if (ConstAnimTwoColor == null) return AnimationFunc(AnimationElement, ConstAnimOneColor.EndColorAnimation);
                        return
                                AnimationFunc(AnimationElement, ConstAnimOneColor.EndColorAnimation) ||
                                AnimationFunc(AnimationElement, ConstAnimTwoColor.EndColorAnimation);
                    }

                    /// <summary>
                    /// Получить следующий кадр анимации цвета
                    /// </summary>
                    /// <param name="ThisColor">Текущий цвет</param>
                    /// <param name="EndColor">Конечный цвет</param>
                    /// <param name="Speed">Скорость</param>
                    /// <returns>Цвет олицитворяющий следующий кадр</returns>
                    private static Color AnimColorChangeIndex(Color ThisColor, Color EndColor, byte Speed)
                    {
                        static byte ColorIndex(byte ThisInd, byte EndInd, byte Speed)
                        {
                            if (Speed >= 255) return EndInd;
                            if (ThisInd < EndInd)
                            {
                                if (ThisInd + Speed <= EndInd) ThisInd += Speed;
                                else return EndInd;
                            }
                            else if (ThisInd > EndInd)
                            {
                                if (ThisInd - Speed >= EndInd) ThisInd -= Speed;
                                else return EndInd;
                            }
                            return ThisInd;
                        }
                        return Color.FromArgb(ColorIndex(ThisColor.R, EndColor.R, Speed), ColorIndex(ThisColor.G, EndColor.G, Speed), ColorIndex(ThisColor.B, EndColor.B, Speed));
                    }
                }

                /// <summary>
                /// Стили анимации цвета
                /// </summary>
                public enum AnimStyleColor
                {
                    /// <summary>
                    /// Анимация фонового цвета
                    /// </summary>
                    BackColor = 1,

                    /// <summary>
                    /// Анимация внутреннего цвета
                    /// </summary>
                    ForeColor = 2
                }
            }

            /// <summary>
            /// Класс инициализации анимации текста
            /// </summary>
            public static class AnimText
            {
                /// <summary>
                /// Массив активных анимации текста
                /// </summary>
                static readonly List<Thread> ActiveElementsAnimationText = [];

                /// <summary>
                /// Массив активных инструкций анимации текста
                /// </summary>
                static readonly List<Instr_AnimText> Instr_AnimTexts = [];

                /// <summary>
                /// Закончить анимацию текста
                /// </summary>
                /// <param name="ObjName">Имя объекта анимации</param>
                public static void RemoveAnimText(string ObjName)
                {
                    for (int i = 0; i < ActiveElementsAnimationText.Count; i++)
                    {
                        if ((ActiveElementsAnimationText[i].Name?.Equals(ObjName) ?? false) && ActiveElementsAnimationText[i].IsAlive)
                        {
                            Instr_AnimTexts[i].Obj.Text = Instr_AnimTexts[i].StartText + Instr_AnimTexts[i].EndText;
                            try { ActiveElementsAnimationText[i].Interrupt(); }
                            catch { }
                            ActiveElementsAnimationText.RemoveAt(i);
                            Instr_AnimTexts.RemoveAt(i);
                            break;
                        }
                    }
                }

                /// <summary>
                /// Класс объекта инструкции анимации цвета
                /// </summary>
                public class Instr_AnimText
                {

                    /// <summary>
                    /// Динамический объект анимации
                    /// </summary>
                    public dynamic Obj { get; }

                    /// <summary>
                    /// Стартовое значение текста
                    /// </summary>
                    public string StartText { get; }

                    /// <summary>
                    /// Текст анимации
                    /// </summary>
                    public string EndText { get; private set; }

                    /// <summary>
                    /// Инициализировать инструкцию анимации текста
                    /// </summary>
                    /// <param name="obj">Объект анимации</param>
                    /// <param name="Text">Текст анимации</param>
                    public Instr_AnimText(dynamic obj, string Text)
                    {
                        try
                        {
                            _ = obj.Text;
                        }
                        catch { throw new ArgumentException("Объект анимации не потдерживает свойства Text", nameof(obj)); }
                        Obj = obj;
                        StartText = obj.Text;
                        EndText = Text;
                        Instr_AnimTexts.Add(this);
                    }

                    /// <summary>
                    /// Анимировать текст по символьно с проверкой многострочности
                    /// </summary>
                    /// <param name="MultiLine">Многострочность</param>
                    /// <param name="SelectionStart">Следить за расположением коретки в конце текста</param>
                    public void AnimInit(bool MultiLine, bool SelectionStart = false)
                    {
                        StopAnimate(StyleAnimateObj.AnimText, Obj.Name);
                        ThreadStart Animating = new(async () =>
                        {
                            if (MultiLine)
                            {
                                Obj.Text = CountSymbols(Obj.Text, "\n") >= Math.Round((Obj.Height + Obj.Font.Size) / 7) ?
                                UpdateLineOutput(Obj.Text) : Obj.Text;
                                EndText = (Obj.Text.Length > 0 ? "\n" : string.Empty) + EndText.Replace("\n", string.Empty);
                            }
                            await Task.Run(() =>
                            {
                                for (int i = 0; i < EndText.Length; i++)
                                {
                                    Obj.Text += EndText[i];
                                    if (SelectionStart) Obj.SelectionStart = Obj.TextLength;
                                    Thread.Sleep(1);
                                }
                            });
                        });
                        Thread ThAnim = new(Animating)
                        { Name = Obj.Name };
                        ActiveElementsAnimationColor.Add(ThAnim);
                        ActiveElementsAnimationColor[^1].Start();
                    }

                    /// <summary>
                    /// Сколько в Text символов Symbol
                    /// </summary>
                    /// <param name="Text">Проверяемый текст</param>
                    /// <param name="Symbol">Символ проверки</param>
                    /// <returns>int: Количество символов</returns>
                    private static int CountSymbols(string Text, string Symbol)
                    {
                        int Count = 0;
                        for (int i = 0; i < Text.Length; i++)
                        {
                            if (Convert.ToString(Text[i]).Equals(Symbol))
                                Count++;
                        }
                        return Count;
                    }

                    /// <summary>
                    /// Удалить из построчного текста InputText одну строку
                    /// </summary>
                    /// <param name="InputText">Текст с отступами \n</param>
                    /// <returns>string: Текст без одной строки сверху</returns>
                    private static string UpdateLineOutput(string InputText)
                    {
                        bool WriteUpdate = false;
                        string OutputText = string.Empty;
                        for (int i = 0; i < InputText.Length; i++)
                        {
                            if (Convert.ToString(InputText[i]).Equals("\n") && !WriteUpdate)
                                WriteUpdate = true;
                            else if (WriteUpdate)
                                OutputText += InputText[i];
                        }
                        return OutputText;
                    }
                }
            }

            /// <summary>
            /// Закрытые вспомогательные функции класса
            /// </summary>
            private static class AuxiliaryFunction
            {
                /// <summary>
                /// Разчёт координат для анимации ПОЗИЦИИ элемента
                /// </summary>
                /// <param name="Input">Стартовая позиция</param>
                /// <param name="Sign">Ориентация элемента: -1 - Влево/Вправо, 1 - Вправо/Вверх</param>
                /// <param name="Shot">Полная инструкция покадровой анимации</param>
                /// <param name="i">Позиция покадровой анимации</param>
                /// <returns>Point: Новая полная позиция элемента</returns>
                public static Point SumCoords((int, int) Input, (int, int) Sign, (List<int>, List<int>) Shot, int i)
                {
                    return new Point(
                        Input.Item1 + (Sign.Item1 * (i < Shot.Item1.Count ? Shot.Item1[i] : 0)),
                        Input.Item2 + (Sign.Item2 * (i < Shot.Item2.Count ? Shot.Item2[i] : 0))
                        );
                }

                /// <summary>
                /// Рассчитать позицию по формуле
                /// </summary>
                /// <param name="ActivateFormule">Активная формула для анимации</param>
                /// <param name="Const">Константы анимации</param>
                /// <param name="UpdatedFormule">Расчитывать ли следующую позицию по формуле</param>
                /// <param name="Accuracy">Дополнительный параметр смягчения анимации</param>
                /// <returns>float: Изменённая константа анимации, x0</returns>
                public static float FormuleSwichUpdatePosition(bool UpdatedFormule, AnimFormule.Formules ActivateFormule, AnimFormule.ConstAnimMove Const)
                {
                    if (UpdatedFormule)
                    {
                        return ActivateFormule switch
                        {
                            AnimFormule.Formules.QuickTransition => (Const.X1 - Const.X0) / Const.Y,
                            AnimFormule.Formules.FastReverseTransition => Const.Y * 2 / (Const.X1 / Const.Y) / (Math.Abs(Const.X1) - Math.Abs(Const.X0)),
                            AnimFormule.Formules.AcceleratingTransition => (Const.X1 - (Const.X1 - 1 - Const.X0)) / Const.Y,
                            _ => 0f,
                        };
                    }
                    else return 0f;
                }
            }
        }
    }
}
