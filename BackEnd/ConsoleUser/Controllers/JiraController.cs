using ConsoleUser.Models.Domain;
using ConsoleUser.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Diagnostics;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using ConsoleUser.Models.DTO;

namespace ConsoleUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JiraController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> JiraEpicTickets(string userType)
        {
            var options = new RestClientOptions("https://ranjurave.atlassian.net/rest/api/2")
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };
            var client = new RestClient(options);
            var request = new RestRequest("/search?jql=issuetype=Epic%20AND%20project=AT%20AND%20status=\"To%20Do\"", Method.Get);
            request.AddHeader("Authorization", "Basic cHJhc2hhbnRrQG1pc3Npb25yZWFkeWhxLmNvbTpuajFVUXJHa2plOVNZR25kUENRd0U4NkE=");
            RestResponse response = await client.ExecuteAsync(request);
            var jiraData = JsonConvert.DeserializeObject<JiraResponse>(response.Content);
            var jiraEpicIssues = new List<JiraEpicData> { };
            if(userType == "Staff")
            {
                var taskRequest = new RestRequest("/search?jql=issuetype=Task%20AND%20project=AT", Method.Get);
                taskRequest.AddHeader("Authorization", "Basic cHJhc2hhbnRrQG1pc3Npb25yZWFkeWhxLmNvbTpuajFVUXJHa2plOVNZR25kUENRd0U4NkE=");
                RestResponse taskResponse = await client.ExecuteAsync(taskRequest);
                var jiraTaskResponse = JsonConvert.DeserializeObject<JiraTasksResponse>(taskResponse.Content);
                jiraEpicIssues = CreateEpicInfoStaff(jiraData, jiraTaskResponse);
            }else{
                jiraEpicIssues = CreateEpicInfoClient(jiraData, userType);

            }

            return Ok(jiraEpicIssues);
        }

        private static List<JiraEpicData> CreateEpicInfoClient(JiraResponse? jiraData, string userType)
        {
            var epicList = new List<JiraEpicData>();
            foreach (var issue in jiraData.issues)
            {
                if (issue.fields.customfield_10045 == userType)
                {
                    var jiraEpic = new JiraEpicData();
                    jiraEpic.id = issue.key;
                    jiraEpic.Name = issue.fields.summary;
                    jiraEpic.StartDate = issue.fields.created;
                    jiraEpic.DueDate = issue.fields.duedate;
                    jiraEpic.Complete = issue.fields.aggregateprogress.percent;
                    jiraEpic.url = issue.fields.status.iconUrl + "browse/" + issue.key;
                    epicList.Add(jiraEpic);
                }

            }
            return (epicList);
        }


        private static List<JiraEpicData> CreateEpicInfoStaff(JiraResponse? jiraData, JiraTasksResponse? jiraTasks)
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
                jiraEpic.StoryPoints = 0;
                foreach(var task in jiraTasks.issues)
                {
                    try
                    {
                        if (task.fields.parent.key == issue.key)
                        {
                            jiraEpic.StoryPoints = jiraEpic.StoryPoints+task.fields.customfield_10046;
                        }
                    }
                    catch { Console.WriteLine("No Parent"); }
                }
                jiraEpic.Budget = 0;
                foreach (var task in jiraTasks.issues)
                {
                    try
                    {
                        if (task.fields.parent.key == issue.key)
                        {
                            jiraEpic.Budget = jiraEpic.Budget+task.fields.customfield_10047;
                        }
                    }
                    catch { Console.WriteLine("No Parent"); }
                }
                jiraEpic.TimeSpent = 0;
                foreach (var task in jiraTasks.issues)
                {
                    try
                    {
                        if (task.fields.parent.key == issue.key)
                        {
                            jiraEpic.TimeSpent = jiraEpic.TimeSpent+task.fields.progress.progress;
                        }
                    }
                    catch { Console.WriteLine("No Parent"); }
                }
                jiraEpic.TimeSpent = jiraEpic.TimeSpent/3600;
                jiraEpic.Complete = issue.fields.aggregateprogress.percent;
                jiraEpic.BudgetRemaining = jiraEpic.Budget - jiraEpic.TimeSpent;
                jiraEpic.urgencyColour =  jiraEpic.BudgetRemaining <= 0 ? "red" : jiraEpic.BudgetRemaining <= 4 ? "orange" : jiraEpic.BudgetRemaining <= 8 ? "yellow" : "green";

                jiraEpic.url = issue.fields.status.iconUrl+"browse/"+issue.key;


                epicList.Add(jiraEpic);
            }
            return epicList;
        }
        [HttpPost]
        [Route("comment")]
        public async Task<IActionResult> addJiraComment(JiraComment jiraComment)
        {
            var options = new RestClientOptions("https://ranjurave.atlassian.net/rest/api/2")
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };
            var client = new RestClient(options);
            string endpoint = "/issue/" + jiraComment.key + "/comment";
            var request = new RestRequest(endpoint, Method.Post);
            request.AddHeader("Authorization", "Basic cHJhc2hhbnRrQG1pc3Npb25yZWFkeWhxLmNvbTpuajFVUXJHa2plOVNZR25kUENRd0U4NkE=");
            string comment = "Name: "+jiraComment.name+"\nMessage: "+jiraComment.message;
            var commentBody = new { body = comment};
            request.AddBody(commentBody);
            try{
                RestResponse response = await client.ExecuteAsync(request);
                return Ok("Comment added Successfully");
            }
            catch{
                return BadRequest("Failed to add Comment");
            }
        }
        [HttpPost]
        [Route("request")]
        public async Task<IActionResult> newJiraRequest(NewJiraRequest newJiraRequest){
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("prashantk@missionreadyhq.com", newJiraRequest.name);
            var subject = newJiraRequest.subject;
            var to = new EmailAddress("prashantk@missionreadyhq.com", "Native Staff");
            var plainTextContent = "";
            var htmlContent = "Email: "+newJiraRequest.email+"\n\n"+newJiraRequest.message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            try
            {
                var response = await client.SendEmailAsync(msg);
                return Ok("Sent Successfully");
            }catch{
                return BadRequest("Failed to Send Request");
            }
            
        }
    }
}
