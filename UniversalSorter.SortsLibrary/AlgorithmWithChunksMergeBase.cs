using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary;

public abstract class AlgorithmWithChunksMergeBase<TValue> : AlgorithmBase<TValue> where TValue : IComparable
{
    protected AlgorithmWithChunksMergeBase(IEnumerable<TValue> items) : base(items) { }

    protected AlgorithmWithChunksMergeBase(IEnumerable<TValue> items, int countThreads) : base(items, countThreads) { }

    public override sealed ThreadSupport ThreadSupport => ThreadSupport.Infinity;

    protected abstract void Sort(int start, int end);

    public override sealed async Task StartMultiThreadingSort()
    {
        int chunk = collection.Count / Threads;

        await Task.WhenAll(
            Enumerable.Range(0, Threads)
                .Select(x =>
                {
                    var start = x * chunk;
                    var end = start + chunk;

                    return Task.Run(() => Sort(start, end));
                }));

        await Task.Run(() => Sort(Threads * chunk, collection.Count));

        MergeChunks(chunk);
    }
}