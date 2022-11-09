using System.Diagnostics.CodeAnalysis;

namespace ConsoleUser.Models.Domain
{
    public class Aggregateprogress
    {
        public int progress { get; set; }
        public int total { get; set; }
        public int percent { get; set; }
    }

    public class AvatarUrls
    {
        [AllowNull] public string _48x48 { get; set; }
        [AllowNull] public string _24x24 { get; set; }
        [AllowNull] public string _16x16 { get; set; }
        [AllowNull] public string _32x32 { get; set; }
    }

    public class Creator
    {
        [AllowNull] public string self { get; set; }
        [AllowNull] public string accountId { get; set; }
        [AllowNull] public AvatarUrls avatarUrls { get; set; }
        [AllowNull] public string displayName { get; set; }
        [AllowNull] public bool active { get; set; }
        [AllowNull] public string timeZone { get; set; }
        [AllowNull] public string accountType { get; set; }
        [AllowNull] public string emailAddress { get; set; }
    }

    public class Customfield10018
    {
        [AllowNull] public bool hasEpicLinkFieldDependency { get; set; }
        [AllowNull] public bool showField { get; set; }
        [AllowNull] public NonEditableReason nonEditableReason { get; set; }
    }

    public class Resolution
    {
        [AllowNull] public string self { get; set; }
        [AllowNull] public string id { get; set; }
        [AllowNull] public string description { get; set; }
        [AllowNull] public string name { get; set; }
    }

    public class Fields
    {
        public DateTime statuscategorychangedate { get; set; }
        [AllowNull] public Issuetype issuetype { get; set; }
        [AllowNull] public object timespent { get; set; }
        [AllowNull] public Project project { get; set; }
        [AllowNull] public List<object> fixVersions { get; set; }
        public int? aggregatetimespent { get; set; }
        [AllowNull] public Resolution resolution { get; set; }
        [AllowNull] public object customfield_10027 { get; set; }
        [AllowNull] public object customfield_10028 { get; set; }
        [AllowNull] public object customfield_10029 { get; set; }
        [AllowNull] public object resolutiondate { get; set; }
        public int workratio { get; set; }
        public DateTime? lastViewed { get; set; }
        [AllowNull] public Watches watches { get; set; }
        public DateTime created { get; set; }
        [AllowNull] public object customfield_10020 { get; set; }
        [AllowNull] public object customfield_10021 { get; set; }
        [AllowNull] public object customfield_10022 { get; set; }
        [AllowNull] public Priority priority { get; set; }
        [AllowNull] public object customfield_10023 { get; set; }
        [AllowNull] public object customfield_10024 { get; set; }
        [AllowNull] public object customfield_10025 { get; set; }
        [AllowNull] public List<object> labels { get; set; }
        [AllowNull] public object customfield_10026 { get; set; }
        [AllowNull] public object customfield_10016 { get; set; }
        [AllowNull] public string customfield_10017 { get; set; }
        [AllowNull]public Customfield10018 customfield_10018 { get; set; }
        [AllowNull]public string customfield_10019 { get; set; }
        [AllowNull]public object aggregatetimeoriginalestimate { get; set; }
        [AllowNull]public object timeestimate { get; set; }
        [AllowNull]public List<object> versions { get; set; }
        [AllowNull]public List<object> issuelinks { get; set; }
        [AllowNull]public object assignee { get; set; }
        public DateTime updated { get; set; }
        [AllowNull]public Status status { get; set; }
        [AllowNull]public List<object> components { get; set; }
        [AllowNull]public object timeoriginalestimate { get; set; }
        [AllowNull]public string description { get; set; }
        [AllowNull]public object customfield_10010 { get; set; }
        [AllowNull]public object customfield_10014 { get; set; }
        [AllowNull]public string customfield_10015 { get; set; }
        [AllowNull]public object customfield_10005 { get; set; }
        [AllowNull]public object customfield_10006 { get; set; }
        [AllowNull]public object security { get; set; }
        [AllowNull]public object customfield_10007 { get; set; }
        [AllowNull]public object customfield_10008 { get; set; }
        [AllowNull]public object customfield_10009 { get; set; }
        public int? aggregatetimeestimate { get; set; }
        [AllowNull]public string summary { get; set; }
        [AllowNull]public Creator creator { get; set; }
        [AllowNull]public List<object> subtasks { get; set; }
        [AllowNull]public object customfield_10040 { get; set; }
        [AllowNull]public object customfield_10041 { get; set; }
        [AllowNull]public object customfield_10042 { get; set; }
        [AllowNull]public object customfield_10043 { get; set; }
        [AllowNull]public Reporter reporter { get; set; }
        [AllowNull]public Aggregateprogress aggregateprogress { get; set; }
        [AllowNull]public object customfield_10001 { get; set; }
        [AllowNull]public string customfield_10045 { get; set; }
        [AllowNull]public object customfield_10002 { get; set; }
        [AllowNull]public object customfield_10003 { get; set; }
        [AllowNull]public object customfield_10004 { get; set; }
        [AllowNull]public List<object> customfield_10038 { get; set; }
        [AllowNull]public object customfield_10039 { get; set; }
        [AllowNull]public object environment { get; set; }
        [AllowNull]public DateTime duedate { get; set; }
        [AllowNull]public Progress progress { get; set; }
        [AllowNull]public Votes votes { get; set; }
    }

