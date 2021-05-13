using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Search.Documents.Indexes;

namespace Bank.AzureSearchService.AzureEntities
{
    class CustomerInAzure
    {
        [SimpleField(IsKey = true, IsFilterable = true)]
        public string Id { get; init; }

        [SearchableField(IsSortable = true)]
        public string GiveName { get; init; }

        [SearchableField(IsSortable = true)]
        public string Surname { get; init; }

        [SearchableField(IsSortable = true)]
        public string City { get; init; }

        [SimpleField(IsSortable = true)]
        public string Name { get; init; }
        
        [SimpleField(IsSortable = true)]
        public string Address { get; init; }
        
        [SimpleField(IsSortable = true)]
        public string NationalId { get; init; }
    }
}
