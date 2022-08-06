using System;
using System.Collections.Generic;

namespace UniversalSorter.SortsLibrary.SortAlgorithms;

/// <summary>
/// Сортировка пузырьком.
/// </summary>
public sealed class BubbleSort<T> : AlgorithmWithChunksMergeBase<T> where T : IComparable
{
    public BubbleSort(IEnumerable<T> items) : base(items) { }
    public BubbleSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }

    protected override void Sort(int start, int end)
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
}