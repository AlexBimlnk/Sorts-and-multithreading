using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary.SortAlgorithms
{
    /// <summary>
    /// Сортировка выбором.
    /// </summary>
    public class SelectionSort<T> : AlgorithmBase<T> where T : IComparable
    {
        public override ThreadSupport ThreadSupport => ThreadSupport.Infinity;


        public SelectionSort() { }
        public SelectionSort(int countThreads) : base(countThreads) { }
        public SelectionSort(IEnumerable<T> items) : base(items) { }
        public SelectionSort(IEnumerable<T> items, int countThreads) : base(items, countThreads) { }


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
            for (int i = start; i < end - 1; i++)
            {
                T min = collection[i];
                int minIndex = i;
                for (int j = i + 1; j < end; j++)
                {
                    if (Compare(min, collection[j]) == 1)
                    {
                        min = collection[j];
                        minIndex = j;
                    }
                }
                Set(collection[i], minIndex, collection);
                Set(min, i, collection);
            }
        }

        private void MergeChunks(int sizeChunk)
        {
            int iteration = 1;
            while (iteration < sizeChunk + 1)
            {
                var left = collection.Take(sizeChunk * iteration).ToList();
                var right = collection.Skip(iteration * sizeChunk).Take(sizeChunk).ToList();
                iteration++;

                int leftCounter = 0; int rightCounter = 0; int outputCounter = 0;

                while (leftCounter < left.Count && rightCounter < right.Count)
                {
                    if (Compare(left[leftCounter], right[rightCounter]) == -1)
                    {
                        collection[outputCounter] = left[leftCounter];
                        leftCounter++;
                    }
                    else
                    {
                        collection[outputCounter] = right[rightCounter];
                        rightCounter++;
                    }
                    outputCounter++;
                }

                while (leftCounter < left.Count)
                {
                    collection[outputCounter] = left[leftCounter];
                    leftCounter++; outputCounter++;
                }
                while (rightCounter < right.Count)
                {
                    collection[outputCounter] = right[rightCounter];
                    rightCounter++; outputCounter++;
                }
            }

        }
    }
}
