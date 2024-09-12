using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using WebApi.IRepositories;
using WebApi.Repositories;
using WebApi.Utility;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoPackagesController : ControllerBase
    {
        private readonly IVideoPackagesRepository _repository;

        private readonly IAccountRepository _accountRepository;

        private readonly IStripeServices _stripeServices;

        private readonly ICurrencyConverterService _currencyConverterService;


        private readonly string _PriceID15 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        private readonly string _PriceID30 = "price_1PWPlVKR3yBF1l8f71BYts44";
        private readonly string _PriceID50 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        private readonly string _PriceID75 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        private readonly string _PriceID100 = "price_1PWKweKR3yBF1l8fXM3cjclV";
      
        public VideoPackagesController(IVideoPackagesRepository repository, IAccountRepository accountRepository, IStripeServices stripeServices, ICurrencyConverterService currencyConverterService)
        {
            _repository = repository;
            _accountRepository = accountRepository;

            _stripeServices = stripeServices;

            _currencyConverterService = currencyConverterService;


        }

        [HttpPost("GetAllVideoPackages/{currency}")]
        public async  Task<Response> GetAllVideoPackages(string currency)
        {

             
            Response response = new Response();

            try
            {
               
                var res = _repository.GetAllVideoPackages();
                
                if (res != null)
                {

                    if (res.Count>0)
                    {
                        decimal rate = await _currencyConverterService.GetExchangeRate("EUR", currency);

                        foreach (var (item, index) in res.Select((item, index) => (item, index)))
                        {

                            res[index].Price = (double?)((decimal) item.Price * rate);


                        }

                    }


                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.ResponseMsg = "Data save successfully!";


                }
                return response;



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
        public  async Task<Response> BuyPackage([FromBody] VideoPackage obj)
        {

            Register claimDTO = null;
            Response response = new Response();
            string priceId = string.Empty;

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);




                int p = _accountRepository.checkPackagesValidations(claimDTO.UserId, obj.PackageID, "VedioPackage");
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
                var res = _repository.BuyPackage(obj.PackageID,claimDTO.UserId,obj.stripeSubscriptionId);
                
                if (res > 0)
                {

                   
                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Pakages Buy successfully!";


                }
                return response;



            }
            catch (DbException ex)
            {
                response = CustomStatusResponse.GetResponse(600);
                response.Token = TokenManager.GenerateToken(claimDTO);
                response.ResponseMsg = ex.Message;
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


        [HttpPost("VideoAvailablity")]

        public  Response VideoAvailablity()
        {

            Response response = new Response();

            Register claimDTO = null;

            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _repository.VideoAvailablity(claimDTO.UserId);

                if (res) 
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "";
                    response.Data = res;

                    return response;

                }
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Please Buy the Plan";
                    response.Data = res;

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
