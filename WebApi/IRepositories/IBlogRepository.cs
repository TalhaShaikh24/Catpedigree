using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static WebApi.Repositories.BlogRepository;


namespace WebApi.IRepositories
{
    public interface IBlogRepository
    {
        Task<BlogCategories> AddBlogCategory(BlogCategories obj);
        Task<BlogCategories> UpdateBlogCategory(BlogCategories obj);
        Task<Blog> AddBlog(Blog obj);
        Task<Blog> UpdateBlog(Blog obj);
        List<Blog> GetHomePageBlogs();
        List<BlogCategories> GetAllBlogCategories();
       
        List<Blog> GetAllBlogs();
        List<Blog> GetAllAdminBlogs();
        BlogResult GetAllBlogsPagination(Blog blog);
        object GetAllBlogDetails(int Id);
        
        
        Blog BlogEditById(int Id);
        int BlogDeleteById(int Id);
        int DeleteCommentById(int Id);
        
        int DeleteBlogCategory(int Id);
        Comment AddComment(Comment obj);
        GetBlogCategoriesAndLatestBlogDTO GetAllBlogCategoriesAndLatestBlog();




    }
}
