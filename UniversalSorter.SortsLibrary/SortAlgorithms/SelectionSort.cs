using System;
using System.Collections.Generic;

namespace UniversalSorter.SortsLibrary.SortAlgorithms;

/// <summary>
/// Сортировка выбором.
/// </summary>
public sealed class SelectionSort<T> : AlgorithmWithChunksMergeBase<T> where T : IComparable
{
    public SelectionSort(IEnumerable<T> items) : base(items) { }
    public SelectionSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }

    protected override void Sort(int start, int end)
    {
        for (int i = start; i < end - 1; i++)
        {
            T min = collection[i];
            int minIndex = i;
            for (int j = i + 1; j < end; j++)
            {
                if (Compare(min, collection[j]) == 1)
                {
                    min = collection[j];
                    minIndex = j;
                }
            }
            Set(collection[i], minIndex, collection);
            Set(min, i, collection);
        }
    }

    public override void StartSort()
    {
        Sort(0, collection.Count);
    }
}
