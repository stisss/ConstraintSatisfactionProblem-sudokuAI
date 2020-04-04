using System;
using System.Collections.Generic;

namespace csp.CSP.VariableHeuristics
{
    class RandomStaticVariableHeuristics<T> : IVariableHeuristics<T>
    {
        public T[] Variables { get; set; }
        public List<int> Order { get; set; }

        private Random random = new Random();


        public RandomStaticVariableHeuristics(T[] variables)
        {
            Variables = variables;
            InitialiseRandomOrder();
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

        private void InitialiseRandomOrder()
        {
            for(int i = 0; i < Variables.Length; i++)
            {
                Order[i] = i;
            }
            Order = Shuffle(Order);
        }

        public List<int> Shuffle(List<int> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }

}
