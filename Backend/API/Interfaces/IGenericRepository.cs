using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        Task InsertAsync(T model);
        void Update(T model);
        Task DeleteAsync(object id);
    }
}