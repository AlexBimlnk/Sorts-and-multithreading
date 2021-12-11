using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary
{
    /// <summary>
    /// Производный класс сортировочных алгоритмов.
    /// </summary>
    /// <typeparam name="T"> Тип данных, предоставленный для сортировки. </typeparam>
    public class AlgorithmBase<T> where T : IComparable
    {
        /// <summary>
        /// Коллекция элементов.
        /// </summary>
        protected readonly List<T> collection = new List<T>();

        /// <summary>
        /// Событие, вызываемое во время перестановки элементов.
        /// </summary>
        public event EventHandler<Tuple<T, T>> SwopEvent;
        /// <summary>
        /// Событие, вызываемое во время сравнения элементов.
        /// </summary>
        public event EventHandler<Tuple<T, T>> CompareEvent;
        /// <summary>
        /// Событие, вызываемое во время начала сортировки.
        /// </summary>
        public event EventHandler<Action> StartSortEvent;
        
        /// <summary>
        /// Возвращает коллекцию.
        /// </summary>
        public List<T> Items => collection;


        public AlgorithmBase() { }

        public AlgorithmBase(IEnumerable<T> items)
        {
            collection.AddRange(items);
        }


        /// <summary>
        /// Вызывает стандартную реализацию сортировки.
        /// </summary>
        public void StartSort()
        {
            StartSortEvent?.Invoke(this, DefaultSort);
            DefaultSort();
        }
        /// <summary>
        /// Вызывает многопоточную реализацию сортировки.
        /// </summary>
        public void StartMultiThreadingSort()
        {
            StartSortEvent?.Invoke(this, MultiThreadingSort);
            MultiThreadingSort();
        }


        /// <summary>
        /// Стандартная реализация сортировки.
        /// </summary>
        protected virtual void DefaultSort()
        {
            collection.Sort();
        }
        /// <summary>
        /// Многопоточная реализация сортировки.
        /// </summary>
        protected virtual void MultiThreadingSort()
        {
            throw new NotSupportedException();
        }


        /// <summary>
        /// Переставляет два элемента местами.
        /// </summary>
        /// <param name="position1"> Номер первого элемента в коллекции. </param>
        /// <param name="position2"> Номер второго элемента в коллекции.</param>
        protected void Swop(int position1, int position2)
        {
            T item = collection[position1];
            collection[position1] = collection[position2];
            collection[position2] = item;
            SwopEvent?.Invoke(this, new Tuple<T, T>(collection[position1], collection[position2]));
        }
        /// <summary>
        /// Сравнивает два элемента.
        /// </summary>
        /// <param name="item1"> Первый элемент. </param>
        /// <param name="item2"> Второй элемент. </param>
        /// <returns></returns>
        protected int Compare(T item1, T item2)
        {
            CompareEvent?.Invoke(this, new Tuple<T, T>(item1, item2));
            return item1.CompareTo(item2);
        }
    }
}
