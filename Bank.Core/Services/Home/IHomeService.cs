using Bank.Core.ViewModels;

namespace Bank.Core.Services.Home
{
    public interface IHomeService
    {
        public IndexViewModel GetStats();
    }
}
