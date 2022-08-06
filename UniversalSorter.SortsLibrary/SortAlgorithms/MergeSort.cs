using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary.SortAlgorithms
{
    /// <summary>
    /// Сортировка слиянием.
    /// </summary>
    public class MergeSort<T> : AlgorithmBase<T> where T : IComparable
    {
        private int _currentThreads = 1;
        public override ThreadSupport ThreadSupport => ThreadSupport.Infinity;


        public MergeSort(IEnumerable<T> items) : base(items) { }
        public MergeSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }


        public override void StartSort()
        {
            Sort(collection);
        }
        public override Task StartMultiThreadingSort()
        {
            SortWithThreads(collection);
            _currentThreads = 1;
            return Task.CompletedTask;
        }


        private void SortWithThreads(List<T> items)
        {
            if (items.Count() == 1)
                return;

            int mid = items.Count() / 2;

            var L = items.Take(mid).ToList();
            var R = items.Skip(mid).ToList();

            Task leftSortThread = null;
            Task rightSortThread = null;

            if (_currentThreads < Threads)
            {
                leftSortThread = new Task(()=>SortWithThreads(L));
                rightSortThread = new Task(() => SortWithThreads(R));
                _currentThreads++;
            }

            leftSortThread?.Start();
            rightSortThread?.Start();

            if (leftSortThread == null)
            {
                SortWithThreads(L);
                SortWithThreads(R);
            }
            else
            {
                Task.WaitAll(leftSortThread, rightSortThread);
            }

            Merge(L, R, items);
        }
        private void Sort(List<T> items)
        {
            if (items.Count() == 1)
                return;

            int mid = items.Count() / 2;

            var L = items.Take(mid).ToList();
            var R = items.Skip(mid).ToList();

            Sort(L);
            Sort(R);

            Merge(L, R, items);
        }


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
    }
}
