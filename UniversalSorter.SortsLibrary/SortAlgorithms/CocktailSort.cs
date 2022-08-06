using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary.SortAlgorithms;

public sealed class CocktailSort<T> : AlgorithmWithChunksMergeBase<T> where T : IComparable
{
    public CocktailSort(IEnumerable<T> items) : base(items) { }
    public CocktailSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }

    protected override void Sort(int start, int end)
    {
        int leftPointer = start;
        int rightPointer = end - 1;

        while (leftPointer < rightPointer)
        {
            var swapCount = 0;

            for (int i = leftPointer; i < rightPointer; i++)
            {
                if (Compare(collection[i], collection[i + 1]) == 1)
                {
                    Swap(i, i + 1);
                    swapCount++;
                }
            }
            rightPointer--;

            if (swapCount == 0)
            {
                break;
            }

            for (int i = rightPointer; i > leftPointer; i--)
            {
                if (Compare(collection[i], collection[i - 1]) == -1)
                {
                    Swap(i, i - 1);
                    swapCount++;
                }
            }
            leftPointer++;

            if (swapCount == 0)
            {
                break;
            }
        }
    }

    public override void StartSort()
    {
        Sort(0, collection.Count);
    }
}