using System;
using System.Collections.Generic;

namespace UniversalSorter.SortsLibrary.SortAlgorithms;

public sealed class ShellSort<T> : AlgorithmWithChunksMergeBase<T> where T : IComparable
{
    public ShellSort(IEnumerable<T> items) : base(items) { }
    public ShellSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }

    protected override void Sort(int start, int end)
    {
        int step = (end - start) / 2;

        while (step > 0)
        {
            for (int i = start + step; i < end; i++)
            {
                int j = i;
                while ((j >= start + step) && Compare(collection[j], collection[j - step]) == -1)
                {
                    Swap(j, j - step);
                    j -= step;
                }
            }
            step /= 2;
        }
    }

    public override void StartSort()
    {
        Sort(0, collection.Count);
    }
}
