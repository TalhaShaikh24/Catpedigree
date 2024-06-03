using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using WebApi.IRepositories;
using WebApi.Utility;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionPackageController : ControllerBase
    {
        private readonly IPromotionPackageRepository _repository;

        public PromotionPackageController(IPromotionPackageRepository repository)
        {
            _repository = repository;
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
        public Response BuyPackage([FromBody] PromotionPackages obj)
        {

            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);
                obj.UserID = claimDTO.UserId;

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





    }
}
