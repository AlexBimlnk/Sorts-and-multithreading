using System.Threading;

namespace UniversalSorter.SortsLibrary;

internal struct CycleLock
{
    private int _resourseInUse;

    public void Enter()
    {
        while (true)
        {
            if (Interlocked.Exchange(ref _resourseInUse, 1) == 0)
                return;
        }
    }

    public void Leave()
    {
        Volatile.Write(ref _resourseInUse, 0);
    }
}