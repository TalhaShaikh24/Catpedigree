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
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _repository;

        public BlogController(IBlogRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("AddBlog")]
        public async Task<Response> AddBlog([FromForm] Blog obj)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                obj.CreatedBy = claimDTO.UserId;

                var res = await _repository.AddBlog(obj);

                if (res == null) return CustomStatusResponse.GetResponse(320);
               
                else
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Blog Create Successfuly!";
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


        [HttpPost("GetAllBlogs")]
        public Response GetAllBlogs()
        {
            Response response = new Response();
         
            try
            {
       
                var res = _repository.GetAllBlogs();

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


        [HttpPost("GetAllAdminBLogs")]
        public Response GetAllAdminBLogs()
        {
            Response response = new Response();

            Register claimDTO = null;

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetAllBlogs();

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


        [HttpPost("GetAllBlogDetails/{Id}")]
        public Response GetAllBlogDetails(int Id)
        {
            Response response = new Response();
         
            try
            {
       
                var res = _repository.GetAllBlogDetails(Id);

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
        


        [HttpPost("AddComment")]
        public Response AddComment([FromBody] Comment obj)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                obj.UserId = claimDTO.UserId;


                var res = _repository.AddComment(obj);

                if (res == null) return CustomStatusResponse.GetResponse(320);

                else
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.ResponseMsg = "Comment Create Successfuly!";
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
                response.ResponseMsg = ex.Message;
                response.Token = TokenManager.GenerateToken(claimDTO);
                return response;
            }
        }





    }
}
