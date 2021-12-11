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
        public BubbleSort() { }
        public BubbleSort(IEnumerable<T> items) : base(items) { }


        public override void StartSort()
        {
            for (int i = 0; i < collection.Count; i++)
            {
                for (int j = 0; j < collection.Count - i - 1; j++)
                {
                    if (Compare(collection[j+1], collection[j]) == -1)
                    {
                        Swap(j + 1, j);
                    }
                }
            }
        }

        public override void StartMultiThreadingSort()
        {
            base.StartMultiThreadingSort();
        }
    }
}
