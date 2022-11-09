using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;

namespace ConsoleUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JiraController : ControllerBase
    {
        [HttpGet]
        public string JiraEpicTickets()
        {
            var options = new RestClientOptions("https://ranjurave.atlassian.net/rest/api/2")
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };
            var client = new RestClient(options);
            var request = new RestRequest("/search?jql=issuetype=Epic%20AND%20project=AT", Method.Get);
            request.AddHeader("Authorization", "Basic cHJhc2hhbnRrQG1pc3Npb25yZWFkeWhxLmNvbTpuajFVUXJHa2plOVNZR25kUENRd0U4NkE=");
            RestResponse response = client.Execute(request);
            // Console.WriteLine(response.Content);
            string rawResponse = response.Content;
            var jsonResponse = JsonObject.Parse(rawResponse);
            return jsonResponse.ToString();
        }
        //[Authorize]
        //public Task<IActionResult> GetJiraTickets(AddUserRequest AddUserRequest)
        //{
        //    return ;
        //}
    }
}
