using System;
using System.Collections.Generic;
using System.Text;

namespace csp
{
    class Constraint<T> where T : class
    {
        private Predicate<T> _constraint;

        public Constraint(Predicate<T> constraint)
        {
            _constraint = constraint;
        }

        public Predicate<T> Constr { get => _constraint; set => _constraint = value; }
    }
}
