namespace csp.CSP.ValueHeuristics
{
    interface IValueHeuristics<T>
    {
        T GetNext(Domain<T> domain);
    }
}
