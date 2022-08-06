using System;

namespace UniversalSorter.SortsLibrary.SortEventArgs;

public class SortEventArg : EventArgs
{
    public SortEventArg(int currentThreadId)
    {
        CurrentThreadId = currentThreadId;
    }

    public int CurrentThreadId { get; }
}
