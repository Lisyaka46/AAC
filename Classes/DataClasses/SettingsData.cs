using System.Reflection;
using System.Text.RegularExpressions;

namespace AAC.Classes.DataClasses
{

    /// <summary>
    /// Класс настроек
    /// </summary>
    public partial class SettingsData
    {
        /// <summary>
        /// Объект параметра настроек
        /// </summary>
        public class SettingsBoolParameter
        {
            /// <summary>
            /// Параметр настроек
            /// </summary>
            private bool Parameter { get; set; }

            /// <summary>
            /// Параметр от которого зависит исходный
            /// </summary>
            public SettingsBoolParameter? ConditionParameter { get; }

            /// <summary>
            /// Реальное состояние параметра
            /// </summary>
            public bool Realy { get => Parameter; }

            /// <summary>
            /// Имя параметра
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Узнать зависимость параметра
            /// </summary>
            /// <returns>Bool: Зависит ли параметр от другого или нет</returns>
            public bool Dependence { get => ConditionParameter != null; }

            /// <summary>
            /// Обычное состояние параметра (может отличаться с реальным .Realy)
            /// </summary>
            public bool Value
            {
                get => (ConditionParameter?.Realy ?? true) && Parameter;
                set => Parameter = value;
            }

            /// <summary>
            /// Создать параметр зависящий от учловного парметра
            /// </summary>
            /// <param name="Param">Значение главного параметра</param>
            /// <param name="Condition">Условный параметр</param>
            public SettingsBoolParameter(string Name, bool Param, SettingsBoolParameter Condition)
            {
                this.Name = Name;
                ConditionParameter = Condition;
                Parameter = Param;
            }

            /// <summary>
            /// Создать независящий параметр от условия
            /// </summary>
            /// <param name="Param">Значение главного параметра</param>
            public SettingsBoolParameter(string Name, bool Param)
            {
                this.Name = Name;
                ConditionParameter = null;
                Parameter = Param;
            }

            /// <summary>
            /// Преобразование параметра в его зачение
            /// </summary>
            /// <param name="Param"></param>
            public static explicit operator bool(SettingsBoolParameter Param) => Param.Value;
        }

        /// <summary>
        /// Объект параметра настроек
        /// </summary>
        /// <remarks>
        /// Создать параметр
        /// </remarks>
        /// <param name="Param">Значение количественного параметра</param>
        /// <param name="Name">Имя параметра</param>
        public class SettingsParameter(string Name, object Param)
        {
            /// <summary>
            /// Параметр настроек
            /// </summary>
            private object Parameter { get; set; } = Param;

            /// <summary>
            /// Имя параметра
            /// </summary>
            public string Name { get; } = Name;

            /// <summary>
            /// Обычное состояние Параметра
            /// </summary>
            public object Value { get => Parameter; set => Parameter = value; }
        }

        /// <summary>
        /// Путь к файлу настроек
        /// </summary>
        public const string DitectoryOptionFile = "Data/Info/Option.r1";

        /// <summary>
        /// Параметр настроек <b>"Градиент специального цвета SC"</b>
        /// </summary>
        public SettingsParameter Color_Gradient_SC { get; }

        /// <summary>
        /// Параметр настроек <b>"Размер текста в консольной строке"</b>
        /// </summary>
        public SettingsParameter Font_Size_Console_Text { get; }

        /// <summary>
        /// Параметр настроек <b>"Выключение Alt режима при закрытии PAC"</b>
        /// </summary>
        public SettingsBoolParameter Alt_Diactivate_PAC { get; }

        /// <summary>
        /// Параметр настроек <b>"Использование специальных цветов"</b>
        /// </summary>
        public SettingsBoolParameter All_SpecialColor_Activate { get; }

        /// <summary>
        /// Параметр настроек <b>"Специальный цвет RGB"</b>
        /// </summary>
        public SettingsBoolParameter SpecialColor_RGB { get; }

        /// <summary>
        /// Параметр настроек <b>"Специальный цвет RGBCC"</b>
        /// </summary>
        public SettingsBoolParameter SpecialColor_RGBCC { get; }

        /// <summary>
        /// Параметр настроек <b>"Специальный цвет SC"</b>
        /// </summary>
        public SettingsBoolParameter SpecialColor_SC { get; }

        /// <summary>
        /// Параметр настроек <b>"Режим разработчика"</b>
        /// </summary>
        public SettingsBoolParameter Developer_Mode { get; }

        /// <summary>
        /// Параметр настроек <b>"Активация голосовых команд"</b>
        /// </summary>
        public SettingsBoolParameter Activation_Microphone { get; }

