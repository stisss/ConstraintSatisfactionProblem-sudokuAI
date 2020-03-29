namespace csp.CSP
{
    class UnaryConstraint<T, V> : IConstraint<T, V> where T : Variable<V>
    {
        public V Value { get; set; }

        public UnaryConstraint(V value)
        {
            Value = value;
        }

        public bool Check(V value)
        {
            return value.Equals(Value);
        }
    }
}
