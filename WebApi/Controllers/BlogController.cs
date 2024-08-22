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
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _repository;
        private FirestoreDb _firestoreDb;

        public BlogController(IBlogRepository repository)
        {
            _repository = repository;
            // Get the path to the service account JSON file in the bin directory
            //string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "serviceAccount.json");

            // Set the environment variable for Google Application Credentials
            //Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);

            // Initialize Firestore
            //_firestoreDb = FirestoreDb.Create("catpedigree-4a415");


            string keyToCheck = "2EO9F5LG3IXHL0GV"; // Replace with your actual key
            DateTime dateToCheck = DateTime.Parse("2024-07-10"); // Replace with the date to verify
        }

        
         


        [HttpPost("GetHomePageBlogs")]
        public Response GetHomePageBlogs()
        {
            Response response = new Response();

            try
            {

                var res = _repository.GetHomePageBlogs();

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

        [HttpPost("GetAllBlogsPagination")]
        public Response GetAllBlogsPagination([FromBody] Blog blog)
        {
            Response response = new Response();

            try
            {
                var result = _repository.GetAllBlogsPagination(blog);

                if (result == null || result.Blogs == null || !result.Blogs.Any())
                {
                    return CustomStatusResponse.GetResponse(320);
                }
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    int currentCount = (blog.PageNumber - 1) * blog.PageSize + result.FetchedCount;

                    response.Data = new
                    {
                        Blogs = result.Blogs,
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



        //Front Website
        [HttpPost("GetAllBlogCategories")]
        public Response GetAllBlogCategories()
        {
            Response response = new Response();


            try
            {

                var res = _repository.GetAllBlogCategories();

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
                response.ResponseMsg = ex.Message;

                return response;
            }
        }




        //Front Website
        [HttpPost("GetAllBlogCategoriesAndLatestBlog")]
        public Response GetAllBlogCategoriesAndLatestBlog()
        {
            Response response = new Response();


            try
            {

                var res = _repository.GetAllBlogCategoriesAndLatestBlog();

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
        





        #region Firebase

        //// GET: api/Blog/{id}
        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(string id)
        //{
        //    try
        //    {
        //        // Reference to the "Blogs" collection
        //        CollectionReference blogsRef = _firestoreDb.Collection("Blogs");

        //        // Get the blog document by document ID
        //        DocumentReference docRef = blogsRef.Document(id);
        //        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        //        // Check if the document exists
        //        if (snapshot.Exists)
        //        {
        //            // Convert the Firestore document to a Blog object
        //            Blog blog = snapshot.ConvertTo<Blog>();

        //            // Optionally, you may want to return the blog object directly
        //            return Ok(blog);
        //        }
        //        else
        //        {
        //            return NotFound($"Blog with ID '{id}' not found.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error retrieving blog: {ex.Message}");
        //    }
        //}

        //// POST: api/Blog
        //[HttpPost("AddBlog")]
        //[HttpPost]
        //public async Task<IActionResult> AddBlog([FromForm] BlogFormData formData)
        //{
        //    try
        //    {
        //        // Handle file upload (feature image)
        //        string featureImagePath = await UploadFeatureImage(formData.FeatureImage);

        //        // Create a new Blog object
        //        Blog newBlog = new Blog
        //        {
        //            BlogID = 1, // Replace with actual ID generation logic
        //            Title = formData.Title,
        //            ShortDescription = formData.ShortDescription,
        //            FeatureImagePath = featureImagePath, // Path to the uploaded image
        //            Username = "JohnDoe", // Replace with actual username logic
        //            Content = formData.Content,
        //            CommentsCount = 0, // Initial comments count
        //            CreatedOn = DateTime.UtcNow,
        //            CreatedBy = 1, // Replace with actual user ID
        //            ModifiedBy = null,
        //            ModifiedOn = null
        //        };

        //        // Reference to the "Blogs" collection
        //        CollectionReference blogsRef = _firestoreDb.Collection("Blogs");

        //        // Add the blog data to Firestore
        //        await blogsRef.AddAsync(newBlog);

        //        return Ok("Blog added successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error adding blog: {ex.Message}");
        //    }
        //}


        #endregion

    }
}
