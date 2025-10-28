using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceHelper.Dependencies
{
    public class SpecificationEvaluator<T> where T : Entity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            IQueryable<T> queryable = inputQuery;
            if (specification.Criteria != null)
            {
                queryable = queryable.Where(specification.Criteria);
            }

            queryable = specification.Includes.Aggregate(queryable, (current, include) => current.Include(include));
            queryable = specification.IncludeStrings.Aggregate(queryable, (current, include) => current.Include(include));
            if (specification.OrderBy != null)
            {
                queryable = queryable.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                queryable = queryable.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.GroupBy != null)
            {
                queryable = queryable.GroupBy(specification.GroupBy).SelectMany((x) => x);
            }

            return queryable;
        }
    }
}
