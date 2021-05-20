using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents;
using Bank.AzureSearchService.AzureEntities;
using Bank.Data.Repositories.Customer;
using Microsoft.Extensions.Configuration;

namespace Bank.AzureSearchService.Services.Search
{
    internal class AzureSearch : IAzureSearch
    {
        private readonly string _url;
        private readonly string _key;
        private readonly string _indexName;

        public AzureSearch(IConfiguration configuration)
        {
            _url = configuration.GetValue<string>("AzureSearch:Url");
            _key = configuration.GetValue<string>("AzureSearch:Key");
            _indexName = configuration.GetValue<string>("AzureSearch:IndexName");
        }

        
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