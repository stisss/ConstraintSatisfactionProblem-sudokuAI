using System;
using System.Collections.Generic;

namespace csp.CSP.VariableHeuristics
{
    class SnakeVariableHeuristics<T> : IVariableHeuristics<T>
    {
        private T[] _variables;
        public T[] Variables { get => _variables; set { _variables = value; InitialiseSnakeOrder(); } }
        public List<int> Order { get; set; }

        int DEFAULT_START_IDX = 0;
        public SnakeVariableHeuristics()
        {
        }

        public int GetNext(List<int> checkedIndices, int index)
        {
            int orderIdx = Order.IndexOf(index);
            checkedIndices.Add(index);
            if (checkedIndices.Count == _variables.Length)
            {
                return -1;
            }
            return Order[orderIdx + 1];
        }

        public int GetStartingIndex()
        {
            return DEFAULT_START_IDX;
        }

        private void InitialiseSnakeOrder()
        {
            Order = new List<int>();

            int rowlength = 9;
            int counter = 0;
            bool right = true;
            while (counter < Variables.Length)
            {
                if (counter % rowlength == 0 && counter != 0)
                {
                    right = !right;
                    counter += rowlength;
                }
                if (right)
                {
                    Order.Add(counter++);
                }
                else
                {
                    Order.Add(--counter);
                }
            }
        }
    }
}
