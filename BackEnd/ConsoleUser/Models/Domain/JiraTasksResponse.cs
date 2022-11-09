using System.Diagnostics.CodeAnalysis;

namespace ConsoleUser.Models.Domain
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class Assignee
    {
        [AllowNull]public string self { get; set; }
        [AllowNull]public string accountId { get; set; }
        [AllowNull]public AvatarUrls avatarUrls { get; set; }
        [AllowNull]public string displayName { get; set; }
        public bool active { get; set; }
        [AllowNull]public string timeZone { get; set; }
        [AllowNull] public string accountType { get; set; }
    }


    public class TaskFields
    {
        public DateTime statuscategorychangedate { get; set; }
        [AllowNull]public Issuetype issuetype { get; set; }
        [AllowNull] public Parent parent { get; set; }
        [AllowNull] public int? timespent { get; set; }
        [AllowNull]public Project project { get; set; }
        [AllowNull] public List<object> fixVersions { get; set; }
        public int? aggregatetimespent { get; set; }
        [AllowNull]public object resolution { get; set; }
        [AllowNull]public object customfield_10027 { get; set; }
        [AllowNull]public object customfield_10028 { get; set; }
        [AllowNull]public object customfield_10029 { get; set; }
        [AllowNull] public object resolutiondate { get; set; }
        public int? workratio { get; set; }
        [AllowNull] public Watches watches { get; set; }
        [AllowNull] public DateTime? lastViewed { get; set; }
        [AllowNull] public DateTime? created { get; set; }
        [AllowNull]public object customfield_10020 { get; set; }
        [AllowNull]public object customfield_10021 { get; set; }
        [AllowNull]public object customfield_10022 { get; set; }
        [AllowNull]public Priority priority { get; set; }
        [AllowNull]public object customfield_10023 { get; set; }
        [AllowNull]public object customfield_10024 { get; set; }
        [AllowNull]public object customfield_10025 { get; set; }
        [AllowNull]public object customfield_10026 { get; set; }
        [AllowNull]public List<object> labels { get; set; }
        [AllowNull]public object customfield_10016 { get; set; }
        [AllowNull]public object customfield_10017 { get; set; }
        [AllowNull]public Customfield10018 customfield_10018 { get; set; }
        [AllowNull] public string customfield_10019 { get; set; }
        public int? timeestimate { get; set; }
        [AllowNull]public object aggregatetimeoriginalestimate { get; set; }
        [AllowNull]public List<object> versions { get; set; }
        [AllowNull]public List<object> issuelinks { get; set; }
        [AllowNull] public Assignee assignee { get; set; }
        public DateTime updated { get; set; }
        [AllowNull]public Status status { get; set; }
        [AllowNull]public List<object> components { get; set; }
        [AllowNull]public object timeoriginalestimate { get; set; }
        [AllowNull]public object description { get; set; }
        [AllowNull]public object customfield_10010 { get; set; }
        [AllowNull]public object customfield_10014 { get; set; }
        [AllowNull]public object customfield_10015 { get; set; }
        [AllowNull]public object customfield_10005 { get; set; }
        [AllowNull]public object customfield_10006 { get; set; }
        [AllowNull] public object customfield_10007 { get; set; }
        public object security { get; set; }
        [AllowNull]public object customfield_10008 { get; set; }
        [AllowNull] public object customfield_10009 { get; set; }
        public int? aggregatetimeestimate { get; set; }
        [AllowNull]public string summary { get; set; }
        [AllowNull]public Creator creator { get; set; }
        [AllowNull]public List<object> subtasks { get; set; }
        [AllowNull]public object customfield_10040 { get; set; }
        [AllowNull]public object customfield_10041 { get; set; }
        [AllowNull]public object customfield_10042 { get; set; }
        [AllowNull]public object customfield_10043 { get; set; }
        [AllowNull]public Reporter reporter { get; set; }
        [AllowNull]public string customfield_10044 { get; set; }
        [AllowNull]public Aggregateprogress aggregateprogress { get; set; }
        [AllowNull]public object customfield_10001 { get; set; }
        [AllowNull]public object customfield_10002 { get; set; }
        [AllowNull]public double? customfield_10046 { get; set; }
        [AllowNull]public double? customfield_10047 { get; set; }
        [AllowNull]public object customfield_10003 { get; set; }
        [AllowNull]public object customfield_10004 { get; set; }
        [AllowNull]public List<object> customfield_10038 { get; set; }
        [AllowNull]public object customfield_10039 { get; set; }
        [AllowNull]public object environment { get; set; }
        [AllowNull]public object duedate { get; set; }
        [AllowNull]public TaskProgress progress { get; set; }
        [AllowNull] public Votes votes { get; set; }
    }

    public class Parent
    {
        [AllowNull] public string id { get; set; }
        [AllowNull]public string key { get; set; }
        [AllowNull]public string self { get; set; }
        [AllowNull] public TaskFields fields { get; set; }
    }


    public class TaskProgress
    {
        public int progress { get; set; }
        public int total { get; set; }
        public int percent { get; set; }
    }

    public class JiraIssue
    {
        [AllowNull]public string expand { get; set; }
        [AllowNull]public string id { get; set; }
        [AllowNull]public string self { get; set; }
        [AllowNull]public string key { get; set; }
        [AllowNull] public TaskFields fields { get; set; }
    }
    public class JiraTasksResponse
    {
        [AllowNull] public string expand { get; set; }
        public int startAt { get; set; }
        public int maxResults { get; set; }
        public int total { get; set; }
        [AllowNull] public List<JiraIssue> issues { get; set; }
    }
}