using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ReadTweets
{
    public static class GetTweets
    {
        [FunctionName("GetTweets")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            using (var httpClient = new HttpClient())
            {
                var apiKey = Environment.GetEnvironmentVariable("API_KEY");
                var apiSecret = Environment.GetEnvironmentVariable("API_SECRET");
                var requestMessageGetToken = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://api.twitter.com/oauth2/token"),
                    Headers =
                        {
                            { "Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}")) },
                        },
                    Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded")
                };

                var tokenReqestResponse = await httpClient.SendAsync(requestMessageGetToken).ConfigureAwait(false);
                var responseContent = tokenReqestResponse.Content.ReadAsStringAsync().Result;
                var responseObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                var accesToken = responseObject["access_token"];

                // Get data from Twitter List : https://twitter.com/i/lists/1572510829198856192
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://api.twitter.com/1.1/lists/statuses.json?list_id=1572510829198856192")
                };

                requestMessage.Headers.Add("Authorization", $"Bearer {accesToken}");
                var response = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
                // Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                var finalResponse = response.Content.ReadAsStringAsync().Result;
                var dynamicObject = JsonConvert.DeserializeObject<dynamic>(finalResponse)!;
                return new OkObjectResult(dynamicObject);
            }
        }
    }
}
