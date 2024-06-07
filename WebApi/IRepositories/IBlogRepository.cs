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
        Task<Blog> UpdateBlog(Blog obj);
        List<Blog> GetHomePageBlogs();
        List<Blog> GetAllBlogs();
        object GetAllBlogDetails(int Id);
        List<Comment> GetAllCommentsByBlogId(int Id);
        List<Reply> GetAllReplyByCommentId(int Id);
        Blog BlogEditById(int Id);
        int BlogDeleteById(int Id);
        int DeleteCommentById(int Id);
        int DeleteReplyId(int Id);
        Comment AddComment(Comment obj);
        Reply UpdateReply(Reply obj);
        Reply SendReply(Reply obj);


    }
}
