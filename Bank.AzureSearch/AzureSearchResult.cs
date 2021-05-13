using System.Collections.Generic;

namespace Bank.AzureSearchService
{
    public class AzureSearchResult
    {
        public long? TotalRowCount { get; init; }
        public IEnumerable<int> Ids { get; init; }
    }
}