        /// <summary>
        /// Параметр настроек <b>"Изменение позиции главной формы за границей экрана"</b>
        /// </summary>
        public SettingsBoolParameter Moving_Border_Screen_Form { get; }

        /// <summary>
        /// Параметр настроек <b>"Подсказки к командам"</b>
        /// </summary>
        public SettingsBoolParameter Hit_Panel { get; }

        /// <summary>
        /// Кол-во вместимых объектов в буфер
        /// </summary>
        public SettingsParameter Buffer_Count_Elements { get; private set; }

        /// <summary>
        /// Параметр настроек <b>"Использовать LAlt режим только для включения, RAlt только для выключения Alt режима в PAC"</b>
        /// </summary>
        public SettingsBoolParameter Alt_OrientationLR_PAC { get; }

        /// <summary>
        /// Клавиша для отключения Alt режима в PAC
        /// </summary>
        public SettingsParameter HC_Alt_Diactivate_PAC { get; private set; }

        /// <summary>
        /// Клавиша для включения Alt режима в PAC
        /// </summary>
        public SettingsParameter HC_Alt_Activate_PAC { get; private set; }

        /// <summary>
        /// Инициализировать все начальные настройки
        /// </summary>
        public SettingsData()
        {
            Color_Gradient_SC = new(nameof(Color_Gradient_SC), Color.Black);
            Font_Size_Console_Text = new(nameof(Font_Size_Console_Text), 10);
            All_SpecialColor_Activate = new(nameof(All_SpecialColor_Activate), true);
            SpecialColor_RGB = new(nameof(SpecialColor_RGB), true, All_SpecialColor_Activate);
            SpecialColor_RGBCC = new(nameof(SpecialColor_RGBCC), true, All_SpecialColor_Activate);
            SpecialColor_SC = new(nameof(SpecialColor_SC), true, All_SpecialColor_Activate);
            Developer_Mode = new(nameof(Developer_Mode), false);
            Activation_Microphone = new(nameof(Activation_Microphone), true);
            Moving_Border_Screen_Form = new(nameof(Moving_Border_Screen_Form), false);
            Buffer_Count_Elements = new(nameof(Buffer_Count_Elements), 50);
            Hit_Panel = new(nameof(Hit_Panel), true);
            Alt_Diactivate_PAC = new(nameof(Alt_Diactivate_PAC), true);
            Alt_OrientationLR_PAC = new(nameof(Alt_OrientationLR_PAC), false);
            HC_Alt_Activate_PAC = new(nameof(HC_Alt_Activate_PAC), Keys.Menu);
            HC_Alt_Diactivate_PAC = new(nameof(HC_Alt_Diactivate_PAC), Keys.Menu);
            if (File.Exists(DitectoryOptionFile))
            {
                Startcs.ObjLog.LOGTextAppend("Открыто чтение файла Option.r1");
                SettingInitialValues(this);
            }
            else
            {
                Startcs.ObjLog.LOGTextAppend("Проигнорирована настройка начальных значений параметров из-за отсутствия файла настроек. Создан новый файл Option.r1");
                File.Create(DitectoryOptionFile);
            }
        }

        [GeneratedRegex(@"=[^:]+")]
        private static partial Regex RegexPatternNameOption();

        [GeneratedRegex(@" [^;]+")]
        private static partial Regex RegexPatternParamOption();

