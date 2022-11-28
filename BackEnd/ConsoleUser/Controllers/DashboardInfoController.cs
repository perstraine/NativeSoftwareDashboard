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
        private readonly IConfiguration configuration;
        public DashboardInfoController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            var BasicAuth = configuration.GetValue<string>("ZendeskAuthKey");
            var Cookie = configuration.GetValue<string>("ZendeskCookieKey");
            var DomainUrl = configuration.GetSection("ZendeskAPI:Domain").Value;
            var UsersUrl = configuration.GetSection("ZendeskAPI:Users").Value;
            var MetrixUrl = configuration.GetSection("ZendeskAPI:Metrix").Value;
            var TicketUrl = configuration.GetSection("ZendeskAPI:Tickets").Value;

            var options = new RestClientOptions(DomainUrl)
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };

            var client = new RestClient(options);
            var ticketRequest = new RestRequest(TicketUrl, Method.Get);
            var metricsRequest = new RestRequest(MetrixUrl, Method.Get);
            var userRequest = new RestRequest(UsersUrl, Method.Get);

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
        [Route("Customer")]
        public async Task<IActionResult> GetCustomerTickets(string email)
        {
            var BasicAuth = configuration.GetValue<string>("ZendeskAuthKey");
            var Cookie = configuration.GetValue<string>("ZendeskCookieKey");
            var DomainUrl = configuration.GetSection("ZendeskAPI:Domain").Value;
            var UsersUrl = configuration.GetSection("ZendeskAPI:Users").Value;
            var MetrixUrl = configuration.GetSection("ZendeskAPI:Metrix").Value;
            var TicketUrl = configuration.GetSection("ZendeskAPI:Tickets").Value;

            var options = new RestClientOptions(DomainUrl)
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };

            var client = new RestClient(options);
            var ticketRequest = new RestRequest(TicketUrl, Method.Get);
            var metricsRequest = new RestRequest(MetrixUrl, Method.Get);
            var userRequest = new RestRequest(UsersUrl, Method.Get);

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
            var dashboardGeneralInfo = CreateUserDetailsInfo(zendeskData, zendeskMetrics, zendeskUsers, email);

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
        private static DashboardGeneralInfo CreateUserDetailsInfo(ZendeskData? zendeskData, ZendeskMetrics? zendeskMetrics, ZendeskUsers? zendeskUsers, string email)
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

            //find user
            User loggedInUser = new User();
            foreach (var user in zendeskUsers.users)
            {
                if (user.email == email)
                {
                    loggedInUser = user;
                }
            }

            foreach (var ticket in zendeskData.tickets)
            {
                if (ticket.requester_id.ToString() == loggedInUser.id.ToString())
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
            }

            return dashboardGeneralInfo;
        }
    }
}
