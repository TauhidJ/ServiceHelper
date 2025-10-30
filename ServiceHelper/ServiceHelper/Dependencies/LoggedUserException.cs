using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHelper.Dependencies
{
    public class LoggedUserException : Exception
    {
        public LoggedUserException(string message)
        : base(message)
        {
        }

        public LoggedUserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
