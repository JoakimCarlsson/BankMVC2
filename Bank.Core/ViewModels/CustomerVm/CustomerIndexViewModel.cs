using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.ViewModels.CustomerVm
{
    public class CustomerIndexViewModel
    {
        public int CustomerId { get; set; }
        [DisplayName("First Name")]
        public string Givenname { get; set; }
        [DisplayName("Last Name")]
        public string Surname { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Birthday { get; set; }
        public string Gender { get; set; }
        [DisplayName("National Id")]
        public string NationalId { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        [DisplayName("Street Address")]
        public string Streetaddress { get; set; }
        public string Country { get; set; }
        [DisplayName("Country Code")]
        public string CountryCode { get; set; }
        [DisplayName("Telephone Country Code")]
        public string Telephonecountrycode { get; set; }
        [DisplayName("Telephone Number")]
        public string Telephonenumber { get; set; }
        [DisplayName("Email Address")]
        public string Emailaddress { get; set; }
    }
}
