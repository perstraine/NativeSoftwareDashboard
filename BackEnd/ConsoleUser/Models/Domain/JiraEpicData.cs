using System.Diagnostics.CodeAnalysis;

namespace ConsoleUser.Models.Domain
{
    public class JiraEpicData
    {
        [AllowNull] public string id { get; set; }
        [AllowNull] public string Account { get; set; }
        [AllowNull] public string Name { get; set; }
        [AllowNull] public DateTime StartDate { get; set; }
        [AllowNull] public DateTime DueDate { get; set; }
        [AllowNull] public double? StoryPoints { get; set; }
        [AllowNull] public double? Budget { get; set; }
        [AllowNull] public double? TimeSpent { get; set; }
        [AllowNull] public double Complete { get; set; }
        [AllowNull] public double? BudgetRemaining { get; set; }
        [AllowNull] public string urgencyColour { get; set; }


        [AllowNull] public string url { get; set; }
    }
}
