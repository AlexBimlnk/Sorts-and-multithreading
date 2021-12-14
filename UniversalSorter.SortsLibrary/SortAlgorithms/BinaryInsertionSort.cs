using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary.SortAlgorithms
{
    public class BinaryInsertionSort<T> : AlgorithmBase<T> where T : IComparable
    {
        public override ThreadSupport ThreadSupport => ThreadSupport.Infinity;


        public BinaryInsertionSort() { }
        public BinaryInsertionSort(int countThreads) : base(countThreads) { }
        public BinaryInsertionSort(IEnumerable<T> items) : base(items) { }
        public BinaryInsertionSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }

        
        public override void StartSort()
        {
            Sort(0, collection.Count);
        }
        public override void StartMultiThreadingSort()
        {
            int chunk = collection.Count / Threads;
            List<Task> tasks = new List<Task>();
            int i = 0;
            for (; i < Threads; i++)
            {
                int start = i * chunk;
                int end = start + chunk;
                tasks.Add(new Task(() => Sort(start, end)));
                tasks[i].Start();
            }
            tasks.Add(new Task(() => Sort(i * chunk, collection.Count)));
            tasks.Last().Start();

            Task.WaitAll(tasks.ToArray());


            MergeChunks(chunk);
        }


        private void Sort(int start, int end)
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
    }
}
