using System.Linq.Expressions;

namespace ServiceHelper.Specification
{
    public class IncludeAggregator<TEntity>
    {
        public IncludeQuery<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> selector)
        {
            IncludeVisitor includeVisitor = new IncludeVisitor();
            includeVisitor.Visit(selector);
            Dictionary<IIncludeQuery, string> dictionary = new Dictionary<IIncludeQuery, string>();
            IncludeQuery<TEntity, TProperty> includeQuery = new IncludeQuery<TEntity, TProperty>(dictionary);
            if (!string.IsNullOrEmpty(includeVisitor.Path))
            {
                dictionary[includeQuery] = includeVisitor.Path;
            }

            return includeQuery;
        }
    }
}
