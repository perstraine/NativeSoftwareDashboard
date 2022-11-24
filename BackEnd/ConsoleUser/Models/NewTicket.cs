namespace ConsoleUser.Models
{
    public class NewTicket
    {
        public Ticket ticket { get; set; }

        public class Comment
        {
            public string body { get; set; }
        }
        public class CustomField
        {
            public long id { get; set; }
            public bool value { get; set; } = true;
        }
        public class Ticket
        {
            public Comment comment { get; set; }
            public string priority { get; set; }
            public string subject { get; set; }
            public string type { get; set; }
            public List<CustomField> custom_fields { get; set; }

            //public string requester_id { get; set; }
        }
    }
}
