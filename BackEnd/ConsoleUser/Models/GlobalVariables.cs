using ConsoleUser.Models.Domain;
using ConsoleUser.Repositories;
using Newtonsoft.Json;
using RestSharp;
using Microsoft.AspNetCore.Mvc;

public static class GlobalVariables
{   
    static readonly IUserRepository userRepository;
    static readonly ICustomerRepository customerRepository;
    static readonly ISupportLevelRepository customerSupportLevelRepository;
    static string BasicAuth = "Basic cmFuanVyYXZlZGV2QGdtYWlsLmNvbTpXZWJkZXZfMjAyMg==";
    static string Cookie = "__cf_bm=NV4J3cTJmKvuqQCpPM5uYQygQbMa1KqcKC74uEfpYRE-1669145356-0-AQ8bmpK/6KOJqFK753Q+XKYR/nOaz6GgpzbhtlKOOBFY/Do8Z5sRIUYKKWLDpkXAsMbZGbWPNXA/jgWFl1wblgDcguCWBfSeLrxzRVI4UGcD; __cfruid=353ffba05ab8b9f0ee0a58e3b51e838c95d2159f-1669143409; __cfruid=5b67cf441a7537636be54b819fa8ae1bf4e0c42a-1669145370; _zendesk_cookie=BAhJIhl7ImRldmljZV90b2tlbnMiOnt9fQY6BkVU--459ed01949a36415c1716b5711271c3d08918307";
    //static RestClientOptions options = new RestClientOptions("https://native2881.zendesk.com")
    //{
    //    ThrowOnAnyError = true,
    //    MaxTimeout = -1  // 1 second
    //};
    //static RestClient client = new RestClient(options);
    static RestRequest usersReq = new RestRequest("/api/v2/users", Method.Get);
    static RestRequest metricsReq = new RestRequest("/api/v2/ticket_metrics.json", Method.Get);

    static RestRequest restRequest2 = usersReq.AddHeader("Authorization", BasicAuth);
    static RestRequest metricsRequest = metricsReq.AddHeader("Authorization", BasicAuth);

    //usersRequest.AddHeader("Cookie", Cookie);
    //metricsRequest.AddHeader("Cookie", Cookie);

    //static RestResponse usersResponse =  await client.ExecuteAsync(usersRequest);
    //static RestResponse metricsResponse = await client.ExecuteAsync(metricsRequest);

    //var zendeskUsers = JsonConvert.DeserializeObject<ZendeskUsers>(usersResponse.Content);
    //var zendeskMetrics = JsonConvert.DeserializeObject<ZendeskMetrics>(metricsResponse.Content);

    public static IEnumerable<ConsoleUser.Models.User> users = userRepository.GetAll();
    public static IEnumerable<ConsoleUser.Models.Customer> customers = customerRepository.GetAll();
    public static IEnumerable<ConsoleUser.Models.CustomerSupportLevel> supportLevel = customerSupportLevelRepository.GetAll();
}
