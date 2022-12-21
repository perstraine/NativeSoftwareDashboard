namespace ConsoleUser.Models.DTO
{
    //Customer code, suport level and Zendesk code from database
    public class Customer
    {
        public string CustomerCode { get; set; }
        public string CustomerCodeZendesk { get; set; }
        public int SupportLevel { get; set; }
    }
}
