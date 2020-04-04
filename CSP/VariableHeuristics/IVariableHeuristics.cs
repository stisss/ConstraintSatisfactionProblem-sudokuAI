using System;
using System.Collections.Generic;
using System.Text;

namespace csp.CSP.VariableHeuristics
{
    interface IVariableHeuristics<T>
    {
        T[] Variables { get; set; }

        int GetNext(List<int> checkedIndices, int index);
    }
}
