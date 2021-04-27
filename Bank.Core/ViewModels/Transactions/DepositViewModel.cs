using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Core.ViewModels.Transactions
{
    public class DepositViewModel
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
