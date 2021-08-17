using System;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthRepository AuthRepository();
        IGenericRepository<T> Repository<T>() where T : class;
        Task<bool> CommitChangesAsync();
    }
}