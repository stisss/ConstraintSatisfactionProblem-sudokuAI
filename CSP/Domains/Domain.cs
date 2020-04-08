using System.Collections.Generic;

namespace csp
{
    class Domain<T>
    {
        public List<T> Values { get; set; }


        public Domain(List<T> values)
        {
            Values = new List<T>(values);
        }


        public Domain(Domain<T> domain)
        {
            Values = new List<T>(domain.Values);
        }
    }
}
