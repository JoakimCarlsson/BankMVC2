﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Core.ViewModels.Transactions
{
    public class TransactionListViewModel
    {
        public IEnumerable<TransactionDetailsViewModel> Transactions { get; set; }
    }
}
