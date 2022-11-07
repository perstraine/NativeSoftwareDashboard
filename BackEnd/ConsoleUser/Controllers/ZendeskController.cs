using ConsoleUser.Controllers;
using ConsoleUser.Models.Domain;
using ConsoleUser.Models.DTO;
using ConsoleUser.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Immutable;
using System.Diagnostics;

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
            var zendeskMetrics = JsonConvert.DeserializeObject<MetricsData>(metricsResponse.Content);

            var customers = customerRepository.GetAll();
            var supportLevel = customerSupportLevelRepository.GetAll();

            // Creating Json for dashboard
            var dashboardTickets = CreateTicketInfo(zendeskData, zendeskMetrics);

            //var customerData = new Customer();


            //return DTO customers
            //var customersDTO = new List<Customer>();
            //customers.ToList().ForEach(customer =>
            //{
            //    var customerDTO = new Customer()
            //    {
            //        CustomerCode = customer.CustomerCode,
            //        CustomerCodeZendesk = customer.CustomerCodeZendesk,
            //        SupportLevel = customer.SupportLevel,
            //    };

            //    customersDTO.Add(customerDTO);
            //});

            return Ok(dashboardTickets);
        }

        // Dashboard specific JSON creation
        private static List<DashboardTicketData> CreateTicketInfo(ZendeskData? zendeskData, MetricsData? metricsData)
        {
            var ticketList = new List<DashboardTicketData>();
            foreach (var ticket in zendeskData.tickets)
            {
                var dashboardTicket = new DashboardTicketData();
                dashboardTicket.id = ticket.id;
                object organisationId = null;
                foreach (var metric in metricsData.ticket_metrics)
                {
                    if( ticket.id == metric.ticket_id)
                    {
                        organisationId = metric.id;
                    }
                }
                dashboardTicket.Organisation = organisationId.ToString();
                if (ticket.subject != null) dashboardTicket.Subject = ticket.subject;
                if (ticket.status != null) dashboardTicket.Status = ticket.status;
                if (ticket.assignee_id != null) dashboardTicket.Recipient = ticket.assignee_id.ToString();//TODO assignee or recepient
                dashboardTicket.Billable = true; //TODO tobe decided
                //replacing numbers instead strings for comparison
                if (ticket.priority != null) dashboardTicket.Priority = (ticket.priority=="urgent" ? 4 : ticket.priority == "high" ? 3 : ticket.priority == "normal" ? 2 : 1).ToString();              
                dashboardTicket.RequestedDate = ticket.created_at.ToString();
                dashboardTicket.TimeDue =  ticket.created_at.AddDays(3).ToString(); //TODO tobe calculated
                if (ticket.type != null) dashboardTicket.Type = ticket.type;
                if (ticket.url != null) dashboardTicket.url = ticket.url;
                ticketList.Add(dashboardTicket);
            }

            ticketList = QuickSortTickets(ticketList, 0, ticketList.Count - 1);

            //replacing back the strings for priority after sorting.
            foreach (var ticket in ticketList)
            {
                ticket.Priority = ticket.Priority == "4" ? "urgent" : ticket.Priority == "3" ? "high" : ticket.Priority == "2" ? "normal" : "low";
            }

            return ticketList;
        }

        public static List<DashboardTicketData> QuickSortTickets(List<DashboardTicketData> ticketListToSort, int leftIndex, int rightIndex)
        {
            var i = leftIndex;
            var j = rightIndex;
            var pivot = ticketListToSort[leftIndex];

            while (i <= j)
            {
                while (Int32.Parse(ticketListToSort[i].Priority) > Int32.Parse(pivot.Priority))
                {
                    i++;
                }

                while (Int32.Parse(ticketListToSort[j].Priority) < Int32.Parse(pivot.Priority))
                {
                    j--;
                }

                if (i <= j)
                {
                    var temp = ticketListToSort[i];
                    ticketListToSort[i] = ticketListToSort[j];
                    ticketListToSort[j] = temp;
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
    }
}
