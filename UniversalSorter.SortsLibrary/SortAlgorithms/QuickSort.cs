using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary.SortAlgorithms;

/// <summary>
/// Быстрая сортировка.
/// </summary>
public class QuickSort<T> : AlgorithmBase<T> where T : IComparable
{
    private int _currentThreads = 1;
    private CycleLock _cycleLock = new();

    public QuickSort(IEnumerable<T> items) : base(items) { }
    public QuickSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }

    public override ThreadSupport ThreadSupport => ThreadSupport.Infinity;

    private void SortIteration(int start, int end, out int left, out int right)
    {
        if (start >= end)
        {
            left = -1;
            right = -1;
            return;
        }
        left = start;
        right = end;
        T mid = collection[(left + right) / 2];
        while (left <= right)
        {
            while (Compare(collection[left], mid) == -1) left++;
            while (Compare(collection[right], mid) == 1) right--;
            if (left <= right)
            {
                Swap(left, right);
                left++;
                right--;
            }
        }
    }

    private void Sort(int start, int end)
    {
        SortIteration(start, end, out int left, out int right);
        if (left == -1 && right == -1)
            return;

        Sort(start, right);
        Sort(left, end);
    }

    private async Task SortWithThreads(int start, int end)
    {
        SortIteration(start, end, out int left, out int right);
        if (left == -1 && right == -1)
            return;

        bool continueInSingleThread = true;

        _cycleLock.Enter();

        if (_currentThreads < Threads)
        {
            _currentThreads++;
            continueInSingleThread = false;
        }

        _cycleLock.Leave();

        if (continueInSingleThread)
        {
            Sort(start, right);
            Sort(left, end);
        }
        else
        {
            await Task.WhenAll(
                    Task.Run(() => SortWithThreads(start, right)),
                    Task.Run(() => SortWithThreads(left, end)));
        }
    }


    public override void StartSort()
    {
        Sort(0, collection.Count - 1);
    }

    public override Task StartMultiThreadingSort()
    {
        _currentThreads = 1;
        return SortWithThreads(0, collection.Count - 1);
    }
}