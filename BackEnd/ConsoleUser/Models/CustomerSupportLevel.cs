using Microsoft.EntityFrameworkCore;

namespace ConsoleUser.Models
{
    [Keyless]
    public class CustomerSupportLevel
    {
        public int SupportLevel { get; set; }   
        public int ResponseTimeUrgent { get; set; }
        public int ResponseTimeHigh { get; set; }
        public int ResponseTimeNormal { get; set; }
        public int ResponseTimeLow { get; set; }
        public int ResolutionTimeUrgent { get; set; }
        public int ResolutionTimeHigh { get; set; }
        public int ResolutionTimeNormal { get; set; }
        public int ResolutionTimeLow { get; set; }
    }
}
