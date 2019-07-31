using System.Net.Http;
using System.Threading.Tasks;

namespace PageNotifier.Worker
{
    public class WebsiteReader : IWebsiteReader
    {
        private readonly HttpClient _httpClient;

        public WebsiteReader(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> ReadPageAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var textContent = await response.Content.ReadAsStringAsync();
            return textContent;
        }
    }
}