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
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ConsoleUser.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class JiraController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public JiraController(IConfiguration configuration){
            this.configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> JiraEpicTickets(string userType)
        {
            var BasicAuth = configuration.GetValue<string>("JiraAuthKey");
            var DomainUrl = configuration.GetSection("JiraApi:Domain").Value;
            var AllEpics = configuration.GetSection("JiraApi:AllEpics").Value;
            var AllTasks = configuration.GetSection("JiraApi:AllTasks").Value;

            var options = new RestClientOptions(DomainUrl)
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };
            var client = new RestClient(options);
            var request = new RestRequest(AllEpics, Method.Get);
            request.AddHeader("Authorization", BasicAuth);
            RestResponse response = await client.ExecuteAsync(request);
            var jiraData = JsonConvert.DeserializeObject<JiraResponse>(response.Content);
            var jiraEpicIssues = new List<JiraEpicData> { };
            if(userType == "Staff")
            {
                var taskRequest = new RestRequest(AllTasks, Method.Get);
                taskRequest.AddHeader("Authorization", BasicAuth);
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
                    jiraEpic.urgencyColour = jiraEpic.Complete > 100 ? "palered" : jiraEpic.Complete > 90 ? "paleorange" : jiraEpic.Complete > 75 ? "paleyellow" : "palegreen";
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
            var BasicAuth = configuration.GetValue<string>("JiraAuthKey");
            var DomainUrl = configuration.GetSection("JiraApi:Domain").Value;
            var options = new RestClientOptions(DomainUrl)
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };
            var client = new RestClient(options);
            string endpoint = "/issue/" + jiraComment.key + "/comment";
            var request = new RestRequest(endpoint, Method.Post);
            request.AddHeader("Authorization", BasicAuth);
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
            var SendGridEmail = configuration.GetSection("SendGrid:Email").Value;

            var apiKey = Environment.GetEnvironmentVariable("SendGrid_API");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(SendGridEmail, newJiraRequest.name);
            var subject = newJiraRequest.subject;
            var to = new EmailAddress(SendGridEmail, "Native Staff");
            var plainTextContent = "";
            var htmlContent = "<p>From: "+newJiraRequest.email+"</p><p>"+newJiraRequest.message + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            try
            {
                var response = await client.SendEmailAsync(msg);
                return Ok("Sent Successfully");
            }
            catch
            {
                return BadRequest("Failed to Send Request");
            }

        }
    }
}
