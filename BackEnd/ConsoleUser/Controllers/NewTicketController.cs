using ConsoleUser.Models;
using ConsoleUser.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace ConsoleUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewTicketController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddTicket(NewTicket newTicket)
        {
            string BasicAuth = "Basic cmFuanVyYXZlZGV2QGdtYWlsLmNvbTpXZWJkZXZfMjAyMg==";
            string Cookie = "__cf_bm=NV4J3cTJmKvuqQCpPM5uYQygQbMa1KqcKC74uEfpYRE-1669145356-0-AQ8bmpK/6KOJqFK753Q+XKYR/nOaz6GgpzbhtlKOOBFY/Do8Z5sRIUYKKWLDpkXAsMbZGbWPNXA/jgWFl1wblgDcguCWBfSeLrxzRVI4UGcD; __cfruid=353ffba05ab8b9f0ee0a58e3b51e838c95d2159f-1669143409; __cfruid=5b67cf441a7537636be54b819fa8ae1bf4e0c42a-1669145370; _zendesk_cookie=BAhJIhl7ImRldmljZV90b2tlbnMiOnt9fQY6BkVU--459ed01949a36415c1716b5711271c3d08918307";
            var options = new RestClientOptions("https://native2881.zendesk.com")
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };
            var client = new RestClient(options);

            var request = new RestRequest("/api/v2/tickets", Method.Post);
            request.AddHeader("Authorization", BasicAuth);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", Cookie);
            var body = newTicket;
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            var zendeskData = JsonConvert.DeserializeObject<NewTicket>(response.Content);
            return Ok(zendeskData);
        }
    }
}
