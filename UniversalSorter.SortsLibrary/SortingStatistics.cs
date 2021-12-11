using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary
{
    public class SortingStatistics<T> where T : IComparable
    {
        private static Stopwatch stopwatch = new Stopwatch();
        public static void Statistics(AlgorithmBase<T> algorithm)
        {
            stopwatch.Start();
            long before = GC.GetTotalMemory(false);            
            algorithm.StartSort();
            stopwatch.Stop();
            long after = GC.GetTotalMemory(false);
            double consumedInMegabytes = (after - before) / (1024 * 1024);

            long timeDefaultSort = stopwatch.ElapsedMilliseconds;
            double memoryDefaultSort = consumedInMegabytes;

            GC.Collect();
            Thread.Sleep(1000);


            stopwatch.Start();
            before = GC.GetTotalMemory(false);
            algorithm.StartMultiThreadingSort();
            stopwatch.Stop();
            after = GC.GetTotalMemory(false);
            consumedInMegabytes = (after - before) / (1024 * 1024);

            long timeMultithreadSort = stopwatch.ElapsedMilliseconds;
            double memoryMultithreadSort = consumedInMegabytes;

            Debug.WriteLine($"Sort: {algorithm.GetType()}\n" +
                            $"Threads support: {algorithm.ThreadSupport}\n" +
                            $"Threads: {algorithm.Threads}\n" +
                            $"Count elemets: {algorithm.Items.Count/1000}k\n" +
                            $"Time default sort: {timeDefaultSort}ms\n" +
                            $"Memory default sort used: {memoryDefaultSort}MByte\n" +
                            $"Time multithread sort: {timeMultithreadSort}ms\n" +
                            $"Memory multithread sort used: {memoryMultithreadSort}MByte\n");
            GC.Collect();
            Thread.Sleep(1000);
        }
    }
}
