using ConsoleUser.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Diagnostics;

namespace ConsoleUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JiraController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> JiraEpicTickets()
        {
            var options = new RestClientOptions("https://ranjurave.atlassian.net/rest/api/2")
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };
            var client = new RestClient(options);
            var request = new RestRequest("/search?jql=issuetype=Epic%20AND%20project=AT", Method.Get);
            request.AddHeader("Authorization", "Basic cHJhc2hhbnRrQG1pc3Npb25yZWFkeWhxLmNvbTpuajFVUXJHa2plOVNZR25kUENRd0U4NkE=");
            RestResponse response = await client.ExecuteAsync(request);
            var jiraData = JsonConvert.DeserializeObject<JiraResponse>(response.Content);
            var jiraEpicIssues = CreateTicketInfo(jiraData);

            return Ok(jiraEpicIssues);
        }

        private static List<JiraEpicData> CreateTicketInfo(JiraResponse? jiraData)
        {
            var epicList = new List<JiraEpicData>();
            foreach (var issue in jiraData.issues)
            {
                var jiraEpic = new JiraEpicData();
                jiraEpic.id = issue.key;
                jiraEpic.Account = issue.fields.customfield_10045;
                jiraEpic.Name = issue.fields.summary;
                jiraEpic.StartDate = issue.fields.created;
                jiraEpic.DueDate = issue.fields.duedate;
                jiraEpic.StoryPoints = issue.fields.aggregatetimeestimate;
                jiraEpic.Complete = issue.fields.aggregateprogress.percent;
                jiraEpic.url = issue.fields.status.iconUrl+"browse/"+issue.key;


                epicList.Add(jiraEpic);
            }
            return epicList;
        }
        
    }
}
