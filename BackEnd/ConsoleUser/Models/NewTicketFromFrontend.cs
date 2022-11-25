namespace ConsoleUser.Models
{
    public class CustomField
    {
        public long id { get; set; }
        public bool value { get; set; }
    }  
    public class NewTicketFromFrontend
    {
        public string description { get; set; }
        public string priority { get; set; }
        public string subject { get; set; }
        public string type { get; set; }
        public string email { get; set; }
        public List<CustomField> custom_fields { get; set; }
    }
}
