using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace UniversalSorter.SortsLibrary.SortAlgorithms;

/// <summary>
/// Сортировка пузырьком.
/// </summary>
public class BubbleSort<T> : AlgorithmBase<T> where T : IComparable
{
    public BubbleSort(IEnumerable<T> items) : base(items) { }
    public BubbleSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }

    public override ThreadSupport ThreadSupport => ThreadSupport.Infinity;

    private void Sort(int start, int end)
    {
        for (int i = 0; i < end - start; i++)
        {
            for (int j = start; j < end - i - 1; j++)
            {
                if (Compare(collection[j + 1], collection[j]) == -1)
                {
                    Swap(j + 1, j);
                }
            }
        }
    }

    public override void StartSort()
    {
        Sort(0, collection.Count);
    }

    public override async Task StartMultiThreadingSort()
    {
        int chunk = collection.Count / Threads;

        await Task.WhenAll(
                Enumerable.Range(0, Threads)
                    .Select(x =>
                    {
                        var start = x * chunk;
                        var end = start + chunk;

                        return Task.Run(() => Sort(start, end));
                    }));

        await Task.Run(() => Sort(Threads * chunk, collection.Count));

        MergeChunks(chunk);
    }
}