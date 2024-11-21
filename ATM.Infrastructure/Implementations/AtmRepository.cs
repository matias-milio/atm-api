using ATM.Core;
using ATM.Core.Contracts;
using ATM.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ATM.Infrastructure.Implementations
{
    public class AtmRepository<T>(AtmDbContext repositoryContext) : IAtmRepository<T> where T : BaseEntity
    {
        protected AtmDbContext Context { get; set; } = repositoryContext;
        private static readonly char[] separator = [','];

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = Context.Set<T>();

            if (filter != null)
               query = query.Where(filter);            

            foreach (var includeProperty in includeProperties.Split(separator, StringSplitOptions.RemoveEmptyEntries))            
                query = query.Include(includeProperty);

            if (orderBy != null)           
                return await orderBy(query).ToListAsync();            
            else            
                return await query.ToListAsync();          
        }

        public async Task<PaginatedList<T>> GetPaginatedAsync(Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "",
           int pageIndex = 0, int pageSize = 10)
        {
            IQueryable<T> query = Context.Set<T>();

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(separator, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            int totalCount = await query.CountAsync();

            if (orderBy != null)            
                query = orderBy(query);           

            query = query
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            List<T> items = await query.ToListAsync();

            return new PaginatedList<T>(items, totalCount, pageIndex, pageSize);
        }

        public async Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter, string includeProperties = "")
        {
            IQueryable<T> query = Context.Set<T>();           

            foreach (var includeProperty in includeProperties.Split(separator, StringSplitOptions.RemoveEmptyEntries))            
                query = query.Include(includeProperty);            
            
            return await query.FirstOrDefaultAsync(filter);            
        }

        public async Task<T> CreateAsync(T entity)
        {
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
        public async Task<int> DeleteAsync(int id)
        {
            var entity = await Context.Set<T>().FindAsync(id);
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
            return id;
        }     
        public async Task<T> UpdateAsync(T entity)
        {
            Context.Set<T>().Update(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
    }
}
