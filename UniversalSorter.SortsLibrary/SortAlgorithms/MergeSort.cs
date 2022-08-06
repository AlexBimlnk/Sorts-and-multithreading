using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary.SortAlgorithms;

/// <summary>
/// Сортировка слиянием.
/// </summary>
public class MergeSort<T> : AlgorithmBase<T> where T : IComparable
{
    private int _currentThreads = 1;
    private readonly CycleLock _cycleLock = new();

    public MergeSort(IEnumerable<T> items) : base(items) { }
    public MergeSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }

    public override ThreadSupport ThreadSupport => ThreadSupport.Infinity;

    private void Merge(List<T> left, List<T> right, List<T> outputItems)
    {
        int leftCounter = 0; int rightCounter = 0; int outputCounter = 0;

        while (leftCounter < left.Count && rightCounter < right.Count)
        {
            if (Compare(left[leftCounter], right[rightCounter]) == -1)
            {
                Set(left[leftCounter], outputCounter, outputItems);
                leftCounter++;
            }
            else
            {
                Set(right[rightCounter], outputCounter, outputItems);
                rightCounter++;
            }
            outputCounter++;
        }

        while (leftCounter < left.Count)
        {
            Set(left[leftCounter], outputCounter, outputItems);
            leftCounter++; outputCounter++;
        }
        while (rightCounter < right.Count)
        {
            Set(right[rightCounter], outputCounter, outputItems);
            rightCounter++; outputCounter++;
        }
    }
    private void Sort(List<T> items)
    {
        if (items.Count == 1)
            return;

        int mid = items.Count / 2;

        var L = items.Take(mid).ToList();
        var R = items.Skip(mid).ToList();

        Sort(L);
        Sort(R);

        Merge(L, R, items);
    }

    private async Task SortWithThreads(List<T> items)
    {
        if (items.Count == 1)
            return;

        int mid = items.Count / 2;

        var L = items.Take(mid).ToList();
        var R = items.Skip(mid).ToList();

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
            Sort(L);
            Sort(R);
        }
        else
        {
            await Task.WhenAll(
                    Task.Run(() => SortWithThreads(L)),
                    Task.Run(() => SortWithThreads(R)));
        }

        Merge(L, R, items);
    }

    public override void StartSort()
    {
        Sort(collection);
    }
    public override Task StartMultiThreadingSort()
    {
        _currentThreads = 1;
        return SortWithThreads(collection);
    }
}
