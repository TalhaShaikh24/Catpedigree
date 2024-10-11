using ClassLibrary;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Common;
using System.IO;
using System.Net.Mail;
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
        private readonly IEmailRepository _emailRepository;

        private readonly IStripeServices _stripeServices;
        private readonly IConfiguration _configuration;

        private readonly string _imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadImages");

        private readonly string _galleryimagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadGallery");

        
        private readonly string _breederlicensePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Profile");



        private readonly IWebHostEnvironment _hostingEnvironment;
        public DashboardController(IDashboardRepository repository, IWebHostEnvironment hostingEnvironment, IConfiguration configuration, IStripeServices stripeServices,IEmailRepository emailRepository)
        {
            _repository = repository;
            _emailRepository = emailRepository;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;

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

                    claimDTO.ProfilePicPath = res.ProfilePicPath;

                    response.Data = claimDTO;
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
        public Response UpdateListingStatus(int Id, string Status, string Reason = "")
        {
            Response response = new Response();
            Register claimDTO = null;

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.UpdateListingStatus(Id, Status, Reason);

                if (res == null) return CustomStatusResponse.GetResponse(320);
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = Status == "Approve" ? "Listing has been approved" : Status == "Pending" ? "Listing added in pending" : "Listing has been rejected";
                    response.Data = res;

                    // Send email if the status is "Reject"
                    if (Status == "Reject")
                    {
                        _emailRepository.SendRejectionEmail(res.Email, Reason);
                        //SendRejectionEmail(res.Email, Reason); // Assuming res has an Email property
                    }

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

        private void SendRejectionEmail(string toEmail, string reason)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                using (var mail = new MailMessage())
                using (var smtpClient = new SmtpClient(smtpSettings["Server"]))
                {
                    mail.From = new MailAddress(smtpSettings["SenderEmail"], smtpSettings["SenderName"]);
                    mail.To.Add(toEmail);
                    mail.Subject = "Listing Rejection Notice";
                    mail.Body = $"Your listing has been rejected for the following reason:\n\n{reason}";

                    smtpClient.Port = int.Parse(smtpSettings["Port"]);
                    smtpClient.Credentials = new System.Net.NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                    smtpClient.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                    smtpClient.Send(mail);
                }
            }
            catch (SmtpException smtpEx)
            {
                // Log SMTP-specific exceptions
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
                // Handle or rethrow according to your needs
            }
            catch (Exception ex)
            {
                // Log other exceptions
                Console.WriteLine($"General Error: {ex.Message}");
                // Handle or rethrow according to your needs
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
        public async Task<Response> Assgin_PromotionPackage_to_List([FromForm] Listing listing)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                listing.CreatedBy = (int)claimDTO.UserId;

                var res = await _repository.Assgin_PromotionPackage_to_List(listing);

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


            if (!Directory.Exists(_galleryimagesPath))
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


                if (images.Count > 0)
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = images;

                }
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = images;
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


                if (images.Count > 0)
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = images;

                }
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = images;
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
                    string fullPath = Path.Combine(_imagesPath, fileName).Replace("\\", "/");


                    // Check if the file exists
                    if (System.IO.File.Exists(fullPath))
                    {
                        galleries.Add(new Gallery
                        {


                            Id = Path.GetFileNameWithoutExtension(fileName).GetHashCode(),
                            FileName = fileName,
                            FilePath = $"{BaseUrl}UploadImages/{fileName}?v={DateTime.UtcNow.Ticks}"
                        });
                    }
                   



                }

                if (galleries.Count > 0)
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = galleries;

                }
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = galleries;
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
                    string fullPath = Path.Combine(_breederlicensePath, fileName).Replace("\\", "/");


                    // Check if the file exists
                    if (System.IO.File.Exists(fullPath))
                    {
                        galleries.Add(new Gallery
                        {
                            Id = Path.GetFileNameWithoutExtension(fileName).GetHashCode(),
                            FileName = fileName,
                            FilePath = $"{BaseUrl}Profile/{fileName}?v={DateTime.UtcNow.Ticks}"
                        });
                    }
                }

                if (galleries.Count > 0 )
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = galleries;
                  
                }
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = galleries;
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

                if (galleries.Count > 0)
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = galleries;

                }
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = galleries;
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

        [HttpPost("replaceBreederLicenseFile")]
        public async Task<Response> BreederLicense(IFormFile file)
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
                    var filePath = Path.Combine("Profile", file.FileName);
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

                        //Gallery gallery = new Gallery();
                        //gallery.FileName = FileName;
                        //gallery.FilePath = filePath;
                        //gallery.CreatedBy = claimDTO.UserId;


                        //_repository.AddGallary(gallery);



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





        [HttpPost("replaceFileGallery")]
        public async Task<Response> replaceFileGallery(IFormFile file)
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
                    var filePath = Path.Combine("UploadGallery", file.FileName);
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



        [HttpPost("UploadNewGalleryOnly")]
        public async Task<Response> UploadNewGalleryOnly(List<IFormFile> files)
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
                        var filePath = Path.Combine("UploadGallery", FileName);
                        string FullFilePath = Path.Combine(_hostingEnvironment.WebRootPath, filePath);

                        if (System.IO.File.Exists(FullFilePath))
                        {

                            System.IO.File.Delete(FullFilePath);
                        }

                        using (var stream = new FileStream(FullFilePath, FileMode.Create))
                        {
                            await item.CopyToAsync(stream);
                        }

                       // save in database

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
                    gallery.FileName = item.Replace("UploadGallery/", string.Empty);
            
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

                        string FullFilePath = _galleryimagesPath + "\\"+item; 


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
       
        [HttpPost("DeleteSelectedMediaPath/{Path}")]
        public Response DeleteSelectedMediaPath(string Path)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                string[] filepaths = Path.Split(',');


                for (int i = 0; i < filepaths.Length; i++)
                {
                    filepaths[i] = filepaths[i].Replace("%2F", "\\");
                  

                }



                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.DeleteSelectedGalleryPath(Path);

                if (res)
                {

                    foreach (var item in filepaths)
                    {
                  
                        string FullFilePath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, item);
                     

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

        [HttpPost("DeleteSelectedPedigreePath/{Path}")]
        public Response DeleteSelectedPedigreePath(string Path)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                string[] filepaths = Path.Split(',');


                for (int i = 0; i < filepaths.Length; i++)
                {
                    filepaths[i] = filepaths[i].Replace("%2F", "\\");
                  

                }



                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

               
                    foreach (var item in filepaths)
                    {
                  
                        string FullFilePath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath,"UploadImages", item);
                     

                        // Check if the file already exists
                        if (System.IO.File.Exists(FullFilePath))
                        {
                            // Delete the existing file
                            System.IO.File.Delete(FullFilePath);
                        }


            


                    }

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = "Success";
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Gallery Deleted successfully!";

                
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
        
        [HttpPost("DeleteSelectedBreederLicensePath/{Path}")]
        public Response DeleteSelectedBreederLicensePath(string Path)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                string[] filepaths = Path.Split(',');


                for (int i = 0; i < filepaths.Length; i++)
                {
                    filepaths[i] = filepaths[i].Replace("%2F", "\\");
                  

                }



                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

               
                    foreach (var item in filepaths)
                    {
                  
                        string FullFilePath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath,"Profile", item);
                     

                        // Check if the file already exists
                        if (System.IO.File.Exists(FullFilePath))
                        {
                            // Delete the existing file
                            System.IO.File.Delete(FullFilePath);
                        }


            


                    }

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = "Success";
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Gallery Deleted successfully!";

                
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

                // Retrieve the listing to get file paths
                dynamic listingFiles = _repository.GetListingFilesById(Id); // Ensure you have this method implemented
                if (listingFiles == null) return CustomStatusResponse.GetResponse(404); // Listing not found

                // Delete listing from the database
                var res = _repository.DeleteListingById(Id);
                if (res > 0)
                {
                    // Delete the video and feature image
                    DeleteFileIfExists(listingFiles.PedigreeFilePath);
                    DeleteFileIfExists(listingFiles.VideoPath);
                    DeleteFileIfExists(listingFiles.FeatureImagePath);

                    // Handle gallery images
                    var galleryImages = string.IsNullOrEmpty(listingFiles.GallaryImagesPath)
                        ? new string[0]
                        : listingFiles.GallaryImagesPath.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var imagePath in galleryImages)
                    {
                        DeleteFileIfExists(imagePath.Trim());
                    }

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Listing has been deleted successfully!";
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






        [HttpPost("GetAllUsersForPricingPackages")]
        public Response GetAllUsersForPricingPackages()
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetAllUsersForPricingPackages();

                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Record Fetched successfully!";

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

        [HttpPost("getAllPackagestoAssgin/{userid}")]
        public Response getAllPackagestoAssgin(int userid)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.getAllPackagestoAssgin(userid);

                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Record Fetched successfully!";

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






        [HttpPost("GetUserpackagesAssigned")]
        public Response GetUserpackagesAssigned()
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetUserpackagesAssigned();

                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Record Fetched successfully!";

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
        public Response IsExpireCoupens(string Id)
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                   _stripeServices.DeleteCouponAsync(Id);

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = "success";
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Code has been Deleted successfully!";

              
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


        #region Blogs

        //Dashboard
        [HttpPost("GetAllAdminBLogs")]
        public Response GetAllAdminBLogs()
        {
            Response response = new Response();

            Register claimDTO = null;

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetAllAdminBlogs();

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
                    response.ResponseMsg = "Blog Added Successfuly!";
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

        // Method to handle file upload (feature image)
        private async Task<string> UploadFeatureImage(IFormFile featureImage)
        {
            if (featureImage == null || featureImage.Length == 0)
            {
                throw new Exception("Feature image is required.");
            }

            // Define a path to save the file temporarily or directly upload to cloud storage
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + featureImage.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await featureImage.CopyToAsync(stream);
            }

            // Return the relative path to the uploaded file
            return Path.Combine("uploads", uniqueFileName).Replace("\\", "/");
        }

        [HttpPost("UpdateBlog")]
        public async Task<Response> UpdateBlog([FromForm] Blog obj)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                obj.ModifiedBy = claimDTO.UserId;

                var res = await _repository.UpdateBlog(obj);

                if (res == null) return CustomStatusResponse.GetResponse(320);

                else
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Blog Updated Successfuly!";
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

        [HttpPost("BlogEditById/{Id}")]
        public Response BlogEditById(int Id)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _repository.BlogEditById(Id);

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

        [HttpPost("BlogDeleteById/{Id}")]
        public Response BlogDeleteById(int Id)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _repository.BlogDeleteById(Id);

                if (res > 0)

                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.ResponseMsg = "Delete Blog Successfuly!";
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = res;
                    return response;
                }

                else return CustomStatusResponse.GetResponse(320);


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

        [HttpPost("GetAllBlogCategories")]
        public Response GetAllBlogCategories()
        {
            Response response = new Response();
            Register claimDTO = null;

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null)
                    return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetAllBlogCategories();

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

        [HttpPost("GetAllDistinctTags")]
        public Response GetAllDistinctTags()
        {
            Response response = new Response();
            Register claimDTO = null;

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null)
                    return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetAllDistinctTags();

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

        [HttpPost("AddBlogCategory")]
        public async Task<Response> AddBlogCategory([FromBody] BlogCategories obj)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = await _repository.AddBlogCategory(obj);

                if (res == null) return CustomStatusResponse.GetResponse(320);

                else
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Category created successfuly!";
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

        [HttpPost("UpdateBlogCategory")]
        public async Task<Response> UpdateBlogCategory([FromBody] BlogCategories obj)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = await _repository.UpdateBlogCategory(obj);

                if (res == null) return CustomStatusResponse.GetResponse(320);

                else
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Category updated Successfuly!";
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

        [HttpPost("DeleteBlogCategory/{Id}")]
        public Response DeleteBlogCategory(int Id)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _repository.DeleteBlogCategory(Id);

                if (res > 0)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.ResponseMsg = "Delete Category Successfuly!";
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = res;
                    return response;
                }

                else return CustomStatusResponse.GetResponse(320);

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

        [HttpPost("SendReply")]
        public Response SendReply([FromBody] Reply obj)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.SendReply(obj);

                if (res == null) return CustomStatusResponse.GetResponse(320);

                else
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.ResponseMsg = "Reply Send Successfuly!";
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

        [HttpPost("GetAllCommentsByBlogId/{Id}")]
        public Response GetAllCommentsByBlogId(int Id)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _repository.GetAllCommentsByBlogId(Id);

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
       
        [HttpPost("GetAllUnreadComments")]
        public Response GetAllUnreadComments()
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _repository.GetAllUnreadComments();

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

        [HttpPost("DeleteCommentById/{Id}")]
        public Response DeleteCommentById(int Id)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _repository.DeleteCommentById(Id);

                if (res > 0)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.ResponseMsg = "Delete Comment Successfuly!";
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = res;
                    return response;
                }

                else return CustomStatusResponse.GetResponse(320);

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

        [HttpPost("GetAllReplyByCommentId/{Id}")]
        public Response GetAllReplyByCommentId(int Id)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _repository.GetAllReplyByCommentId(Id);

                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.ResponseMsg = "Delete Comment Successfuly!";
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = res;
                    return response;
                }

                else return CustomStatusResponse.GetResponse(320);

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

        [HttpPost("UpdateReply")]
        public Response UpdateReply([FromBody] Reply obj)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _repository.UpdateReply(obj);

                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.ResponseMsg = "Update Reply Successfuly!";
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = res;
                    return response;
                }

                else return CustomStatusResponse.GetResponse(320);

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

        [HttpPost("DeleteReplyId/{Id}")]
        public Response DeleteReplyId(int Id)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _repository.DeleteReplyId(Id);

                if (res > 0)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.ResponseMsg = "Delete Reply Successfuly!";
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = res;
                    return response;
                }

                else return CustomStatusResponse.GetResponse(320);

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

        [HttpPost("GetPromotionPackagesWithDays")]
        public Response GetPromotionPackagesWithDays()
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

            

                var res = _repository.GetPromotionPackagesWithDays();

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


        [HttpPost("AssignPromotionPackageToUser")]
        public Response AssignPromotionPackageToUser([FromBody]AssignPromotionPackage obj)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                obj.CreatedBy = claimDTO.UserId;

                var res = _repository.AssignPromotionPackageToUser(obj);

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
       
        
        
        [HttpPost("getAllUsersPromotionPackages")]
        public Response getAllUsersPromotionPackages()
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


            

                var res = _repository.getAllUsersPromotionPackages();

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



        [HttpPost("GetAdvertisementPackagesAndUsers")]
        public Response GetAdvertisementPackagesAndUsers()
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);




                var res = _repository.GetAdvertisementPackagesAndUsers();

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



        [HttpPost("AssignAdvertisementPackage")]
        public Response AssignAdvertisementPackage([FromBody] UserAdvertisementPackage obj)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                obj.CreatedBy = claimDTO.UserId;

                var res = _repository.AssignAdvertisementPackage(obj);

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


        [HttpPost("GetAssignedUserAdvertisements")]
        public Response GetAssignedUserAdvertisements()
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);



                var res = _repository.GetAssignedUserAdvertisements();

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

        //UploadImage
        [HttpPost("UploadBlogImage")]
        public async Task<Response> UploadBlogImage(IFormFile file)
        {
            Response response = new Response();
            Register claimDTO = null;

            try
            {
                // Validate the token
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null)
                {
                    response = CustomStatusResponse.GetResponse(401);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Unauthorized: Invalid token.";
                    return response;
                }

                // Check if the file is null or empty
                if (file == null || file.Length == 0)
                {
                    response = CustomStatusResponse.GetResponse(600);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "File is empty.";
                    response.Data = null;
                    return response;
                }

                // Define the path for saving the file
                var uploadsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "UploadBLogs");

                // Ensure the directory exists
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                // Generate a unique file name if the file already exists
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);
                var newFileName = file.FileName;
                var filePath = Path.Combine(uploadsFolderPath, newFileName);
                int count = 1;

                while (System.IO.File.Exists(filePath))
                {
                    newFileName = $"{fileName}_{count}{fileExtension}";
                    filePath = Path.Combine(uploadsFolderPath, newFileName);
                    count++;
                }

                // Save the file asynchronously
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await file.CopyToAsync(stream);
                }

                // Construct the URL
                var fileUrlPath = Path.Combine("UploadBLogs", newFileName).Replace(Path.DirectorySeparatorChar, '/');
                string imageUrl = $"{BaseUrl.TrimEnd('/')}/{fileUrlPath}";

                // Prepare success response
                response = CustomStatusResponse.GetResponse(200);
                response.Token = TokenManager.GenerateToken(claimDTO);
                response.Data = imageUrl;
                response.ResponseMsg = "File uploaded successfully!";
                return response;
            }
            catch (Exception ex)
            {
                // Log exception details here if needed
                response = CustomStatusResponse.GetResponse(500);
                response.Token = TokenManager.GenerateToken(claimDTO);
                response.ResponseMsg = $"An error occurred: {ex.Message}";
                return response;
            }
        }



        #endregion


        [HttpPost("GetAllListingFiltersDashboard")]
        public Response GetAllListingFiltersDashboard()
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _repository.GetAllListingFiltersDashboard();

                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Record Fetched successfully!";

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
