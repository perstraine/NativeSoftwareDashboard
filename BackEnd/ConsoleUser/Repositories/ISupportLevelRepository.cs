using ConsoleUser.Models;

namespace ConsoleUser.Repositories
{
    public interface ISupportLevelRepository
    {
        IEnumerable<CustomerSupportLevel> GetAll();
    }
}
