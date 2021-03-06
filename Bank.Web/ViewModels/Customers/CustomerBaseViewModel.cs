using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bank.Web.ViewModels.Customers
{
    public class CustomerBaseViewModel
    {
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public List<SelectListItem> Genders { get; set; }
        public string SelectedGender { get; set; }
        public string TelephoneCountryCode { get; set; }
        public string TelephoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? Birthday { get; set; }
        public string NationalId { get; set; }
    }
}
