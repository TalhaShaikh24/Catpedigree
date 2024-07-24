using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{

    // ViewModel to receive form data
    public class BlogCategories
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    public class BlogFormData
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public IFormFile FeatureImage { get; set; }
    }

    [FirestoreData]
    public class Blog
    {
        public int BlogID { get; set; }
        [FirestoreProperty]
        public string? Title { get; set; }
        [FirestoreProperty]
        public string? ShortDescription { get; set; }
        [FirestoreProperty]
        public string? FeatureImagePath { get; set; }
        [FirestoreProperty]
        public IFormFile? FeatureImage { get; set; }
        [FirestoreProperty]
        public string? Username { get; set; }
        [FirestoreProperty]
        public string? Content { get; set; }
        [FirestoreProperty]
        public int? CommentsCount { get; set; } 
        [FirestoreProperty]
        public int? BlogCategoryId { get; set; } 
        [FirestoreProperty]
        public string? Tags { get; set; }
        [FirestoreProperty]
        public DateTime CreatedOn { get; set; }
        [FirestoreProperty]
        public int? CreatedBy { get; set; }
        [FirestoreProperty]
        public int? ModifiedBy { get; set; }
        [FirestoreProperty]
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
