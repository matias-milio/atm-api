using System.Linq.Expressions;

namespace ATM.Core.Contracts
{
    public interface IAtmRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
        Task<PaginatedList<T>> GetPaginatedAsync(Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "",
           int pageIndex = 0, int pageSize = 10);
        Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter, string includeProperties = "");       
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<int> DeleteAsync(int id);
    }
}
