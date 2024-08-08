using ClassLibrary;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Data.Common;
using System.Reflection.Metadata;
using WebApi.IRepositories;
using WebApi.Utility;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepository _repository;
        

        public ContactController(IContactRepository repository)
        {
            _repository = repository;
            
        }

        [HttpPost("AddContact")]
        public async Task<Response> AddContact([FromBody] Contact contactInfo)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                var res = await _repository.AddContact(contactInfo);

                if (res == null) return CustomStatusResponse.GetResponse(320);

                else
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.ResponseMsg = "Thanks for submitting form!";
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


    }
}
