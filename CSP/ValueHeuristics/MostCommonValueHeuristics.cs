using System;
using System.Collections.Generic;
using System.Text;

namespace csp.CSP.ValueHeuristics
{
    class MostCommonValueHeuristics<V, T> : IValueHeuristics<T> where V : Variable<T>
    {
        public V[] Variables { get; set; }

        public T GetNext(Domain<T> domain)
        {
            if (domain.Values.Count == 1)
            {
                return domain.Values[0];
            }
            else
            {
                int[] occurences = GetOccurences(domain);
                int maxIndex = GetIndexOfMax(occurences);
                var temp = domain.Values[maxIndex];
                domain.Values.Remove(temp);
                return temp;
            }

        }

        private int[] GetOccurences(Domain<T> domain)
        {
            int numberOfDifferentValues = domain.Values.Count;
            int[] occurences = new int[numberOfDifferentValues];

            for (int i = 0; i < numberOfDifferentValues; i++)
            {
                occurences[i] = 0;
            }

            for(int i = 0; i < numberOfDifferentValues; i++)
            {
                foreach(var Var in Variables)
                {
                    if(Var.Value.Equals(domain.Values[i]))
                    {
                        occurences[i]++;
                    }
                }
            }
            return occurences;
        }

        private int GetIndexOfMax(int[] occurences)
        {
            int max = 0;
            int maxIdx = 0;
            for (int i = 0; i < occurences.Length; i++)
            {
                if (max < occurences[i])
                {
                    max = occurences[i];
                    maxIdx = i;
                }
            }
            return maxIdx;
        }
    }
}
