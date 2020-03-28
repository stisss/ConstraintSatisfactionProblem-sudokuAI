using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace csp
{
    abstract class Variable<T> : IComparable
    {
        public Domain<T> Domain { get; set; }
        public T Value { get; set; }



        public int CompareTo(object other)
        {
            if (other == null) return 1;

            Variable<T> v = other as Variable<T>;

            if (v.Domain.Values.Count < this.Domain.Values.Count)
                return -1;
            else
                return 1;
        }
    }
}
