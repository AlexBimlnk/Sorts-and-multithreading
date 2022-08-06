namespace UniversalSorter.SortsLibrary.SortEventArgs;

public sealed class SwapEventArg : SortEventArg
{
    public SwapEventArg(int currentThreadId, int firstIndex, int secondIndex) 
        : base(currentThreadId)
    {
        FirstIndex = firstIndex;
        SecondIndex = secondIndex;
    }

    public int FirstIndex { get; }
    public int SecondIndex { get; }
}
