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
    public class PackagesController : ControllerBase
    {
        private readonly IPackagesRepository _repository;

        public PackagesController(IPackagesRepository repository)
        {
            _repository = repository;
        }

        
        [HttpPost("GetAllPackages")]
        public Response GetAllPackages()
        {
            Response response = new Response();

            try
            {

                var res = _repository.GetAllPackages();

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
        public Response BuyPackage([FromBody] UserPackages obj)
        {

            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);
                obj.UserID = claimDTO.UserId;

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
    }
}
