using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Bank.AzureSearchService.Services.Search
{
    public interface IAzureSearch
    {
        public Task<AzureSearchResult> SearchCustomersAsync(string q, string orderBy, int offset, int limit);
    }
}