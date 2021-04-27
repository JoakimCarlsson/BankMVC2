using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.ViewModels.Customers
{
    public class CustomerSearchListViewModel
    {
        public string Q { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<CustomerSearchViewModel> Customers { get; set; }
    }

    public class CustomerSearchViewModel
    {
        public int CustomerId { get; set; }
        public DateTime? Birthday { get; set; }
        public string Givenname { get; set; }
        public string Surname { get; set; }
        public string Streetaddress { get; set; }
        public string City { get; set; }
    }
}
