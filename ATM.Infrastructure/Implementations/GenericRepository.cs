using ATM.Core.Contracts;
using ATM.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ATM.Infrastructure.Implementations
{
    public class GenericRepository<T>(DbContext repositoryContext) : IGenericRepository<T> where T : BaseEntity
    {
        protected DbContext Context { get; set; } = repositoryContext;

        public async Task<IEnumerable<T>> GetAllAsync() => await Context.Set<T>().ToListAsync();
        public async Task<T> GetByIdAsync(int id) => await Context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
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
