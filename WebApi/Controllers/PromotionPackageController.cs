using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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




        public PromotionPackageController(IPromotionPackageRepository repository, IStripeServices stripeServices, IAccountRepository accountRepository)
        {
            _repository = repository;
            _stripeServices = stripeServices;
            
            _accountRepository = accountRepository;
        }


        [HttpPost("GetAllPromotionPackages")]
        public Response GetAllPromotionPackages()
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);
               
                var res = _repository.GetAllPromotionPackages();

                if (res == null) {


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



        [HttpPost("GetPromotionCost/{Id}")]
        public Response GetPromotionCost(int Id)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetPromotionCost(Id);

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
