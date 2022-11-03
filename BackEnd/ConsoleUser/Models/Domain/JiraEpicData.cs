using System.Diagnostics.CodeAnalysis;

namespace ConsoleUser.Models.Domain
{
    public class JiraEpicData
    {
        [AllowNull] public string id { get; set; }
        [AllowNull] public string Account { get; set; }
        [AllowNull] public string Name { get; set; }
        [AllowNull] public DateTime StartDate { get; set; } // TODO confirm requested or creted time
        [AllowNull] public DateTime DueDate { get; set; }
        [AllowNull] public int? StoryPoints { get; set; }
        [AllowNull] public string Budget { get; set; }
        [AllowNull] public int TimeSpent { get; set; }
        [AllowNull] public int Complete { get; set; }
        [AllowNull] public string url { get; set; }
    }
}
