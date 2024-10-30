using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using WebApi.IRepositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        private readonly IShowRepository _repository;
        public ShowController(IShowRepository showRepository)
        {
            _repository = showRepository;
                
        }

        [HttpPost("GetAllShowsPagination")]
        public Response GetAllShowsPagination([FromBody] Show obj)
        {
            Response response = new Response();

            try
            {
                var result = _repository.GetAllShowsPagination(obj);

                if (result == null || result.Shows == null || !result.Shows.Any())
                {
                    return CustomStatusResponse.GetResponse(320);
                }
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    int currentCount = (obj.PageNumber - 1) * obj.PageSize + result.FetchedCount;

                    response.Data = new
                    {
                        Shows = result.Shows,
                        TotalCount = result.TotalCount,
                        CurrentCount = currentCount
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

        [HttpPost("GetAllShowDetails/{Id}")]
        public Response GetAllShowDetails(int Id)
        {
            Response response = new Response();

            try
            {
                var result = _repository.GetAllShowDetails(Id);

            
                    response = CustomStatusResponse.GetResponse(200);

                     response.Data = result;
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

    }
}
