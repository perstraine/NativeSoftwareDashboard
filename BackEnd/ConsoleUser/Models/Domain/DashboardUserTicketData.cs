using System.Diagnostics.CodeAnalysis;

namespace ConsoleUser.Models.Domain
{
    //Zendesk tickets from a particular user to be displayed on dashboard
    public class DashboardUserTicketData
    {
        [AllowNull] public int Id { get; set; }
        [AllowNull] public string Subject { get; set; }
        [AllowNull] public string RequestedDate { get; set; }
        [AllowNull] public string FirstResponse { get; set; }
        [AllowNull] public string LastUpdate { get; set; } 
        [AllowNull] public string TimeDue { get; set; }
        [AllowNull] public string Priority { get; set; }
        [AllowNull] public string Type { get; set; }
        [AllowNull] public string url { get; set; }
        [AllowNull] public string TrafficLight { get; set; }
    }
}

