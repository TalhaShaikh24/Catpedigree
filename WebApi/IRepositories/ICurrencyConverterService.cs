namespace WebApi.IRepositories
{
    public interface ICurrencyConverterService
    {
        Task<decimal> ConvertCurrency(decimal amount, string fromCurrency, string toCurrency);

        Task<List<decimal>> ConvertCurrencyList(List<decimal> amounts, string fromCurrency, string toCurrency);

        public  Task<decimal> GetExchangeRate(string fromCurrency, string toCurrency);

    }
}
