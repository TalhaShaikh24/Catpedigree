using Newtonsoft.Json;
using WebApi.IRepositories;

namespace WebApi.Repositories
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl;

        public CurrencyConverterService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ExchangeRateApi:ApiKey"];
            _apiUrl = configuration["ExchangeRateApi:ApiUrl"];
        }

        public async Task<decimal> ConvertCurrency(decimal amount, string fromCurrency, string toCurrency)
        {
            var rate = await GetExchangeRate(fromCurrency, toCurrency);
            return amount * rate;
        }

        public async Task<List<decimal>> ConvertCurrencyList(List<decimal> amounts, string fromCurrency, string toCurrency)
        {
            var rate = await GetExchangeRate(fromCurrency, toCurrency);
            return amounts.Select(amount => amount * rate).ToList();
        }

        public async Task<decimal> GetExchangeRate(string fromCurrency, string toCurrency)
        {
            var url = $"{_apiUrl}/{_apiKey}/latest/{fromCurrency}";
            var response = await _httpClient.GetStringAsync(url);
            var rates = JsonConvert.DeserializeObject<CurrencyRatesResponse>(response);

            if (rates.ConversionRates.TryGetValue(toCurrency, out var rate))
            {
                return rate;
            }

            throw new Exception("Currency conversion failed.");
        }


    }

    public class CurrencyRatesResponse
    {
        [JsonProperty("conversion_rates")]
        public Dictionary<string, decimal> ConversionRates { get; set; }
    }

}
