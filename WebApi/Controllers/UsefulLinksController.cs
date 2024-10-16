using ClassLibrary;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Data.Common;
using System.Reflection.Metadata;
using WebApi.IRepositories;
using WebApi.Repositories;
using WebApi.Utility;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsefulLinksController : ControllerBase
    {
        private readonly IUsefulLinksRepository _repository;

        public UsefulLinksController(IUsefulLinksRepository repository)
        {
            _repository = repository;
           
        }


        [HttpPost("GetAllUsefulLinks")]
        public Response GetAllUsefulLinks()
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetAllUsefulLinks();

                if (res == null) return CustomStatusResponse.GetResponse(320);
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
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
                response.ResponseMsg = "Internal server error!";
                response.Token = TokenManager.GenerateToken(claimDTO);
                return response;
            }
        }

        [HttpPost("AddUsefulLink")]

        public async Task<Response> AddUsefulLink([FromForm] UsefulLinks obj)
        {

            Response response = new Response();

            Register claimDTO = null;

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                if (obj.UsefulLinkFile != null)
                {
                    obj.CreatedBy = claimDTO.UserId;

                    var res = await _repository.AddUsefulLink(obj);

                    if (res == null) return CustomStatusResponse.GetResponse(320);

                    else
                    {

                        response = CustomStatusResponse.GetResponse(200);
                        response.Token = TokenManager.GenerateToken(claimDTO);
                        response.ResponseMsg = "Useful Link Added Successfuly!";
                        response.Data = res;
                        
                    }
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
