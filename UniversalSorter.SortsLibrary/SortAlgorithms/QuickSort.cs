using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary.SortAlgorithms
{
    /// <summary>
    /// Быстрая сортировка.
    /// </summary>
    public class QuickSort<T> : AlgorithmBase<T> where T : IComparable
    {
        public override ThreadSupport ThreadSupport => ThreadSupport.Infinity;


        public QuickSort() { }
        public QuickSort(IEnumerable<T> items) : base(items) { }
        public QuickSort(int countThreads) : base(countThreads) { }
        public QuickSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }


        public override void StartSort()
        {
            Sort(0, collection.Count - 1);
        }
        public override void StartMultiThreadingSort()
        {
            SortWithThreads(0, collection.Count - 1);
            currentThreads = 1;
        }


        private void SortWithThreads(int start, int end)
        {
            int left, right;
            SortIteration(start, end, out left, out right);
            if (left == -1 && right == -1)
                return;

            Task leftSortThread = null;
            Task rightSortThread = null;

            if (currentThreads < Threads)
            {
                leftSortThread = new Task(() => SortWithThreads(start, right));
                rightSortThread = new Task(() => SortWithThreads(left, end));
                currentThreads++;
            }

            leftSortThread?.Start();
            rightSortThread?.Start();

            if (leftSortThread == null)
            {
                Sort(start, right);
                Sort(left, end);
            }
            else
            {
                Task.WaitAll(leftSortThread, rightSortThread);
            }
        }
        private void Sort(int start, int end)
        {
            int left, right;
            SortIteration(start, end, out left, out right);
            if (left == -1 && right == -1)
                return;
            

            Sort(start, right);
            Sort(left, end);
        }


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
    }
}
