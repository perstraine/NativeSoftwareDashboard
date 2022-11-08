// TODO delete unwanted fields

using System.Diagnostics.CodeAnalysis;

namespace ConsoleUser.Models.Domain
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AgentWaitTimeInMinutes
    {
        public object calendar { get; set; }
        public object business { get; set; }
    }

    public class FirstResolutionTimeInMinutes
    {
        public object calendar { get; set; }
        public object business { get; set; }
    }

    public class FullResolutionTimeInMinutes
    {
        public object calendar { get; set; }
        public object business { get; set; }
    }

    public class OnHoldTimeInMinutes
    {
        public int calendar { get; set; }
        public int business { get; set; }
    }

    public class ReplyTimeInMinutes
    {
        public int? calendar { get; set; }
        public int? business { get; set; }
    }

    public class RequesterWaitTimeInMinutes
    {
        public int? calendar { get; set; }
        public int? business { get; set; }
    }

    public class ZendeskMetrics
    {
        public List<TicketMetric> ticket_metrics { get; set; }
        public object next_page { get; set; }
        public object previous_page { get; set; }
        public int count { get; set; }
    }

    public class TicketMetric
    {
        public string url { get; set; }
        public object id { get; set; }
        public int ticket_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int group_stations { get; set; }
        public int assignee_stations { get; set; }
        public int reopens { get; set; }
        public int replies { get; set; }
        public DateTime assignee_updated_at { get; set; }
        public DateTime requester_updated_at { get; set; }
        public DateTime status_updated_at { get; set; }
        public DateTime initially_assigned_at { get; set; }
        public DateTime assigned_at { get; set; }
        public object solved_at { get; set; }
        public DateTime latest_comment_added_at { get; set; }
        public ReplyTimeInMinutes reply_time_in_minutes { get; set; }
        public FirstResolutionTimeInMinutes first_resolution_time_in_minutes { get; set; }
        public FullResolutionTimeInMinutes full_resolution_time_in_minutes { get; set; }
        public AgentWaitTimeInMinutes agent_wait_time_in_minutes { get; set; }
        public RequesterWaitTimeInMinutes requester_wait_time_in_minutes { get; set; }
        public OnHoldTimeInMinutes on_hold_time_in_minutes { get; set; }
    }
}


