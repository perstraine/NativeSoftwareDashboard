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
using Microsoft.AspNetCore.Authorization;

namespace ConsoleUser.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class NewTicketController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public NewTicketController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> AddTicket(NewTicketFromFrontend newTicketFromFrontend)
        {
            var BasicAuth = configuration.GetValue<string>("ZendeskAuthKey");
            var Cookie = configuration.GetValue<string>("ZendeskCookieKey");
            var DomainUrl = configuration.GetSection("ZendeskAPI:Domain").Value;
            var UsersUrl = configuration.GetSection("ZendeskAPI:Users").Value;
            var MetrixUrl = configuration.GetSection("ZendeskAPI:Metrix").Value;
            var TicketUrl = configuration.GetSection("ZendeskAPI:Tickets").Value;
            var BillableId = configuration.GetValue<string>("BillableId");

            var options = new RestClientOptions(DomainUrl)
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };
            var client = new RestClient(options);
            var usersRequest = new RestRequest(UsersUrl, Method.Get);
            usersRequest.AddHeader("Authorization", BasicAuth);
            usersRequest.AddHeader("Cookie", Cookie);
            RestResponse usersResponse = await client.ExecuteAsync(usersRequest);
            var zendeskUsers = JsonConvert.DeserializeObject<ZendeskUsers>(usersResponse.Content);

            NewZendeskTicket.NewTicket ticketToAdd = new NewZendeskTicket.NewTicket();
            NewZendeskTicket.Ticket ticketData = CreateNewTicket(newTicketFromFrontend, zendeskUsers, BillableId);
            ticketToAdd.ticket = ticketData;

            var request = new RestRequest(TicketUrl, Method.Post);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", Cookie);
            
            request.AddParameter("application/json", ticketToAdd, ParameterType.RequestBody);

            client.Execute(request);

            return Ok();
        }

        private NewZendeskTicket.Ticket CreateNewTicket(NewTicketFromFrontend? responseData, ZendeskUsers? zendeskUsers, string customFieldBillableId)
        {
            NewZendeskTicket.CustomField billable = new NewZendeskTicket.CustomField();
            billable.id = long.Parse(customFieldBillableId);
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
