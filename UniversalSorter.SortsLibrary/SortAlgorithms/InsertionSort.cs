using System;
using System.Collections.Generic;

namespace UniversalSorter.SortsLibrary.SortAlgorithms;

/// <summary>
/// Сортировка вставками.
/// </summary>
public sealed class InsertionSort<T> : AlgorithmWithChunksMergeBase<T> where T : IComparable
{
    public InsertionSort(IEnumerable<T> items) : base(items) { }
    public InsertionSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }

    protected override void Sort(int start, int end)
    {
        for (int i = start + 1; i < end; i++)
        {
            var temp = collection[i];
            int j = i;
            while (j > start && Compare(temp, collection[j - 1]) == -1)
            {
                Set(collection[j - 1], j, collection);
                j--;
            }
            Set(temp, j, collection);
        }
    }

    public override void StartSort()
    {
        Sort(0, collection.Count);
    }
}
