using System.Linq;

namespace csp.CSP.ValueHeuristics
{
    class BasicValueHeuristics<T> : IValueHeuristics<T>
    {
        public T GetNext(Domain<T> domain)
        {
                var temp = domain.Values.Last();
                domain.Values.Remove(temp);
                return temp;
        }
    }
}
