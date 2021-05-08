using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bank.Core.ViewModels.Customers
{
    public class CustomerRegisterViewModel : CustomerBaseViewModel
    {
        public DateTime? Birthday { get; set; }
        public string NationalId { get; set; }
    }
}
