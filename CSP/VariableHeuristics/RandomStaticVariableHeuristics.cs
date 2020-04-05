using System;
using System.Collections.Generic;
using System.Linq;

namespace csp.CSP.VariableHeuristics
{
    class RandomStaticVariableHeuristics<T> : IVariableHeuristics<T>
    {
        private T[] _variables;
        public T[] Variables { get=>_variables; set { _variables = value; InitialiseRandomOrder(); Order.Remove(DEFAULT_START_IDX);} }
        public List<int> Order { get; set; }

        private Random random = new Random();

        int DEFAULT_START_IDX = 0;


        public RandomStaticVariableHeuristics()
        {
        }


        public int GetNext(List<int> checkedIndices, int index)
        {
            var temp = Order.Last();
            Order.Remove(temp);
            checkedIndices.Add(temp);
            return temp;
        }

        private void InitialiseRandomOrder()
        {
            Order = new List<int>();
            for (int i = 0; i < Variables.Length; i++)
            {
                Order.Add(i);
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