    public class Issue
    {
        [AllowNull] public string expand { get; set; }
        [AllowNull] public string id { get; set; }
        [AllowNull] public string self { get; set; }
        [AllowNull] public string key { get; set; }
        [AllowNull] public Fields fields { get; set; }
    }

    public class Issuetype
    {
        [AllowNull] public string self { get; set; }
        [AllowNull] public string id { get; set; }
        [AllowNull] public string description { get; set; }
        [AllowNull] public string iconUrl { get; set; }
        [AllowNull] public string name { get; set; }
        [AllowNull] public bool subtask { get; set; }
        [AllowNull] public int avatarId { get; set; }
        [AllowNull] public string entityId { get; set; }
        [AllowNull] public int hierarchyLevel { get; set; }
    }

    public class NonEditableReason
    {
        [AllowNull] public string reason { get; set; }
        [AllowNull] public string message { get; set; }
    }

    public class Priority
    {
        [AllowNull] public string self { get; set; }
        [AllowNull] public string iconUrl { get; set; }
        [AllowNull] public string name { get; set; }
        [AllowNull] public string id { get; set; }
    }

    public class Progress
    {
        [AllowNull] public int progress { get; set; }
        [AllowNull] public int total { get; set; }
    }

    public class Project
    {
        [AllowNull] public string self { get; set; }
        [AllowNull] public string id { get; set; }
        [AllowNull] public string key { get; set; }
        [AllowNull] public string name { get; set; }
        [AllowNull] public string projectTypeKey { get; set; }
        [AllowNull] public bool simplified { get; set; }
        [AllowNull] public AvatarUrls avatarUrls { get; set; }
    }

    public class Reporter
    {
        [AllowNull] public string self { get; set; }
        [AllowNull] public string accountId { get; set; }
        [AllowNull] public AvatarUrls avatarUrls { get; set; }
        [AllowNull] public string displayName { get; set; }
        [AllowNull] public bool active { get; set; }
        [AllowNull] public string timeZone { get; set; }
        [AllowNull] public string accountType { get; set; }
        [AllowNull] public string emailAddress { get; set; }
    }

    public class JiraResponse
    {
        [AllowNull] public string expand { get; set; }
        [AllowNull] public int startAt { get; set; }
        [AllowNull] public int maxResults { get; set; }
        [AllowNull] public int total { get; set; }
        [AllowNull] public List<Issue> issues { get; set; }
    }

    public class Status
    {
        [AllowNull] public string self { get; set; }
        [AllowNull] public string description { get; set; }
        [AllowNull] public string iconUrl { get; set; }
        [AllowNull] public string name { get; set; }
        [AllowNull] public string id { get; set; }
        [AllowNull] public StatusCategory statusCategory { get; set; }
    }

    public class StatusCategory
    {
        [AllowNull] public string self { get; set; }
        [AllowNull] public int id { get; set; }
        [AllowNull] public string key { get; set; }
        [AllowNull] public string colorName { get; set; }
        [AllowNull] public string name { get; set; }
    }

    public class Votes
    {
        [AllowNull] public string self { get; set; }
        [AllowNull] public int votes { get; set; }
        [AllowNull] public bool hasVoted { get; set; }
    }

    public class Watches
    {
        [AllowNull] public string self { get; set; }
        [AllowNull] public int watchCount { get; set; }
        [AllowNull] public bool isWatching { get; set; }
    }

}