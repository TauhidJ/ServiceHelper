using System.Linq.Expressions;

namespace ServiceHelper.Dependencies
{
    public interface IRepository<TEntity> where TEntity : Entity, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        Task<TEntity?> GetByIdAsync(params object?[]? keyValues);

        Task<TEntity?> GetAsync(ISpecification<TEntity> spec);

        Task<IReadOnlyList<TEntity>> ListAllAsync();

        Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<int> CountAsync(ISpecification<TEntity> spec);

        Task<bool> AnyAsync(ISpecification<TEntity> spec);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> criteria);

        bool Any(ISpecification<TEntity> spec);

        bool Any(Expression<Func<TEntity, bool>> criteria);
    }
}
