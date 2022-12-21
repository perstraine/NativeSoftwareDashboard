using Microsoft.EntityFrameworkCore;

namespace ConsoleUser.Models
{
    [Keyless]
    public class Customer
    {
        public string CustomerCode { get; set; }
        public string CustomerCodeZendesk { get; set; }
        public int SupportLevel { get; set; }
    }
}
