namespace ConsoleUser.Models
{
    public class NewTicket
    {
        public Ticket ticket { get; set; }

        public class Comment
        {
            public string body { get; set; }
        }

        public class Ticket
        {
            public Comment comment { get; set; }
            public string priority { get; set; }
            public string subject { get; set; }
            public string type { get; set; }
            public string requester_id { get; set; }
        }
    }
}
