using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UniversalSorter.SortsLibrary.SortAlgorithms;

namespace UniversalSorter.SortsLibrary.Tests
{
    [TestClass()]
    public class SortTests
    {
        private static Random rnd = new Random();

        private static List<int> inputList = new List<int>();
        private static List<int> sortedList = new List<int>();

        private static void M1(object sender, Tuple<int, int, int?> tuple)
        {
            Debug.WriteLine($"M1| {((AlgorithmBase<int>)sender).GetType()}|Thread id: {tuple.Item3}");
        }

        [ClassInitialize()]
        public static void Init(TestContext context)
        {
            for (int i = 0; i < 10000; i++)
            {
                inputList.Add(rnd.Next(0, 1000));
            }

            sortedList.AddRange(inputList.OrderBy(x => x).ToArray());
            Debug.WriteLine($"Elemets: {inputList.Count/1000}k");
        }


        [TestMethod, TestCategory("StatMerge"), Ignore]
        public void SortingStatisticsTest()
        {
            // arrange
            var sortAlgorithm1 = new MergeSort<int>(inputList);
            sortAlgorithm1.Threads = 2;
            var sortAlgorithm2 = new MergeSort<int>(inputList);
            sortAlgorithm2.Threads = 4;
            var sortAlgorithm3 = new MergeSort<int>(inputList);
            sortAlgorithm3.Threads = 8;



            // act
            SortingStatistics<int>.Statistics(sortAlgorithm1);
            SortingStatistics<int>.Statistics(sortAlgorithm2);
            SortingStatistics<int>.Statistics(sortAlgorithm3);

            // assert
            Assert.IsTrue(true);
        }


        [TestMethod, TestCategory(".NET Sort")]
        public void StandartSort()
        {
            // arrange
            List<int> myCollection = new List<int>();
            myCollection.AddRange(inputList);

            // act
            myCollection.Sort();

            // assert
            for (int i = 0; i < inputList.Count; i++)
            {
                Assert.AreEqual(sortedList[i], myCollection[i]);
            }
        }


        [TestMethod(), TestCategory("MergeSortAlgorithm")]
        public void MergeSortTest()
        {
            // arrange
            var sortAlgorithm = new MergeSort<int>(inputList);
            //sortAlgorithm.CompareEvent += M1;

            // act
            sortAlgorithm.StartSort();

            // assert
            for (int i = 0; i < inputList.Count; i++)
            {
                Assert.AreEqual(sortedList[i], sortAlgorithm.Items[i]);
            }
        }
        [TestMethod(), TestCategory("MergeSortAlgorithm")]
        public void Merge2ThreadingSortTest()
        {
            // arrange
            var sortAlgorithm = new MergeSort<int>(inputList);
            sortAlgorithm.Threads = 2;
            //sortAlgorithm.CompareEvent += M1;

            // act
            sortAlgorithm.StartMultiThreadingSort();

            // assert
            for (int i = 0; i < inputList.Count; i++)
            {
                Assert.AreEqual(sortedList[i], sortAlgorithm.Items[i]);
            }
        }


        [TestMethod(), TestCategory("BubbleSortAlgorithm"), Ignore]
        public void BubbleSortTest()
        {
            // arrange
            var sortAlgorithm = new BubbleSort<int>(inputList);
            //sortAlgorithm.CompareEvent += M1;

            // act
            sortAlgorithm.StartSort();

            // assert
            for (int i = 0; i < inputList.Count; i++)
            {
                Assert.AreEqual(sortedList[i], sortAlgorithm.Items[i]);
            }
        }
        [TestMethod(), TestCategory("BubbleSortAlgorithm")]
        public void Bubble100ThreadingSortTest()
        {
            // arrange
            var sortAlgorithm = new BubbleSort<int>(inputList);
            //sortAlgorithm.CompareEvent += M1;
            sortAlgorithm.Threads = 100; //100 Потоков давали максимальный прирост производительности на 20к

            // act
            sortAlgorithm.StartMultiThreadingSort();

            // assert
            for (int i = 0; i < inputList.Count; i++)
            {
                Assert.AreEqual(sortedList[i], sortAlgorithm.Items[i]);
            }
        }


        [TestMethod(), TestCategory("QuickSortAlgorithm")]
        public void QuickSortTest()
        {
            // arrange
            var sortAlgorithm = new QuickSort<int>(inputList);
            //sortAlgorithm.CompareEvent += M1;

            // act
            sortAlgorithm.StartSort();

            // assert
            for (int i = 0; i < inputList.Count; i++)
            {
                Assert.AreEqual(sortedList[i], sortAlgorithm.Items[i]);
            }
        }
        [TestMethod(), TestCategory("QuickSortAlgorithm")]
        public void Quick2ThreadingSortTest()
        {
            // arrange
            var sortAlgorithm = new QuickSort<int>(inputList);
            //sortAlgorithm.CompareEvent += M1;
            sortAlgorithm.Threads = 8;

            // act
            sortAlgorithm.StartMultiThreadingSort();

            // assert
            for (int i = 0; i < inputList.Count; i++)
            {
                Assert.AreEqual(sortedList[i], sortAlgorithm.Items[i]);
            }
        }


        [TestMethod(), TestCategory("SelectionSortAlgorithm")]
        public void SelectionSortTest()
        {
            // arrange
            var sortAlgorithm = new SelectionSort<int>(inputList);
            //sortAlgorithm.CompareEvent += M1;

            // act
            sortAlgorithm.StartSort();

            // assert
            for (int i = 0; i < inputList.Count; i++)
            {
                Assert.AreEqual(sortedList[i], sortAlgorithm.Items[i]);
            }
        }
        [TestMethod(), TestCategory("SelectionSortAlgorithm")]
        public void Selection100ThreadingSortTest()
        {
            // arrange
            var sortAlgorithm = new SelectionSort<int>(inputList);
            //sortAlgorithm.CompareEvent += M1;
            sortAlgorithm.Threads = 100;

            // act
            sortAlgorithm.StartMultiThreadingSort();

            // assert
            for (int i = 0; i < inputList.Count; i++)
            {
                Assert.AreEqual(sortedList[i], sortAlgorithm.Items[i]);
            }
        }
    }
}
