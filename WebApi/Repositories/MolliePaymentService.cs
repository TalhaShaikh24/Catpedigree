using Mollie.Api.Client;
using Mollie.Api.Models.Customer.Request;
using Mollie.Api.Models;
using Mollie.Api.Models.Customer.Response;

using Mollie.Api.Models.Subscription.Request;
using Mollie.Api.Models.Subscription.Response;
using WebApi.IRepositories;
using WebApi.DBManager;
using ClassLibrary;
using Dapper;
using System.Data;
using System.Text;
using System.Text.Json;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using System.Reflection;

namespace WebApi.Repositories
{
    public class MolliePaymentService : IMolliePaymentService
    {
        private readonly CustomerClient _customerClient;
        private readonly MandateClient _mandateClient;
        private readonly SubscriptionClient _subscriptionClient;
        private readonly IDapper _dapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        string apiKey;
        public MolliePaymentService(IConfiguration configuration, IDapper dapper, IWebHostEnvironment hostingEnvironment)
        {
             apiKey = configuration["Mollie:ApiKey"];
            _customerClient = new CustomerClient(apiKey);
            _mandateClient = new MandateClient(apiKey);
            _subscriptionClient = new SubscriptionClient(apiKey);
            _dapper = dapper;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<CustomerResponse> CreateCustomerAsync(MollieTransaction obj)
        {
            var customerRequest = new CustomerRequest
            {
                Name = obj.name,
                Email = obj.email
            };


            


            var response= await _customerClient.CreateCustomerAsync(customerRequest);


            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CustomerID",response.Id ?? (object)DBNull.Value, DbType.String, ParameterDirection.Input);
            parameters.Add("@email", obj.email ?? (object)DBNull.Value, DbType.String, ParameterDirection.Input);
            var data = _dapper.Insert<int>(@"[dbo].[sp_InsertMollieTransactionCustomerID]", parameters);


            return response;

        }

        public async Task<MandateResponse> CreateMandateAsync(MandateRequest mandateRequest, string apiKey)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var jsonContent = JsonSerializer.Serialize(mandateRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("https://api.mollie.com/v2/customers/{customerId}/mandates", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var mandateResponse = JsonSerializer.Deserialize<MandateResponse>(responseContent);
                    return mandateResponse;
                }
                else
                {
                    // Handle error response
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error: {response.StatusCode}, Details: {errorContent}");
                }
            }
        }


        public async Task<SubscriptionResponse> CreateSubscriptionAsync(string email, decimal amount, string interval, string description)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@email", email, DbType.String, ParameterDirection.Input);
            var data = _dapper.Get<MollieTransaction>(@"[dbo].[sp_GetMollieTransactionCustomerID]", parameters);



            var subscriptionRequest = new SubscriptionRequest
            {
                Amount = new Amount(Currency.EUR, amount.ToString("0.00")),
                Interval = interval,
                Description = description
            };

            var response= await _subscriptionClient.CreateSubscriptionAsync(data.CustomerID, subscriptionRequest);

            DynamicParameters parameters2 = new DynamicParameters();


            parameters2.Add("@subscriptionId", response.Id ?? (object)DBNull.Value, DbType.String, ParameterDirection.Input);
            parameters2.Add("@email", email ?? (object)DBNull.Value, DbType.String, ParameterDirection.Input);
           _dapper.Update<string>(@"[dbo].[sp_AddMollieTransactionsubscriptionId]", parameters);

            return response;


        }

        public async Task<SubscriptionResponse> GetSubscriptionAsync(string email)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@email", email, DbType.String, ParameterDirection.Input);
            var data = _dapper.Get<MollieTransaction>(@"[dbo].[sp_GetMollieTransactionSubscriptionID]", parameters);


            return await _subscriptionClient.GetSubscriptionAsync(data.CustomerID, data.subscriptionId);
        }

        public async Task<SubscriptionResponse> SubscriptionAsync(MollieTransaction obj)
        {

             // create Customer
            var customerRequest = new CustomerRequest
            {
                Name = obj.name,
                Email = obj.email
            };

            var customerResponse = await _customerClient.CreateCustomerAsync(customerRequest);


            var paymentRequest = new PaymentRequest
            {
                Amount = new Mollie.Api.Models.Amount("EUR", "10.00"),
                Description = "Order #12345",
               RedirectUrl = "https://localhost:7297/Packages/Pricing",
                //WebhookUrl = "https://webshop.example.org/payments/webhook/",
                Method = "creditcard"
            };

            PaymentResponse payment = await _customerClient.CreateCustomerPayment(customerResponse.Id, paymentRequest);



            var mandateRequest = new MandateRequest
            {
                Method = "directdebit",
                ConsumerName = new MandateConsumerName { ConsumerName = "John Doe" },
                ConsumerAccount = new MandateConsumerAccount { ConsumerAccount = "NL55INGB0000000000" }
            };
            var mandaterespo = CreateMandateAsync(mandateRequest, apiKey);

            var subscriptionRequest = new SubscriptionRequest
            {
                Amount = new Amount(Currency.EUR, obj.Amount.ToString("0.00")),
                Interval = obj.interval,
                
                Description = obj.Description
            };

            return await _subscriptionClient.CreateSubscriptionAsync(customerResponse.Id, subscriptionRequest);

        }
    }
}
