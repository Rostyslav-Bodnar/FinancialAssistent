using Newtonsoft.Json;

namespace FinancialAssistent.Helpers
{
    public class MonobankHttpClient
    {
        private readonly HttpClient _httpClient;
        public MonobankHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> SendRequest<T>(string url, string token)
        {
            _httpClient.DefaultRequestHeaders.Remove("X-Token");
            _httpClient.DefaultRequestHeaders.Add("X-Token", token);

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }
    }

}
