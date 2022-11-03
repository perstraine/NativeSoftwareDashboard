using ConsoleUser.Models.Domain;
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

        // Dashboard specific JSON creation
        private static List<DashboardTicketData> CreateTicketInfo(ZendeskData? zendeskData)
        {
            var ticketList = new List<DashboardTicketData>();
            foreach (var ticket in zendeskData.tickets)
            {
                var dashboardTicket = new DashboardTicketData();
                dashboardTicket.id = ticket.id; 
                if (ticket.organization_id != null) dashboardTicket.Organisation = ticket.organization_id.ToString();
                if (ticket.subject != null) dashboardTicket.Subject = ticket.subject;
                if (ticket.assignee_id != null) dashboardTicket.Recipient = ticket.assignee_id.ToString();//TODO assignee or recepient
                dashboardTicket.Billable = true; //TODO tobe decided
                //replacing numbers instead strings for comparison
                if (ticket.priority != null) dashboardTicket.Priority = (ticket.priority=="urgent" ? 4 : ticket.priority == "high" ? 3 : ticket.priority == "normal" ? 2 : 1).ToString();
                if (ticket.created_at != null) dashboardTicket.RequestedTime = ticket.created_at;
                if (ticket.due_at != null) dashboardTicket.TimeDue = ticket.updated_at; //TODO tobe calculated
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
