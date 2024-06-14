using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Common;
using WebApi.IRepositories;
using WebApi.Utility;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private string BaseUrl = "";
        private readonly IDashboardRepository _repository;

        private readonly string _imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadImages");

        private readonly IWebHostEnvironment _hostingEnvironment;
        public DashboardController(IDashboardRepository repository, IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;

            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";
        }

        [HttpPost("GetAllDashboard")]
        public Response GetAllDashboard()
        { 
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetJsonDataAsync(claimDTO.UserId);
                
                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Data save successfully!";

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



        [HttpPost("GetAllDropdowns")]
        public Response GetAllDropdowns()
        { 
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetAllDropdowns(claimDTO.UserId);
                
                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Data save successfully!";

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


        #region Account Controller Profiles Methods

        [HttpPost("GetProfileDetailById")]
        public Response GetProfileDetailById()
        {
            Response response = new Response();

            Register claimDTO = null;

            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetProfileDetailById(claimDTO.UserId);

                response = CustomStatusResponse.GetResponse(200);
                response.Token = TokenManager.GenerateToken(claimDTO);
                if (res != null)
                {

                    response.Data = res;
                    response.ResponseMsg = "Data Update successfully!";


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


        [HttpPost("UpdateProfile")]
        public async Task<Response> UpdateProfile([FromForm] Register formData)
        {
            Response response = new Response();
            Register claimDTO = null;

            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                formData.UserId = claimDTO.UserId;

                var res = await _repository.UpdateProfile(formData);
                response = CustomStatusResponse.GetResponse(200);
                
                if (res != null)
                {

                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Data Update successfully!";


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

        #endregion



        #region Listing Controller Listing Medthods

        [HttpPost("GetAllMyListings")]
        public Response GetAllMyListings()
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetAllMyListings(claimDTO.UserId);

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
        
        
        
        [HttpPost("GetAllListings")]
        public Response GetAllListings()
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetAllListings();

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


        [HttpPost("GetListingDetailById/{Id}")]

        public Response GetListingDetailById(int Id)
        {
            Response response = new Response();
            Register claimDTO = null;

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _repository.GetListingDetailById(Id);

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



        [HttpPost("UpdateListing")]

        public async Task<Response> UpdateListing([FromForm] Listing obj)
        {
            Response response = new Response();
            Register claimDTO = null;

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                obj.ModifiedBy = claimDTO.UserId;

                var res = await _repository.UpdateListing(obj);

                if (res == null) return CustomStatusResponse.GetResponse(320);
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Data Update SuccessFully";
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
                response.ResponseMsg = "Internal server error!";
                response.Token = TokenManager.GenerateToken(claimDTO);
                return response;
            }
        }


        [HttpPost("UpdateListingStatus")]
        public Response UpdateListingStatus(int Id, string Status)
        {
            Response response = new Response();
            Register claimDTO = null;

            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res =_repository.UpdateListingStatus(Id, Status);

                if (res == null) return CustomStatusResponse.GetResponse(320);
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Data Update SuccessFully";
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
                response.ResponseMsg = "Internal server error!";
                response.Token = TokenManager.GenerateToken(claimDTO);
                return response;
            }
        }
        #endregion





        [HttpPost("GetListing_ProdictionPackages")]
        public Response GetListing_ProdictionPackages()
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetListing_ProdictionPackages(claimDTO.UserId);

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


        [HttpPost("Assgin_PromotionPackage_to_List")]
        public Response Assgin_PromotionPackage_to_List([FromBody] Listing listing)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                listing.CreatedBy = (int)claimDTO.UserId;

                var res = _repository.Assgin_PromotionPackage_to_List(listing);

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
                response.ResponseMsg = ex.Message;
                response.Token = TokenManager.GenerateToken(claimDTO);
                return response;
            }
        }



        [HttpPost("GetAllGallery")]
        public Response GetAllGallery()
        {

            Response response = new Response();
            Register claimDTO = null;
            try
            {
               claimDTO = TokenManager.GetValidateToken(Request);

               if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


            if (!Directory.Exists(_imagesPath))
            {
                    response = CustomStatusResponse.GetResponse(600);
                    response.ResponseMsg = "Image directory not found.";
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = null;
                    return response;
                }

                var images = Directory.GetFiles(_imagesPath)
                   .Select(filePath => new Gallery
                   {
                       Id = Path.GetFileNameWithoutExtension(filePath).GetHashCode(),
                       FileName = Path.GetFileName(filePath),
                       FilePath = $"{BaseUrl}UploadImages/{Path.GetFileName(filePath)}?v={DateTime.UtcNow.Ticks}"
                   })
                   .OrderByDescending(g => g.FilePath)
                   .ToList();

                if (images == null) return CustomStatusResponse.GetResponse(320);
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = images;
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


        [HttpPost("replaceFile")]
        public async Task<Response> Replace(IFormFile file)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null)
                    return CustomStatusResponse.GetResponse(401);

                if (file == null || file.Length == 0)
                {
                    response = CustomStatusResponse.GetResponse(600);
                    response.ResponseMsg = "File is empty";
                    response.Data = null;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    return response;
                }
                else
                {
                    var filePath = Path.Combine("UploadImages", file.FileName);
                    string FullFilePath = Path.Combine(_hostingEnvironment.WebRootPath, filePath);

                    // Check if the file already exists
                    if (System.IO.File.Exists(FullFilePath))
                    {
                        // Delete the existing file
                        System.IO.File.Delete(FullFilePath);
                    }

                    // Save the new file
                    using (var stream = new FileStream(FullFilePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = file.FileName;
                    response.ResponseMsg = "File replaced successfully!";
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

        [HttpPost("UploadNewGallery")]
        public async Task<Response> UploadNewGallery(IFormFile file)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null)
                    return CustomStatusResponse.GetResponse(401);

                if (file == null || file.Length == 0)
                {
                    response = CustomStatusResponse.GetResponse(600);
                    response.ResponseMsg = "File is empty";
                    response.Data = null;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    return response;
                }
                else
                {
                    string FileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(file.FileName);
                    var filePath = Path.Combine("UploadImages", FileName);
                    string FullFilePath = Path.Combine(_hostingEnvironment.WebRootPath, filePath);

                    if (System.IO.File.Exists(FullFilePath))
                    {

                        System.IO.File.Delete(FullFilePath);
                    }

                    using (var stream = new FileStream(FullFilePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = file.FileName;
                    response.ResponseMsg = "File Upload successfully!";
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

        [HttpPost("UploadSelectedGalleryPath/{Path}")]
        public Response UploadSelectedGalleryPath(string Path)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.UploadSelectedGalleryPath(Path);

                if (res)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Gallery save successfully!";

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

    }
}
