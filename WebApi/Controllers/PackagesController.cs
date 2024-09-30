using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models;
using Stripe;
using Stripe.Checkout;
using System.Data.Common;
using WebApi.IRepositories;
using WebApi.Repositories;
using WebApi.Utility;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly IPackagesRepository _repository;
        private readonly IStripeServices _stripeServices;
        private readonly IAccountRepository _accountRepository;
        private readonly string _PriceID15 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        private readonly string _PriceID30 = "price_1PWPlVKR3yBF1l8f71BYts44";
        private readonly string _PriceID50 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        private readonly string _PriceID75 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        private readonly string _PriceID100 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        const string endpointSecret = "we_1Q4dZcKR3yBF1l8ferVqnC05";




        private string webUrl = "";

        private readonly ICurrencyConverterService _currencyConverterService;
        public PackagesController(IPackagesRepository repository, IStripeServices stripeServices, IConfiguration configuration,IAccountRepository accountRepository, ICurrencyConverterService currencyConverterService) 
        {
            _repository = repository;
            _stripeServices = stripeServices;
            webUrl = configuration.GetSection("UrlSetting").GetSection("baseWebUrl").Value ?? "";
            _accountRepository = accountRepository;

            _currencyConverterService = currencyConverterService;
        }

        


        [HttpPost("GetAllPackages/{currency}")]
        public async Task<Response> GetAllPackages(string currency)
        {
            Response response = new Response();

            try
            {

                var res = _repository.GetAllPackages();

                if (res.Count>0)
                {
                    decimal rate = await _currencyConverterService.GetExchangeRate("EUR", currency);
                    foreach (var (item, index) in res.Select((item, index) => (item, index)))
                    {

                        res[index].Price = Math.Round((decimal)(item.Price * rate), 2);


                    }
                }


                if (res == null) return CustomStatusResponse.GetResponse(320);

                else
                {

                    response = CustomStatusResponse.GetResponse(200);

                    response.ResponseMsg = "Blog Create Successfuly!";
                    response.Data = res;
                    return response;
                }
            }
            catch (DbException ex)
            {
                response = CustomStatusResponse.GetResponse(600);
                response.ResponseMsg = ex.Message;


                return response;
            }
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.ResponseMsg = ex.Message;

                return response;
            }
        }

        [HttpPost("BuyPackage")]
        public async Task<Response> BuyPackage([FromBody] UserPackages obj)
        {

            Register claimDTO = null;
            Response response = new Response();
            string priceId = string.Empty;


            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);
                obj.UserID = claimDTO.UserId;


                int p = _accountRepository.checkPackagesValidations(claimDTO.UserId, obj.PackageID, "pricing");


                if (p>0)
                {

                    if (p==15)
                    {

                        priceId = _PriceID15;

                    }

                    else if (p == 30)
                    {
                        priceId = _PriceID30;

                    }
                    
                    else if (p == 50)
                    {
                        priceId = _PriceID50;

                    }
                    else if (p == 75)
                    {
                        priceId = _PriceID75;

                    }

                    else 
                    {
                        priceId = _PriceID100;

                    }


                    var customerRespinse = await _stripeServices.CreateSubscriptionAsync(claimDTO.Email, obj.CardNumber,
                    obj.expireMonth, obj.expireYear, obj.cvc, priceId, obj.CouponCode);


                    obj.stripeSubscriptionId = customerRespinse;

                }





                var res = _repository.BuyPackage(obj);
                
                if (res != null)
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;

                    response.ResponseMsg = "Package purchased successfully! Thank you for your order!";


                    response.Token = TokenManager.GenerateToken(claimDTO);


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
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.ResponseMsg = ex.Message;
                response.Token = TokenManager.GenerateToken(claimDTO);
                return response;
            }

        }





        [HttpPost("AssignPackage")]
        public async Task<Response> AssignPackage([FromBody] UserPackages obj)
        {

            Register claimDTO = null;
            Response response = new Response();
            string priceId = string.Empty;


            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                obj.CreatedBy = claimDTO.UserId;


                var res = _repository.AssignPackage(obj);

                if (res != null)
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.ResponseMsg = "Package has been assigned successfully!";
                    response.Token = TokenManager.GenerateToken(claimDTO);
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
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.ResponseMsg = ex.Message;
                response.Token = TokenManager.GenerateToken(claimDTO);
                return response;
            }

        }


        public class CheckoutSessionRequest
        {
            public string PriceId { get; set; }
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CheckoutSessionRequest request)
        {
            if (string.IsNullOrEmpty(request.PriceId))
            {
                return BadRequest("PriceId is required.");
            }

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
                SuccessUrl = webUrl + "dashboard?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = webUrl + "dashboard",
            };

            var service = new SessionService();
            Session session;

            try
            {
                session = await service.CreateAsync(options);
            }
            catch (StripeException e)
            {
                
                return StatusCode(500, "An error occurred while creating the checkout session.");
            }

            return Ok(new { id = session.Id });
        }


        // Success endpoint to confirm payment
        [HttpGet("success")]
        public IActionResult Success(string session_id)
        {
            // You can fetch the session details to confirm the payment
            var service = new SessionService();
            var session = service.Get(session_id);

            if (session.PaymentStatus == "paid")
            {
                // Handle success, such as updating the database or showing confirmation to the user
                return Ok("Payment successful!");
            }

            return BadRequest("Payment not confirmed.");
        }

        // Cancel endpoint to handle failed/canceled payment
        [HttpGet("cancel")]
        public IActionResult Cancel()
        {
            return BadRequest("Payment was canceled.");
        }



        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            // Read the request body
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            Console.WriteLine($"Webhook JSON: {json}"); // Log the raw JSON payload

            Event stripeEvent;

            try
            {
                // Construct the event
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

            // Handle the event
            Console.WriteLine($"Handling event type: {stripeEvent.Type}"); // Log before switch
            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    await HandlePaymentSucceeded(paymentIntent);
                    break;

                case Events.InvoicePaymentSucceeded:
                    var invoice = stripeEvent.Data.Object as Invoice;
                    await HandleInvoicePaymentSucceeded(invoice);
                    break;

                default:
                    Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
                    break;
            }

            return Ok();
        }

        private async Task HandlePaymentSucceeded(PaymentIntent paymentIntent)
        {
            // Extract necessary data from the payment intent
            var customerId = paymentIntent.CustomerId;
            var amount = paymentIntent.AmountReceived;
            var currency = paymentIntent.Currency;
            var paymentStatus = paymentIntent.Status;
            

            // Here, you would typically save the data to your database
            // For example:
            var paymentRecord = new 
            {
                CustomerId = customerId,
                Amount = amount,
                Currency = currency,
                Status = paymentStatus,
                PaymentDate = DateTime.UtcNow // Save the current date
            };

           
        }

        private async Task HandleInvoicePaymentSucceeded(Invoice invoice)
        {
            // Extract necessary data from the invoice
            var customerId = invoice.CustomerId;
            var subscriptionId = invoice.SubscriptionId;
            var amountPaid = invoice.AmountPaid;
            var currency = invoice.Currency;
            var paymentStatus = invoice.Status; // This will usually be "paid" if the payment succeeded

            // Create a record for the payment in your database
            var invoiceRecord = new 
            {
                CustomerId = customerId,
                SubscriptionId = subscriptionId,
                AmountPaid = amountPaid,
                Currency = currency,
                Status = paymentStatus,
                PaymentDate = DateTime.UtcNow // Save the current date and time
            };

           }

    }
}
