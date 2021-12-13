using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary.SortAlgorithms
{
    /// <summary>
    /// Сортировка вставками.
    /// </summary>
    public class InsertionSort<T> : AlgorithmBase<T> where T : IComparable
    {
        public override ThreadSupport ThreadSupport => ThreadSupport.Infinity;


        public InsertionSort() { }
        public InsertionSort(int countThreads) : base(countThreads) { }
        public InsertionSort(IEnumerable<T> items) : base(items) { }
        public InsertionSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }


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
                int j = i;
                while (j > start && Compare(temp, collection[j - 1]) == -1)
                {
                    Set(collection[j - 1], j, collection);
                    j--;
                }
                Set(temp, j, collection);
            }            
        }
    }
}
