using System.Collections.Generic;

namespace csp.CSP.VariableHeuristics
{
    interface IVariableHeuristics<T>
    {
        T[] Variables { get; set; }

        int GetNext(List<int> checkedIndices, int index);

        int GetStartingIndex();
    }
}
