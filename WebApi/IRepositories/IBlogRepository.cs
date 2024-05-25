using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace WebApi.IRepositories
{
    public interface IBlogRepository
    {
        Task<Blog> AddBlog(Blog obj);

        List<Blog> GetAllBlogs();
        object GetAllBlogDetails(int Id);

        Comment AddComment(Comment obj);


    }
}
