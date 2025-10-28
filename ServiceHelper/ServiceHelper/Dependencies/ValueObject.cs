namespace ServiceHelper.Dependencies
{
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (left == null ^ right == null)
            {
                return false;
            }

            return left?.Equals(right) ?? true;
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !EqualOperator(left, right);
        }

        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            ValueObject obj2 = (ValueObject)obj;
            IEnumerator<object> enumerator = GetAtomicValues().GetEnumerator();
            IEnumerator<object> enumerator2 = obj2.GetAtomicValues().GetEnumerator();
            while (enumerator.MoveNext() && enumerator2.MoveNext())
            {
                if (enumerator.Current == null ^ enumerator2.Current == null)
                {
                    return false;
                }

                if (enumerator.Current != null && !enumerator.Current.Equals(enumerator2.Current))
                {
                    return false;
                }
            }

            if (!enumerator.MoveNext())
            {
                return !enumerator2.MoveNext();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (from x in GetAtomicValues()
                    select x?.GetHashCode() ?? 0).Aggregate((x, y) => x ^ y);
        }

        public ValueObject GetCopy()
        {
            return MemberwiseClone() as ValueObject;
        }
    }
}
