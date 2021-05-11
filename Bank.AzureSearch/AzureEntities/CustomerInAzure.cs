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
        public string Id { get; set; }

        [SearchableField(IsSortable = true)]
        public string GiveName { get; set; }

        [SearchableField(IsSortable = true)]
        public string Surname { get; set; }

        [SearchableField(IsSortable = true)]
        public string City { get; set; }
    }
}
