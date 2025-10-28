using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel;

namespace ServiceHelper.Dependencies
{
    public abstract class DbContextBase<TDbContext> : DbContext, IUnitOfWork, IDisposable where TDbContext : DbContextBase<TDbContext>
    {
        private readonly IMediator _mediator;

        private IDbContextTransaction? _currentTransaction;

        public DbContextBase(DbContextOptions<TDbContext> options)
            : base(options)
        {
        }

        public DbContextBase(DbContextOptions<TDbContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            //configurationBuilder.Properties<Zouid>().HaveConversion<ZouidConverter>().HaveMaxLength(300)
            //    .AreUnicode(unicode: false);
            //configurationBuilder.Properties<Zrid>().HaveConversion<ZridConverter>().HaveMaxLength(300)
            //    .AreUnicode(unicode: false);
            //configurationBuilder.Properties<ZeroMaterial>().HaveConversion<ZeroMaterialConverter>();
            //configurationBuilder.Properties<Zeid>().HaveConversion<ZeidConverter>().HaveMaxLength(300)
            //    .AreUnicode(unicode: false);
            configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeConverter>();
            //configurationBuilder.Properties<MaterialType>().HaveConversion<MaterialTypeConverter>();
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
            }
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
            {
                return;
            }

            try
            {
                await SaveChangesAsync(cancellationToken);
                _currentTransaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await BeginTransactionAsync(cancellationToken);
            try
            {
                await base.SaveChangesAsync(cancellationToken);
                await _mediator.DispatchDomainEvents((TDbContext)this);
                await CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }
    }
}
