﻿using ConsoleUser.Controllers;
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
        private readonly ICustomerRepository customerRepository;
        private readonly ISupportLevelRepository customerSupportLevelRepository;

        public CustomerViewController(ICustomerRepository customerRepository, ISupportLevelRepository customerSupportLevelRepository, IUserRepository userRepository)
        {
            this.userRepository = userRepository;
            this.customerRepository = customerRepository;
            this.customerSupportLevelRepository = customerSupportLevelRepository;
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
            var customers = customerRepository.GetAll();
            var supportLevel = customerSupportLevelRepository.GetAll();

            foreach (var user in users)
            {
                if(user.UserType == userType)
                {
                    userEmail = user.Email;
                    break;
                }
            }
  
            var client = new RestClient(options);
            var usersRequest = new RestRequest("/api/v2/users", Method.Get);
            var metricsRequest = new RestRequest("/api/v2/ticket_metrics.json", Method.Get);

            usersRequest.AddHeader("Authorization", BasicAuth);
            metricsRequest.AddHeader("Authorization", BasicAuth);

            usersRequest.AddHeader("Cookie", Cookie);
            metricsRequest.AddHeader("Cookie", Cookie);

            RestResponse usersResponse = await client.ExecuteAsync(usersRequest);
            RestResponse metricsResponse = await client.ExecuteAsync(metricsRequest);

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

            var dashboardUserTicketData = CreateUserTicketData(userTicket.tickets, zendeskUsers, zendeskMetrics.ticket_metrics, customers, supportLevel);

            return Ok(dashboardUserTicketData);
        }

        //Creating the tickets
        private static List<DashboardUserTicketData> CreateUserTicketData(List<UserTicketData.Ticket> tickets, ZendeskUsers? zendeskUsers, List<TicketMetric> ticketMetrices, IEnumerable<ConsoleUser.Models.Customer> customers, IEnumerable<ConsoleUser.Models.CustomerSupportLevel> supportLevel)
        {
            if(tickets == null) { return null; }

            var dashboardUserTicketData = new List<DashboardUserTicketData>();

            foreach (var ticket in tickets)
            {
                var userTicketData = new DashboardUserTicketData();
                userTicketData.Id = ticket.id;
                userTicketData.Subject = ticket.subject;
                userTicketData.RequestedDate = ticket.created_at.ToLocalTime().ToString();
                userTicketData.FirstResponse = GetFirstUpdate(ticket, ticketMetrices);
                userTicketData.LastUpdate = GetLastUpdate(ticket, ticketMetrices);
                userTicketData.TimeDue = GetTimeDueMinusOffHours(ticket,ticket.created_at,ticket.priority,zendeskUsers,customers,supportLevel);
                userTicketData.Priority = ticket.priority;
                userTicketData.Type = ticket.type;
                userTicketData.url = ticket.url;
                userTicketData.TrafficLight = GetTrafficLight(DateTime.Parse(userTicketData.TimeDue));

                dashboardUserTicketData.Add(userTicketData);
            }

            dashboardUserTicketData = QuickSortTickets(dashboardUserTicketData, 0, dashboardUserTicketData.Count - 1);

            return dashboardUserTicketData;
        }

        private static string GetFirstUpdate(UserTicketData.Ticket ticket, List<TicketMetric> ticketMetrices)
        {
            foreach (var ticketMatrix in ticketMetrices)
            {
                if (ticket.id == ticketMatrix.ticket_id)
                {
                    return ticketMatrix.updated_at.ToLocalTime().ToString();
                }
            }

            return ticket.created_at.ToLocalTime().ToString();
        }

        private static string GetLastUpdate(UserTicketData.Ticket ticket, List<TicketMetric> ticketMetrices)
        {
            foreach (var ticketMatrix in ticketMetrices)
            {
                if (ticket.id == ticketMatrix.ticket_id)
                {
                    return ticketMatrix.assignee_updated_at.ToLocalTime().ToString();
                }
            }

            return ticket.created_at.ToLocalTime().ToString();
        }

        private static string GetTimeDueMinusOffHours(UserTicketData.Ticket ticket, DateTime requestedDate, string priority, ZendeskUsers? zendeskUsers, IEnumerable<ConsoleUser.Models.Customer> customers, IEnumerable<ConsoleUser.Models.CustomerSupportLevel> supportLevel)//DateTime requestedDate, UserTicketData.Ticket ticket, IEnumerable<ConsoleUser.Models.User> zendeskUsers, IEnumerable<ConsoleUser.Models.Customer> customers, IEnumerable<ConsoleUser.Models.CustomerSupportLevel> supportLevel)
        {
            requestedDate = requestedDate.ToLocalTime();
            string organisation = GetZendeskUserName(zendeskUsers, ticket);
            //If ticket logged after hours, then log time changed to next working day morning.
            double resolutionTime = 0;
            double workHoursPerDay = 8;
            int dayStartHours = 8;
            int dayStartMinutes = 30;
            int dayEndHours = 17;
            int dayEndMinutes = 0;
            DateTime requestedDayStart = new DateTime(requestedDate.Year, requestedDate.Month, requestedDate.Day, dayStartHours, dayStartMinutes, 0);
            DateTime businessHoursEnd = new DateTime(requestedDate.Year, requestedDate.Month, requestedDate.Day, dayEndHours, dayEndMinutes, 0);
            DateTime nextBusinessHoursStart = requestedDate.AddDays(1);
            nextBusinessHoursStart = new DateTime(nextBusinessHoursStart.Year, nextBusinessHoursStart.Month, nextBusinessHoursStart.Day, dayStartHours, dayStartMinutes, 0);

            // if next day is weekend find next Monday
            if (nextBusinessHoursStart.DayOfWeek == DayOfWeek.Saturday)
            {
                nextBusinessHoursStart = nextBusinessHoursStart.AddDays(2);
            }
            else if (nextBusinessHoursStart.DayOfWeek == DayOfWeek.Sunday)
            {
                nextBusinessHoursStart = nextBusinessHoursStart.AddDays(1);
            }

            if (DateTime.Compare(requestedDate, requestedDayStart) < 0) { requestedDate = requestedDayStart; } // If ticket logged before work hours
            if (DateTime.Compare(requestedDate, businessHoursEnd) > 0) { requestedDate = nextBusinessHoursStart; } // if ticket logged after work hours

            //Finding resolution time based on customer tier and ticket priority
            foreach (var customer in customers)
            {
                if (customer.CustomerCodeZendesk == organisation)
                {
                    int customerSupportLevel = customer.SupportLevel;

                    foreach (var level in supportLevel)
                    {
                        if (level.SupportLevel == customerSupportLevel)
                        {
                            resolutionTime = priority == "urgent" ? level.ResolutionTimeUrgent : priority == "high" ? level.ResolutionTimeHigh : priority == "normal" ? level.ResolutionTimeNormal : level.ResolutionTimeLow;
                        }
                    }
                }
            }

            //Adding off business hours to time due
            DateTime timeDue = requestedDate; //starting timeDue to ticket requested date
            businessHoursEnd = new DateTime(timeDue.Year, timeDue.Month, timeDue.Day, dayEndHours, dayEndMinutes, 0); // setting business hours end time for the current timeDue
            double timeLeftInDay = businessHoursEnd.Subtract(timeDue).TotalHours; // Balance time left in that day.

            if (resolutionTime <= timeLeftInDay) // If work can be finished in the same day.
            {
                timeDue = timeDue.AddHours(resolutionTime); // Adding response time to timeDue
                return timeDue.ToString();
            }

            // If cannot be completed in same day
            while (resolutionTime > timeLeftInDay) // Loop reducing 8 hrs until balance time is enough for the day
            {
                resolutionTime -= timeLeftInDay; // reducing work done for the day.
                timeLeftInDay = workHoursPerDay;  // Adding next days 8 hours.
                timeDue = timeDue.AddDays(1); // Adding a day to timeDue
                timeDue = new DateTime(timeDue.Year, timeDue.Month, timeDue.Day, dayStartHours, dayStartMinutes, 0); //setting day start time
                if (timeDue.DayOfWeek == DayOfWeek.Saturday)
                {
                    timeDue = timeDue.AddDays(2);
                }
            }
            timeDue = timeDue.AddDays(1);
            timeDue = new DateTime(timeDue.Year, timeDue.Month, timeDue.Day, dayStartHours, dayStartMinutes, 0); // Adding a day to timeDue
            // If added day is on saturday then skip to next Monday
            if (timeDue.DayOfWeek == DayOfWeek.Saturday)
            {
                timeDue = timeDue.AddDays(2);
            }
            timeDue = timeDue.AddHours(resolutionTime); // Adding balance time to 

            return timeDue.ToString();
        }

        private static string GetZendeskUserName(ZendeskUsers? zendeskUsers, UserTicketData.Ticket ticket)
        {
            foreach (var user in zendeskUsers.users)
            {
                if (ticket.requester_id.ToString() == user.id.ToString())
                {
                    return user.name;
                }
            }
            return "null";
        }
        
        private static string GetTrafficLight(DateTime timeDue)
        {
            float veryHigh = 2;
            float high = 8;
            float normal = 24;
            float low = 48;
            double dueHours = (timeDue - DateTime.Now.ToLocalTime()).TotalHours;
            return dueHours <= veryHigh ? "red" : dueHours <= high ? "orange" : dueHours <= normal ? "yellow" : "green";
        }
        
        private static List<DashboardUserTicketData> QuickSortTickets(List<DashboardUserTicketData> dashboardUserTicketData, int leftIndex, int rightIndex)
        {
            var i = leftIndex;
            var j = rightIndex;
            var pivot = dashboardUserTicketData[leftIndex];

            while (i <= j)
            {
                while (DateTime.Parse(dashboardUserTicketData[i].TimeDue) < DateTime.Parse(pivot.TimeDue))
                {
                    i++;
                }

                while (DateTime.Parse(dashboardUserTicketData[j].TimeDue) > DateTime.Parse(pivot.TimeDue))
                {
                    j--;
                }

                if (i <= j)
                {
                    (dashboardUserTicketData[j], dashboardUserTicketData[i]) = (dashboardUserTicketData[i], dashboardUserTicketData[j]); // swap using tuples
                    i++;
                    j--;
                }
            }

            if (leftIndex < j)
                QuickSortTickets(dashboardUserTicketData, leftIndex, j);

            if (i < rightIndex)
                QuickSortTickets(dashboardUserTicketData, i, rightIndex);

            return dashboardUserTicketData;
        }
    }
}