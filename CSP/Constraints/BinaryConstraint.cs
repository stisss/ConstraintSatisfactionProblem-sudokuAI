namespace csp.CSP
{
    class BinaryConstraint<T, V> : IConstraint<T, V> where T : Variable<V> 
    {
        public int OtherIndex { get; set; }
        public T[] Variables { get; set; }


        public BinaryConstraint(int otherIndex, T[] variables)
        {
            OtherIndex = otherIndex;
            Variables = variables;
        }


        public bool Check(V value)
        {
            return !value.Equals(Variables[OtherIndex].Value);
        }
    }
}