        /// <summary>
        /// Присвоить все данные согласно файлу Option.r1
        /// </summary>
        /// <param name="Data">Настройки куда ссылается объект</param>
        private static void SettingInitialValues(SettingsData Data)
        {
            static void Fail(string TypeName, string Name, string Value) => Startcs.ObjLog.LOGTextAppend($"FAIL => {TypeName} ({Name}: \"{Value}\")");
            static void SetValueBool(SettingsBoolParameter boolParameter, string Value)
            {
                bool BoolParam = Value.Equals("1");
                if (BoolParam || Value.Equals("0")) boolParameter.Value = BoolParam;
                else Fail(boolParameter.GetType().Name, nameof(boolParameter), Value);
            }
            static void SetValue(SettingsParameter Parameter, object Value)
            {
                if (Parameter.Value.GetType() == typeof(Color))
                {
                    MatchCollection ColorMatch = ThemeData.RegexPatternColorPatamTheme().Matches((string)Value);
                    if (Convert.ToInt32(ColorMatch[0].Value) > 255 || Convert.ToInt32(ColorMatch[1].Value) > 255 || Convert.ToInt32(ColorMatch[2].Value) > 255)
                        Fail(Parameter.GetType().Name, nameof(Parameter), Value.ToString() ?? string.Empty);
                    else Parameter.Value = Color.FromArgb(Convert.ToInt32(ColorMatch[0].Value), Convert.ToInt32(ColorMatch[1].Value), Convert.ToInt32(ColorMatch[2].Value));
                }
                else if (Parameter.Value.GetType() == typeof(int))
                {
                    if (!MiniFunctions.Stringint(Value.ToString() ?? string.Empty)) Parameter.Value = Convert.ToInt32(Value);
                    else Fail(Parameter.Value.GetType().Name, nameof(Parameter), Value.ToString() ?? string.Empty);
                }
                else
                {
                    if (Parameter.Value.GetType().Name.Equals(Value.GetType().Name)) Parameter.Value = Value;
                    else Fail(Parameter.GetType().Name, nameof(Parameter), Value.ToString() ?? string.Empty);
                }
            }

            string[] CollectionName = [.. RegexPatternNameOption().Matches(File.ReadAllText(DitectoryOptionFile)).Select(i => i.Value[1..])];
            string[] CollectionValue = [.. RegexPatternParamOption().Matches(File.ReadAllText(DitectoryOptionFile)).Select(i => i.Value[1..])];

            for (int i = 0; i < CollectionName.Length; i++)
            {
                Startcs.ObjLog.LOGTextAppend($"{i + 1}/{CollectionName.Length}. Имя: {CollectionName[i]} / Параметр: {CollectionValue[i]}");
                switch (CollectionName[i])
                {
                    case nameof(Hit_Panel):
                    case nameof(Developer_Mode):
                    case nameof(Activation_Microphone):
                    case nameof(Alt_Diactivate_PAC):
                    case nameof(All_SpecialColor_Activate):
                    case nameof(SpecialColor_RGB):
                    case nameof(SpecialColor_RGBCC):
                    case nameof(SpecialColor_SC):
                    case nameof(Moving_Border_Screen_Form):
                    case nameof(Alt_OrientationLR_PAC):
                        if (Data.GetType().GetProperty(CollectionName[i])?.GetValue(Data, null) is SettingsBoolParameter BoolParameter) SetValueBool(BoolParameter, CollectionValue[i]);
                        else Startcs.ObjLog.LOGTextAppend($"Параметр был проигнорирован. (Нулевой объект чтения {CollectionName[i]} => SettingsBoolParameter)");
                        break;

                    case nameof(Buffer_Count_Elements):
                    case nameof(Font_Size_Console_Text):
                    case nameof(Color_Gradient_SC):
                        if (Data.GetType().GetProperty(CollectionName[i])?.GetValue(Data, null) is SettingsParameter Parameter) SetValue(Parameter, CollectionValue[i]);
                        else Startcs.ObjLog.LOGTextAppend($"Параметр был проигнорирован. (Нулевой объект чтения {CollectionName[i]} => SettingsParameter)");
                        break;

                    case nameof(HC_Alt_Activate_PAC):
                    case nameof(HC_Alt_Diactivate_PAC):
                        if (Data.GetType().GetProperty(CollectionName[i])?.GetValue(Data, null) is SettingsParameter KeyParameter)
                        {
                            SetValue(KeyParameter, Enum.Parse(typeof(Keys), CollectionValue[i]));
                        }
                        else Startcs.ObjLog.LOGTextAppend($"Параметр был проигнорирован. (Нулевой объект чтения {CollectionName[i]} => SettingsParameter)");
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Установка параметра или нового значения
        /// </summary>
        /// <param name="Name">Имя параметра</param>
        /// <param name="NewValue">Новое значение параметра</param>
        /// <returns>Состояние выполнения действия</returns>
        public bool SetParamOption(string Name, object NewValue)
        {
            if (File.Exists(DitectoryOptionFile))
            {
                if (NewValue.GetType() == typeof(Color)) NewValue = $"={((Color)NewValue).R};{((Color)NewValue).G};{((Color)NewValue).B}";
                else if (NewValue.GetType() == typeof(bool)) NewValue = (bool)NewValue ? "1" : "0";

                string[] AllLines = File.ReadAllLines(DitectoryOptionFile);
                if (AllLines.Any(i => i.Contains($"={Name}:")))
                {
                    AllLines = [.. AllLines.Select(i =>
                    {
                        if (i.Contains($"={Name}:")) return $"={Name}: {NewValue};";
                        else return i;
                    })];
                }
                else AllLines = [.. AllLines.Append($"={Name}: {NewValue};")];
                File.WriteAllLines(DitectoryOptionFile, AllLines);
                return true;
            }
            else throw new Exception($"При изменении параметра \"{Name}\" на значение \"{NewValue}\" активная директория не распазнала путь \"..{DitectoryOptionFile}\"");
        }
    }
}
