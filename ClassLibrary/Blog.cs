using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Blog
    {
        public int BlogID { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? FeatureImagePath { get; set; }
        public IFormFile? FeatureImage { get; set; }
        public string? Username { get; set; }
        public string? Content { get; set; }
        public int? CommentsCount { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public int BlogID { get; set; }
        public int UserId { get; set; }
        public string? CommentText { get; set; }
        public string? UserName { get; set; }
        public DateTime? CommentDate { get; set; }
    }

    public class Reply
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string? ReplyText { get; set; }
        public DateTime? ReplyDate { get; set; }
    }

}
