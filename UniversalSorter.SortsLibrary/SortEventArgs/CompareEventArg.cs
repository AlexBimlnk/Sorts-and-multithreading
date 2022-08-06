using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSorter.SortsLibrary.SortEventArgs;
public sealed class CompareEventArg<TValue> : SortEventArg
{
    public CompareEventArg(int currentThreadId, TValue firstValue, TValue secondValue) 
        : base(currentThreadId)
    {
        FirstValue = firstValue;
        SecondValue = secondValue;
    }

    public TValue FirstValue { get; }
    public TValue SecondValue { get; }
}
