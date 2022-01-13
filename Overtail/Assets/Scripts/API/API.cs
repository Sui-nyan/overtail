using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System;

namespace Overtail.API
{
    class API
    {
#nullable enable
        public static string? Token { get; set; }
#nullable disable
        private static readonly string _base = "https://overtail.schindlerfelix.de/";
        private static readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// GET data from API endpoint
        /// </summary>
        /// <param name="endpoint">API endpoint (without starting '/')</param>
        /// <param name="auth">Wheather to send API.Token or not</param>
        /// <returns>API answer as string</returns>
        public static async Task<string> GET(string endpoint, bool auth = true)
        {
            if (auth)
            {
                if (Token == null || Token == "")
                    throw new ArgumentException("User is not logged in");
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }

            HttpResponseMessage res = await _client.GetAsync(_base + endpoint);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// POST data to API endpoint
        /// </summary>
        /// <param name="endpoint">API endpoint (without starting '/')</param>
        /// <param name="data">Data as [Key, Value] pairs</param>
        /// <param name="auth">Wheather to send API.Token or not</param>
        /// <returns>API answer as string</returns>
        public static async Task<string> POST(string endpoint, Dictionary<string, string> data, bool auth = true)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(data);
            if (auth && Token != null)
                content.Headers.Add("Authorization", "Bearer " + Token);
            HttpResponseMessage res = await _client.PostAsync(_base + endpoint, content);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// PUT data to API endpoint
        /// </summary>
        /// <param name="endpoint">API endpoint (without starting '/')</param>
        /// <param name="data">Data as [Key, Value] pairs</param>
        /// <param name="auth">Wheather to send API.Token or not</param>
        /// <returns>API answer as string</returns>
        public static async Task<string> PUT(string endpoint, Dictionary<string, string> data, bool auth = true)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(data);
            if (auth)
                if (Token == null || Token == "")
                    throw new ArgumentException("User is not logged in");
                else
                    content.Headers.Add("Authorization", "Bearer " + Token);
            HttpResponseMessage res = await _client.PutAsync(_base + endpoint, content);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// PUT json data to API endpoint
        /// </summary>
        /// <param name="endpoint">API endpoint (without starting '/')</param>
        /// <param name="data">JSON data as string</param>
        /// <param name="auth">Wheather to send API.Token or not</param>
        /// <returns>API answer as string</returns>
        public static async Task<string> PUT(string endpoint, string data, bool auth = true)
        {
            StringContent content = new StringContent(data);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            if (auth)
                if (Token == null || Token == "")
                    throw new ArgumentException("User is not logged in");
                else
                    content.Headers.Add("Authorization", "Bearer " + Token);
            HttpResponseMessage res = await _client.PutAsync(_base + endpoint, content);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Send DELETE request to API endpoint
        /// </summary>
        /// <param name="endpoint">API endpoint (without starting '/')</param>
        /// <param name="auth">Wheather to send API.Token or not</param>
        /// <returns>API answer as string</returns>
        public static async Task<string> DELETE(string endpoint, bool auth = true)
        {
            HttpResponseMessage res;
            if (auth)
            {
                if (Token == null || Token == "")
                    throw new ArgumentException("User is not logged in");
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Delete, _base + endpoint))
                {
                    requestMessage.Headers.Add("Authorization", "Bearer " + Token);

                    res = await _client.SendAsync(requestMessage);
                    res.EnsureSuccessStatusCode();
                    return await res.Content.ReadAsStringAsync();
                }
            }

            res = await _client.DeleteAsync(_base + endpoint);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStringAsync();
        }
    }
}
