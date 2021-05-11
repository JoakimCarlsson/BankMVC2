using Azure.Search.Documents.Indexes;

namespace Bank.Search.Azure
{
    public class CustomerInAzure
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