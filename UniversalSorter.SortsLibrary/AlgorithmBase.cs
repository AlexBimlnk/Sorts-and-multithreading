using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace UniversalSorter.SortsLibrary
{
    /// <summary>
    /// Производный класс сортировочных алгоритмов.
    /// </summary>
    /// <typeparam name="T"> Тип данных, предоставленный для сортировки. </typeparam>
    public abstract class AlgorithmBase<T> where T : IComparable
    {
        /// <summary>
        /// Число потоков, которые нужно использовать при многопоточной сортировке.
        /// </summary>
        private int _threadsMax = 2;
        /// <summary>
        /// Текущее кол-во потоков.
        /// </summary>
        protected int currentThreads = 1;
        /// <summary>
        /// Коллекция элементов.
        /// </summary>
        protected List<T> collection = new List<T>();


        /// <summary>
        /// Событие, вызываемое во время перестановке элементов.
        /// Третий параметр - id потока, в котором выполнено действие.
        /// </summary>
        public event EventHandler<Tuple<T, T, int?>> SwapEvent;
        /// <summary>
        /// Событие, вызываемое во время сравнения элементов. 
        /// Третий параметр - id потока, в котором выполнено действие.
        /// </summary>
        public event EventHandler<Tuple<T, T, int?>> CompareEvent;
        /// <summary>
        /// Событие, вызываемое во постановки элемента на указанное место. 
        /// Третий параметр - id потока, в котором выполнено действие.
        /// </summary>
        public event EventHandler<Tuple<T, int, int?>> SetEvent;


        /// <summary>
        /// Возвращает коллекцию.
        /// </summary>
        public List<T> Items => collection;
        /// <summary>
        /// Возвращает статус, отражающий кол-во потоков, которые возможно установить.
        /// </summary>
        public abstract ThreadSupport ThreadSupport { get; }
        /// <summary>
        /// Возвращает или устанавливает кол-во потоков,
        /// которые алгоритм может использовать для многопоточной сортировки.
        /// Установка потоков регламентируется свойством <see cref="ThreadSupport"/>.
        /// </summary>
        public int Threads
        {
            get
            {
                if (ThreadSupport == ThreadSupport.None)
                    return currentThreads;
                else
                    return _threadsMax;
            }
            set
            {
                if (ThreadSupport != ThreadSupport.None && value >= 2)
                {
                    if ( (ThreadSupport == ThreadSupport.Double && value == 2) 
                       || ThreadSupport != ThreadSupport.Double)
                        _threadsMax = value;
                }
            }
        }


        public AlgorithmBase() { }
        public AlgorithmBase(int countThreads) { Threads = countThreads; }
        public AlgorithmBase(IEnumerable<T> items)
        {
            collection.AddRange(items);
        }
        public AlgorithmBase(IEnumerable<T> items, int countThreads)
        {
            collection.AddRange(items);
            Threads = countThreads;
        }


        /// <summary>
        /// Вызывает стандартную реализацию сортировки.
        /// </summary>
        public abstract void StartSort();
        /// <summary>
        /// Вызывает многопоточную реализацию сортировки.
        /// </summary>
        public abstract void StartMultiThreadingSort();

        /// <summary>
        /// Ставит элемент на заданную позицию в указанной коллекции.
        /// </summary>
        /// <param name="item"> Элемент, который нужно установить. </param>
        /// <param name="position"> Позиция, на которую нужно установить элемент. </param>
        /// <param name="items"> Коллекция, в которой происходит установка элемента. </param>
        protected void Set(T item, int position, List<T> items)
        {
            if(0<=position && position < items.Count)
            {
                items[position] = item;
                SetEvent?.Invoke(this, new Tuple<T, int, int?>(item, position, Task.CurrentId));
            }
        }
        /// <summary>
        /// Переставляет два элемента местами.
        /// </summary>
        /// <param name="position1"> Номер первого элемента в коллекции. </param>
        /// <param name="position2"> Номер второго элемента в коллекции.</param>
        protected void Swap(int position1, int position2)
        {
            T item = collection[position1];
            collection[position1] = collection[position2];
            collection[position2] = item;
            SwapEvent?.Invoke(this, new Tuple<T, T, int?>(collection[position1], collection[position2], Task.CurrentId));
        }
        /// <summary>
        /// Сравнивает два элемента.
        /// Если левый элемент меньше вернет -1
        /// </summary>
        /// <param name="item1"> Первый элемент. </param>
        /// <param name="item2"> Второй элемент. </param>
        /// <returns></returns>
        protected int Compare(T item1, T item2)
        {
            CompareEvent?.Invoke(this, new Tuple<T, T, int?>(item1, item2, Task.CurrentId));
            return item1.CompareTo(item2);
        }
    }
}
