using ClassLibrary;
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
        [Route("Dashboard/UpdateListing")]
        public Task<object> UpdateListing([FromForm] Listing obj)
        {

            return HttpClientUtility.CustomHttpListing(BaseUrl, "api/Dashboard/UpdateListing", obj, HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/UpdateListingStatus")]
        public Task<object> UpdateListingStatus(int Id, string Status)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/UpdateListingStatus?Id=" + Id + "&Status=" + Status, "", HttpContext);
        }


        [HttpPost]
        [Route("Dashboard/GetAllListings")]
        public Task<object> GetAllListings()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Dashboard/GetAllListings","", HttpContext);
        }



        [HttpPost]
        [Route("Dashboard/GetAllAdminBLogs")]
        public Task<object> GetAllAdminBLogs()
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Blog/GetAllAdminBLogs", "", HttpContext);
        }

        [HttpPost]
        [Route("Dashboard/AddBlog")]
        public Task<object> AddBlog([FromForm] Blog obj)
        {
           
            return HttpClientUtility.CustomHttpBlog(BaseUrl, "api/Blog/AddBlog",obj,HttpContext);

        }       
        
        
        [HttpPost]
        [Route("Dashboard/UpdateBlog")]
        public Task<object> UpdateBlog([FromForm] Blog obj)
        {
           
            return HttpClientUtility.CustomHttpBlog(BaseUrl, "api/Blog/UpdateBlog", obj,HttpContext);

        }       
        
        
        [HttpPost]
        [Route("Dashboard/SendReply")]
        public Task<object> SendReply([FromBody] Reply obj)
        {
            var content = JsonConvert.SerializeObject(obj);

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Blog/SendReply", content, HttpContext);

        }


        [HttpPost]
        [Route("Dashboard/GetAllCommentsByBlogId")]
        public Task<object> GetAllCommentsByBlogId(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Blog/GetAllCommentsByBlogId/"+Id,"",HttpContext);

        }


        [HttpPost]
        [Route("Dashboard/BlogEditById")]
        public Task<object> BlogEditById(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Blog/BlogEditById/" + Id, "", HttpContext);

        }


        [HttpPost]
        [Route("Dashboard/BlogDeleteById")]
        public Task<object> BlogDeleteById(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Blog/BlogDeleteById/" + Id, "", HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/DeleteCommentById")]
        public Task<object> DeleteCommentById(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Blog/DeleteCommentById/" + Id, "", HttpContext);

        }


        [HttpPost]
        [Route("Dashboard/GetAllReplyByCommentId")]
        public Task<object> GetAllReplyByCommentId(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Blog/GetAllReplyByCommentId/"+Id, "", HttpContext);

        }

        [HttpPost]
        [Route("Dashboard/UpdateReply")]
        public Task<object> UpdateReply([FromBody] Reply obj)
        {
            var content = JsonConvert.SerializeObject(obj);

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Blog/UpdateReply",content, HttpContext);

        }


        [HttpPost]
        [Route("Dashboard/DeleteReplyId")]
        public Task<object> DeleteReplyId(int Id)
        {

            return HttpClientUtility.CustomHttpDashboard(BaseUrl, "api/Blog/DeleteReplyId/"+Id, "", HttpContext);

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
    }
}
