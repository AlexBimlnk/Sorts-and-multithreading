using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary.Tests
{
    [TestClass()]
    public class SortTests
    {
        private static Random rnd = new Random();

        private static List<int> inputList = new List<int>();
        private static List<int> sortedList = new List<int>();

        [ClassInitialize()]
        public static void Init(TestContext context)
        {
            for (int i = 0; i < 100; i++)
            {
                inputList.Add(rnd.Next(0, 1000));
            }

            sortedList.AddRange(inputList.OrderBy(x => x).ToArray());
        }

        [TestMethod(), TestCategory("BaseAlgorithm")]
        public void AlgorithmBaseSortTest()
        {
            // arrange
            var baseAlgorithm = new AlgorithmBase<int>(inputList);

            // act
            baseAlgorithm.StartSort();

            // assert
            for (int i = 0; i < inputList.Count; i++)
            {
                Assert.AreEqual(sortedList[i], baseAlgorithm.Items[i]);
            }
        }

        [TestMethod(), TestCategory("BaseAlgorithm")]
        public void AlgorithmBaseMultiThreadingSortTest()
        {
            // arrange
            var baseAlgorithm = new AlgorithmBase<int>(inputList);

            // act-assert
            Assert.ThrowsException<NotSupportedException>(() => baseAlgorithm.StartMultiThreadingSort());
        }
    }
}
