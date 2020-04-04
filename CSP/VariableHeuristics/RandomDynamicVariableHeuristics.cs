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
            checkedIndices.Add(uncheckedIndices[randomIdx]);
            return uncheckedIndices[randomIdx];
        }
    }
}
