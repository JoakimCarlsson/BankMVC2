using System;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using Bank.Data.Data;
using Bank.Data.Models;

namespace Bank.Search.Azure
{
    internal class AzureUpdater : IAzureUpdater
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _url = "https://bankmvc2search.search.windows.net";
        private readonly string _key = "F1CE54AA4F9E785A49F93980D29D00B2";
        private readonly string _indexName = "customers";
        public AzureUpdater(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Run()
        {
            CreateIndexIfNotExists();

            var searchClient = new SearchClient(new Uri(_url), _indexName, new AzureKeyCredential(_key));
            var batch = new IndexDocumentsBatch<CustomerInAzure>();

            foreach (Customer customer in _dbContext.Customers)
            {
                var customerInAzure = new CustomerInAzure
                {
                    City = customer.City,
                    GiveName = customer.Givenname,
                    Id = customer.CustomerId.ToString(),
                    Surname = customer.Surname
                };
                batch.Actions.Add(new IndexDocumentsAction<CustomerInAzure>(IndexActionType.MergeOrUpload, customerInAzure));
            }

            IndexDocumentsResult result = searchClient.IndexDocuments(batch);
        }

        private void CreateIndexIfNotExists()
        {
            var serviceEndPoint = new Uri(_url);
            var credential = new AzureKeyCredential(_key);
            var adminClient = new SearchIndexClient(serviceEndPoint, credential);

            var fieldBuilder = new FieldBuilder();
            var searchFields = fieldBuilder.Build(typeof(CustomerInAzure));
            var definition = new SearchIndex(_indexName, searchFields);

            adminClient.CreateOrUpdateIndex(definition);
        }
    }
}