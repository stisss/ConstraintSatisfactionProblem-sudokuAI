using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace csp
{
    class Domain<T>
    {
        public List<T> Values { get; set; }


        public Domain(List<T> values)
        {
            Values = values;
        }


        public Domain(Domain<T> domain)
        {
            Values = new List<T>(domain.Values);
        }
    }
}
