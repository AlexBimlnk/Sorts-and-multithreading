using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary.SortAlgorithms
{
    public class MergeSort<T> : AlgorithmBase<T> where T : IComparable
    {
        public override ThreadSupport ThreadSupport => ThreadSupport.Infinity;

        public MergeSort() { }

        public MergeSort(int countThreads) { Threads = countThreads; }

        public MergeSort(IEnumerable<T> items) : base(items) { }

        public MergeSort(IEnumerable<T> items, int countThreads) : base(items) { Threads = countThreads; }



        public override void StartSort()
        {
            collection = Sort(collection);
        }

        public override void StartMultiThreadingSort()
        {
            SortWithThreads(collection);
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

            if (currentThreads < Threads)
            {
                leftSortThread = new Task(()=>SortWithThreads(L));
                rightSortThread = new Task(() => SortWithThreads(R));
                currentThreads++;
            }

            leftSortThread?.Start();
            rightSortThread?.Start();

            if (leftSortThread == null)
            {
                SortWithThreads(L);
                SortWithThreads(R);
            }
            else if (rightSortThread == null) 
            {
                SortWithThreads(R);
                Task.WaitAll(leftSortThread);
            }
            else
            {
                Task.WaitAll(leftSortThread, rightSortThread);
            }

            int i = 0; int j = 0; int k = 0;

            while (i < L.Count && j < R.Count)
            {
                if (Compare(L[i], R[j]) == -1)
                {
                    items[k] = L[i];
                    i++;
                }
                else
                {
                    items[k] = R[j];
                    j++;
                }
                k++;
            }

            while (i < L.Count)
            {
                items[k] = L[i];
                i++; k++;
            }
            while (j < R.Count)
            {
                items[k] = R[j];
                j++; k++;
            }
        }

        private List<T> Sort(List<T> items)
        {
            if (items.Count() == 1)
                return items;

            int mid = items.Count() / 2;

            var L = items.Take(mid).ToList();
            var R = items.Skip(mid).ToList();

            return Merge(Sort(L), Sort(R));
        }

        private List<T> Merge(List<T> leftCollection, List<T> rightCollection)
        {
            int i = 0; int j = 0;
            var result = new List<T>();

            var mergeTest = new List<T>();

            while (i < leftCollection.Count && j < rightCollection.Count)
            {
                if (Compare(leftCollection[i], rightCollection[j]) == -1)
                {
                    result.Add(leftCollection[i]);
                    i++;
                }
                else
                {
                    result.Add(rightCollection[j]);
                    j++;
                }
            }

            while (i < leftCollection.Count)
            {
                result.Add(leftCollection[i]);
                i++;
            }
            while (j < rightCollection.Count)
            {
                result.Add(rightCollection[j]);
                j++;
            }

            
            return result;
        }
    }
}
