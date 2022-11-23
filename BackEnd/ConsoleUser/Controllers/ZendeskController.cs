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
    public class ZendeskController : ControllerBase
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ISupportLevelRepository customerSupportLevelRepository;

        public ZendeskController(ICustomerRepository customerRepository, ISupportLevelRepository customerSupportLevelRepository)
        {
            this.customerRepository = customerRepository;
            this.customerSupportLevelRepository = customerSupportLevelRepository;
        }

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
            //var metricsRequest = new RestRequest("/api/v2/ticket_metrics.json", Method.Get);
            var usersRequest = new RestRequest("/api/v2/users", Method.Get);

            ticketRequest.AddHeader("Authorization", BasicAuth);
            //metricsRequest.AddHeader("Authorization", BasicAuth);
            usersRequest.AddHeader("Authorization", BasicAuth);

            //TODO do we actually need cookies
            ticketRequest.AddHeader("Cookie", Cookie);
            //metricsRequest.AddHeader("Cookie", Cookie);
            usersRequest.AddHeader("Cookie", Cookie);

            RestResponse ticketResponse = await client.ExecuteAsync(ticketRequest);
            //RestResponse metricsResponse = await client.ExecuteAsync(metricsRequest);
            RestResponse usersResponse = await client.ExecuteAsync(usersRequest);

            var zendeskData = JsonConvert.DeserializeObject<ZendeskData>(ticketResponse.Content);
            //var zendeskMetrics = JsonConvert.DeserializeObject<ZendeskMetrics>(metricsResponse.Content);
            var zendeskUsers = JsonConvert.DeserializeObject<ZendeskUsers>(usersResponse.Content);

            var customers = customerRepository.GetAll();
            var supportLevel = customerSupportLevelRepository.GetAll();

            // Creating Json for dashboard
            var dashboardTickets = CreateTicketInfo(zendeskData, zendeskUsers, customers, supportLevel);

            return Ok(dashboardTickets);
        }

        // Dashboard specific JSON creation
        private static List<DashboardTicketData> CreateTicketInfo(ZendeskData? zendeskData, ZendeskUsers? zendeskUsers, IEnumerable<ConsoleUser.Models.Customer> customers, IEnumerable<ConsoleUser.Models.CustomerSupportLevel> supportLevel)
        {
            var ticketList = new List<DashboardTicketData>();
            foreach (var ticket in zendeskData.tickets)
            {
                if(ticket.status != "solved" && ticket.status != "closed")
                {
                    var dashboardTicket = new DashboardTicketData();

                    dashboardTicket.id = ticket.id;
                    dashboardTicket.Organisation = GetZendeskUserName(zendeskUsers, ticket);
                    if (ticket.subject != null) dashboardTicket.Subject = ticket.subject;
                    if (ticket.status != null) dashboardTicket.Status = ticket.status;
                    if (ticket.assignee_id != null) dashboardTicket.Recipient = ticket.assignee_id.ToString();//TODO assignee or recepient
                    dashboardTicket.Billable = GetBillableCustomField(ticket.custom_fields[0]);
                    dashboardTicket.Priority = ticket.priority;
                    dashboardTicket.RequestedDate = ticket.created_at.ToLocalTime().ToString();
                    //dashboardTicket.TimeDue = GetTimeDue(dashboardTicket.Organisation, dashboardTicket.Priority, DateTime.Parse(dashboardTicket.RequestedDate), customers, supportLevel).ToString();
                    dashboardTicket.TimeDue = GetTimeDueMinusOffHours(dashboardTicket.Organisation, dashboardTicket.Priority, DateTime.Parse(dashboardTicket.RequestedDate), customers, supportLevel).ToString();
                    if (ticket.type != null) dashboardTicket.Type = ticket.type;
                    if (ticket.url != null) dashboardTicket.url = "https://native9107.zendesk.com/agent/tickets/" + ticket.id.ToString();
                    dashboardTicket.TrafficLight = GetTrafficLight(DateTime.Parse(dashboardTicket.TimeDue));
                    ticketList.Add(dashboardTicket);
                }
            }

            ticketList = QuickSortTickets(ticketList, 0, ticketList.Count - 1);

            return ticketList;
        }

        //Getting billable field
        private static bool GetBillableCustomField(object jObject)
        {
            var jsonString = JsonConvert.SerializeObject(jObject);
            var fieldDict = JObject.Parse(jsonString);
            return fieldDict["value"].ToObject<bool>();
        }

        //Finding user name from zendesk users api
        static string GetZendeskUserName(ZendeskUsers? zendeskUsers, Ticket ticket)
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

        //Sort tickets on time due to finish
        public static List<DashboardTicketData> QuickSortTickets(List<DashboardTicketData> ticketListToSort, int leftIndex, int rightIndex)
        {
            var i = leftIndex;
            var j = rightIndex;
            var pivot = ticketListToSort[leftIndex];

            while (i <= j)
            {
                while (DateTime.Parse(ticketListToSort[i].TimeDue) < DateTime.Parse(pivot.TimeDue))
                {
                    i++;
                }

                while (DateTime.Parse(ticketListToSort[j].TimeDue) > DateTime.Parse(pivot.TimeDue))
                {
                    j--;
                }

                if (i <= j)
                {
                    (ticketListToSort[j], ticketListToSort[i]) = (ticketListToSort[i], ticketListToSort[j]); // swap using tuples
                    i++;
                    j--;
                }
            }

            if (leftIndex < j)
                QuickSortTickets(ticketListToSort, leftIndex, j);

            if (i < rightIndex)
                QuickSortTickets(ticketListToSort, i, rightIndex);

            return ticketListToSort;
        }

        //Creating time due based on user tier and ticket priority
        private static DateTime GetTimeDue(string organisation, string priority, DateTime requestedDate, IEnumerable<ConsoleUser.Models.Customer> customers, IEnumerable<ConsoleUser.Models.CustomerSupportLevel> supportLevel)
        {
            DateTime timeDue = DateTime.Now;
            foreach (var customer in customers)
            {
                if(customer.CustomerCode == organisation)
                {
                    int customerSupportLevel = customer.SupportLevel;

                    foreach (var level in supportLevel)
                    {
                        if(level.SupportLevel == customerSupportLevel)
                        {
                            var resolutionTime = priority == "urgent" ? level.ResolutionTimeUrgent : priority == "high" ? level.ResolutionTimeHigh : priority == "normal" ? level.ResolutionTimeNormal : level.ResolutionTimeLow;
                            timeDue = requestedDate.AddHours(resolutionTime);
                            //return requestedDate.AddHours(resolutionTime);
                        }
                    }
                }
            }
            
            return timeDue;
        }

        private static DateTime GetTimeDueMinusOffHours(string organisation, string priority, DateTime requestedDate, IEnumerable<ConsoleUser.Models.Customer> customers, IEnumerable<ConsoleUser.Models.CustomerSupportLevel> supportLevel)
        {
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

            // If requested date is off hours or on weekend then requested date is the next working day.
            if(DateTime.Compare(requestedDate, requestedDayStart) < 0 ) { requestedDate = requestedDayStart; }
            if(DateTime.Compare(requestedDate, businessHoursEnd) > 0 ) { requestedDate = nextBusinessHoursStart; }
            


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
                return timeDue;
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
            
            return timeDue;
        }

        //Generating colour codes based on 4 different time frames
        private static string GetTrafficLight(DateTime timeDue)
        {
            float veryHigh = 2;
            float high = 8;
            float normal = 24;
            float low = 48;
            double dueHours = (timeDue - DateTime.Now.ToLocalTime()).TotalHours;           
            return dueHours <= veryHigh ? "red" : dueHours <= high ? "orange" : dueHours <= normal ? "yellow" : "green"; 
        }
    }
}