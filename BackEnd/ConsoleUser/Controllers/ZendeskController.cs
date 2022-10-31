using ConsoleUser.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;

namespace Zendesk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZendeskController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            var options = new RestClientOptions("https://missionready5835.zendesk.com")
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };
            var client = new RestClient(options);
            var request = new RestRequest("/api/v2/tickets", Method.Get);
            request.AddHeader("Authorization", "Basic cmFuanVyYXZlQGdtYWlsLmNvbTpXZWJkZXZfMjAyMg==");
            //TODO do we actually need cookies
            request.AddHeader("Cookie", "__cfruid=453dcbc27454258463f1141b2d97bb40b6e79538-1666769874; _zendesk_cookie=BAhJIhl7ImRldmljZV90b2tlbnMiOnt9fQY6BkVU--459ed01949a36415c1716b5711271c3d08918307; _zendesk_session=TU9uMjhreXZMQWR5M0YyMThRYVBpWlBEV2VHWW5WWTJmZEdXZE13T2FGeG9zN2ZuOWEwaU1rSStlR29KSUpFbitqbXZZY2dyOUx1SHF4ZTZWU1AxdGpDSy9MeWlNRThtalRWQ0tmUlI5OERDRk0zK0NjdW9TOXo4Z3RpTmhYbUMtLUMzc0RUOGZNVkVkOElLVHg3bTJFMkE9PQ%3D%3D--90ca0ca180d8b1630519eeffa7e18f3dd2ece3e7");
            RestResponse response = await client.ExecuteAsync(request);
            var zendeskData = JsonConvert.DeserializeObject<ZendeskData>(response.Content);

            // Creating Json for dashboard
            var dashboardTickets = CreateTicketInfo(zendeskData);
            return Ok(dashboardTickets);
        }

        // Dashboard JSON creation function
        private static List<DashboardTicketData> CreateTicketInfo(ZendeskData? zendeskData)
        {
            var ticketList = new List<DashboardTicketData>();
            foreach (var ticket in zendeskData.tickets)
            {
                var dashboardTicket = new DashboardTicketData();
                if (ticket.organization_id != null) dashboardTicket.Organisation = ticket.organization_id.ToString();
                if (ticket.subject != null) dashboardTicket.Subject = ticket.subject;
                if (ticket.assignee_id != null) dashboardTicket.Recipient = ticket.assignee_id.ToString();//TODO assignee or recepient
                dashboardTicket.Billable = true; //TODO tobe decided
                if (ticket.priority != null) dashboardTicket.Priority = ticket.priority;
                if (ticket.created_at != null) dashboardTicket.RequestedTime = ticket.created_at;
                if (ticket.due_at != null) dashboardTicket.TimeDue = ticket.updated_at; //TODO tobe calculated
                if (ticket.priority != null) dashboardTicket.Type = ticket.priority;
                if (ticket.url != null) dashboardTicket.url = ticket.url;
                ticketList.Add(dashboardTicket);
            }
            return ticketList;
        }
    }
}
