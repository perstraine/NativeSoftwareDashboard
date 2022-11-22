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
using System.Threading;

namespace Zendesk.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerViewController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public CustomerViewController(ICustomerRepository customerRepository, ISupportLevelRepository customerSupportLevelRepository, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets(string userType)
        {
            string BasicAuth = "Basic cmFuanVyYXZlZGV2QGdtYWlsLmNvbTpXZWJkZXZfMjAyMg==";
            string Cookie = "__cf_bm=NV4J3cTJmKvuqQCpPM5uYQygQbMa1KqcKC74uEfpYRE-1669145356-0-AQ8bmpK/6KOJqFK753Q+XKYR/nOaz6GgpzbhtlKOOBFY/Do8Z5sRIUYKKWLDpkXAsMbZGbWPNXA/jgWFl1wblgDcguCWBfSeLrxzRVI4UGcD; __cfruid=353ffba05ab8b9f0ee0a58e3b51e838c95d2159f-1669143409; __cfruid=5b67cf441a7537636be54b819fa8ae1bf4e0c42a-1669145370; _zendesk_cookie=BAhJIhl7ImRldmljZV90b2tlbnMiOnt9fQY6BkVU--459ed01949a36415c1716b5711271c3d08918307";
            var options = new RestClientOptions("https://native2881.zendesk.com")
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };

            var userEmail = "";
            var users = userRepository.GetAll();
            foreach(var user in users)
            {
                if(user.UserType == userType)
                {
                    userEmail = user.Email;
                    break;
                }
            }
  

            var client = new RestClient(options);
            var ticketRequest = new RestRequest("/api/v2/tickets", Method.Get);
            var usersRequest = new RestRequest("/api/v2/users", Method.Get);
            var metricsRequest = new RestRequest("/api/v2/ticket_metrics.json", Method.Get);

            ticketRequest.AddHeader("Authorization", BasicAuth); 
            usersRequest.AddHeader("Authorization", BasicAuth);
            metricsRequest.AddHeader("Authorization", BasicAuth);

            ticketRequest.AddHeader("Cookie", Cookie);
            usersRequest.AddHeader("Cookie", Cookie);
            metricsRequest.AddHeader("Cookie", Cookie);

            RestResponse ticketResponse = await client.ExecuteAsync(ticketRequest);
            RestResponse usersResponse = await client.ExecuteAsync(usersRequest);
            RestResponse metricsResponse = await client.ExecuteAsync(metricsRequest);

            var zendeskData = JsonConvert.DeserializeObject<ZendeskData>(ticketResponse.Content);
            var zendeskUsers = JsonConvert.DeserializeObject<ZendeskUsers>(usersResponse.Content);
            var zendeskMetrics = JsonConvert.DeserializeObject<ZendeskMetrics>(metricsResponse.Content);

            var userId = "";
            foreach (var user in zendeskUsers.users)
            {
                if (user.email == userEmail)
                {
                    userId = user.id.ToString();
                }
            }
            var apiString = "/api/v2/users/" + userId + "/tickets/requested";
            var userTicketRequest = new RestRequest(apiString, Method.Get);
            userTicketRequest.AddHeader("Authorization", BasicAuth);
            userTicketRequest.AddHeader("Cookie", Cookie);
            RestResponse UserTicketResponse = await client.ExecuteAsync(userTicketRequest);
            var userTicket = JsonConvert.DeserializeObject<UserTicketData.UserTicketData>(UserTicketResponse.Content);

            var dashboardUserTicketData = CreateUserTicketData(userTicket.tickets);



            return Ok(dashboardUserTicketData);
            //return Ok(userTicket);
        }

        private static List<DashboardUserTicketData> CreateUserTicketData(List<UserTicketData.Ticket> tickets)
        {
            var dashboardUserTicketData = new List<DashboardUserTicketData>();

            foreach (var ticket in tickets)
            {
                var userTicketData = new DashboardUserTicketData();
                userTicketData.Id = ticket.id;
                userTicketData.Subject = ticket.subject;
                userTicketData.RequestedDate = ticket.created_at.ToString();
                userTicketData.FirstResponse = ticket.updated_at.ToString();
                userTicketData.LastUpdate = ticket.updated_at.ToString();
                userTicketData.TimeDue = ticket.updated_at.ToString();
                userTicketData.Priority = ticket.priority;
                userTicketData.Type = ticket.type;
                userTicketData.url = ticket.url;
                userTicketData.TrafficLight = "red";
                userTicketData.SortPriority = 1;

                dashboardUserTicketData.Add(userTicketData);
            }
            return dashboardUserTicketData;
        }
    }
}