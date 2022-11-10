using ConsoleUser.Controllers;
using ConsoleUser.Models.Domain;
using ConsoleUser.Models.DTO;
using ConsoleUser.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Net.Sockets;

namespace Zendesk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardInfoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            var options = new RestClientOptions("https://native9107.zendesk.com")
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };

            var client = new RestClient(options);
            var ticketRequest = new RestRequest("/api/v2/tickets", Method.Get);

            ticketRequest.AddHeader("Authorization", "Basic cmF2ZWVuZHJhbnJhbmp1QGdtYWlsLmNvbTpXZWJkZXZfMjAyMg==");

            //TODO do we actually need cookies
            ticketRequest.AddHeader("Cookie", "__cfruid=48f83724a725243fd95c678dd50f3dd2d953d978-1667859886; _zendesk_cookie=BAhJIhl7ImRldmljZV90b2tlbnMiOnt9fQY6BkVU--459ed01949a36415c1716b5711271c3d08918307");
           
            RestResponse ticketResponse = await client.ExecuteAsync(ticketRequest);

            var zendeskData = JsonConvert.DeserializeObject<ZendeskData>(ticketResponse.Content);

            // Creating Json for dashboard
            var dashboardGeneralInfo = CreateTicketInfo(zendeskData);

            return Ok(dashboardGeneralInfo);
        }

        // Dashboard specific JSON creation
        private static DashboardGeneralInfo CreateTicketInfo(ZendeskData? zendeskData)
        {
            var dashboardGeneralInfo = new DashboardGeneralInfo();
            dashboardGeneralInfo.id = 0;
            foreach (var ticket in zendeskData.tickets)
            {
                if(ticket.status == "solved" || ticket.status == "closed")
                {
                    dashboardGeneralInfo.ClosedTickets++;
                }
                else if (ticket.status != "on_hold" && ticket.status != "pending")
                {
                    dashboardGeneralInfo.ActiveTickets++;
                }

                if(ticket.priority == "urgent" && ticket.status != "solved" && ticket.status != "closed")
                {
                    dashboardGeneralInfo.UrgentTickets++;
                }
            }

            return dashboardGeneralInfo;
        }
    }
}
