using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Text;

namespace Overtail
{
    class API
    {
#nullable enable
        public static string? Token { get; set; }
#nullable disable
        private static readonly string _base = "https://overtail.schindlerfelix.de/";
        private static readonly HttpClient _client = new HttpClient();

        /// <summary>
        /// Checks if the user token is valid, throws exception if not
        /// </summary>
        /// <exception cref="NotLoggedInException">Thrown when user is not logged in</exception>
        /// <exception cref="UnvalidTokenException">Thrown when token is older than 30 days</exception>
        private static void CheckToken()
        {
            if (Token == null)
                throw new NotLoggedInException();
            else
            {
                string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(Token));
                DateTime validUntil = DateTime.ParseExact(decoded.Split('.')[2], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime today = DateTime.Today;
                TimeSpan a = today.Subtract(validUntil);
                if (!(a.Days > 0 && a.Days <= 30))
                {
                    // TODO: Popup with password input for revalidating
                    throw new UnvalidTokenException("Token unvalid since " + (a.Days - 30) + " days");
                }
            }
        }

        #region Requests
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
                CheckToken();
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
            if (auth)
            {
                CheckToken();
                content.Headers.Add("Authorization", "Bearer " + Token);
            }
            HttpResponseMessage res = await _client.PostAsync(_base + endpoint, content);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStringAsync();
        }
        #endregion

        #region Exceptions
        [Serializable]
        public class NotLoggedInException : Exception
        {
            public NotLoggedInException()
            { }

            public NotLoggedInException(string message)
                : base(message)
            { }

            public NotLoggedInException(string message, Exception innerException)
                : base(message, innerException)
            { }
        }

        [Serializable]
        public class UnvalidTokenException : Exception
        {
            public UnvalidTokenException()
            { }

            public UnvalidTokenException(string message)
                : base(message)
            { }

            public UnvalidTokenException(string message, Exception innerException)
                : base(message, innerException)
            { }
        }
        #endregion
    }
}
