using Bank.Core.ViewModels;
using Bank.Web.Data;

namespace Bank.Core.Services.Home
{
    public interface IHomeService
    {
        public IndexViewModel GetStats();
    }
}
