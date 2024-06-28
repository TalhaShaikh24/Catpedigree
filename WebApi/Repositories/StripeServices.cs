using ClassLibrary;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Stripe.Forwarding;
using WebApi.IRepositories;

namespace WebApi.Repositories
{
    public class StripeServices : IStripeServices
    {

        private readonly StripeSettings _stripeSettings;

        private const string _Catbasic = "price_1PVunPKR3yBF1l8f4VUznFAW";
      
        public StripeServices(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

        }



      public   string CreatePaymentMethod(string cardNumber, int? expmonth,int? expyear,string cvc)
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


        public string SubscribeToStripePlanWithPaymentMethod(string customerId, string Catbasic, string paymentMethodId)
        {
            var options = new SubscriptionCreateOptions
            {
                Customer = customerId,
                Items = new List<SubscriptionItemOptions>
        {
            new SubscriptionItemOptions
            {
                Price = Catbasic,
            },
        },
                DefaultPaymentMethod = paymentMethodId,
            };

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
                // Cancel the subscription in Stripe
                CancelSub(existingSubscriptionId);

                // Update the subscription details in the database
               
            }
            else
            {
               
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

            // Check if the customer already exists in Stripe
            var existingCustomer = customerService.List(new CustomerListOptions { Email = email }).FirstOrDefault();

            if (existingCustomer != null)
            {
                return existingCustomer.Id;
            }
            else
            {
                // If the customer doesn't exist, create a new customer in Stripe
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
                // Return the first subscription found (assuming the customer has only one subscription at a time)
                return subscriptions.First().Id;
            }

            return null;
        }

        public async Task<string> CreateSubscriptionAsync(string email, string cardNumber, int? expmonth, int? expyear, string cvc, string priceID)
        {
            string customerId = string.Empty;




            var customerService = new CustomerService();

            // Check if the customer already exists in Stripe
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



            // Create a PaymentMethod using card information
            var paymentMethodId = CreatePaymentMethod(cardNumber, expmonth, expyear, cvc);

            // Attach the PaymentMethod to the customer
            AttachPaymentMethodToCustomer(paymentMethodId, customerId);



            // Create a subscription using the PaymentMethod
            string subscriptionId = SubscribeToStripePlanWithPaymentMethod(customerId, priceID, paymentMethodId);

            return subscriptionId;

        }

        public string CreatePaymentMethod(string cardNumber, int expmonth, int expyear, string cvc)
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

   
    }
}
