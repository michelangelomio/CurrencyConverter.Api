using Netwealth.Currency.Interview.Test.Business.Clients.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Netwealth.Currency.Interview.Test.Business.Clients
{
    public class HttpClientBase : IHttpClientBase
    {
        private readonly HttpClient _httpClient;

        public HttpClientBase(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string url, string accessKey = default)
        {
            if (!string.IsNullOrEmpty(accessKey))
            {
                url = AppendCodeParameter(url, accessKey);
            }

            using var response = await _httpClient.GetAsync(url);

            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        private string AppendCodeParameter(string path, string accessKey)
        {
            var querySymbol = path.Contains('?') ? '&' : '?';
            return $"{path}{querySymbol}access_key={accessKey}";
        }
    }
}
