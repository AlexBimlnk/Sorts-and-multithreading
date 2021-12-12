using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace UniversalSorter.SortsLibrary.SortAlgorithms
{
    public class BubbleSort<T> : AlgorithmBase<T> where T : IComparable
    {
        private bool swapped = true;

        public override ThreadSupport ThreadSupport => ThreadSupport.Infinity;

        public BubbleSort() { }
        public BubbleSort(IEnumerable<T> items) : base(items) { }


        public override void StartSort()
        {
            Sort(0, collection.Count);
        }

        public override void StartMultiThreadingSort()
        {
            int chunk = collection.Count / Threads;
            Task[] tasks = new Task[Threads];
            for(int i = 0; i<tasks.Length; i++)
            {
                int start = i * chunk;
                int end = start + chunk;
                tasks[i] = new Task(() => Sort(start, end));
                tasks[i].Start();
            }
            Task.WaitAll(tasks);

            MergeChunks(chunk);
        }

        private void Sort(int start, int end)
        {
            for (int i = 0; i < end - start; i++)
            {
                for (int j = start; j < end - i - 1; j++)
                {
                    if (Compare(collection[j + 1], collection[j]) == -1)
                    {
                        Swap(j + 1, j);
                    }
                }
            }
        }

        private void MergeChunks(int sizeChunk)
        {
            int iteration = 1;
            while(iteration < collection.Count / sizeChunk)
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
