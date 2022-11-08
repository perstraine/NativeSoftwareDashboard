using System.Diagnostics.CodeAnalysis;

namespace ConsoleUser.Models.Domain
{
    public class DashboardTicketData
    {
        [AllowNull] public int id { get; set; }
        [AllowNull] public string Organisation { get; set; }
        [AllowNull] public string Subject { get; set; }
        [AllowNull] public string Status { get; set; }
        [AllowNull] public string Recipient { get; set; }
        [AllowNull] public bool Billable { get; set; }
        [AllowNull] public string Priority { get; set; }
        [AllowNull] public string RequestedDate { get; set; } // TODO confirm requested or creted time
        [AllowNull] public string TimeDue { get; set; } 
        [AllowNull] public string Type { get; set; }
        [AllowNull] public string url { get; set; }
        [AllowNull] public string TrafficLight { get; set; }
        [AllowNull] public int SortPriority { get; set; }

        // trying Enums
        //private Priority _priority; 
        //public Priority PriorityType
        //{
        //    get { return _priority; }
        //    set { _priority = value; }
        //}
    }
    // trying Enums
    //public enum Priority
    //{
    //    low,
    //    normal,
    //    high,
    //    urgent
    //}
}

