using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Net.Http.Headers;
using System.Text;

namespace ZendeskTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZendeskController : ControllerBase
    {
        [HttpGet]
        public string Test1()
        {
            var options = new RestClientOptions("https://missionreadyhq.zendesk.com")
            {
                ThrowOnAnyError = true,
                MaxTimeout = 10000  // 1 second
            };
            var client = new RestClient(options);

            var request = new RestRequest("/api/v2/tickets", Method.Get);
            request.AddHeader("Authorization", "Basic cmFuanVyQG1pc3Npb25yZWFkeWhxLmNvbTpXZWJkZXZfMjAyMg==");
            request.AddHeader("Cookie", "__cfruid=8b9babaa1ec4fd576b69e257cc511d3a6166b118-1666666042; _zendesk_cookie=BAhJIhl7ImRldmljZV90b2tlbnMiOnt9fQY6BkVU--459ed01949a36415c1716b5711271c3d08918307; _zendesk_session=MEY3YzhoZmVnR09rMjY4T0JJeFl5M2dWYVgvaU1xdnlUQUJhVDhERTRZRit6bDU3bWhtODZBaUVmWlB6OUt5cjJXaGljWUtkM0xxWVI3ZG1vemlSeGFZSXBKSUl2c1cyK2VNWkFDdElGWkJYd1dodkl6bXkweFNOWVZ0TytzT04tLVBXOGY0MkFxVDN2a1Rsajc4VU9zdkE9PQ%3D%3D--4d0e90c57130fd5091d548bb6fbd02492d644285");
            RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            string rawResponse = response.Content;
            return rawResponse;
        }
    }
}
