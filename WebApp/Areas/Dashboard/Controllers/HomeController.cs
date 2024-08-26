using ClassLibrary;
using ClassLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.HttpMethods;

namespace WebApp.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class HomeController : Controller
    {

        private string BaseUrl = "";
        public HomeController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }
        public IActionResult Index()
        {
            return View();
        }

		
		[Route("Dashboard/Users")]
        public IActionResult Users()
        {
            return View();
        }

        
        [Route("Dashboard/MyListing")]
        public IActionResult MyListing()
        {
            return View();
        }

        [Route("Dashboard/ListingApproval")]
        public IActionResult ListingApproval()
        {
            return View();
        }



        [Route("Dashboard/AdvertisementApprovals")]
        public IActionResult AdvertisementApprovals()
        {
            return View();
        }


        [Route("Dashboard/Gallary")]
        public IActionResult Gallary()
        {
            return View();
        }

        
        [Route("Dashboard/Media")]
        public IActionResult Media()
        {
            return View();
        }







        public IActionResult AddCategory()
        {
            return View();
        }

        [Route("Dashboard/Profile")]
        public IActionResult Profile()
        {
            return View();
        }

        [Route("Dashboard/Blog")]
        public IActionResult Blog()
        {
            return View();
        }
        
        [Route("Dashboard/BlogCategories")]
        public IActionResult BlogCategories()
        {
            return View();
        }

        [Route("Dashboard/Blogs")]
        public IActionResult Blogs()
        {
            return View();
        }


        [Route("Dashboard/EditBlog")]
        public IActionResult EditBlog()
        {
            return View();
        }


        [Route("Dashboard/Comments")]
        public IActionResult Comments()
        {
            return View();
        }

        [Route("Dashboard/Replies")]
        public IActionResult Replies()
        {
            return View();
        }


        [Route("Dashboard/PromotionPackages")]
        public IActionResult PromotionPackages()
        {
            return View();
        }

        [Route("Dashboard/Advertisement")]
        public IActionResult Advertisement()
        {
            return View();
        }

        [HttpGet("Dashboard/Advertisementpackages")]
        public IActionResult Advertisementpackages()
        {
            return View();
        }

        [HttpGet("Dashboard/PedigreeGallery")]
        public IActionResult PedigreeGallery()
        {
            return View();
        }

        
        [HttpGet("Dashboard/BreederLicense")]
        public IActionResult BreederLicense()
        {
            return View();
        }
        
        [HttpGet("Dashboard/VideoGallery")]
        public IActionResult VideoGallery()
        {
            return View();
        }


        [HttpGet("Dashboard/Coupons")]
        public IActionResult Coupons()
        {
            return View();
        }


        [HttpGet("Dashboard/CouponsList")]
        public IActionResult CouponsList()
        {
            return View();
        }

        
        [HttpGet("Dashboard/UserEditProfile")]
        public IActionResult UserEditProfile()
        {

            
            return View();
        }




        [HttpGet("Dashboard/AssignPricingPackage")]
        public IActionResult AssignPricingPackage()
        {


            return View();
        }





        [HttpGet("Dashboard/AssignUserPromotionPackagesToUser")]
        public IActionResult AssignUserPromotionPackagesToUser()
        {
            return View();

        }

        [HttpGet("Dashboard/AssignAdvertisement")]
        public IActionResult AssignAdvertisement()
        {
            return View();

        }




        [HttpPost]
        [Route("Dashboard/GetPedigreeGallery")]
        public Task<object> GetPedigreeGallery()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/PedigreeGallery", "", HttpContext);

        }
        
        [HttpPost]
        [Route("Dashboard/GetAllGallary")]
        public Task<object> GetAllGallary()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllGallery", "", HttpContext);

        } 

        [HttpPost]
        [Route("Dashboard/GetAllMedia")]
        public Task<object> GetAllMedia()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllMedia", "", HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/GetAllBreederLicense")]
        public Task<object> GetAllBreederLicense()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllBreederLicense", "", HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/GetAllVideosGallery")]
        public Task<object> GetAllVideosGallery()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllVideosGallery", "", HttpContext);

        }


        [HttpPost]
        [Route("Dashboard/replaceFile")]
        public Task<object> replaceFile(IFormFile file)
        {

            return HttpClientUtility.CustomHttpreplaceFileDashboard(BaseUrl, "api/Dashboard/replaceFile", file, HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/replaceFileGallery")]
        public Task<object> replaceFileGallery(IFormFile file)
        {

            return HttpClientUtility.CustomHttpreplaceFileDashboard(BaseUrl, "api/Dashboard/replaceFileGallery", file, HttpContext);

        }

      

        [HttpPost]
        [Route("Dashboard/UploadNewGallery")]
        public Task<object> UploadNewGallery(List<IFormFile> files)
        {

            return HttpClientUtility.CustomHttSingleFileDashboard(BaseUrl, "api/Dashboard/UploadNewGallery", files, HttpContext);

        }
              

        [HttpPost]
        [Route("Dashboard/UploadNewGalleryOnly")]
        public Task<object> UploadNewGalleryOnly(List<IFormFile> files)
        {

            return HttpClientUtility.CustomHttSingleFileDashboard(BaseUrl, "api/Dashboard/UploadNewGalleryOnly", files, HttpContext);

        }







        [HttpPost]
        [Route("Dashboard/GetAllDashboard")]
        public Task<object> GetAllDashboard()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllDashboard", "", HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/GetAllDropdowns")]
        public Task<object> GetAllDropdowns()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllDropdowns", "", HttpContext);

        }


        [HttpPost]
        [Route("Dashboard/GetAllMyListings")]
        public Task<object> GetAllMyListings()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllMyListings", "", HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/UploadSelectedGalleryPath")]
        public Task<object> UploadSelectedGalleryPath([FromBody]string Path)
        {
            var content = JsonConvert.SerializeObject(Path);


            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/UploadSelectedGalleryPath", content, HttpContext);

        }



        [HttpPost]
        [Route("Dashboard/DeleteSelectedGalleryPath/{Path}")]
        public Task<object> DeleteSelectedGalleryPath(string Path)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/DeleteSelectedGalleryPath/" + Path, "", HttpContext);

        }

          [HttpPost]
        [Route("Dashboard/DeleteSelectedMediaPath/{Path}")]
        public Task<object> DeleteSelectedMediaPath(string Path)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/DeleteSelectedMediaPath/" + Path, "", HttpContext);

        }


        [HttpPost]
        [Route("Dashboard/GetListingDetailById/{Id}")]
        public Task<object> GetListingDetailById(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetListingDetailById/" + Id, "", HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/GetProfileDetailById")]
        public Task<object> GetProfileDetailById()
        {
            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetProfileDetailById", "", HttpContext);
        }


        [HttpPost]
        [Route("Dashboard/UpdateProfile")]
        public Task<object> UpdateProfile([FromForm] Register obj)
        {

            return HttpClientUtility.CustomHttpIfileDashboard(BaseUrl, "api/Dashboard/UpdateProfile", obj, HttpContext);

        }
        
        [HttpPost]
        [Route("Dashboard/UpdateUserProfile")]
        public Task<object> UpdateUserProfile([FromForm] Register obj)
        {

            return HttpClientUtility.CustomHttpIfileDashboardUserUpdate(BaseUrl, "api/Dashboard/UpdateUserProfile", obj, HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/UpdateListing")]
        public Task<object> UpdateListing([FromForm] Listing obj)
        {

            return HttpClientUtility.CustomHttpListing(BaseUrl, "api/Dashboard/UpdateListing", obj, HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/UpdateListingStatus")]
        public Task<object> UpdateListingStatus(int Id, string Status, string Reason = "")
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/UpdateListingStatus?Id=" + Id + "&Status=" + Status + "&Reason=" + Reason, "", HttpContext);
        }


        [HttpPost]
        [Route("Dashboard/UserAdvertisementStatus")]
        public Task<object> UserAdvertisementStatus(int Id, string Status)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Advertisement/UpdateUserAdvertisementStatus?Id=" + Id + "&Status=" + Status, "", HttpContext);
        }


        [HttpPost]
        [Route("Dashboard/GetAllListings")]
        public Task<object> GetAllListings()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllListings","", HttpContext);
        }

        [HttpPost]
        [Route("Dashboard/GetallUserAdvertisementForApprovals")]
        public Task<object> GetallUserAdvertisementForApprovals()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Advertisement/GetallUserAdvertisementForApprovals", "", HttpContext);
        }


        [HttpPost]
        [Route("Dashboard/DeleteListingById")]
        public Task<object> DeleteListingById(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/DeleteListingById/" + Id, "", HttpContext);

        }


        [HttpPost]
        [Route("Dashboard/GetListing_ProdictionPackages")]
        public Task<object> GetListing_ProdictionPackages()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetListing_ProdictionPackages", "", HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/Assgin_PromotionPackage_to_List")]
        public Task<object> Assgin_PromotionPackage_to_List([FromBody] Listing obj)
        {
            var content = JsonConvert.SerializeObject(obj);

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/Assgin_PromotionPackage_to_List", content, HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/GetAdvertisementPackage/{currency}")]
        public Task<object> GetAdvertisementPackage(string currency)
        {
            if (currency==null)
            {
                currency = "EUR";

            }
            var content = "";

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Advertisement/GetAdvertisementPackage/"+ currency, content, HttpContext);

        }


        [HttpPost]
        [Route("Dashboard/BuyAdvertisementPackage")]
        public Task<object> BuyAdvertisementPackage([FromBody] UserAdvertisementPackage userAdvertisementPackage)
        {
            var content = JsonConvert.SerializeObject(userAdvertisementPackage);

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Advertisement/BuyAdvertisementPackage", content, HttpContext);

        }


        [HttpPost]
        [Route("Dashboard/UserAdvertisementPackages")]
        public Task<object> UserAdvertisementPackages()
        {
            var content = "";

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Advertisement/UserAdvertisementPackages", content, HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/UtilizePurchasedAdvertisementPackage")]
        public Task<object> UtilizePurchasedAdvertisementPackage([FromForm] UtilizePurchasedAdvertisementPackage obj)
        {;

            return HttpClientUtility.CustomHttpUtilizeAdvertisementPackage(BaseUrl, "api/Advertisement/UtilizePurchasedAdvertisementPackage", obj, HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/GetAllUsers")]
        public Task<object> GetAllUsers()
        {

            string content = "";

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllUsers", content, HttpContext);

        }

        [HttpPost("Dashboard/AddCouponsCodes")]
        public Task<object> AddCouponsCodes([FromBody] CouponCodes obj)
        {
            string content = JsonConvert.SerializeObject(obj);
            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/AddCouponsCodes", content, HttpContext);
        }


        [HttpPost("Dashboard/UpdateUserRoles")]
        public Task<object> UpdateUserRoles([FromBody] userRolesUpdate obj)
        {
            string content = JsonConvert.SerializeObject(obj);
            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/UpdateUserRoles", content, HttpContext);
        }

        [HttpPost("Dashboard/GetCouponCodes")]
        public Task<object> GetCouponCodes()
        {
            string content = "";
            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetCouponCodes", content, HttpContext);
        }
        
        [HttpPost]
        [Route("Dashboard/IsExpireCoupens")]

        public Task<object> IsExpireCoupens(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/IsExpireCoupens/" + Id, "", HttpContext);

        } 

        [HttpPost]
        [Route("Dashboard/ActiveDeactiveCode")]
        public Task<object> ActiveDeactiveCode(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/ActiveDeactiveCode/" + Id, "", HttpContext);

        }

        [HttpPost("Dashboard/UserEdit/{id}")]
        public  Task<object> UserEdit(int id)
        {


            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Account/UserInfo/" + id, "", HttpContext);

        

        }
        
        [HttpPost("Dashboard/UpdateActiveInActiveUser/{id}")]
        public  Task<object> UpdateActiveInActiveUser(int id)
        {


            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/UpdateActiveInActiveUser/" + id, "", HttpContext);

        

        } 
        
        [HttpPost("Dashboard/DeleteUser/{id}")]
        public  Task<object> DeleteUser(int id)
        {


            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/DeleteUser/" + id, "", HttpContext);

        

        }

        [HttpPost("Dashboard/GetAllUsersForPricingPackages")]
        public  Task<object> GetAllUsersForPricingPackages()
        {


            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllUsersForPricingPackages", "", HttpContext);

        

        }

        [HttpPost("Dashboard/getAllPackagestoAssgin/{userid}")]
        public  Task<object> getAllPackagestoAssgin(int userid)
        {


            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/getAllPackagestoAssgin/"+ userid, "", HttpContext);

        

        }
       
        [HttpPost("Dashboard/AssignPackage")]
        public  Task<object> AssignPackage([FromBody] UserPackages obj)
        {

            var content = JsonConvert.SerializeObject(obj);
            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Packages/AssignPackage", content, HttpContext);

        

        }

        [HttpPost("Dashboard/GetUserpackagesAssigned")]
        public Task<object> GetUserpackagesAssigned()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetUserpackagesAssigned", "", HttpContext);



        }


        
        [HttpPost("Dashboard/GetPromotionPackagesWithDaysRes")]
        public Task<object> GetPromotionPackagesWithDaysRes()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetPromotionPackagesWithDays", "", HttpContext);



        }  
        
        
        [HttpPost("Dashboard/AssignPromotionPackageToUser")]
        public Task<object> AssignPromotionPackageToUser([FromBody] AssignPromotionPackage obj)
        {


            var content = JsonConvert.SerializeObject(obj);
            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/AssignPromotionPackageToUser", content, HttpContext);



        }



        [HttpPost("Dashboard/getAllUsersPromotionPackages")]
        public Task<object> getAllUsersPromotionPackages()
        {


            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/getAllUsersPromotionPackages", "", HttpContext);



        }


        [HttpPost("Dashboard/GetAdvertisementPackagesAndUsers")]
        public Task<object> GetAdvertisementPackagesAndUsers()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAdvertisementPackagesAndUsers", "", HttpContext);



        }


        [HttpPost("Dashboard/AssignAdvertisementPackage")]
        public Task<object> AssignAdvertisementPackage([FromBody] UserAdvertisementPackage obj)
        {


            var content = JsonConvert.SerializeObject(obj);
            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/AssignAdvertisementPackage", content, HttpContext);



        }


        [HttpPost("Dashboard/GetAssignedUserAdvertisements")]
        public Task<object> GetAssignedUserAdvertisements()
        {


            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAssignedUserAdvertisements", "", HttpContext);



        }







        #region Blogs

        [HttpPost]
        [Route("Dashboard/GetAllAdminBLogs")]
        public Task<object> GetAllAdminBLogs()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllAdminBLogs", "", HttpContext);
        }

        [HttpPost]
        [Route("Dashboard/AddBlog")]
        public Task<object> AddBlog([FromForm] Blog obj)
        {

            return HttpClientUtility.CustomHttpBlog(BaseUrl, "api/Dashboard/AddBlog", obj, HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/UpdateBlog")]
        public Task<object> UpdateBlog([FromForm] Blog obj)
        {

            return HttpClientUtility.CustomHttpBlog(BaseUrl, "api/Dashboard/UpdateBlog", obj, HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/BlogEditById")]
        public Task<object> BlogEditById(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/BlogEditById/" + Id, "", HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/BlogDeleteById")]
        public Task<object> BlogDeleteById(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/BlogDeleteById/" + Id, "", HttpContext);

        }

        
        [HttpPost]
        [Route("Dashboard/GetAllBlogCategories")]
        public Task<object> GetAllBlogCategories()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllBlogCategories", "", HttpContext);
        }

        [HttpPost]
        [Route("Dashboard/GetAllDistinctTags")]
        public Task<object> GetAllDistinctTags()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllDistinctTags", "", HttpContext);
        }
        
        [HttpPost]
        [Route("Dashboard/GetAllUnreadComments")]
        public Task<object> GetAllUnreadComments()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllUnreadComments", "", HttpContext);
        }

        [HttpPost]
        [Route("Dashboard/AddBlogCategory")]
        public Task<object> AddBlogCategory([FromBody] BlogCategories obj)
        {
            var content = JsonConvert.SerializeObject(obj);
            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/AddBlogCategory", content, HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/UpdateBlogCategory")]
        public Task<object> UpdateBlogCategory([FromBody] BlogCategories obj)
        {
            var content = JsonConvert.SerializeObject(obj);
            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/UpdateBlogCategory", content, HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/DeleteBlogCategory")]
        public Task<object> DeleteBlogCategory(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/DeleteBlogCategory/" + Id, "", HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/SendReply")]
        public Task<object> SendReply([FromBody] Reply obj)
        {
            var content = JsonConvert.SerializeObject(obj);

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/SendReply", content, HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/GetAllCommentsByBlogId")]
        public Task<object> GetAllCommentsByBlogId(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllCommentsByBlogId/" + Id, "", HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/DeleteCommentById")]
        public Task<object> DeleteCommentById(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/DeleteCommentById/" + Id, "", HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/GetAllReplyByCommentId")]
        public Task<object> GetAllReplyByCommentId(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllReplyByCommentId/" + Id, "", HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/UpdateReply")]
        public Task<object> UpdateReply([FromBody] Reply obj)
        {
            var content = JsonConvert.SerializeObject(obj);

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/UpdateReply", content, HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/DeleteReplyId")]
        public Task<object> DeleteReplyId(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/DeleteReplyId/" + Id, "", HttpContext);

        }
        #endregion



        [HttpPost]
        [Route("Dashboard/GetAllListingFiltersDashboard")]
        public Task<object> GetAllListingFiltersDashboard()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllListingFiltersDashboard", "", HttpContext);

        }
    }
}
