// TODO delete unwanted fields

using System.Diagnostics.CodeAnalysis;

namespace ConsoleUser.Models.Domain
{
    public class ZendeskData
    {
        [AllowNull] public List<Ticket> tickets { get; set; }
        [AllowNull] public object next_page { get; set; }
        [AllowNull] public object previous_page { get; set; }
        [AllowNull] public int count { get; set; }
    }
    public class CustomField
    {
        public object id { get; set; }
        public bool value { get; set; }
    }
    public class Field
    {
        public object id { get; set; }
        public bool value { get; set; }
    }
    public class Ticket
    {
        [AllowNull] public string testData { get; set; }
        [AllowNull] public string url { get; set; }
        [AllowNull] public int id { get; set; }
        [AllowNull] public object external_id { get; set; }
        [AllowNull] public Via via { get; set; }
        [AllowNull] public DateTime created_at { get; set; }
        [AllowNull] public DateTime updated_at { get; set; }
        [AllowNull] public string type { get; set; }
        [AllowNull] public string subject { get; set; }
        [AllowNull] public string raw_subject { get; set; }
        [AllowNull] public string description { get; set; }
        [AllowNull] public string priority { get; set; }
        [AllowNull] public string status { get; set; }
        [AllowNull] public object recipient { get; set; }
        [AllowNull] public object requester_id { get; set; }
        [AllowNull] public object submitter_id { get; set; }
        [AllowNull] public object assignee_id { get; set; }
        [AllowNull] public object organization_id { get; set; }
        [AllowNull] public object group_id { get; set; }
        [AllowNull] public List<object> collaborator_ids { get; set; }
        [AllowNull] public List<object> follower_ids { get; set; }
        [AllowNull] public List<object> email_cc_ids { get; set; }
        [AllowNull] public object forum_topic_id { get; set; }
        [AllowNull] public object problem_id { get; set; }
        [AllowNull] public bool has_incidents { get; set; }
        [AllowNull] public bool is_public { get; set; }
        [AllowNull] public object due_at { get; set; }
        [AllowNull] public List<string> tags { get; set; }
        [AllowNull] public List<object> custom_fields { get; set; }
        [AllowNull] public object satisfaction_rating { get; set; }
        [AllowNull] public List<object> sharing_agreement_ids { get; set; }
        [AllowNull] public object custom_status_id { get; set; }
        [AllowNull] public List<object> fields { get; set; }
        [AllowNull] public List<object> followup_ids { get; set; }
        [AllowNull] public object ticket_form_id { get; set; }
        [AllowNull] public object brand_id { get; set; }
        [AllowNull] public bool allow_channelback { get; set; }
        [AllowNull] public bool allow_attachments { get; set; }
    }

    public class From
    {
    }

    public class To
    {
    }

    public class Source
    {
        [AllowNull] public From from { get; set; }
        [AllowNull] public To to { get; set; }
        [AllowNull] public object rel { get; set; }
    }

    public class Via
    {
        [AllowNull] public string channel { get; set; }
        [AllowNull] public Source source { get; set; }
    }
}