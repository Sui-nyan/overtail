namespace Overtail.API
{
    [Serializable]
    abstract class IAPIData
    {

    }

    class ItemData : IAPIData
    {
        public readonly int slot;
        public readonly string id;
        public readonly int amount;
    }

    class LoginData : IAPIData
    {
        public readonly string uuid;
        public readonly string token;
    }

    class RegisterData : IAPIData
    {
        public readonly bool success;
    }

    class API
    {
        public static string? Token { get; set; }
        private static readonly string _base = "https://overtail.schindlerfelix.de/";
        private static readonly HttpClient _client = new HttpClient();

        public static async Task<string> GET(string endpoint, bool auth = true)
        {
            if (auth && Token != null)
            {
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, _base + endpoint))
                    {
                        requestMessage.Headers.Add("Authorization", "Bearer " + Token);

                        HttpResponseMessage res = await _client.SendAsync(requestMessage);
                        // res.EnsureSuccessStatusCode();
                        return await res.Content.ReadAsStringAsync();
                    }
            }

            else
            {
                return await _client.GetStringAsync(_base + endpoint);
                // HttpResponseMessage res = await _client.GetAsync(_base + endpoint);
                // res.EnsureSuccessStatusCode();
                // return await res.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> POST(string endpoint, Dictionary<string,string> data, bool auth = true)
        {
            FormUrlEncodedContent content = new(data);
            if (auth && Token != null)
                content.Headers.Add("Authorization", "Bearer " + Token);
            HttpResponseMessage res = await _client.PostAsync(_base + endpoint, content);
            // res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStringAsync();
        }

        public static async Task<string> PUT(string endpoint, Dictionary<string, string> data, bool auth = true)
        {
            FormUrlEncodedContent content = new(data);
            if (auth && Token != null)
                content.Headers.Add("Authorization", "Bearer " + Token);
            HttpResponseMessage res = await _client.PatchAsync(_base + endpoint, content);
            // res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStringAsync();
        }

        public static async Task<string> DELETE(string endpoint, bool auth = true)
        {
            if (auth && Token != null)
            {
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Delete, _base + endpoint))
                {
                    requestMessage.Headers.Add("Authorization", "Bearer " + Token);

                    HttpResponseMessage res = await _client.SendAsync(requestMessage);
                    // res.EnsureSuccessStatusCode();
                    return await res.Content.ReadAsStringAsync();
                }
            }

            else
            {
                HttpResponseMessage res = await _client.DeleteAsync(_base + endpoint);
                // res.EnsureSuccessStatusCode();
                return await res.Content.ReadAsStringAsync();
            }
        }
    }
}
