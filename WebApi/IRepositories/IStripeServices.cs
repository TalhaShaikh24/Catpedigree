using ClassLibrary;
using Stripe;

namespace WebApi.IRepositories
{
    public interface IStripeServices
    {
        string CreatePaymentMethod(string cardNumber, int? expmonth, int? expyear, string cvc);
        void AttachPaymentMethodToCustomer(string paymentMethodId, string customerId);
        string SubscribeToStripePlanWithPaymentMethod(string customerId, string priceId, string paymentMethodId, string couponId = null);
        void CancelSubscription(string email);
        void CancelSub(string subscriptionId);
        string CreateOrRetrieveStripeCustomer(string email);
        string GetExistingSubscriptionId(string customerId);
        Task<string> CreateSubscriptionAsync(string email, string cardNumber, int? expmonth, int? expyear, string cvc, string priceID, string couponCode = null);
        string ValidateCoupon(string email, string couponCode);
        string CreateDiscountCoupon( decimal discountPercentage,string CouponName);

        int AddCouponsCodes(CouponCodes obj);

    }
}
