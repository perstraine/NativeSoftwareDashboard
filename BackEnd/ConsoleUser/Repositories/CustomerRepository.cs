using ConsoleUser.Data;
using ConsoleUser.Models;

namespace ConsoleUser.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext customerDBContext;

        public CustomerRepository(CustomerDbContext customerDBContext)
        {
            this.customerDBContext = customerDBContext;
        }
        public IEnumerable<Customer> GetAll()
        {
            return customerDBContext.Customer.ToList();
        }
    }
}
