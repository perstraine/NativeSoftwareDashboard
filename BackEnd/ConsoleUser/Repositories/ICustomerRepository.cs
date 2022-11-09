using ConsoleUser.Models;

namespace ConsoleUser.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
    }
}
