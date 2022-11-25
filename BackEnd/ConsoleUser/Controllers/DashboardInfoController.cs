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
            string BasicAuth = "Basic cmFuanVyYXZlZGV2QGdtYWlsLmNvbTpXZWJkZXZfMjAyMg==";
            string Cookie = "__cf_bm=NV4J3cTJmKvuqQCpPM5uYQygQbMa1KqcKC74uEfpYRE-1669145356-0-AQ8bmpK/6KOJqFK753Q+XKYR/nOaz6GgpzbhtlKOOBFY/Do8Z5sRIUYKKWLDpkXAsMbZGbWPNXA/jgWFl1wblgDcguCWBfSeLrxzRVI4UGcD; __cfruid=353ffba05ab8b9f0ee0a58e3b51e838c95d2159f-1669143409; __cfruid=5b67cf441a7537636be54b819fa8ae1bf4e0c42a-1669145370; _zendesk_cookie=BAhJIhl7ImRldmljZV90b2tlbnMiOnt9fQY6BkVU--459ed01949a36415c1716b5711271c3d08918307";

            var options = new RestClientOptions("https://native2881.zendesk.com")
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };

            var client = new RestClient(options);
            var ticketRequest = new RestRequest("/api/v2/tickets", Method.Get);
            var metricsRequest = new RestRequest("/api/v2/ticket_metrics.json", Method.Get);
            var userRequest = new RestRequest("/api/v2/users.json", Method.Get);

            ticketRequest.AddHeader("Authorization", BasicAuth);
            metricsRequest.AddHeader("Authorization", BasicAuth);
            userRequest.AddHeader("Authorization", BasicAuth);

            ticketRequest.AddHeader("Cookie", Cookie);
            metricsRequest.AddHeader("Cookie", Cookie);
            userRequest.AddHeader("Cookie", Cookie);

            RestResponse ticketResponse = await client.ExecuteAsync(ticketRequest);
            RestResponse metricsResponse = await client.ExecuteAsync(metricsRequest);
            RestResponse userResponse = await client.ExecuteAsync(userRequest);

            var zendeskData = JsonConvert.DeserializeObject<ZendeskData>(ticketResponse.Content);
            var zendeskMetrics = JsonConvert.DeserializeObject<ZendeskMetrics>(metricsResponse.Content);
            var zendeskUsers = JsonConvert.DeserializeObject<ZendeskUsers>(userResponse.Content);

            // Creating Json for dashboard
            var dashboardGeneralInfo = CreateTicketInfo(zendeskData, zendeskMetrics, zendeskUsers);

            return Ok(dashboardGeneralInfo);
        }

        [HttpGet]
        [Route("customer")]
        public async Task<IActionResult> GetCustomerTickets(string email)
        {
            string BasicAuth = "Basic cmFuanVyYXZlZGV2QGdtYWlsLmNvbTpXZWJkZXZfMjAyMg==";
            string Cookie = "__cf_bm=NV4J3cTJmKvuqQCpPM5uYQygQbMa1KqcKC74uEfpYRE-1669145356-0-AQ8bmpK/6KOJqFK753Q+XKYR/nOaz6GgpzbhtlKOOBFY/Do8Z5sRIUYKKWLDpkXAsMbZGbWPNXA/jgWFl1wblgDcguCWBfSeLrxzRVI4UGcD; __cfruid=353ffba05ab8b9f0ee0a58e3b51e838c95d2159f-1669143409; __cfruid=5b67cf441a7537636be54b819fa8ae1bf4e0c42a-1669145370; _zendesk_cookie=BAhJIhl7ImRldmljZV90b2tlbnMiOnt9fQY6BkVU--459ed01949a36415c1716b5711271c3d08918307";

            var options = new RestClientOptions("https://native2881.zendesk.com")
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };

            var client = new RestClient(options);
            var ticketRequest = new RestRequest("/api/v2/tickets", Method.Get);
            var metricsRequest = new RestRequest("/api/v2/ticket_metrics.json", Method.Get);
            var userRequest = new RestRequest("/api/v2/users", Method.Get);

            ticketRequest.AddHeader("Authorization", BasicAuth);
            metricsRequest.AddHeader("Authorization", BasicAuth);
            userRequest.AddHeader("Authorization", BasicAuth);

            ticketRequest.AddHeader("Cookie", Cookie);
            metricsRequest.AddHeader("Cookie", Cookie);
            userRequest.AddHeader("Cookie", Cookie);

            RestResponse ticketResponse = await client.ExecuteAsync(ticketRequest);
            RestResponse metricsResponse = await client.ExecuteAsync(metricsRequest);
            RestResponse userResponse = await client.ExecuteAsync(userRequest);

            var zendeskData = JsonConvert.DeserializeObject<ZendeskData>(ticketResponse.Content);
            var zendeskMetrics = JsonConvert.DeserializeObject<ZendeskMetrics>(metricsResponse.Content);
            var zendeskUsers = JsonConvert.DeserializeObject<ZendeskUsers>(userResponse.Content);

            // Creating Json for dashboard
            var dashboardGeneralInfo = CreateUserTicketInfo(zendeskData, zendeskMetrics, zendeskUsers, email);

            return Ok(dashboardGeneralInfo);
        }

        // Dashboard specific JSON creation
        private static DashboardGeneralInfo CreateTicketInfo(ZendeskData? zendeskData, ZendeskMetrics? zendeskMetrics, ZendeskUsers? zendeskUsers)
        {
            //Finding Sunday of the week
            var sundayDate = DateTime.Today.ToLocalTime();
            while (sundayDate.DayOfWeek != DayOfWeek.Sunday)
            {
                sundayDate = sundayDate.AddDays(-1);
            }

            //Finding the tickets
            var dashboardGeneralInfo = new DashboardGeneralInfo();
            dashboardGeneralInfo.id = 0;

            foreach (var ticket in zendeskData.tickets)
            {
                if (ticket.status == "solved" || ticket.status == "closed")
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

                if (ticket.priority == "urgent" && ticket.status != "solved" && ticket.status != "closed")
                {
                    dashboardGeneralInfo.UrgentTickets++;
                }
            }

            return dashboardGeneralInfo;
        }

        // Custer Dashboard specific JSON creation
        private static DashboardGeneralInfo CreateUserTicketInfo(ZendeskData? zendeskData, ZendeskMetrics? zendeskMetrics, ZendeskUsers? zendeskUsers, string email)
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
            
            //find user
            User loggedInUser = new User();
            foreach(var user in zendeskUsers.users)
            {
                if(user.email == email)
                {
                    loggedInUser = user;
                }
            }
            dashboardGeneralInfo.Customer = loggedInUser.name;

            foreach (var ticket in zendeskData.tickets)
            {
                if(ticket.status == "solved" || ticket.status == "closed" && ticket.requester_id == loggedInUser.id)
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
