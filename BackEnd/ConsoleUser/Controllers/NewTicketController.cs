using ConsoleUser.Models;
using ConsoleUser.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.ComponentModel;

namespace ConsoleUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewTicketController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddTicket(NewTicketFromFrontend newTicketFromFrontend)
        {
            string BasicAuth = "Basic cmFuanVyYXZlZGV2QGdtYWlsLmNvbTpXZWJkZXZfMjAyMg==";
            string Cookie = "__cfruid=ceb12173d3cab3d119fbd3324ebacedd641c6ac7-1669319739; _zendesk_cookie=BAhJIhl7ImRldmljZV90b2tlbnMiOnt9fQY6BkVU--459ed01949a36415c1716b5711271c3d08918307";
            
            var options = new RestClientOptions("https://native2881.zendesk.com")
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };
            var client = new RestClient(options);
            var usersRequest = new RestRequest("/api/v2/users", Method.Get);
            usersRequest.AddHeader("Authorization", BasicAuth);
            usersRequest.AddHeader("Cookie", Cookie);
            RestResponse usersResponse = await client.ExecuteAsync(usersRequest);
            var zendeskUsers = JsonConvert.DeserializeObject<ZendeskUsers>(usersResponse.Content);

            NewZendeskTicket.NewTicket ticketToAdd = new NewZendeskTicket.NewTicket();
            NewZendeskTicket.Ticket ticketData = CreateNewTicket(newTicketFromFrontend, zendeskUsers);
            ticketToAdd.ticket = ticketData;

            var request = new RestRequest("/api/v2/tickets", Method.Post);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", Cookie);
            
            request.AddParameter("application/json", ticketToAdd, ParameterType.RequestBody);

            RestResponse response = client.Execute(request);

            return Ok();
        }

        private NewZendeskTicket.Ticket CreateNewTicket(NewTicketFromFrontend? responseData, ZendeskUsers? zendeskUsers)
        {
            NewZendeskTicket.CustomField billable = new NewZendeskTicket.CustomField();
            billable.id = 12765904262169;
            billable.value = true;

            var newticket = new NewZendeskTicket.Ticket();
            newticket.custom_fields = new List<NewZendeskTicket.CustomField>();
            
            newticket.custom_fields.Add(billable);
            newticket.type = responseData.type;
            newticket.priority = responseData.priority; 
            newticket.subject = responseData.subject;
            newticket.description = responseData.description;
            foreach (var user in zendeskUsers.users)
            {
                if (user.email == responseData.email)
                {
                    string tempid = user.id.ToString();
                    newticket.requester_id = long.Parse(tempid);
                }
            }

            return (newticket);
        }
    }
}
