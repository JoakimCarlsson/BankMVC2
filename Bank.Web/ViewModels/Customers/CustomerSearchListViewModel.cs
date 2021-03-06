using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bank.Web.ViewModels.Customers
{
    public class CustomerSearchListViewModel
    {
        public PagingViewModel PagingViewModel { get; set; }
        public IEnumerable<CustomerSearchViewModel> Customers { get; set; }
    }

    public class PagingViewModel
    {
        public string Q { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public int CurrentRowCount { get; set; }
        public int MaxRowCount { get; set; }
        public int TotalPages { get; set; }
        public string OppositeSortOrder { get; set; }
    }

    public class CustomerSearchViewModel
    {
        public int CustomerId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public string? NationalId { get; set; }
        public string Givenname { get; set; }
        public string Surname { get; set; }
        public string Streetaddress { get; set; }
        public string City { get; set; }
    }
}
