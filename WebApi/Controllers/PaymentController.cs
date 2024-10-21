using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using WebApi.IRepositories;
using ClassLibrary;
using WebApi.Utility;
using System.Data.Common;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {

          private readonly IPackagesRepository _repository;
        private readonly IPromotionPackageRepository _promotionPackageRepository;
        private readonly IStripeServices _stripeServices;
        private readonly IAccountRepository _accountRepository;
        private readonly string _PriceID15 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        private readonly string _PriceID30 = "price_1PWPlVKR3yBF1l8f71BYts44";
        private readonly string _PriceID50 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        private readonly string _PriceID75 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        private readonly string _PriceID100 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        // This is your Stripe CLI webhook secret for testing your endpoint locally.

        //For Testing
       // const string endpointSecret = "whsec_5faaa8f893e8ee04fe332d778da9a4b4807614c9f57791447440cfdcb58bca33";

        //For Live
        const string endpointSecret = "whsec_vHh97mrXLLgRnbDHNtgehD4OxPegYSPX";



        private string webUrl = "";

        private readonly ICurrencyConverterService _currencyConverterService;
        private readonly IAdvertisementServices _advertisementServices;

        public PaymentController(IPackagesRepository repository, IStripeServices stripeServices, IConfiguration configuration,IAccountRepository accountRepository, ICurrencyConverterService currencyConverterService,
            IPromotionPackageRepository promotionPackageRepository
            ,IAdvertisementServices advertisementServices

            )
        {
            _repository = repository;
            _stripeServices = stripeServices;
            webUrl = configuration.GetSection("UrlSetting").GetSection("baseWebUrl").Value ?? "";
            _accountRepository = accountRepository;

            _currencyConverterService = currencyConverterService;
            _promotionPackageRepository = promotionPackageRepository;
            _advertisementServices = advertisementServices;
        }

        [HttpPost("create-checkout-session")]
        public async Task<Response> CreateCheckoutSession([FromBody] CheckoutSessionRequest request)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);



                int p = _accountRepository.checkPackagesValidations(claimDTO.UserId, request.PurchasedProductID, request.packageType);

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
{
                        "bancontact",
                        "blik",
                        "card",
                        "eps",
                        "ideal",
                        "klarna",
                        "paypal",
                        "twint"
                    },

                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            Price = request.PriceId,
                            Quantity = 1,
                        },
                    },
                    //Mode = "subscription",
                    Mode = "payment",
                    AllowPromotionCodes = true,
                    AutomaticTax = new SessionAutomaticTaxOptions
                    {
                        Enabled = true,
                    },
                    SuccessUrl = webUrl + "home/thankyou?session_id={CHECKOUT_SESSION_ID}",
                    CancelUrl = webUrl + "dashboard",
                    Metadata = new Dictionary<string, string>
                    {
                        { "package_type", request.packageType },
                        { "PurchasedProductID", request.PurchasedProductID.ToString() },
                        { "user_id", claimDTO.UserId.ToString() },
                        { "Days", request.Days.ToString() },
                    },
                };

                var service = new SessionService();
                Session session = await service.CreateAsync(options);

                if (session != null)
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = new { id = session.Id };
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "";
                }
                return response;
            }
            catch (DbException ex)
            {
                response = CustomStatusResponse.GetResponse(600);
                response.ResponseMsg = ex.Message;
                response.Token = TokenManager.GenerateToken(claimDTO);
                return response;
            }
            catch (StripeException e)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.Token = TokenManager.GenerateToken(claimDTO);
                response.ResponseMsg = "An error occurred while creating the checkout session: " + e.Message;
                return response;
            }
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.Token = TokenManager.GenerateToken(claimDTO);
                response.ResponseMsg = ex.Message;
                return response;
            }
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            Console.WriteLine($"Webhook JSON: {json}"); // Log the raw JSON payload

            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    endpointSecret, // Ensure this matches your Stripe CLI setup
                    throwOnApiVersionMismatch: false
                );

                Console.WriteLine($"Received event: {stripeEvent.Type}"); // Log the event type
            }
            catch (StripeException e)
            {
                Console.WriteLine($"Stripe error: {e.Message}");
                return BadRequest("Invalid signature or error processing the event.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error processing webhook: {e.Message}");
                return BadRequest("Error processing webhook.");
            }

            Console.WriteLine($"Handling event type: {stripeEvent.Type}"); // Log before switch
            switch (stripeEvent.Type)
            {
                case Events.CheckoutSessionCompleted:
                    var checkoutSession = stripeEvent.Data.Object as Session;

                    if (!string.IsNullOrEmpty(checkoutSession.PaymentIntentId))
                    {
                        // Fetch the invoice using the ID
                        var paymentIntentService = new PaymentIntentService();
                        var paymentIntent = await paymentIntentService.GetAsync(checkoutSession.PaymentIntentId);

                        // Now extract the metadata from the invoice
                        if (paymentIntent.Metadata.TryGetValue("package_type", out var packageTypePI1))
                        {
                            Console.WriteLine($"Package Type from PaymentIntent Invoice: {packageTypePI1}");
                        }
                        else
                        {
                            Console.WriteLine("package_type not found in invoice metadata from PaymentIntent.");
                        }
                        // Safely access metadata from the payment intent
                        if (checkoutSession != null &&
                        checkoutSession.Metadata.TryGetValue("package_type", out var packageTypePI) &&
                        checkoutSession.Metadata.TryGetValue("user_id", out var userIdPI) &&
                        checkoutSession.Metadata.TryGetValue("PurchasedProductID", out var purchasedProductIdPI))
                        { // Handle subscription ID accordingly

                        
                            switch (packageTypePI)
                            {
                                case "pricing":
                                    await _repository.BuyPackageAsync(userIdPI, purchasedProductIdPI, paymentIntent.Id);
                                    break;

                                case "Advertisement":
                                    await _advertisementServices.BuyAdvertisementPackage(userIdPI, purchasedProductIdPI, paymentIntent.Id);
                                    break;

                                case "PromotionPackage":
                                    // Handle promotion payment success
                                    checkoutSession.Metadata.TryGetValue("Days", out var Days);
                                    await _promotionPackageRepository.BuyPromotionPackageAsync(userIdPI, purchasedProductIdPI, paymentIntent.Id, Convert.ToInt32(Days));


                                    break;

                                default:
                                    Console.WriteLine($"Unknown package type: {packageTypePI}");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("PaymentIntent metadata is missing.");
                        }
                    }


                    else if (checkoutSession.PaymentStatus == "paid")
                    {
                        // Process the order even if no PaymentIntentId is available
                        Console.WriteLine("Session was completed with no payment (100% discount).");

                        // Use metadata to handle the purchase
                        if (checkoutSession.Metadata.TryGetValue("package_type", out var packageTypePI) &&
                            checkoutSession.Metadata.TryGetValue("user_id", out var userIdPI) &&
                            checkoutSession.Metadata.TryGetValue("PurchasedProductID", out var purchasedProductIdPI))
                        {
                            switch (packageTypePI)
                            {
                                case "pricing":
                                    await _repository.BuyPackageAsync(userIdPI, purchasedProductIdPI, null); // PaymentIntentId is null
                                    break;

                                case "Advertisement":
                                    await _advertisementServices.BuyAdvertisementPackage(userIdPI, purchasedProductIdPI, null); // PaymentIntentId is null
                                    break;

                                case "PromotionPackage":
                                    if (checkoutSession.Metadata.TryGetValue("Days", out var days))
                                    {
                                        await _promotionPackageRepository.BuyPromotionPackageAsync(userIdPI, purchasedProductIdPI, null, Convert.ToInt32(days)); // PaymentIntentId is null
                                    }
                                    break;

                                default:
                                    Console.WriteLine($"Unknown package type: {packageTypePI}");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Checkout session metadata is missing.");
                        }
                    }

                    break;

                default:
                    Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
                    break;
            }

            return Ok();
        }


        [HttpPost("create-coupon-and-promo")]
        public async Task<Response> CreateCouponAndPromotionCode([FromBody] CouponAndPromotionRequest request)
        {
            Register claimDTO = null;
            Response response = new Response();
            try
            {


                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);



                // Call the service to create both the coupon and the promotion code with specific user assignments
                var promotionCode = await _stripeServices.CreateCouponAndPromotionCodeAsync(
                request.Name,
                request.AmountOff,
                request.Currency,
            
                request.AllowedUsers,
                request.ExpiresAt
            );

                if (promotionCode != null)
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = promotionCode;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "";
                }
                return response;
            }
            catch (Exception ex) {
                response = CustomStatusResponse.GetResponse(500);
                response.Token = TokenManager.GenerateToken(claimDTO);
                response.ResponseMsg = ex.Message;
                return response;
            }
          //  return Ok(promotionCode);
        }

        [HttpPost("GetAllCoupons")]
        public async Task<Response> GetAllCoupons()
        {
            Register claimDTO = null;
            Response response = new Response();
            try
            {


                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);
                var coupons = await _stripeServices.GetAllCouponsAsync();

                response = CustomStatusResponse.GetResponse(200);
                response.Data = coupons;
                response.Token = TokenManager.GenerateToken(claimDTO);
                response.ResponseMsg = "";

                return response;    
            }
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.Token = TokenManager.GenerateToken(claimDTO);
                response.ResponseMsg = ex.Message;
                return response;
            }
             
        }
    }
}
