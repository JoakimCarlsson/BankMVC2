using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Core.Data;
using Bank.Core.Model;
using Bank.Core.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Bank.Core.Repository.DispositionRep
{
    public class DispositionRepository : BaseRepository<Disposition>, IDispositionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DispositionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IQueryable<Disposition>> ListAllByCustomerIdAsync(int customerId)
        {
            return _dbContext.Dispositions.Include(a => a.Account).Where(i => i.CustomerId == customerId).AsQueryable();
        }
    }
}