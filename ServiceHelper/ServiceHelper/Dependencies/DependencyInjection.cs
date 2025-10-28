using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceHelper.Dependencies
{
    public static class DependencyInjection
    {
        public static void AddSQLServerEFCoreSpecificationServices<TDbContext>(this IServiceCollection services, string connectionString, Type repositoryType, bool useNetTopology = false) where TDbContext : DbContext, IUnitOfWork
        {
            services.AddSQLServerEFCoreSpecificationServices<TDbContext>(connectionString, repositoryType, useNetTopology, new Type[1] { repositoryType });
        }

        public static void AddSQLServerEFCoreSpecificationServices<TDbContext>(this IServiceCollection services, string connectionString, Type repositoryType, bool useNetTopology = false, params Type[] handlerAssemblyMarkerTypes) where TDbContext : DbContext, IUnitOfWork
        {
            string connectionString2 = connectionString;
            //services.AddMediatR(handlerAssemblyMarkerTypes);
            services.AddDbContext<TDbContext>(delegate (DbContextOptionsBuilder c)
            {
                c.UseSqlServer(connectionString2, delegate (SqlServerDbContextOptionsBuilder options)
                {
                    if (useNetTopology)
                    {
                        //options.UseNetTopologySuite();
                    }
                });
            });
            services.AddTransient((Func<IServiceProvider, IUnitOfWork>)((ctx) => ctx.GetService<TDbContext>()));
            services.AddTransient(typeof(IRepository<>), repositoryType);
        }
    }
}
