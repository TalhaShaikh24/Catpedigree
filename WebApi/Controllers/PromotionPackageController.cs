using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models;
using Stripe;
using System.Data.Common;
using WebApi.IRepositories;
using WebApi.Utility;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class  PromotionPackageController : ControllerBase
    {
        private readonly IPromotionPackageRepository _repository;
        private readonly IStripeServices _stripeServices;
        private readonly IAccountRepository _accountRepository;
        private readonly string _PriceID15 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        private readonly string _PriceID30 = "price_1PWPlVKR3yBF1l8f71BYts44";
        private readonly string _PriceID50 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        private readonly string _PriceID75 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        private readonly string _PriceID100 = "price_1PWKweKR3yBF1l8fXM3cjclV";

        private readonly ICurrencyConverterService _currencyConverterService;



        public PromotionPackageController(IPromotionPackageRepository repository, IStripeServices stripeServices, IAccountRepository accountRepository, ICurrencyConverterService currencyConverterService)
        {
            _repository = repository;
            _stripeServices = stripeServices;
            
            _accountRepository = accountRepository;

            _currencyConverterService = currencyConverterService;
        }


        [HttpPost("GetAllPromotionPackages/{currency}")]
        public async Task<Response> GetAllPromotionPackages(string currency)
        {
           
            Response response = new Response();

            try
            {
                
                
               
                var res = _repository.GetAllPromotionPackages();
                if (res.Count>0)
                {
                    decimal rate = await _currencyConverterService.GetExchangeRate("EUR", currency);

                    for (int i = 0; i < res.Count; i++)
                    {
                        for (int j = 0; j < res[i].promotionCosts.Count; j++)
                        {
                            res[i].promotionCosts[j].Cost= Math.Round((decimal)(res[i].promotionCosts[j].Cost * rate), 2);
                        }

                    }
                    
                }

                if (res == null) {


                    response = CustomStatusResponse.GetResponse(320);
                    return response;
                }

                else
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.ResponseMsg = "Successfuly!";
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
        public async Task<Response> BuyPackage([FromBody] PromotionPackages obj)
        {

            Register claimDTO = null;
            Response response = new Response();
            string priceId = string.Empty;


            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);
                obj.UserID = claimDTO.UserId;

                int p = _accountRepository.checkPackagesValidations(claimDTO.UserId, obj.PromotionPackagesID, "PromotionPackage");


                if (p > 0)
                {

                    // need to change the PriceID becuase of video package 
                    if (p == 3)
                    {

                        priceId = _PriceID15;

                    }
                     if (p == 15)
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
                    obj.expireMonth, obj.expireYear, obj.cvc, priceId);


                    obj.stripeSubscriptionId = customerRespinse;

                }





                var res = _repository.BuyPromotionPackage(obj);

                if (res != null)
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;

                    response.ResponseMsg = " Purchased Successfuly!";

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



        [HttpPost("GetPromotionCost")]
        public  async Task<Response> GetPromotionCost([FromBody] PromotionsCostCur obj)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);



                var res = _repository.GetPromotionCost(obj.Id);



                if (res.Count>0)
                {
                    decimal rate = await _currencyConverterService.GetExchangeRate("EUR", obj.currency);
                    foreach (var (item, index) in res.Select((item, index) => (item, index)))
                    {

                        res[index].Cost = Math.Round((decimal)(item.Cost * rate), 2);


                    }
                }


                if (res == null)
                {


                    response = CustomStatusResponse.GetResponse(320);
                    response.Token = TokenManager.GenerateToken(claimDTO);

                    return response;
                }

                else
                {

                    response = CustomStatusResponse.GetResponse(200);

                    response.ResponseMsg = "Successfuly!";
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    return response;
                }
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
