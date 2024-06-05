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
    public class VendorController : ControllerBase
    {
           private readonly IVendorRepository _repository;



        public VendorController(IVendorRepository repository)
        {
            _repository = repository;
        }


        [HttpPost("GetVednorDataAndList/{Id}")]
        public Response GetVednorDataAndList(int Id)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetVednorDataAndList(Id);

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

