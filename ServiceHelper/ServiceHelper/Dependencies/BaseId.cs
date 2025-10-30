using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServiceHelper.Dependencies
{
    public abstract class BaseId
    {
        public string Value { get; }

        protected BaseId(string value)
        {
            Value = value;
        }

        protected static void EnsureValidNumericId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException("value");
            }

            if (!Regex.IsMatch(value, "^\\d+(:\\d+)*$"))
            {
                throw new ArgumentException("Invalid format of the value.", "value");
            }
        }

        public bool IsChildOf(BaseId parent)
        {
            return parent.IsParentOf(this);
        }

        public bool IsParentOf(BaseId child)
        {
            if (child.Value.Length <= Value.Length)
            {
                return false;
            }

            int i;
            for (i = 0; i < Value.Length; i++)
            {
                if (!Value[i].Equals(child.Value[i]))
                {
                    return false;
                }
            }

            return child.Value[i] == ':';
        }

        public override bool Equals(object obj)
        {
            if (obj is BaseId baseId)
            {
                return Value.Equals(baseId.Value, StringComparison.Ordinal);
            }

            if (obj is string value)
            {
                return Value.Equals(value, StringComparison.Ordinal);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode(StringComparison.Ordinal);
        }

        public override string? ToString()
        {
            return Value;
        }

        public static explicit operator string?(BaseId? zouid)
        {
            return zouid?.Value;
        }
    }
}
