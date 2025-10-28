using ServiceHelper.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHelper
{
    public interface IIncludeQuery
    {
        Dictionary<IIncludeQuery, string> PathMap { get; }

        IncludeVisitor Visitor { get; }

        HashSet<string> Paths { get; }
    }
}
