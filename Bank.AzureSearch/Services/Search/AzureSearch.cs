using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents;
using Bank.AzureSearchService.AzureEntities;
using Bank.Data.Repositories.Customer;

namespace Bank.AzureSearchService.Services.Search
{
    internal class AzureSearch : IAzureSearch
    {
        private readonly ICustomerRepository _customerRepository;

        public AzureSearch(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        
        private readonly string _url = "https://bankmvc2search.search.windows.net"; //todo move me too appsettings.json
        private readonly string _key = "F1CE54AA4F9E785A49F93980D29D00B2";
        private readonly string _indexName = "customers";
        
        public async Task<AzureSearchResult> SearchCustomersAsync(string q, string sortField, string sortOrder, int offset, int limit)
        {
            var searchClient = new SearchClient(new Uri(_url), _indexName, new AzureKeyCredential(_key));
            
            string orderBy = string.IsNullOrWhiteSpace(sortOrder) ? $"{sortField}" : $"{sortField} {sortOrder}";

            var searchParameters = new SearchOptions
            {
                OrderBy = {orderBy},
                Skip = offset,
                Size = limit,
                IncludeTotalCount = true
            };

            var searchResult = await searchClient.SearchAsync<CustomerInAzure>(q, searchParameters);
            
            return new AzureSearchResult
            {
                Ids = searchResult.Value.GetResults().Select(result => int.Parse(result.Document.Id)),
                TotalRowCount = searchResult.Value.TotalCount
            };
        }
    }
}