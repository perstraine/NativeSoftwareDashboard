namespace NewZendeskTicket
{
    public class CustomField
    {
        public long id { get; set; }
        public bool value { get; set; }
    }

    public class NewTicket
    {
        public Ticket ticket { get; set; }
    }

    public class Ticket
    {
        public string priority { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public long requester_id { get; set; }
        public List<CustomField> custom_fields { get; set; }
    }
}
