using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ServiceHelper.Dependencies
{
    public abstract class RepositoryBase<TEntity, TDbContext> : IRepository<TEntity> where TEntity : Entity, IAggregateRoot where TDbContext : DbContextBase<TDbContext>
    {
        protected readonly TDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public RepositoryBase(TDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task<int> CountAsync(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task<TEntity?> GetByIdAsync(params object?[]? keyValues)
        {
            return await _context.Set<TEntity>().FindAsync(keyValues);
        }

        public async Task<TEntity?> GetAsync(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<TEntity>> ListAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<bool> AnyAsync(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).AnyAsync();
        }

        public bool Any(ISpecification<TEntity> spec)
        {
            return ApplySpecification(spec).Any();
        }

        public bool Any(Expression<Func<TEntity, bool>> criteria)
        {
            return _context.Set<TEntity>().Any(criteria);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> criteria)
        {
            return await _context.Set<TEntity>().AnyAsync(criteria);
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(_context.Set<TEntity>().AsQueryable(), spec);
        }
    }
}
