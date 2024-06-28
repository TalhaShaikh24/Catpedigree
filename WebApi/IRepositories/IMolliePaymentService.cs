using Mollie.Api.Client;
using Mollie.Api.Models.Customer.Request;
using Mollie.Api.Models.Customer.Response;

using Mollie.Api.Models.Subscription.Request;
using Mollie.Api.Models.Subscription.Response;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using ClassLibrary;
namespace WebApi.IRepositories
{
    public interface IMolliePaymentService
    {
          Task<CustomerResponse> CreateCustomerAsync(MollieTransaction obj);


        Task<MandateResponse> CreateMandateAsync(MandateRequest mandateRequest, string apiKey);

        Task<SubscriptionResponse> CreateSubscriptionAsync(string customerId, decimal amount, string interval, string description);


        Task<SubscriptionResponse> GetSubscriptionAsync(string email);


        public Task<SubscriptionResponse> SubscriptionAsync(MollieTransaction obl);






    }
}
