using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniversalSorter.SortsLibrary.SortEventArgs;

namespace UniversalSorter.SortsLibrary;

/// <summary>
/// Производный класс сортировочных алгоритмов.
/// </summary>
/// <typeparam name="TValue"> Тип данных, предоставленный для сортировки. </typeparam>
public abstract class AlgorithmBase<TValue> where TValue : IComparable
{
    private int _currentThreads = 1;

    /// <summary>
    /// Коллекция элементов.
    /// </summary>
    protected List<TValue> collection = new();

    protected AlgorithmBase(IEnumerable<TValue> items)
    {
        collection.AddRange(items ?? throw new ArgumentNullException(nameof(items)));
    }
    protected AlgorithmBase(IEnumerable<TValue> items, int countThreads)
        : this(items)
    {
        if (countThreads < 1 || ThreadSupport is ThreadSupport.None && countThreads > 1)
            throw new ArgumentOutOfRangeException(nameof(countThreads));

        _currentThreads = countThreads;
    }

    /// <summary>
    /// Возвращает статус, отражающий кол-во потоков, которые возможно установить.
    /// </summary>
    public abstract ThreadSupport ThreadSupport { get; }

    /// <summary>
    /// Возвращает сортируемую коллекцию.
    /// </summary>
    public IReadOnlyCollection<TValue> Items => collection;

    /// <summary>
    /// Возвращает или устанавливает кол-во потоков,
    /// которые алгоритм может использовать для многопоточной сортировки.
    /// Установка потоков регламентируется свойством <see cref="SortsLibrary.ThreadSupport"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Возникает при устанлвке значения, 
    /// если число устанавливаемых потоков меньше единицы.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Возникает при установке значения, 
    /// когда сортировка не поддерживает многопоточность.
    /// </exception>
    public int Threads
    {
        get { return _currentThreads; }
        set
        {
            if (ThreadSupport is not ThreadSupport.None)
            {
                _currentThreads = value switch
                {
                    < 1 => throw new ArgumentOutOfRangeException(nameof(value)),
                    _ => _currentThreads = value,
                };
            }
            else
                throw new InvalidOperationException($"Thread support is: {ThreadSupport}");
        }
    }

    /// <summary>
    /// Событие, вызываемое во время перестановке элементов.
    /// </summary>
    public event EventHandler<SwapEventArg> SwapEvent;
    /// <summary>
    /// Событие, вызываемое во время сравнения элементов. 
    /// </summary>
    public event EventHandler<CompareEventArg<TValue>> CompareEvent;
    /// <summary>
    /// Событие, вызываемое во постановки элемента на указанное место. 
    /// </summary>
    public event EventHandler<SetEventArg<TValue>> SetEvent;

    private void ThrowIfInvalidIndexes(IReadOnlyCollection<TValue> items, params int[] indexes)
    {
        if (indexes.Any(index => index < 0 || index >= items.Count))
            throw new ArgumentOutOfRangeException(nameof(indexes));
    }

    /// <summary>
    /// Ставит элемент на заданную позицию в указанной коллекции.
    /// </summary>
    /// <param name="item"> Элемент, который нужно установить. </param>
    /// <param name="position"> Позиция, на которую нужно установить элемент. </param>
    /// <param name="items"> Коллекция, в которой происходит установка элемента. </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Когда позиция выходит за границы коллекции.
    /// </exception>
    protected void Set(TValue item, int position, List<TValue> items)
    {
        ThrowIfInvalidIndexes(items, position);

        items[position] = item;
        SetEvent?.Invoke(this, new SetEventArg<TValue>(Thread.CurrentThread.ManagedThreadId, item, position));
    }

    /// <summary>
    /// Переставляет два элемента местами.
    /// </summary>
    /// <param name="position1"> Номер первого элемента в коллекции. </param>
    /// <param name="position2"> Номер второго элемента в коллекции.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Когда любая из позиций выходит за границы коллекции.
    /// </exception>
    protected void Swap(int position1, int position2)
    {
        ThrowIfInvalidIndexes(collection, position1, position2);

        (collection[position2], collection[position1]) = (collection[position1], collection[position2]);
        SwapEvent?.Invoke(this, new SwapEventArg(Thread.CurrentThread.ManagedThreadId, position1, position2));
    }

    /// <summary>
    /// Сравнивает два элемента.
    /// Если левый элемент меньше вернет -1
    /// </summary>
    /// <param name="item1"> Первый элемент. </param>
    /// <param name="item2"> Второй элемент. </param>
    /// <returns></returns>
    protected int Compare(TValue item1, TValue item2)
    {
        CompareEvent?.Invoke(this, new CompareEventArg<TValue>(Thread.CurrentThread.ManagedThreadId, item1, item2));
        return item1.CompareTo(item2);
    }

    /// <summary>
    /// Этот метод производит слияние отсортированных кусков массива.
    /// Используется для слияния кусков, полученных в результате многопоточной
    /// итерационной сортировки.
    /// </summary>
    /// <param name="sizeChunk"> Размер чанка массива. </param>
    protected void MergeChunks(int sizeChunk)
    {
        int iteration = 1;
        while (iteration < collection.Count / sizeChunk + 1)
        {
            var left = collection.Take(sizeChunk * iteration).ToList();
            var right = collection.Skip(iteration * sizeChunk).Take(sizeChunk).ToList();

            int leftCounter = 0; int rightCounter = 0; int outputCounter = 0;

            while (leftCounter < left.Count && rightCounter < right.Count)
            {
                if (Compare(left[leftCounter], right[rightCounter]) == -1)
                {
                    collection[outputCounter] = left[leftCounter];
                    leftCounter++;
                }
                else
                {
                    collection[outputCounter] = right[rightCounter];
                    rightCounter++;
                }
                outputCounter++;
            }

            while (leftCounter < left.Count)
            {
                collection[outputCounter] = left[leftCounter];
                leftCounter++; outputCounter++;
            }
            while (rightCounter < right.Count)
            {
                collection[outputCounter] = right[rightCounter];
                rightCounter++; outputCounter++;
            }

            iteration++;
        }
    }

    /// <summary>
    /// Записывает новую коллекцию для сортировки.
    /// </summary>
    /// <param name="items"> Коллекция, которую нужно отсортировать. </param>
    /// <exception cref="ArgumentNullException"> 
    /// Когда <paramref name="items"/> равен <see langword="null"/>.
    /// </exception>
    public void ResetCollection(IEnumerable<TValue> items)
    {
        collection.Clear();
        collection.AddRange(items ?? throw new ArgumentNullException(nameof(items)));
    }

    /// <summary>
    /// Вызывает стандартную реализацию сортировки.
    /// </summary>
    public abstract void StartSort();
    /// <summary>
    /// Вызывает многопоточную реализацию сортировки.
    /// </summary>
    public abstract Task StartMultiThreadingSort();
}