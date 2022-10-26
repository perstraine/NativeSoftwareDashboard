﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;

namespace ZendeskTest2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZendeskController : ControllerBase
    {
        [HttpGet]
        public string ZendeskTickets()
        {
            var options = new RestClientOptions("https://missionready5835.zendesk.com")
            {
                ThrowOnAnyError = true,
                MaxTimeout = -1  // 1 second
            };
            var client = new RestClient(options);

            var request = new RestRequest("/api/v2/tickets", Method.Get);
            request.AddHeader("Authorization", "Basic cmFuanVyYXZlQGdtYWlsLmNvbTpXZWJkZXZfMjAyMg==");
            //request.AddHeader("Cookie", "__cfruid=453dcbc27454258463f1141b2d97bb40b6e79538-1666769874; _zendesk_cookie=BAhJIhl7ImRldmljZV90b2tlbnMiOnt9fQY6BkVU--459ed01949a36415c1716b5711271c3d08918307; _zendesk_session=TU9uMjhreXZMQWR5M0YyMThRYVBpWlBEV2VHWW5WWTJmZEdXZE13T2FGeG9zN2ZuOWEwaU1rSStlR29KSUpFbitqbXZZY2dyOUx1SHF4ZTZWU1AxdGpDSy9MeWlNRThtalRWQ0tmUlI5OERDRk0zK0NjdW9TOXo4Z3RpTmhYbUMtLUMzc0RUOGZNVkVkOElLVHg3bTJFMkE9PQ%3D%3D--90ca0ca180d8b1630519eeffa7e18f3dd2ece3e7");
            RestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            string rawResponse = response.Content;
            var jsonResponse = JsonObject.Parse(rawResponse);
            return jsonResponse.ToString();
        }
    }
}