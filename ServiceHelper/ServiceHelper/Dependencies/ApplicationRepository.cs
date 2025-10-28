
namespace ServiceHelper.Dependencies
{
    public class ApplicationRepository<TEntity> : RepositoryBase<TEntity, ApplicationDbContext> where TEntity : Entity, IAggregateRoot
    {
        public ApplicationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
