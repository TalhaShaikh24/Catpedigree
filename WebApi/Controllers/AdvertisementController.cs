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
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementServices _repository;
        private readonly IEmailRepository _emailRepository;

        private readonly IAccountRepository _accountRepository;

        private readonly IStripeServices _stripeServices;

        private readonly ICurrencyConverterService _currencyConverterService;

        private readonly string _PriceID15 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        private readonly string _PriceID30 = "price_1PWPlVKR3yBF1l8f71BYts44";
        private readonly string _PriceID50 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        private readonly string _PriceID75 = "price_1PWKweKR3yBF1l8fXM3cjclV";
        private readonly string _PriceID100 = "price_1PWKweKR3yBF1l8fXM3cjclV";

        public AdvertisementController(IAdvertisementServices repository, IAccountRepository accountRepository, IStripeServices stripeServices, ICurrencyConverterService currencyConverterService, IEmailRepository emailRepository)
        {
            _repository = repository;
            _emailRepository = emailRepository;
            _accountRepository = accountRepository;
            _stripeServices = stripeServices;
            _currencyConverterService = currencyConverterService;
        }



        [HttpPost("GetHomeAdvertisments/{Id}")]
        public Response GetHomeAdvertisments(int Id)
        {

            Response response = new Response();

            try
            {
                

                var res = _repository.GetHomeAdvertisments(Id);

                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.ResponseMsg = "Data Fatched successfully!";

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
        [HttpPost("GetSidebarAdvertisments/{Id}")]
        public Response GetSidebarAdvertisments(int Id)
        {

            Response response = new Response();

            try
            {
                

                var res = _repository.GetHomeAdvertisments(Id);

                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.ResponseMsg = "Data Fatched successfully!";

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


        [HttpPost("GetAdvertisementPackagesDashboard/{currency}")]
        public async Task<Response> GetAdvertisementPackagesDashboard(string currency)
        {

            Register claimDTO = null;
            Response response = new Response();
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetAdvertisementPackage();

                if (res != null)
                {

                    if (res.Count>0)
                    {
                        decimal rate = await _currencyConverterService.GetExchangeRate("EUR", currency);

                        foreach (var (item, index) in res.Select((item, index) => (item, index)))
                        {

                            res[index].AdvertisementPackageCost = Math.Round(item.AdvertisementPackageCost * rate,2);


                        }



                    }

                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = res;
                    response.ResponseMsg = "Data Fatched successfully!";

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

        [HttpPost("GetAdvertisementPackage/{currency}")]
        public async Task<Response> GetAdvertisementPackage(string currency)
        {
           
            Response response = new Response();

            try
            {
                

                var res = _repository.GetAdvertisementPackage();

                if (res != null)
                {

                    if (res.Count>0)
                    {
                        decimal rate = await _currencyConverterService.GetExchangeRate("EUR", currency);

                        foreach (var (item, index) in res.Select((item, index) => (item, index)))
                        {

                            res[index].AdvertisementPackageCost = Math.Round(item.AdvertisementPackageCost * rate,2);


                        }



                    }

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.ResponseMsg = "Data Fatched successfully!";

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

        


        [HttpPost("BuyAdvertisementPackage")]
        public async Task<Response> BuyAdvertisementPackage([FromBody] UserAdvertisementPackage obj)
        {
            Register claimDTO = null;
            Response response = new Response();
            string priceId = string.Empty;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                obj.UserId = claimDTO.UserId;
                obj.CreatedBy = claimDTO.UserId;
                int p = _accountRepository.checkPackagesValidations(claimDTO.UserId, obj.AdvertisementPackageID, "Advertisement");
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


                var res = _repository.BuyAdvertisementPackage(obj);

                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Packages Purchased successfully!";

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


        [HttpPost("UserAdvertisementPackages")]
        public Response UserAdvertisementPackages()
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _repository.userAdvertisementPackages(claimDTO.UserId);

                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Data Fatched successfully!";

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


        [HttpPost("GetallUserAdvertisementForApprovals")]
        public Response GetallUserAdvertisementForApprovals()
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _repository.GetallUserAdvertisementForApprovals();

                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Data Fatched successfully!";

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


        [HttpPost("UpdateUserAdvertisementStatus")]
        public Response UpdateUserAdvertisementStatus(int Id, string Status, string Reason = null)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _repository.UpdateUserAdvertisementStatus(Id,Status,Reason);

                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Data Saved successfully!";

                    // Send email if the status is "Reject"
                    if (Status == "Reject")
                    {
                        _emailRepository.SendRejectionEmail(res.Email, Reason);
                        //SendRejectionEmail(res.Email, Reason); // Assuming res has an Email property
                    }

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



        [HttpPost("UtilizePurchasedAdvertisementPackage")]
        public async Task<Response> UtilizePurchasedAdvertisementPackage([FromForm] UtilizePurchasedAdvertisementPackage obj)
        {

            Response response = new Response();

            Register claimDTO = null;


            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                obj.UserId = claimDTO.UserId;


            
               
                 var    res = await _repository.utilizePurchasedAdvertisementPackageAsync(obj);

                 
                        response = CustomStatusResponse.GetResponse(200);
                        response.Token = TokenManager.GenerateToken(claimDTO);
                        response.ResponseMsg = "Data Save SuccessFully";
                        response.Data = res;

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
