using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.ViewModels.Customers
{
    public class CustomerSearchListViewModel
    {
        public PagingViewModel PagingViewModel { get; set; }
        public IEnumerable<CustomerSearchViewModel> Customers { get; set; }
    }

    public class PagingViewModel
    {
        public string Q { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public int CurrentRowCount { get; set; }
        public int MaxRowCount { get; set; }
        public int TotalPages { get; set; }
    }

    public class CustomerSearchViewModel
    {
        public int CustomerId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Birthday { get; set; }
        public string Givenname { get; set; }
        public string Surname { get; set; }
        public string Streetaddress { get; set; }
        public string City { get; set; }
    }
}
