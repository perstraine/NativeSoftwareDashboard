using System.Diagnostics.CodeAnalysis;

namespace ConsoleUser.Models.Domain
{
    public class ZendeskTicketData
    {
        [AllowNull] public string Organisation { get; set; }
        [AllowNull] public string Subject { get; set; }
        [AllowNull] public string Recipient { get; set; }
        [AllowNull] public bool Billable { get; set; }
        [AllowNull] public string Priority { get; set; }
        [AllowNull] public DateTime RequestedTime { get; set; } // TODO confirm requested or creted time
        [AllowNull] public DateTime TimeDue { get; set; } 
        [AllowNull] public string type { get; set; }
    }
}

