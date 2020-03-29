using csp.CSP;
using System.Collections.Generic;

namespace csp.Variables
{
    abstract class CSP<T,V> where T : Variable<V>
    {
        public T[] Variables { get; set; }
        public List<IConstraint<T, V>>[] Constraints { get; set; }

        public List<V[]> Solutions;



    }
}
