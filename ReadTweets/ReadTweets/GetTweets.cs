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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            using (var httpClient = new HttpClient())
            {
                string apiKey = Environment.GetEnvironmentVariable("API_KEY");
                string apiSecret = Environment.GetEnvironmentVariable("API_SECRET");
                string twitterListId = Environment.GetEnvironmentVariable("TWITTER_LIST_ID");
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
                string responseContent = tokenReqestResponse.Content.ReadAsStringAsync().Result;
                var responseObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                string accesToken = responseObject["access_token"];
                
                // Get data from Twitter List
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://api.twitter.com/1.1/lists/statuses.json?list_id={twitterListId}"),
                };

                requestMessage.Headers.Add("Authorization", $"Bearer {accesToken}");
                var response = await httpClient.SendAsync(requestMessage).ConfigureAwait(false);
                var finalResponse = response.Content.ReadAsStringAsync().Result;
                var dynamicObject = JsonConvert.DeserializeObject<dynamic>(finalResponse)!;
                return new OkObjectResult(dynamicObject);
            }
        }
    }
}
