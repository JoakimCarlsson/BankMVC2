using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Bank.Data.Data;

namespace Bank.Search.Azure
{
    public interface IAzureSearcher
    {
        void Run();
    }

    public class AzureSearcher : IAzureSearcher
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _url = "https://bankmvc2search.search.windows.net";
        private readonly string _key = "F1CE54AA4F9E785A49F93980D29D00B2";
        private readonly string _indexName = "customers";

        public AzureSearcher(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Run()
        {
            var searchClient = new SearchClient(new Uri(_url), _indexName, new AzureKeyCredential(_key));

            while (true)
            {
                Console.WriteLine("Enter Search..... ");
                string query = Console.ReadLine();

                var searchOptions = new SearchOptions
                {
                    OrderBy = { "Surname asc" },
                    Skip = 0,
                    Size = 5,
                    IncludeTotalCount = true,
                };

                var searchResult = searchClient.Search<CustomerInAzure>(query, searchOptions);

                foreach (SearchResult<CustomerInAzure> result in searchResult.Value.GetResults())
                {
                    var customer = _dbContext.Customers.First(i => i.CustomerId == int.Parse(result.Document.Id));
                    Console.WriteLine($"{customer.Givenname} {customer.Surname} {customer.Birthday} {customer.Country} {customer.City}");
                }
                Console.WriteLine($"Result Amount: {searchResult.Value.TotalCount}");
            }
        }
    }
}