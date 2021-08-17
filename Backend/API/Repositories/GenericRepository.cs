using System.Collections.Generic;
using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private DbContext _context;
        private DbSet<T> _dbSet;
        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task DeleteAsync(object id)
        {
            var foundEntity = await _dbSet.FindAsync(id);
            _dbSet.Remove(foundEntity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task InsertAsync(T model)
        {
            await _dbSet.AddAsync(model);
        }

        public void Update(T model)
        {
            _context.Attach(model);
            _context.Entry(model).State = EntityState.Modified;
        }
    }
}