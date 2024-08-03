using ClassLibrary;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Data.Common;
using System.Reflection.Metadata;
using WebApi.IRepositories;
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
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "serviceAccount.json");

            // Set the environment variable for Google Application Credentials
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);

            // Initialize Firestore
            _firestoreDb = FirestoreDb.Create("catpedigree-4a415");


            string keyToCheck = "2EO9F5LG3IXHL0GV"; // Replace with your actual key
            DateTime dateToCheck = DateTime.Parse("2024-07-10"); // Replace with the date to verify
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

        [HttpPost("GetAllBlogCategories")]
        public Response GetAllBlogCategories()
        {
            Response response = new Response();

            Register claimDTO = null;

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

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

                if (res!=null)
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
