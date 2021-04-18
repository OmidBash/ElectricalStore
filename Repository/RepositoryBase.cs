using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Contracts.Repositories;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ElectricalStoreContext RepositoryContext { get; set; }

        public RepositoryBase(ElectricalStoreContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            // return this.RepositoryContext.Set<T>()
            //     .Where(expression);

            IQueryable<T> result = this.RepositoryContext.Set<T>()
                .Where(expression)
                .AsNoTracking();
            DisplayStates(RepositoryContext.ChangeTracker.Entries(), "FindByCondition");
            return result;
        }

        public Task<int> Count()
        {
            return this.RepositoryContext.Set<T>().CountAsync();
        }

        public void Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);

            DisplayStates(RepositoryContext.ChangeTracker.Entries(), "Update");
        }

        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);

            DisplayStates(RepositoryContext.ChangeTracker.Entries(), "Delete");
        }

        public void DeleteRange(List<T> entities)
        {
            this.RepositoryContext.Set<T>().RemoveRange(entities);

            DisplayStates(RepositoryContext.ChangeTracker.Entries(), "DeleteRange");
        }

        public void DetachEntity(List<T> entities)
        {
            foreach (T entity in entities)
                this.RepositoryContext.Entry<T>(entity).State = EntityState.Detached;

            DisplayStates(RepositoryContext.ChangeTracker.Entries(), "DetachEntity");
        }

        public void DisplayStates(IEnumerable<EntityEntry> entries, string verb)
        {
            Console.WriteLine($"-----------------------------{verb}-----------------------------");

            if (!entries.Any())
                Console.WriteLine("No Entity");

            foreach (var entry in entries)
            {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: { entry.State.ToString()}");
                // Console.WriteLine($"OriginalValues: { entry.OriginalValues.GetValue<Guid>("Id")}");
            }
        }
    }
}

