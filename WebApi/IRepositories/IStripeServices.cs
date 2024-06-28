using Stripe;

namespace WebApi.IRepositories
{
    public interface IStripeServices
    {
        Task<string> CreateSubscriptionAsync(string email, string? cardNumber, int? expmonth, int? expyear, string? cvc, string priceID);



         string CreatePaymentMethod(string cardNumber, int expmonth, int expyear, string cvc);


         void AttachPaymentMethodToCustomer(string paymentMethodId, string customerId);


         string SubscribeToStripePlanWithPaymentMethod(string customerId, string Catbasic, string paymentMethodId);

         void CancelSubscription(string email);


         void CancelSub(string subscriptionId);

         string CreateOrRetrieveStripeCustomer(string email);

        string GetExistingSubscriptionId(string customerId);
        
    }
}
