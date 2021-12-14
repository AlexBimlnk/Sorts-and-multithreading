using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary.SortAlgorithms
{
    public class ShellSort<T> : AlgorithmBase<T> where T : IComparable
    {
        public override ThreadSupport ThreadSupport => ThreadSupport.Infinity;


        public ShellSort() { }
        public ShellSort(int countThreads) : base(countThreads) { }
        public ShellSort(IEnumerable<T> items) : base(items) { }
        public ShellSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }


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
            int step = (end - start) / 2;

            while (step > 0)
            {
                for (int i = start + step; i < end; i++)
                {
                    int j = i;
                    while ((j >= start + step) && Compare(collection[j], collection[j - step]) == -1)
                    {
                        Swap(j, j - step);
                        j -= step;
                    }
                }
                step /= 2;
            }
        }
    }
}
