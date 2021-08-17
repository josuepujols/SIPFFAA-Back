using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using API.Repositories;

namespace API.Data
{
    public class UnitOfWork 
    {
        private DbContext _context;

        public UnitOfWork(DbContext context) // TODO: change to FileContext instance
        {
            _context = context;
        }

        public UnitOfWork()
        {
           // _context = new MainContext();
        }

        // public IAuthRepository AuthRepository() 
        // {
        //     return new AuthRepository((MainContext) _context); // _context is parsed from DbContext to MainContext
        // }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }

        public async Task<bool> CommitChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        
    }
}