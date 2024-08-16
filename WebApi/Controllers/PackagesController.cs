using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models;
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


                int p = _accountRepository.checkPackagesValidations(claimDTO.UserId, obj.PackageID, "Listing");


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




    }
}
