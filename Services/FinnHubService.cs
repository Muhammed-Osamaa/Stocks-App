using Microsoft.Extensions.Configuration;
using ServiceContracts;
using System.Net.Http;
using System.Text.Json;

namespace StocksApp.Services
{
    public class FinnHubService : IFinnHubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public FinnHubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string,object>?> GetStockPriceQuote(string symbole)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={symbole}&token={_configuration["StockAPI"]}"),
                    Method = HttpMethod.Get,
                };
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = httpResponseMessage.Content.ReadAsStream();
                StreamReader streamReader = new StreamReader(stream);
                string response = await streamReader.ReadToEndAsync();
                var responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if(responseDictionary == null)
                {
                    throw new InvalidOperationException("No response from finnhub server");
                }
                if (responseDictionary.ContainsKey("error"))
                {
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
                }
                return responseDictionary;
            }
        }
    }
}
