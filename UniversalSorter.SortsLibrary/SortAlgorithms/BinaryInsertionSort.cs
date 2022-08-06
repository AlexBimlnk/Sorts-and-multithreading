using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary.SortAlgorithms
{
    public class BinaryInsertionSort<TValue> : AlgorithmBase<TValue> where TValue : IComparable
    {
        public BinaryInsertionSort(IEnumerable<TValue> items) : base(items) { }
        public BinaryInsertionSort(IEnumerable<TValue> items, int countThreads) : base(items, countThreads) { }

        public override ThreadSupport ThreadSupport => ThreadSupport.Infinity;

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

        public override void StartSort()
        {
            Sort(0, collection.Count);
        }
        public override async Task StartMultiThreadingSort()
        {
            int chunk = collection.Count / Threads;

            await Task.WhenAll(
                Enumerable.Range(0, Threads)
                    .Select(x =>
                    {
                        var start = x * chunk;
                        var end = start + chunk;

                        return Task.Run(() => Sort(start, end));
                    }));

            //List<Task> tasks = new List<Task>();
            //int i = 0;
            //for (; i < Threads; i++)
            //{
            //    int start = i * chunk;
            //    int end = start + chunk;
            //    tasks.Add(new Task(() => Sort(start, end)));
            //    tasks[i].Start();
            //}
            //tasks.Add(new Task(() => Sort(i * chunk, collection.Count)));
            //tasks.Last().Start();

            //Task.WaitAll(tasks.ToArray());


            MergeChunks(chunk);
        }
    }
}
