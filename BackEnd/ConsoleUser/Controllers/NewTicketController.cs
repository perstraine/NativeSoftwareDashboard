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
            NewTicket ticketData = CreateNewTicket(newTicketFromFrontend);
            string BasicAuth = "Basic cmFuanVyYXZlZGV2QGdtYWlsLmNvbTpXZWJkZXZfMjAyMg==";
            string Cookie = "__cfruid=ceb12173d3cab3d119fbd3324ebacedd641c6ac7-1669319739; _zendesk_cookie=BAhJIhl7ImRldmljZV90b2tlbnMiOnt9fQY6BkVU--459ed01949a36415c1716b5711271c3d08918307";
            var options = new RestClientOptions("https://native2881.zendesk.com")
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };
            var client = new RestClient(options);

            var request = new RestRequest("/api/v2/tickets", Method.Post);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", Cookie);
            request.AddParameter("application/json", ticketData.ticket, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            
            return Ok(response);
        }

        private NewTicket CreateNewTicket(NewTicketFromFrontend? responseData)
        {
            var billable = new NewTicket.CustomField();
            billable.id = 12776405299737;
            billable.value = true;

            var newticket = new NewTicket();
            newticket.ticket.custom_fields = new List<NewTicket.CustomField>();
            
            newticket.ticket.custom_fields.Add(billable);
            newticket.ticket.type = responseData.type;
            newticket.ticket.priority = responseData.priority; 
            newticket.ticket.subject = responseData.subject;
            newticket.ticket.description = responseData.description;
            newticket.ticket.requester_id = 12776405299737;

            return (newticket);
        }
    }
}
