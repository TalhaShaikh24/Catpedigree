using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Web;
using WebApp.HttpMethods;

namespace WebApp.Controllers
{
    public class BlogController : Controller
    {

        private string BaseUrl = "";
        public BlogController(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BlogDetails()
        {
            return View();
        }


        [HttpPost]
        public Task<object> GetHomePageBlogs()
        {
            string content = "";

            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Blog/GetHomePageBlogs", content, HttpContext);
        }

        [HttpPost]
        public Task<object> GetAllBlogs()
        {
            string content = "";

            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Blog/GetAllBlogs", content, HttpContext);
        }

        [HttpPost]
        public Task<object> GetAllBlogsPagination([FromBody] Blog blog)
        {
            string content = JsonConvert.SerializeObject(blog);

            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Blog/GetAllBlogsPagination", content, HttpContext);
        }

        [HttpPost]
        public Task<object> GetAllBlogCategories()
        {
            string content = "";

            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Blog/GetAllBlogCategories", content, HttpContext);
        }

        [HttpPost]
        public Task<object> GetAllBlogDetails(int Id)
        {
            string content = "";

            return HttpClientUtility.CustomHttpWithoutToken(BaseUrl, "api/Blog/GetAllBlogDetails/" + Id, content, HttpContext);
        }

        [HttpPost]
        public Task<object> AddComment([FromBody] Comment obj)
        {
            string content = JsonConvert.SerializeObject(obj);

            return HttpClientUtility.CustomHttp(BaseUrl, "api/Blog/AddComment", content, HttpContext);
        }

    }
}