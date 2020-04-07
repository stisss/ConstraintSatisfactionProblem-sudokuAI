using System;

namespace csp.CSP.ValueHeuristics
{
    class RandomValueHeuristics<T> : IValueHeuristics<T>
    {
        public T GetNext(Domain<T> domain)
        {
            Random random = new Random();
            int index = random.Next(domain.Values.Count);
            T temp = domain.Values[index];
            domain.Values.Remove(temp);
            return temp;
        }
    }
}
