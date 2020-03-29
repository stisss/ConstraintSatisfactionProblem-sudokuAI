using System;

namespace csp
{
    abstract class Variable<T> : IComparable
    {
        public Domain<T> Domain { get; set; }
        public T Value { get; set; }


        public int CompareTo(object other)
        {
            if (other == null) 
                return 1;
            else 
                return -1;
        }
    }
}
