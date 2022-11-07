namespace ConsoleUser.Models
{
    public class SupportLevel
    {
        public int Id { get; set; }
        public int Level { get; set; }   
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
