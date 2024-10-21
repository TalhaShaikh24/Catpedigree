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


        [HttpPost("GetAllUsefulLinksForGuest")]
        public Response GetAllUsefulLinksForGuest()
        {
            Response response = new Response();
            try
            {

                var res = _repository.GetAllUsefulLinks();

                if (res == null) return CustomStatusResponse.GetResponse(320);
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
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
                response.ResponseMsg = "Internal server error!";
                return response;
            }
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

        [HttpPost("GetUsefulLinkById/{Id}")]
        public Response GetUsefulLinkById(int Id)
        {
            Response response = new Response();
            Register claimDTO = null;

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _repository.GetUsefulLinkById(Id);

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
       
        
        [HttpPost("UpdateUsefulLinkById")]
        public async Task<Response> UpdateUsefulLinkById([FromForm] UsefulLinks obj)
        {

            Response response = new Response();

            Register claimDTO = null;

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

               
                    obj.ModifiedBy = claimDTO.UserId;

                    var res = await _repository.UpdateUsefulLinkById(obj);

                    if (res == null) return CustomStatusResponse.GetResponse(320);

                    else
                    {

                        response = CustomStatusResponse.GetResponse(200);
                        response.Token = TokenManager.GenerateToken(claimDTO);
                        response.ResponseMsg = "Useful Link Added Successfuly!";
                        response.Data = res;
                        
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

        [HttpPost("DeleteUsefulLinkById/{Id}")]
        public Response DeleteUsefulLinkById(int Id)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                // Retrieve the listing to get file paths
                UsefulLinks usefulLinks = _repository.GetUsefulLinkById(Id); // Ensure you have this method implemented
                if (usefulLinks.UsefulLinkFilePath == null) return CustomStatusResponse.GetResponse(404); // Listing not found

                var res = _repository.DeleteUsefulLinkById(Id);

                if (res != null)
                {
                    // Delete the video and feature image
                    DeleteFileIfExists(usefulLinks.UsefulLinkFilePath);
                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Useful Link has been deleted successfuly!";

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

        private void DeleteFileIfExists(string relativePath)
        {
            if (!string.IsNullOrEmpty(relativePath))
            {
                // Construct the full path using the wwwroot directory
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
        }

    }
}
