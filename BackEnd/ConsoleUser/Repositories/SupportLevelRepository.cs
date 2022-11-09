using ConsoleUser.Data;
using ConsoleUser.Models;

namespace ConsoleUser.Repositories
{
    public class SupportLevelRepository : ISupportLevelRepository
    {
        private readonly SupportLevelDbContext supportLevelDBContext;

        public SupportLevelRepository(SupportLevelDbContext supportLevelDBContext)
        {
            this.supportLevelDBContext = supportLevelDBContext;
        }
        public IEnumerable<CustomerSupportLevel> GetAll()
        {
            return supportLevelDBContext.CustomerSupportLevel.ToList();
        }
    }
}
