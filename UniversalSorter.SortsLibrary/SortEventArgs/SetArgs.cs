namespace UniversalSorter.SortsLibrary.SortEventArgs;

public sealed class SetEventArg<TItem> : SortEventArg
{
    public SetEventArg(int currentThreadId, TItem item, int index) 
        : base(currentThreadId)
    {
        Item = item;
        Index = index;
    }

    public TItem Item { get; }
    public int Index { get; }
}
