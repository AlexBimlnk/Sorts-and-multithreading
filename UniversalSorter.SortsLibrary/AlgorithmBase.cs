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
    public class AlgorithmBase<T> where T : IComparable
    {
        /// <summary>
        /// Коллекция элементов.
        /// </summary>
        protected List<T> collection = new List<T>();

        /// <summary>
        /// Число потоков, которые нужно использовать при многопоточной сортировке.
        /// </summary>
        private int _threads = 2;
        /// <summary>
        /// Текущее кол-во потоков.
        /// </summary>
        protected int currentThreads = 1;

        /// <summary>
        /// Событие, вызываемое во время перестановке элементов.
        /// Третий параметр - id потока, в котором выполнено действие.
        /// </summary>
        public event EventHandler<Tuple<T, T, int?>> SwopEvent;
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
        public virtual ThreadSupport ThreadSupport => ThreadSupport.None;

        public int Threads
        {
            get
            {
                if (ThreadSupport == ThreadSupport.None)
                    return 0;
                else
                    return _threads;
            }
            set
            {
                if (ThreadSupport != ThreadSupport.None && value >= 2)
                {
                    if ( (ThreadSupport == ThreadSupport.Double && value == 2) 
                       || ThreadSupport != ThreadSupport.Double)
                        _threads = value;
                }
            }
        }

        public AlgorithmBase() { }

        public AlgorithmBase(IEnumerable<T> items)
        {
            collection.AddRange(items);
        }


        /// <summary>
        /// Вызывает стандартную реализацию сортировки.
        /// </summary>
        public virtual void StartSort()
        {
            BaseSort();
        }
        /// <summary>
        /// Вызывает многопоточную реализацию сортировки.
        /// </summary>
        public virtual void StartMultiThreadingSort()
        {
            // TODO: Clear this method.
            //Task task1 = new Task(() =>
            //{
            //    Debug.WriteLine($"Start task1 id : {Task.CurrentId}");
            //    Debug.WriteLine($"Task1 id: {Task.CurrentId}");
            //    Debug.WriteLine($"End task1 id : {Task.CurrentId}");
            //});
            //Task task2 = new Task(() =>
            //{
            //    Debug.WriteLine($"Start task2 id : {Task.CurrentId}");
            //    Debug.WriteLine(Thread.CurrentThread.Name);
            //    Debug.WriteLine("Start await 2s");
            //    Thread.Sleep(200);
            //    Debug.WriteLine($"Task2 id: {Task.CurrentId}");
            //    Debug.WriteLine($"End task2 id : {Task.CurrentId}");
            //});
            //Task task3 = new Task(() =>
            //{
            //    Debug.WriteLine($"Start task3 id : {Task.CurrentId}");
            //    Debug.WriteLine($"Task3 id: {Task.CurrentId}");
            //    Debug.WriteLine($"End task3 id : {Task.CurrentId}");
            //});
            //task1.Start();
            //task2.Start();
            //task3.Start();
            //Task.WaitAll(task1, task2, task3);
            //Debug.WriteLine(task2.IsCompleted);
            throw new NotSupportedException();
        }

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
            SwopEvent?.Invoke(this, new Tuple<T, T, int?>(collection[position1], collection[position2], Task.CurrentId));
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

        private void BaseSort()
        {
            collection.Sort();
        }
    }
}
