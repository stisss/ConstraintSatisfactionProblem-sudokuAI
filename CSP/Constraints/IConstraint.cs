namespace csp.CSP
{
    interface IConstraint<T, V>
    {
        bool Check(V value);
    }
}
