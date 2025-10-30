using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHelper.Dependencies
{
    public class DateTimeValue : ValueObject
    {
        public DateTime Value { get; private set; }

        public DateTimeValue(DateTime value)
        {
            Value = value;
        }

        public static Result<DateTimeValue> Create(DateTime value)
        {
            if (value == default)
            {
                return Result.Failure<DateTimeValue>(new InvalidDateTimeError("DateTime cannot be default."));
            }

            return Result.Success(new DateTimeValue(value));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static explicit operator DateTime(DateTimeValue dateTimeValue)
        {
            return dateTimeValue.Value;
        }

        public static implicit operator DateTimeValue(DateTime value)
        {
            return new DateTimeValue(value);
        }

        public class InvalidDateTimeError : Error
        {
            public InvalidDateTimeError(string message) : base(message) { }
        }
    }
}
