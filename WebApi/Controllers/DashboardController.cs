using ClassLibrary;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Common;
using System.IO;
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

        private readonly IStripeServices _stripeServices;

        private readonly string _imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadImages");

        private readonly IWebHostEnvironment _hostingEnvironment;
        public DashboardController(IDashboardRepository repository, IWebHostEnvironment hostingEnvironment, IConfiguration configuration, IStripeServices stripeServices)
        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;

            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";
            _stripeServices = stripeServices;   
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

        [HttpPost("GetAllUsers")]
        public Response GetAllUsers()
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetAllUsers();

                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Users retrived successfuly";

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




        [HttpPost("UpdateUserProfile")]
        public async Task<Response> UpdateUserProfile([FromForm] Register formData)
        {
            Response response = new Response();
            Register claimDTO = null;

            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


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





        [HttpPost("UpdateUserRoles")]
        public async Task<Response> UpdateUserRoles([FromBody] userRolesUpdate userRoles)
        {
            Response response = new Response();
            Register claimDTO = null;

            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res =  _repository.UpdateRoles(userRoles);
                response = CustomStatusResponse.GetResponse(200);

                if (res >0)
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
                    response.ResponseMsg = "Listing Updated SuccessFully";
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
                    response.ResponseMsg = Status == "Approve" ? "Listing has been approved" : Status == "Pending" ? "Listing added in pending" : "Listing has been rejected";
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

                var images = _repository.GetAllGallary().Select(fileInfo => new Gallery
                {
                    Id = Path.GetFileNameWithoutExtension(fileInfo.FileName).GetHashCode(),
                    FileName = fileInfo.FileName,
                    FilePath = $"{BaseUrl}{fileInfo.FilePath}?v={DateTime.UtcNow.Ticks}"
                })
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



        

        [HttpPost("GetAllMedia")]
        public Response GetAllMedia()
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
                .Select(filePath => new FileInfo(filePath))
                .OrderByDescending(fileInfo => fileInfo.LastWriteTime)
                .Select(fileInfo => new Gallery
                {
                    Id = Path.GetFileNameWithoutExtension(fileInfo.Name).GetHashCode(),
                    FileName = fileInfo.Name,
                    FilePath = $"{BaseUrl}UploadImages/{fileInfo.Name}?v={DateTime.UtcNow.Ticks}",
                    GalleryImagesPath= $"UploadImages/{fileInfo.Name}",
                })
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




        [HttpPost("PedigreeGallery")]
        public Response PedigreeGallery()
        {

            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);




                var res = _repository.GetAllPedigreeGallary();


                List<Gallery> galleries = new List<Gallery>();


                foreach (var item in res)
                {
                    string fileName = Path.GetFileName(item);
                    galleries.Add(new Gallery
                    {


                        Id = Path.GetFileNameWithoutExtension(fileName).GetHashCode(),
                        FileName = fileName,
                        FilePath = $"{BaseUrl}UploadImages/{fileName}?v={DateTime.UtcNow.Ticks}"
                    });



                }

                if (res == null) return CustomStatusResponse.GetResponse(320);
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = galleries;
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


        [HttpPost("GetAllBreederLicense")]
        public Response GetAllBreederLicense()
        {

            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);




                var res = _repository.GetAllBreederLicense();


                List<Gallery> galleries = new List<Gallery>();


                foreach (var item in res)
                {
                    string fileName = Path.GetFileName(item);
                    galleries.Add(new Gallery
                    {


                        Id = Path.GetFileNameWithoutExtension(fileName).GetHashCode(),
                        FileName = fileName,
                        FilePath = $"{BaseUrl}Profile/{fileName}?v={DateTime.UtcNow.Ticks}"
                    });



                }

                if (res == null) return CustomStatusResponse.GetResponse(320);
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = galleries;
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

        [HttpPost("GetAllVideosGallery")]
        public Response GetAllVideosGallery()
        {

            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);




                var res = _repository.GetAllVideosGallery();


                List<Gallery> galleries = new List<Gallery>();


                foreach (var item in res)
                {
                    string fileName = Path.GetFileName(item);
                    galleries.Add(new Gallery
                    {


                        Id = Path.GetFileNameWithoutExtension(fileName).GetHashCode(),
                        FileName = fileName,
                        FilePath = $"{BaseUrl}UploadVideos/{fileName}?v={DateTime.UtcNow.Ticks}"
                    });



                }

                if (res == null) return CustomStatusResponse.GetResponse(320);
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = galleries;
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
        public async Task<Response> UploadNewGallery(List<IFormFile> files)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null)
                    return CustomStatusResponse.GetResponse(401);


                foreach (var item in files)
                {
                    if (item == null || item.Length == 0)
                    {
                        response = CustomStatusResponse.GetResponse(600);
                        response.ResponseMsg = "File is empty";
                        response.Data = null;
                        response.Token = TokenManager.GenerateToken(claimDTO);
                        return response;
                    }
                    else
                    {
                        string FileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(item.FileName);
                        var filePath = Path.Combine("UploadImages", FileName);
                        string FullFilePath = Path.Combine(_hostingEnvironment.WebRootPath, filePath);

                        if (System.IO.File.Exists(FullFilePath))
                        {

                            System.IO.File.Delete(FullFilePath);
                        }

                        using (var stream = new FileStream(FullFilePath, FileMode.Create))
                        {
                            await item.CopyToAsync(stream);
                        }

                    //    save in database

                        Gallery gallery = new Gallery();
                        gallery.FileName = FileName;
                        gallery.FilePath = filePath;
                        gallery.CreatedBy = claimDTO.UserId;


                        _repository.AddGallary(gallery);



                    }

                }


                response = CustomStatusResponse.GetResponse(200);
                response.Token = TokenManager.GenerateToken(claimDTO);
                response.Data = "";
                response.ResponseMsg = "File Upload successfully!";
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

        [HttpPost("UploadSelectedGalleryPath")]
        public Response UploadSelectedGalleryPath([FromBody]string Path)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var imagespath = Path.Split(",");

                foreach (var item in imagespath)
                {

                    string FullFilePath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, item);


                    //    save in database

                    Gallery gallery = new Gallery();
                    gallery.FileName = item.Replace("UploadImages/", string.Empty);
            
                    gallery.FilePath = item;
                    gallery.CreatedBy = claimDTO.UserId;



                    _repository.AddGallary(gallery);
                }




                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = true;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Gallery save successfully!";

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



        [HttpPost("DeleteSelectedGalleryPath/{Path}")]
        public Response DeleteSelectedGalleryPath(string Path)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                string[] filepaths = Path.Split(',');

                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.DeleteSelectedGalleryPath(Path);

                if (res)
                {

                    foreach (var item in filepaths)
                    {
                        string filePath = System.IO.Path.Combine("UploadImages", item.TrimStart());

                        string FullFilePath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, filePath);
                     

                        // Check if the file already exists
                        if (System.IO.File.Exists(FullFilePath))
                        {
                            // Delete the existing file
                            System.IO.File.Delete(FullFilePath);
                        }


            


                    }




                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Gallery Deleted successfully!";

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


        [HttpPost("DeleteListingById/{Id}")]
        public Response DeleteListingById(int Id)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.DeleteListingById(Id);

                if (res > 0)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Reecord Deleted successfully!";

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



        #region  CouponsCodes


        [HttpPost("AddCouponsCodes")]
        public Response AddCouponsCodes([FromBody] CouponCodes obj)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _stripeServices.AddCouponsCodes(obj);

                if (res > 0)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "CouponCode  generated successfully!";

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




        [HttpPost("GetCouponCodes")]
        public Response GetCouponCodes()
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetCouponCodes();

           

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "CouponCode  generated successfully!";

                
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




        [HttpPost("ActiveDeactiveCode/{Id}")]
        public Response ActiveDeactiveCode(int Id)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.ActiveDeactiveCouponCode(Id);

                if (res > 0)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Code Status Changed successfully!";

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



        [HttpPost("UpdateActiveInActiveUser/{Id}")]
        public Response UpdateActiveInActiveUser(int Id)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.UpdateActiveInActiveUser(Id);

                if (res > 0)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "User Status Changed successfully!";

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
        [HttpPost("DeleteUser/{Id}")]
        public Response DeleteUser(int Id)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.DeleteUser(Id);

                if (res > 0)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "User Deleted successfully!";

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
        



        [HttpPost("IsExpireCoupens/{Id}")]
        public Response IsExpireCoupens(int Id)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.IsExpireCoupens(Id);

                if (res > 0)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Code Status Changed successfully!";

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




    }
}
