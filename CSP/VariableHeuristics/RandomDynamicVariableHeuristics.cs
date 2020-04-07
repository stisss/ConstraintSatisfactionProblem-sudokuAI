using System;
using System.Collections.Generic;

namespace csp.CSP.VariableHeuristics
{
    class RandomDynamicVariableHeuristics<T> : IVariableHeuristics<T>
    {
        public T[] Variables { get; set; }

        public RandomDynamicVariableHeuristics()
        {
        }


        public int GetNext(List<int> checkedIndices, int index)
        {
            checkedIndices.Add(index);
            var uncheckedIndices = new List<int>();
            for (int i = 0; i < Variables.Length; i++)
            {
                if (!checkedIndices.Contains(i))
                {
                    uncheckedIndices.Add(i);
                }
            }

            Random random = new Random();
            int randomIdx = random.Next(uncheckedIndices.Count);
            return uncheckedIndices.Count == 0 ? -1 : uncheckedIndices[randomIdx];
        }


        public int GetStartingIndex()
        {
            return 0;
        }
    }
}
