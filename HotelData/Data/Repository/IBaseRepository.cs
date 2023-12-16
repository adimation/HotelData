using System.Linq.Expressions;

namespace HotelData.Data.Repository
{
    public interface IBaseRepository<T>
    {
        Task<List<T>> GetAllAsync();

        Task<T> GetAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);

        //Task<T> GetByNameAsync(Expression<Func<T, bool>> filter);
        Task<T> CreateAsync(T dbRecord);

        Task<IList<T>> CreateAllAsync(IList<T> dbRecords);

        Task<T> UpdateAsync(T dbRecord);

        Task<bool> DeleteAsync(T dbRecord);
    }
}
