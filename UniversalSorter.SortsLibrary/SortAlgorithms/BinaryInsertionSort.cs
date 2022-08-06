using System;
using System.Collections.Generic;

namespace UniversalSorter.SortsLibrary.SortAlgorithms;

public sealed class BinaryInsertionSort<TValue> : AlgorithmWithChunksMergeBase<TValue> where TValue : IComparable
{
    public BinaryInsertionSort(IEnumerable<TValue> items) : base(items) { }
    public BinaryInsertionSort(IEnumerable<TValue> items, int countThreads) : base(items, countThreads) { }

    protected override void Sort(int start, int end)
    {
        for (int i = start + 1; i < end; i++)
        {
            var temp = collection[i];
            int right = i - 1;
            int left = start;
            int mid = (left + right) / 2;
            while (left <= right)
            {
                if (Compare(temp, collection[mid]) == -1)
                    right = mid - 1;
                else
                    left = mid + 1;
                mid = (left + right) / 2;
            }
            for (int j = i - 1; j >= left; j--)
            {
                Set(collection[j], j + 1, collection);
            }
            Set(temp, left, collection);
        }
    }

    public override void StartSort()
    {
        Sort(0, collection.Count);
    }
}