using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SpotifyEnhancer.Config;
using SpotifyEnhancer.DataAccess.Interfaces;
using SpotifyEnhancer.DataAccess.Models;

namespace SpotifyEnhancer.DataAccess
{
    public class AppleMusicHttpClient : IAppleMusicHttpClient
    {
        private readonly AppleMusicConfig _config;
        private readonly HttpClient _httpClient;
        private readonly ILogger<AppleMusicHttpClient> _logger;

        public AppleMusicHttpClient(HttpClient httpClient, IOptions<AppleMusicConfig> config,
            ILogger<AppleMusicHttpClient> logger)
        {
            _httpClient = httpClient;
            _config = config.Value;
            _logger = logger;
        }
        
        
        public async Task<Library> GetUserLibrary(string userToken, string offsetEndpoint = "")
        {
            var endpoint = "/me/library/songs";

            if (!String.IsNullOrEmpty(offsetEndpoint))
            {
                endpoint = offsetEndpoint;
            }
            
            var request = SetupHttpRequest(endpoint, HttpMethod.Get, userToken);
            var response = await _httpClient.SendAsync(request);

            if(!response.IsSuccessStatusCode)
            {
                throw new Exception($"Couldn't access user library. Received {response.StatusCode} and content: {response.Content}");
            }
            
            var library =
                JsonConvert.DeserializeObject<Library>(response.Content.ReadAsStringAsync().Result);

            return library;
        }
        
        /// <summary>
        /// Sets up the auth header for the apple music request
        /// </summary>
        /// <returns></returns>
        private string GetAuthHeader()
        {
            var token = _config.DeveloperKey;
            return $"Bearer {token}";
        }

        /// <summary>
        /// Setup an http request using a pat token as the authorization method
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="endpoint"></param>
        /// <param name="methodType"></param>
        /// <param name="userToken"></param>
        /// <returns></returns>
        private HttpRequestMessage SetupHttpRequest(string endpoint,  HttpMethod methodType, string userToken= "")
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://api.music.apple.com/v1/{endpoint}"),
                Method = methodType
            };
            request.Headers.TryAddWithoutValidation("Authorization", GetAuthHeader());

            if (!String.IsNullOrEmpty(userToken))
            {
                request.Headers.TryAddWithoutValidation("Music-User-Token", userToken);
            }
            
            return request;
        }
        
    }
}