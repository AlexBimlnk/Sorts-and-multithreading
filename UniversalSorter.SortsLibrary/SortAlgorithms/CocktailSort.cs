using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary.SortAlgorithms
{
    public class CocktailSort<T> : AlgorithmBase<T> where T : IComparable
    {
        public override ThreadSupport ThreadSupport => ThreadSupport.Infinity;

        public CocktailSort(IEnumerable<T> items) : base(items) { }
        public CocktailSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }


        public override void StartSort()
        {
            Sort(0, collection.Count);
        }
        public override Task StartMultiThreadingSort()
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

            return Task.CompletedTask;
        }

        
        private void Sort(int start, int end)
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
    }
}
