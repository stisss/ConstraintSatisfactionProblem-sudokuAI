using System;
using System.Collections.Generic;
using System.Linq;

namespace csp.CSP.VariableHeuristics
{
    class RandomStaticVariableHeuristics<T> : IVariableHeuristics<T>
    {
        private T[] _variables;
        public T[] Variables { get=>_variables; set { _variables = value; InitialiseRandomOrder();} }
        public List<int> Order { get; set; }

        private Random random = new Random();

        int DEFAULT_START_IDX = 0;


        public RandomStaticVariableHeuristics()
        {
        }


        public int GetNext(List<int> checkedIndices, int index)
        {
            //int temp;
            //int orderIdx = Order.IndexOf(index);
            //if (orderIdx == Order.Count-1)
            //{
            //    temp = Order.Count;
            //}
            //else
            //{
            //    temp = Order[orderIdx + 1];
            //}
            //checkedIndices.Add(temp);
            //return temp;

            int orderIdx = Order.IndexOf(index);
            checkedIndices.Add(index);
            if(checkedIndices.Count == _variables.Length)
            {
                return -1;
            }
            return Order[orderIdx + 1];
        }

        private void InitialiseRandomOrder()
        {
            Order = new List<int>();
            for (int i = 0; i < Variables.Length; i++)
            {
                Order.Add(i);
            }
            Order = Shuffle(Order);

            //for (int i = 0; i < Variables.Length; i++)
            //{
            //    Console.Write(Order[i] + " ");
            //}
            //Console.WriteLine();
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

        public int GetStartingIndex()
        {
            return Order[DEFAULT_START_IDX];
        }
    }

}
