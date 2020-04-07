using System;
using System.Collections.Generic;
using System.Text;

namespace csp.CSP.VariableHeuristics
{
    class BasicVariableHeuristics<T> : IVariableHeuristics<T>
    {
        public T[] Variables { get; set; }
        public BasicVariableHeuristics()
        {
        }

        public int GetNext(List<int> checkedIndices, int index)
        {
            int next = index + 1;
            checkedIndices.Add(index);
            return next;
        }

        public int GetStartingIndex()
        {
            return 0;
        }
    }
}
