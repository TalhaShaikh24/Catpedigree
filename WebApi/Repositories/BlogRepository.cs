
using ClassLibrary;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DBManager;
using WebApi.IRepositories;

namespace WebApi.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly IDapper _dapper;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public BlogRepository(IDapper dapper, IWebHostEnvironment hostingEnvironment)
        {
            _dapper = dapper;
            _hostingEnvironment = hostingEnvironment;
        }

		public List<BlogCategories> GetAllBlogCategories()
		{
			DynamicParameters parameters = new DynamicParameters();
			var data = _dapper.GetAll<BlogCategories>(@"[sp_GetAllBlogCategories]", parameters);

			return data;
		}
		public async Task<BlogCategories> AddBlogCategory(BlogCategories obj)
        {

          

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CategoryName", obj.CategoryName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Description", obj.Description, DbType.String, ParameterDirection.Input);
            parameters.Add("@CreatedOn", DateTime.Now, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<BlogCategories>(@"[sp_AddBlogCategory]", parameters);

            return data;
        }  
        public async Task<BlogCategories> UpdateBlogCategory(BlogCategories obj)
        {

          

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", obj.Id, DbType.String, ParameterDirection.Input);
            parameters.Add("@CategoryName", obj.CategoryName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Description", obj.Description, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<BlogCategories>(@"[sp_UpdateBlogCategory]", parameters);

            return data;
        }  
        public async Task<Blog> AddBlog(Blog obj)
        {

            if (obj.FeatureImage != null)
            {
                string FeatureImageName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.FeatureImage.FileName);
                string FeatureFilePath = Path.Combine("UploadBLogs", FeatureImageName);
                string FeatureFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, FeatureFilePath);

                using (var stream = new FileStream(FeatureFilePathDirectory, FileMode.Create))
                {
                    await obj.FeatureImage.CopyToAsync(stream);
                }
                obj.FeatureImagePath = FeatureFilePath;
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Title", obj.Title, DbType.String, ParameterDirection.Input);
            parameters.Add("@ShortDescription", obj.ShortDescription, DbType.String, ParameterDirection.Input);
            parameters.Add("@FeatureImagePath", obj.FeatureImagePath, DbType.String, ParameterDirection.Input);
            parameters.Add("@Content", obj.Content, DbType.String, ParameterDirection.Input);
            parameters.Add("@BlogCategoryId", obj.BlogCategoryId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@Tags", obj.Tags, DbType.String, ParameterDirection.Input);
            parameters.Add("@CreatedBy", obj.CreatedBy, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<Blog>(@"[sp_AddBlog]", parameters);

            return data;
        }  
        
        public async Task<Blog> UpdateBlog(Blog obj)
        {

            if (obj.FeatureImage != null)
            {
                string FeatureImageName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.FeatureImage.FileName);
                string FeatureFilePath = Path.Combine("UploadBLogs", FeatureImageName);
                string FeatureFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, FeatureFilePath);

                using (var stream = new FileStream(FeatureFilePathDirectory, FileMode.Create))
                {
                    await obj.FeatureImage.CopyToAsync(stream);
                }
                obj.FeatureImagePath = FeatureFilePath;
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@BlogID", obj.BlogID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Title", obj.Title, DbType.String, ParameterDirection.Input);
            parameters.Add("@ShortDescription", obj.ShortDescription, DbType.String, ParameterDirection.Input);
            parameters.Add("@FeatureImagePath", obj.FeatureImagePath, DbType.String, ParameterDirection.Input);
            parameters.Add("@Content", obj.Content, DbType.String, ParameterDirection.Input);
            parameters.Add("@BlogCategoryId", obj.BlogCategoryId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@Tags", obj.Tags, DbType.String, ParameterDirection.Input);
            parameters.Add("@ModifiedBy", obj.ModifiedBy, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.Get<Blog>(@"[sp_UpdateBlog]", parameters);

            return data;
        }

        public Comment AddComment(Comment obj)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@BlogID", obj.BlogID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CommentText", obj.CommentText, DbType.String, ParameterDirection.Input);
            parameters.Add("@UserId", obj.UserId, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.Get<Comment>(@"[sp_AddComment]", parameters);

            return data;
        }

        public int BlogDeleteById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Insert<int>(@"[sp_BlogDeleteById]", parameters);
            return data;
        }  
        
        
        public int DeleteCommentById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Insert<int>(@"[sp_DeleteCommentById]", parameters);
            return data;
        }

        public Blog BlogEditById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Insert<Blog>(@"[sp_BlogEditById]", parameters);
            return data;
        }

        public object GetAllBlogDetails(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id",Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.GetMultipleObjects(@"[sp_GetAllBlogDetails]", parameters,gr=>gr.Read<Blog>(),gr=>gr.Read<Comment>(), gr => gr.Read<Reply>());
            return data;
        }

        public List<Blog> GetHomePageBlogs()
        {
              DynamicParameters parameters = new DynamicParameters();
              var data = _dapper.GetAll<Blog>(@"[sp_GetHomePageBlogs]", parameters);

            return data;
        }
      
       
        
        public List<Blog> GetAllBlogs()
        {
              DynamicParameters parameters = new DynamicParameters();
              var data = _dapper.GetAll<Blog>(@"[sp_GetAllBlogs]", parameters);

            return data;
        }


       

        public BlogResult GetAllBlogsPagination(Blog blog)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PageNumber", blog.PageNumber);
            parameters.Add("@PageSize", blog.PageSize);
            parameters.Add("@BlogCategoryId", blog.BlogCategoryId == 0 ? null : blog.BlogCategoryId);
            parameters.Add("@Title", blog.Title == "" ? null : blog.Title);

            var data = _dapper.GetAll<Blog>(@"[dbo].[sp_GetAllBlogsPagination]", parameters).ToList();
            int totalCount = data.Any() ? data.First().TotalCount : 0;
            int fetchedCount = data.Count;

            return new BlogResult
            {
                Blogs = data,
                TotalCount = totalCount,
                FetchedCount = fetchedCount
            };
        }






        public List<Comment> GetAllCommentsByBlogId(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.GetAll<Comment>(@"[sp_GetAllCommentsByBlogId]", parameters);
            return data;
        }

        public Reply SendReply(Reply obj)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CommentId", obj.CommentId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@UserId", obj.UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ReplyText", obj.ReplyText, DbType.String, ParameterDirection.Input);
            var data = _dapper.Insert<Reply>(@"[sp_AddReply]", parameters);
            return data;
        }

        public List<Reply> GetAllReplyByCommentId(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.GetAll<Reply>(@"[sp_GetAllReplyByCommentId]", parameters);
            return data;
        }

        public Reply UpdateReply(Reply obj)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", obj.Id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@UserId", obj.UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CommentId", obj.CommentId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ReplyText", obj.ReplyText, DbType.String, ParameterDirection.Input);
            var data = _dapper.Update<Reply>(@"[sp_UpdateReply]", parameters);
            return data;
        }

        public int DeleteReplyId(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Insert<int>(@"[sp_DeleteReplyId]", parameters);
            return data;
        }
        public int DeleteBlogCategory(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Insert<int>(@"[sp_DeleteBlogCategoryById]", parameters);
            return data;
        }
    }

  }

