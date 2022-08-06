using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace UniversalSorter.SortsLibrary
{
    public class SortingStatistics<T> where T : IComparable
    {
        private int _stepIterationThreads = 1;
        private TimeSpan _timeWork;

        private readonly List<T> _items = new();

        private readonly AlgorithmBase<T> _algorithm;

        private readonly Stopwatch _stopwatch = new();
        private readonly Dictionary<int, TimeSpan> _threadsTimeMemory = new();

        private readonly StringBuilder _information = new();

        public SortingStatistics(
            AlgorithmBase<T> algorithm,
            int maxThreadsCount = 2,
            int stepIterationThreads = 1)
        {
            _algorithm = algorithm ?? throw new ArgumentNullException(nameof(algorithm));
            _items.AddRange(algorithm.Items);
            MaxThreadsCount = maxThreadsCount;
            StepIterationThreads = stepIterationThreads;

            _information.AppendLine($"General settings:\n" +
                                    $"\tSort type: {_algorithm.GetType().Name}\n" +
                                    $"\tSort support thread: {_algorithm.ThreadSupport}\n" +
                                    $"\tItems count: {ItemsCount}\n" +
                                    $"\tMaximum threads: {MaxThreadsCount}\n" +
                                    $"\tStep iteration threads: {_stepIterationThreads}");
        }

        public IReadOnlyDictionary<int, TimeSpan> GetTreadsTimeMemory => _threadsTimeMemory;
        public int ItemsCount => _algorithm.Items.Count;
        public int MinThreadsCount => 1;
        public int MaxThreadsCount { get; set; }

        /// <summary>
        /// Шаг итерационного начисления потоков для сбора статистики
        /// влияния кол-ва потоков на сортировку.
        /// </summary>
        public int StepIterationThreads
        {
            get { return _stepIterationThreads; }
            set
            {
                if (0 < value && value < MaxThreadsCount)
                {
                    _stepIterationThreads = value;
                }
            }
        }

        private void StatisticsFromMethod(Action action)
        {
            _stopwatch.Start();

            action();

            _stopwatch.Stop();
            _timeWork = _stopwatch.Elapsed;
            _stopwatch.Reset();
        }

        public static string GetStatistics(SortingStatistics<T> statistics) =>
            statistics.GetStatistics();

        public string GetStatistics() => _information.ToString();

        public void StartCollectingStatistics()
        {
            for (int i = MinThreadsCount; i <= MaxThreadsCount; i += _stepIterationThreads)
            {
                _algorithm.Threads = i;
                StatisticsFromMethod(() => _algorithm.StartMultiThreadingSort());

                _threadsTimeMemory.Add(i, _timeWork);

                _information.AppendLine($"Threads: {i}\n" +
                                        $"\tTime work: {_timeWork.TotalMilliseconds}ms\n");

                _algorithm.ResetCollection(_items);
            }

        }

        public int BetterThreadsCountOfTime() =>
            _threadsTimeMemory.MinBy(pair => pair.Value).Key;
    }
}
