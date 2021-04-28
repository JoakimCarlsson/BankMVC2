using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Repository.Base
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public BaseRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetByIdAsync(int id) //virtual
        {
            return await _dbContext.Set<T>().FindAsync(id).ConfigureAwait(false);
        }

        public Task<IQueryable<T>> ListAllAsync()
        {
            return Task.FromResult(_dbContext.Set<T>().AsQueryable());
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public Task<IQueryable<T>> GetPagedResponseAsync(int page, int size) //virtual
        {
            return Task.FromResult(_dbContext.Set<T>().Skip((page - 1) * size).Take(size).AsNoTracking().AsQueryable());
        }
    }
}
