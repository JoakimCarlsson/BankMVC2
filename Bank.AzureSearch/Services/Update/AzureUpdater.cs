using System;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Bank.AzureSearchService.AzureEntities;
using Bank.Data.Models;
using Bank.Data.Repositories.Customer;
using Microsoft.Extensions.Azure;

namespace Bank.AzureSearchService.Services.Update
{
    class AzureUpdater : IAzureUpdater
    {
        private readonly ICustomerRepository _customerRepository;

        private readonly string _url = "https://bankmvc2search.search.windows.net"; //todo, move me onto config.json, / appsettings.json.
        private readonly string _key = "F1CE54AA4F9E785A49F93980D29D00B2";
        private readonly string _indexName = "customers";

        public AzureUpdater(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task RunCustomerUpdateBatchAsync()
        {
            await CreateIndexIfNotExistsAsync().ConfigureAwait(false);

            var searchClient = new SearchClient(new Uri(_url), _indexName, new AzureKeyCredential(_key));
            var batch = new IndexDocumentsBatch<CustomerInAzure>();

            var customers = await _customerRepository.ListAllAsync().ConfigureAwait(false);

            foreach (Customer customer in customers)
            {
                var customerInAzure = new CustomerInAzure
                {
                    City = customer.City,
                    GiveName = customer.Givenname,
                    Id = customer.CustomerId.ToString(),
                    Surname = customer.Surname
                };
                //Console.WriteLine($"Adding {customer.CustomerId} {customer.Givenname} {customer.Surname}");
                batch.Actions.Add(new IndexDocumentsAction<CustomerInAzure>(IndexActionType.MergeOrUpload, customerInAzure));
            }
            var result = await searchClient.IndexDocumentsAsync(batch);
        }

        public async Task AddOrUpdateCustomerInAzure(Customer customer)
        {
            var searchClient = new SearchClient(new Uri(_url), _indexName, new AzureKeyCredential(_key));
            var batch = new IndexDocumentsBatch<CustomerInAzure>();

            var testCustomer = new CustomerInAzure
            {
                Id = customer.CustomerId.ToString(),
                GiveName = customer.Givenname,
                Surname = customer.Surname,
                City = customer.City,
            };
            
            batch.Actions.Add(new IndexDocumentsAction<CustomerInAzure>(IndexActionType.MergeOrUpload, testCustomer));
            var result = await searchClient.IndexDocumentsAsync(batch);
        }

        private async Task CreateIndexIfNotExistsAsync()
        {
            var serviceEndPoint = new Uri(_url);
            var credential = new AzureKeyCredential(_key);
            var adminClient = new SearchIndexClient(serviceEndPoint, credential);

            var fieldBuilder = new FieldBuilder();
            var searchFields = fieldBuilder.Build(typeof(CustomerInAzure));
            var definition = new SearchIndex(_indexName, searchFields);

            await adminClient.CreateOrUpdateIndexAsync(definition).ConfigureAwait(false);
        }
    }
}