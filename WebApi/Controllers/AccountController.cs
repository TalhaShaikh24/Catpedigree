using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Data.Common;
using WebApi.IRepositories;
using WebApi.Utility;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _repository;

        public AccountController(IAccountRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("Authenticate")]
        public Response Authenticate([FromBody] Register obj)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO=_repository.Authenticate(obj);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(320);
                else
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = new
                    {
                        DataObj = claimDTO,
                    };
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


        [HttpPost("RegisterUser")]
        public async Task<Response> RegisterUser([FromForm] Register formData)
        {
            Response response = new Response();

            try
            {

                var res = await _repository.RegisterUser(formData);
                response = CustomStatusResponse.GetResponse(200);
                response.Token = null;
                if (res != null)
                {

                    response.Data = res;
                    response.ResponseMsg = "Data save successfully!";


                }
                return response;



            }
            catch (DbException ex)
            {
                response = CustomStatusResponse.GetResponse(600);
                response.Token = null;
                response.ResponseMsg = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.Token = null;
                response.ResponseMsg = ex.Message;
                return response;
            }

        }


    }
}
