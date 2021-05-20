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
using Microsoft.Extensions.Configuration;

namespace Bank.AzureSearchService.Services.Update
{
    class AzureUpdater : IAzureUpdater
    {
        private readonly ICustomerRepository _customerRepository;

        private readonly string _url;
        private readonly string _key;
        private readonly string _indexName;

        public AzureUpdater(IConfiguration configuration, ICustomerRepository customerRepository)
        {
            _url = configuration.GetValue<string>("AzureSearch:Url");
            _key = configuration.GetValue<string>("AzureSearch:Key");
            _indexName = configuration.GetValue<string>("AzureSearch:IndexName");

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
                    Surname = customer.Surname,
                    Name = $"{customer.Givenname} {customer.Surname}",
                    Address = customer.Streetaddress,
                    NationalId = string.IsNullOrWhiteSpace(customer.NationalId) ? "" : customer.NationalId,
                };
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