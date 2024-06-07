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
            Response response = new Response();

            try
            {
                
                var res = _repository.GetVednorDataAndList(Id);

                if (res == null)
                {


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


        [HttpPost("GetAllVendors")]
        public Response GetAllVendors()
        {
            Response response = new Response();

            try
            {
            
                var res = _repository.GetAllVendors();

                if (res == null)
                {


                    
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

    }
}

