using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.ViewModels
{
    public class IndexViewModel
    {
        public int TotalCustomers { get; set; }
        public int TotalAccounts { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
