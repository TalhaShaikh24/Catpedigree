using ClassLibrary;
using Dapper;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Stripe.Forwarding;
using System.Data;
using WebApi.DBManager;
using WebApi.IRepositories;

namespace WebApi.Repositories
{
    public class StripeServices : IStripeServices
    {

        private readonly StripeSettings _stripeSettings;
        private readonly IDapper _dapper;
        private const string _Catbasic = "price_1PVunPKR3yBF1l8f4VUznFAW";
      
        public StripeServices(IOptions<StripeSettings> stripeSettings, IDapper dapper)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            _dapper = dapper;   
        }



        public string CreatePaymentMethod(string cardNumber, int? expmonth, int? expyear, string cvc)
        {
            var options = new PaymentMethodCreateOptions
            {
                Type = "card",
                Card = new PaymentMethodCardOptions
                {
                    Number = cardNumber,
                    ExpMonth = Convert.ToInt64(expmonth),
                    ExpYear = Convert.ToInt64(expyear),
                    Cvc = cvc,
                },
            };

            var service = new PaymentMethodService();
            var paymentMethod = service.Create(options);

            return paymentMethod.Id;
        }

        public void AttachPaymentMethodToCustomer(string paymentMethodId, string customerId)
        {
            var paymentMethodService = new PaymentMethodService();
            var attachOptions = new PaymentMethodAttachOptions
            {
                Customer = customerId,
            };

            paymentMethodService.Attach(paymentMethodId, attachOptions);
        }

        public string SubscribeToStripePlanWithPaymentMethod(string customerId, string priceId, string paymentMethodId, string couponId = null)
        {
            var options = new SubscriptionCreateOptions
            {
                Customer = customerId,
                Items = new List<SubscriptionItemOptions>
        {
            new SubscriptionItemOptions
            {
                Price = priceId,
            },
        },
                DefaultPaymentMethod = paymentMethodId,
            };

            if (!string.IsNullOrEmpty(couponId))
            {
                var discountOptions = new SubscriptionDiscountOptions
                {
                    Coupon = couponId
                };
                options.Discounts = new List<SubscriptionDiscountOptions> { discountOptions };

            }

            var service = new SubscriptionService();
            var subscription = service.Create(options);

            return subscription.Id;
        }

        public void CancelSubscription(string email)
        {
            string customerId = CreateOrRetrieveStripeCustomer(email);
            string existingSubscriptionId = GetExistingSubscriptionId(customerId);

            if (!string.IsNullOrEmpty(existingSubscriptionId))
            {
                CancelSub(existingSubscriptionId);
            }
        }

        public void CancelSub(string subscriptionId)
        {
            var service = new SubscriptionService();
            service.Cancel(subscriptionId);
        }

        public string CreateOrRetrieveStripeCustomer(string email)
        {
            var customerService = new CustomerService();
            var existingCustomer = customerService.List(new CustomerListOptions { Email = email }).FirstOrDefault();

            if (existingCustomer != null)
            {
                return existingCustomer.Id;
            }
            else
            {
                var options = new CustomerCreateOptions
                {
                    Email = email,
                };

                var newCustomer = customerService.Create(options);
                return newCustomer.Id;
            }
        }

        public string GetExistingSubscriptionId(string customerId)
        {
            var subscriptionService = new SubscriptionService();
            var options = new SubscriptionListOptions { Customer = customerId };
            var subscriptions = subscriptionService.List(options);

            if (subscriptions.Any())
            {
                return subscriptions.First().Id;
            }

            return null;
        }

        public async Task<string> CreateSubscriptionAsync(string email, string cardNumber, int? expmonth, int? expyear, string cvc, string priceID, string couponCode = null)
        {
            string customerId = string.Empty;

            var customerService = new CustomerService();
            var existingCustomer = customerService.List(new CustomerListOptions { Email = email }).FirstOrDefault();

            if (existingCustomer != null)
            {
                customerId = existingCustomer.Id;
            }
            else
            {
                var customerCreateOptions = new CustomerCreateOptions
                {
                    Email = email,
                };

                var CreatecustomerService = new CustomerService();
                var customer = await CreatecustomerService.CreateAsync(customerCreateOptions);
                customerId = customer.Id;
            }

            var paymentMethodId = CreatePaymentMethod(cardNumber, expmonth, expyear, cvc);
            AttachPaymentMethodToCustomer(paymentMethodId, customerId);

            string couponId = null;

            if (!string.IsNullOrEmpty(couponCode))
            {
                couponId = ValidateCoupon(email,couponCode);
            }

            string subscriptionId = SubscribeToStripePlanWithPaymentMethod(customerId, priceID, paymentMethodId, couponId);

            return subscriptionId;
        }

        public string ValidateCoupon(string email, string couponCode)
        {

            DynamicParameters parameters = new DynamicParameters();


            parameters.Add("@CouponCode", couponCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@email", email, DbType.String, ParameterDirection.Input);


            var data = _dapper.Insert<int>(@"dbo.[sp_CouponCodeExpireValidation]", parameters);

            if (data>0)
            {

                var couponService = new CouponService();
                var coupons = couponService.List(new CouponListOptions()).ToList();
                var coupon = coupons.FirstOrDefault(c => c.Id == couponCode);

                if (coupon != null && coupon.Valid)
                {
                    return coupon.Id;
                }

                else
                {
                    return null;
                }
            }

            else
            {
                return null;
            }
        }

        public string CreateDiscountCoupon(decimal discountPercentage)
        {
            var options = new CouponCreateOptions
            {
                PercentOff = discountPercentage,
                Duration = "repeating",
                DurationInMonths = 1 // Equivalent to approximately 30 days
            };

            var service = new CouponService();
            var coupon = service.Create(options);

            return coupon.Id;
        }
        public int AddCouponsCodes(CouponCodes obj)
        {

            DynamicParameters parameters = new DynamicParameters();


           string code= CreateDiscountCoupon(obj.DiscountPercentage);

            parameters.Add("@DiscountPercentage", obj.DiscountPercentage, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@CouponCode", code, DbType.String, ParameterDirection.Input);
            parameters.Add("@userId", obj.UserId, DbType.Int32, ParameterDirection.Input);

            parameters.Add("@CreatedBy", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);


            var data = _dapper.Insert<int>(@"dbo.[sp_Add_CouponsCodes]", parameters);

            return data;
        }

    
    }
}
