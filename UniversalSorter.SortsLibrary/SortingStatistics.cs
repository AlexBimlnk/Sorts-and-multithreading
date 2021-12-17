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
        private const int _minThreadsCount = 2;

        private Stopwatch _stopwatch = new Stopwatch();
        private AlgorithmBase<T> _algorithm;

        private List<T> _items = new List<T>();
        private TimeSpan _timeWork;
        private double _memoryUsed = 0;
        private int _maxThreadsCount = 2;
        private int _stepIterationThreads = 1;
        private Dictionary<int, Tuple<TimeSpan, double>> _threadsTimeMemory = new Dictionary<int, Tuple<TimeSpan, double>>();

        private StringBuilder _information = new StringBuilder();


        /// <summary>
        /// Возвращает время потраченное на сортировку коллекции.
        /// </summary>

        public TimeSpan TimeWork => _timeWork;
        public double TimeWorkInMs => _timeWork.Milliseconds;
        public double MemoryUsed => _memoryUsed;
        public Dictionary<int, Tuple<TimeSpan, double>> GetTreadsTimeMemory => _threadsTimeMemory;
        public int ItemsCount { get; private set; }
        public int MinThreadsCount => _minThreadsCount;
        public int MaxThreadsCount
        {
            get { return _maxThreadsCount; }
            set
            {
                if (2 <= MaxThreadsCount)
                {
                    _maxThreadsCount = value;
                }
            }
        }
        /// <summary>
        /// Шаг итерационного начисления потоков для сбора статистики
        /// влияния кол-ва потоков на сортировку.
        /// </summary>
        public int StepIterationThreads
        {
            get { return _stepIterationThreads; }
            set
            {
                if (0 < value && value < _maxThreadsCount)
                {
                    _stepIterationThreads = value;
                }
            }
        }

        public static StringBuilder GetStatistics(SortingStatistics<T> statistics)
        {
            return statistics.GetStatistics();
        }

        public SortingStatistics(AlgorithmBase<T> algorithm, int maxThreadsCount = 2,
                                 int stepIterationThreads = 1)
        {
            _algorithm = algorithm;
            _items.AddRange(algorithm.Items);
            ItemsCount = algorithm.Items.Count;
            MaxThreadsCount = maxThreadsCount;
            StepIterationThreads = stepIterationThreads;

            _information.AppendLine($"General settings:\n" +
                                    $"\tSort type: {_algorithm.GetType().Name}\n" +
                                    $"\tSort support thread: {_algorithm.ThreadSupport}\n" +
                                    $"\tItems count: {ItemsCount}\n" +
                                    $"\tMaximum threads: {_maxThreadsCount}\n" +
                                    $"\tStep iteration threads: {_stepIterationThreads}");
        }


        public void StartCollectingStatistics()
        {
            StatisticsFromMethod(() => _algorithm.StartSort());

            _information.AppendLine($"Threads: 1\n" +
                                    $"\tTime work: {_timeWork.Milliseconds}ms\n" +
                                    $"\tMemory used: {_memoryUsed}MByte");

            ResetItems();
            
            Thread.Sleep(500);

            for(int i = _minThreadsCount; i<=MaxThreadsCount; i += _stepIterationThreads)
            {
                _algorithm.Threads = i;
                StatisticsFromMethod(() => _algorithm.StartMultiThreadingSort());

                _threadsTimeMemory.Add(i, new Tuple<TimeSpan, double>(_timeWork, _memoryUsed));

                _information.AppendLine($"Threads: {i}\n" +
                                        $"\tTime work: {_timeWork.Milliseconds}ms\n" +
                                        $"\tMemory used: {_memoryUsed}MByte");

                ResetItems();

                Thread.Sleep(500);
            }
            
        }

        public StringBuilder GetStatistics()
        {
            return _information;
        }

        public int BetterThreadsCountOfTime()
        {
            TimeSpan minTime = _threadsTimeMemory.Values.ToList()[0].Item1;
            int betterThreadsCount = _threadsTimeMemory.Keys.ToList()[0];

            foreach(var i in _threadsTimeMemory.Keys)
            {
                if(_threadsTimeMemory[i].Item1 < minTime)
                {
                    minTime = _threadsTimeMemory[i].Item1;
                    betterThreadsCount = i;
                }
            }

            return betterThreadsCount;
        }

        private void StatisticsFromMethod(Action action)
        {
            long before = GC.GetTotalMemory(false);
            _stopwatch.Start();

            action();

            _stopwatch.Stop();
            long after = GC.GetTotalMemory(false);

            _memoryUsed = (after - before) / (1024 * 1024);
            _timeWork = _stopwatch.Elapsed;
            _stopwatch.Reset();
            GC.Collect();
        }

        private void ResetItems()
        {
            _algorithm.Items.Clear();
            _algorithm.Items.AddRange(_items);
        }
    }
}
