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
    public override ThreadSupport ThreadSupport => ThreadSupport.Infinity;


    public BubbleSort(IEnumerable<T> items) : base(items) { }
    public BubbleSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }


    public override void StartSort()
    {
        Sort(0, collection.Count);
    }
    public override Task StartMultiThreadingSort()
    {
        int chunk = collection.Count / Threads;
        List<Task> tasks = new List<Task>();
        int i = 0;
        for(; i < Threads; i++)
        {
            int start = i * chunk;
            int end = start + chunk;
            tasks.Add(new Task(() => Sort(start, end)));
            tasks[i].Start();
        }
        tasks.Add(new Task(() => Sort(i * chunk, collection.Count)));
        tasks.Last().Start();

        Task.WaitAll(tasks.ToArray());

        MergeChunks(chunk);

        return Task.CompletedTask;
    }


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
}