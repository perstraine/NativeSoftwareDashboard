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
            var metricsRequest = new RestRequest("/api/v2/ticket_metrics.json", Method.Get);

            ticketRequest.AddHeader("Authorization", "Basic cmF2ZWVuZHJhbnJhbmp1QGdtYWlsLmNvbTpXZWJkZXZfMjAyMg==");
            metricsRequest.AddHeader("Authorization", "Basic cmF2ZWVuZHJhbnJhbmp1QGdtYWlsLmNvbTpXZWJkZXZfMjAyMg==");

            //TODO do we actually need cookies
            ticketRequest.AddHeader("Cookie", "__cfruid=48f83724a725243fd95c678dd50f3dd2d953d978-1667859886; _zendesk_cookie=BAhJIhl7ImRldmljZV90b2tlbnMiOnt9fQY6BkVU--459ed01949a36415c1716b5711271c3d08918307");
            metricsRequest.AddHeader("Cookie", "__cf_bm=KxlySeaioTuVYvuFk4E5tRI8BOWLks6ByFoscguzUOI-1667859951-0-Ac9xh1i7NuOLkLmngWxRlUgF6yk+oGtLlnHg4MgzJ4VxGYAuk2O39dvMOHMT6RbdxhM5Hqft8CvRxOFHUu7Hqxs6qh6HI1xHSqrrODKXfd/E; __cfruid=d6d7639fe6c226ee32f5b8fed32a369d01cc9511-1667859951; __cfruid=92c90c6eb9ec4f99a11232886662497fe9237eb9-1667859962; _zendesk_cookie=BAhJIhl7ImRldmljZV90b2tlbnMiOnt9fQY6BkVU--459ed01949a36415c1716b5711271c3d08918307");

            RestResponse ticketResponse = await client.ExecuteAsync(ticketRequest);
            RestResponse metricsResponse = await client.ExecuteAsync(metricsRequest);

            var zendeskData = JsonConvert.DeserializeObject<ZendeskData>(ticketResponse.Content);
            var zendeskMetrics = JsonConvert.DeserializeObject<ZendeskMetrics>(metricsResponse.Content);

            // Creating Json for dashboard
            var dashboardGeneralInfo = CreateTicketInfo(zendeskData, zendeskMetrics);

            return Ok(dashboardGeneralInfo);
        }

        // Dashboard specific JSON creation
        private static DashboardGeneralInfo CreateTicketInfo(ZendeskData? zendeskData, ZendeskMetrics? zendeskMetrics)
        {
            //Finding Sunday of the week
            var sundayDate = DateTime.Today.ToLocalTime();
            while(sundayDate.DayOfWeek != DayOfWeek.Sunday)
            {
                sundayDate = sundayDate.AddDays(-1);
            }
            
            //Finding the tickets
            var dashboardGeneralInfo = new DashboardGeneralInfo();
            dashboardGeneralInfo.id = 0;
            foreach (var ticket in zendeskData.tickets)
            {
                if(ticket.status == "solved" || ticket.status == "closed")
                {
                    var ticketid = ticket.id;
                        foreach (var metrics in zendeskMetrics.ticket_metrics)
                        {
                            if (metrics.ticket_id == ticketid)
                            {
                                DateTime solvedDate = (DateTime)metrics.solved_at;
                                // closed tickets added only if ticket is solved beween sunday and 7 days after it.
                                if (solvedDate.ToLocalTime() >= sundayDate && solvedDate <= sundayDate.AddDays(7)) 
                                {
                                    dashboardGeneralInfo.ClosedTickets++;
                                }
                            }
                        }
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
