using System;
using System.Collections.Generic;
using System.Text;

namespace csp.CSP
{
    interface IConstraint<T, V>
    {
        bool Check(V value);
    }
}
