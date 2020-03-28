using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace csp.Variables
{
    class CSP<T,V> where T : Variable<V>
    {
        public T[][] Variables { get; set; }
        public List<Constraint<T>> Constraints { get; set; }

    }
}
