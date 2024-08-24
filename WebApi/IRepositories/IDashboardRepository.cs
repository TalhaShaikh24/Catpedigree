using ClassLibrary;

namespace WebApi.IRepositories
{
    public interface IDashboardRepository
    {
        dynamic GetJsonDataAsync(int userId);
        object GetAllDropdowns(int Id);

        object GetListing_ProdictionPackages(int id);

        public Listing Assgin_PromotionPackage_to_List(Listing listing);
        List<Listing> GetAllMyListings(int Id);
        List<Listing> GetAllListings();
        Listing GetListingDetailById(int Id);

        Task<Listing> UpdateListing(Listing obj);

        Task<Register> UpdateProfile(Register obj);

        Listing UpdateListingStatus(int Id, string Status, string Reason = ""); 

        int DeleteListingById(int Id);

        Register GetProfileDetailById(int Id);

        bool UploadSelectedGalleryPath(string Path);


        List<string> GetAllPedigreeGallary();
        List<string> GetAllBreederLicense();
        List<string> GetAllVideosGallery();
        public ListUsersDTO GetAllUsers();

        List<CouponCodes> GetCouponCodes();

        int ActiveDeactiveCouponCode(int id);
        int IsExpireCoupens(int id);

        public bool DeleteSelectedGalleryPath(string Path);


        public List<Gallery> GetAllGallary();


        public int AddGallary(Gallery obj);
        public int UpdateRoles(userRolesUpdate obj);
        public int UpdateActiveInActiveUser(int UserId);
        public int DeleteUser(int UserId);


        public List<Register> GetAllUsersForPricingPackages();


        public List<Package> getAllPackagestoAssgin(int userid);
        
        public List<GetUserpackagesAssigned> GetUserpackagesAssigned();

        

        #region Blogs
        List<Blog> GetAllAdminBlogs();
        Task<Blog> AddBlog(Blog obj);
        Task<Blog> UpdateBlog(Blog obj);
        Blog BlogEditById(int Id);
        int BlogDeleteById(int Id);
        List<BlogCategories> GetAllBlogCategories();
        List<Blog> GetAllDistinctTags();
        Task<BlogCategories> AddBlogCategory(BlogCategories obj);
        Task<BlogCategories> UpdateBlogCategory(BlogCategories obj);
        int DeleteBlogCategory(int Id);
        Reply SendReply(Reply obj);
        List<Comment> GetAllCommentsByBlogId(int Id);
        List<Blog> GetAllUnreadComments();
        int DeleteCommentById(int Id);
        List<Reply> GetAllReplyByCommentId(int Id);
        Reply UpdateReply(Reply obj);
        int DeleteReplyId(int Id);
        Comment AddComment(Comment obj);
        #endregion














    }
}
