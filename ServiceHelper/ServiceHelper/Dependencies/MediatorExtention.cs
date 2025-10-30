using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ServiceHelper.DomainEvent;

namespace ServiceHelper.Dependencies
{
    public static class MediatorExtention
    {
        public static async Task DispatchDomainEvents<TDbContext>(this IMediator mediator, TDbContext context) where TDbContext : DbContextBase<TDbContext>
        {
            IEnumerable<EntityEntry<Entity>> source = from m in context.ChangeTracker.Entries<Entity>()
                                                      where m.Entity.DomainEvents != null && m.Entity.DomainEvents.Any()
                                                      select m;
            List<IDomainEvent> list = source.SelectMany((m) => m.Entity.DomainEvents).ToList();
            source.ToList().ForEach(delegate (EntityEntry<Entity> m)
            {
                m.Entity.ClearDomainEvents();
            });
            foreach (IDomainEvent item in list)
            {
                await mediator.Publish(item);
            }
        }
    }
}
