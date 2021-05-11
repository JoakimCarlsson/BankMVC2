namespace Bank.Web.ViewModels.Statistics
{
    public class TopCustomerViewModel
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
