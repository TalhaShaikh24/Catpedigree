
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
            parameters.Add("@CreatedBy", obj.CreatedBy, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<Blog>(@"[sp_AddBlog]", parameters);

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

        public object GetAllBlogDetails(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id",Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.GetMultipleObjects(@"[sp_GetAllBlogDetails]", parameters,gr=>gr.Read<Blog>(),gr=>gr.Read<Comment>());
            return data;
        }

        public List<Blog> GetAllBlogs()
        {
              DynamicParameters parameters = new DynamicParameters();
              var data = _dapper.GetAll<Blog>(@"[sp_GetAllBlogs]", parameters);

            return data;
        }
    }

        
    }

