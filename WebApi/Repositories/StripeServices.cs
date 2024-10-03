using ClassLibrary;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Stripe.Forwarding;
using System.Data;
using WebApi.DBManager;
using WebApi.IRepositories;
using static WebApi.Controllers.PackagesController;

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

        public string CreateDiscountCoupon(decimal discountPercentage,string couponId, int? customDays)
        {
            var customExpirationDate = DateTime.UtcNow.AddDays((int)customDays);
            var options = new CouponCreateOptions
            {
                Id = couponId,
                PercentOff = discountPercentage,
                Duration = "once", // You can manage expiration with your logic
                Metadata = new Dictionary<string, string>
    {
        { "expiration_date", customExpirationDate.ToString("o") }
    }
            };

           

            var service = new CouponService();
            var coupons = service.List(new CouponListOptions()).ToList();
            var checkcoupon = coupons.FirstOrDefault(c => c.Id == couponId);


            if (checkcoupon == null)
            {
                var coupon = service.Create(options);

                return coupon.Id;
            }
            else
            {

                return couponId;


            }




        }
        public int AddCouponsCodes(CouponCodes obj)
        {

            DynamicParameters parameters = new DynamicParameters();


           string code= CreateDiscountCoupon(obj.DiscountPercentage,obj.CouponName,obj.CouponsDays);

            parameters.Add("@DiscountPercentage", obj.DiscountPercentage, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@CouponCode", code, DbType.String, ParameterDirection.Input);

            parameters.Add("@CodeName", obj.CouponName, DbType.String, ParameterDirection.Input);


                parameters.Add("@CouponsDays", obj.CouponsDays, DbType.Int32, ParameterDirection.Input);



            parameters.Add("@userId", obj.UserId, DbType.Int32, ParameterDirection.Input);

            parameters.Add("@CreatedBy", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);


            var data = _dapper.Insert<int>(@"dbo.[sp_Add_CouponsCodes]", parameters);

            return data;
        }

        // New Stripe Implementation

        public class CheckoutSessionRequest
        {
            public string PriceId { get; set; }
        }


        public async  Task<string> CreateCheckoutSession([FromBody] CheckoutSessionRequest request)
        {
      
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                Price = request.PriceId,
                Quantity = 1,
            },
        },
                Mode = "subscription",
                AllowPromotionCodes = true,
                SuccessUrl = "http://localhost:7297/success?session_id={CHECKOUT_SESSION_ID}", // Use http for local
                CancelUrl = "http://localhost:7297/cancel",
            };

            var service = new SessionService();
            Session session;

            try
            {
                session = await service.CreateAsync(options);
            }
            catch (StripeException e)
            {

                return e.Message;
            }

            return session.Id;
        }
        ///  New Coupon Code



        // Create Coupon
        public async Task<Coupon> CreateCouponAsync(string couponName, long amountOff, string currency, List<string> userEmails, DateTime? expiresAt = null)
        {
            var options = new CouponCreateOptions
            {
                Name = couponName, // Same name for coupon and promotion code
                PercentOff = Convert.ToDecimal(amountOff),
                Currency = currency,

            };


            // Check if userEmails is provided and not empty
            if (userEmails != null && userEmails.Count > 0)
            {
                options.Metadata = new Dictionary<string, string>
              {
                  { "allowed_users", string.Join(",", userEmails) } // Store the list of allowed users as metadata
              };
            }

            var service = new CouponService();
            var coupon = await service.CreateAsync(options);

            return coupon;
        }

        // Create Promotion Code with Coupon
        public async Task<PromotionCode> CreatePromotionCodeAsync(string couponId, string promotionCodeName, List<string> userEmails, DateTime? expiresAt = null)
        {
            var options = new PromotionCodeCreateOptions
            {
                Coupon = couponId, // Reference the coupon ID
                Code = promotionCodeName, // Same name as coupon
                
                ExpiresAt = expiresAt?.ToUniversalTime(), // Optional expiry date for the promotion code
            };

            // Check if userEmails is provided and not empty
            if (userEmails != null && userEmails.Count > 0)
            {
                options.Metadata = new Dictionary<string, string>
              {
                  { "allowed_users", string.Join(",", userEmails) } // Store the list of allowed users as metadata
              };
                      }

            var service = new PromotionCodeService();
            var promotionCode = await service.CreateAsync(options);

            return promotionCode;
        }

        // Combine both Coupon and Promotion Code creation with allowed users
        public async Task<PromotionCode> CreateCouponAndPromotionCodeAsync(string name, long amountOff, string currency, List<string> userEmails, DateTime? expiresAt = null)
        {
            // First, create the coupon
            var coupon = await CreateCouponAsync(name, amountOff, currency, userEmails, expiresAt);

            // Then, create the promotion code using the coupon ID
            var promotionCode = await CreatePromotionCodeAsync(coupon.Id, name, userEmails, expiresAt);

            return promotionCode;
        }

       
    public async Task<List<PromotionCodeDto>> GetAllCouponsAsync()
        {
            var promotionCodeService = new PromotionCodeService();
            var promotionCodeOptions = new PromotionCodeListOptions
            {
                Limit = 100 // Max is 100
            };

            var promotionCodes = new List<PromotionCodeDto>();
            var list = await promotionCodeService.ListAsync(promotionCodeOptions);

            // Filter the list based on Active property
            var filteredList = list.Where(x => x.Active).ToList();

            // Create a new StripeList from the filtered list
            var newList = new Stripe.StripeList<Stripe.PromotionCode>
            {
                Data = filteredList,
               
            };

            // Create a dictionary to hold coupon details for quick access
            var couponService = new CouponService();
            var couponDetails = new Dictionary<string, Coupon>();

            foreach (var promoCode in newList.Data)
            {
                // Fetch coupon details if not already fetched
                if (!couponDetails.ContainsKey(promoCode.Coupon.Id))
                {
                    var coupon = await couponService.GetAsync(promoCode.Coupon.Id);
                    couponDetails[coupon.Id] = coupon;
                }

                // Add to the result list
                var couponInfo = couponDetails[promoCode.Coupon.Id];
                promotionCodes.Add(new PromotionCodeDto
                {
                    Id = promoCode.Id,
                    IsActive = promoCode.Active,
                    Code = promoCode.Code,
                    ExpiresAt = promoCode.ExpiresAt,
                    AmountOff = couponInfo.AmountOff,
                    PercentOff = couponInfo.PercentOff,
                    Currency = couponInfo.Currency,
                    Name = couponInfo.Name,
                    Metadata = promoCode.Metadata,
                    CoupenCodeID= couponInfo.Id,
                });
            }

            return promotionCodes;
        }
        public async Task DeleteCouponAsync(string couponId)
        {
           
                // Deleting the coupon by ID
                var couponService = new CouponService();
                await couponService.DeleteAsync(couponId);

           

              
               
            
            
        }
    }
}
