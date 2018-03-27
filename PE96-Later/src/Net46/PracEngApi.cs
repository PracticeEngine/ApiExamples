using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class PracEngApi
    {
        private readonly string appID;
        private readonly string appKey;
        private readonly string apiAuth;

        private HttpClient client;

        public PracEngApi(string appID, string appKey, string apiAuth)
        {
            this.appID = appID;
            this.appKey = appKey;
            this.apiAuth = apiAuth;
        }

        /// <summary>
        /// Builds the Client to access the Apis after getting an AuthorizationToken
        /// </summary>
        /// <returns>A Client with the Authentication Token Set</returns>
        private async Task<HttpClient> GetClient()
        {
            var color = Console.ForegroundColor;

            if (client != null)
            {
                return client;
            }
            client = new HttpClient();

            // Do Authorization (stopwatch just for fun)
            var sw = Stopwatch.StartNew();
            var disco = await DiscoveryClient.GetAsync(apiAuth);
            var tokenClient = new TokenClient(disco.TokenEndpoint, appID, appKey, style: AuthenticationStyle.PostValues);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("pe.api");
            sw.Stop();

            if (!tokenResponse.IsError)
            {

                // We are Authorized, set the Token on the client (so it goes in the Auth Header) 
                // also note the expiration, so we can refresh at an appropriate time 
                client.SetBearerToken(tokenResponse.AccessToken);

                // Show some Details
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Got JWT Token: ");
                Console.WriteLine(tokenResponse.AccessToken);
                Console.WriteLine($"Token expires after {tokenResponse.ExpiresIn}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(tokenResponse.Error);
                Console.WriteLine(tokenResponse.ErrorDescription);
            }
            Console.WriteLine("Auth took: " + sw.ElapsedMilliseconds + " ms");
            Console.ForegroundColor = color;

            return client;
        }

        /// <summary>
        /// Returns Collection of PEJob_Header
        /// </summary>
        /// <param name="contIndex">The ContIndex of the Client</param>
        /// <returns>Returns a List of Active Jobs for a Client</returns>
        public async Task<JToken> ApiGet(string url)
        {
            var sw = Stopwatch.StartNew();
            var peClient = await GetClient();
            sw.Restart();
            var json = await peClient.GetStringAsync(url);
            sw.Stop();
            Console.WriteLine("Call took: " + sw.ElapsedMilliseconds + " ms");
            if (String.IsNullOrEmpty(json))
            {
                return JValue.CreateNull();
            }
            return JToken.Parse(json);
        }

        /// <summary>
        /// Performs a POST to an API Method
        /// </summary>
        /// <param name="url">The URL to Post to</param>
        /// <param name="content">The HTTP Content to send in the POST</param>
        /// <returns></returns>
        public async Task<JToken> ApiPost(string url, HttpContent content)
        {
            var sw = Stopwatch.StartNew();
            var peClient = await GetClient();
            sw.Restart();
            var response = await peClient.PostAsync(url, content);
            sw.Stop();
            Console.WriteLine("Call took: " + sw.ElapsedMilliseconds + " ms");
            if (response.Content.Headers.ContentLength > 0)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JToken.Parse(json);
            }
            return JValue.CreateNull();
        }
    }
}
