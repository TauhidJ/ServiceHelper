

namespace ServiceHelper.Specification
{
    public class IncludeQuery<TEntity, TPreviousProperty> : IIncludeQuery<TEntity, TPreviousProperty>, IIncludeQuery
    {
        public Dictionary<IIncludeQuery, string> PathMap { get; } = new Dictionary<IIncludeQuery, string>();


        public IncludeVisitor Visitor { get; } = new IncludeVisitor();


        public HashSet<string> Paths => PathMap.Select<KeyValuePair<IIncludeQuery, string>, string>((KeyValuePair<IIncludeQuery, string> x) => x.Value).ToHashSet();

        public IncludeQuery(Dictionary<IIncludeQuery, string> pathMap)
        {
            PathMap = pathMap;
        }
    }
}
