using System.Collections.Generic;

namespace Bank.Web.ViewModels.Statistics
{
    public class CountryStatisticsViewModel
    {
        public List<Item> Countries { get; set; }
    }

    public class Item
    {
        public string Country { get; set; }
        public int CustomerAmount { get; set; }
        public int AccountAmount { get; set; }
        public decimal AccountsTotalBalance { get; set; }
    }
}
