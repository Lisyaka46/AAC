using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAC.GUI;

namespace AAC.Classes
{
    /// <summary>
    /// Лист определённых объектов ярлыков
    /// </summary>
    /// <typeparam name="T">Объекты ярлыков</typeparam>
    public class ListLabel<T> : List<T> where T : IELLabelAccess
    {
        /// <summary>
        /// Добавить и отсортировать добавляемый ярлык
        /// </summary>
        /// <param name="Element">Добавляемый объект ярыка</param>
        public new void Add(T Element)
        {
            int NumStatus = (int)Element.Status;
            if (Count > 0)
            {
                if (NumStatus == 0 && this[^1].Status == TypeLabel.System || NumStatus == 2)
                {
                    Element.SetLocationIndex(Count);
                    base.Add(Element);
                }
                else
                {
                    int i = 0;
                    while (NumStatus >= (int)this[i].Status && i < Count) i++;
                    Element.SetLocationIndex(i);
                    Insert(i++, Element);
                    for (; i < Count; i++) this[i].SetLocationIndex(i);
                }
            }
            else base.Add(Element);
        }

        /// <summary>
        /// Удалить объект ярлыка по индексу
        /// </summary>
        /// <param name="Index">Индекс объекта ярлыка</param>
        public new void RemoveAt(int Index)
        {
            this[Index].Dispose();
            base.RemoveAt(Index);
        }
    }
}
