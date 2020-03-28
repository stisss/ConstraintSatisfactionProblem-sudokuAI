using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace csp
{
    class Constraint<T, V> where T : Variable<V>
    {
        private Predicate<T> _constraint;

        public Constraint(int otherIndex, ICollection variables)
        {

        }

        public Predicate<T> Constr { get => _constraint; set => _constraint = value; }
    }
}
