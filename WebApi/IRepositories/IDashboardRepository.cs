using ClassLibrary;

namespace WebApi.IRepositories
{
    public interface IDashboardRepository
    {
        dynamic GetJsonDataAsync(int userId);
        object GetAllDropdowns(int Id);

        List<Register> GetAllUsersDropdown();
        List<DTORoleScreenPermission> GetAllRoleScreenPermissions();
        List<Roles> GetAllRoleDropdownByUserId(int UserId);

        

        List<PermissionScreens> GetAllPermissionScreensDropdown();

        List<RoleScreenPermission> AddRoleScreenPermission(ScreenPermission obj);

        int DeletePermission(int Id);

        object GetListing_ProdictionPackages(int id);

        public Task<Listing> Assgin_PromotionPackage_to_List(Listing listing);
        List<Listing> GetAllMyListings(int Id);
        List<Listing> GetAllListings();
        Listing GetListingFilesById(int Id);
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
        public List<string> GetAllMedia();


        public int AddGallary(Gallery obj);
        public int AddMedia(Gallery obj);
        public int UpdateRoles(userRolesUpdate obj);
        public int UpdateActiveInActiveUser(int UserId);
        public int DeleteUser(int UserId);


        public List<Register> GetAllUsersForPricingPackages();


        public List<Package> getAllPackagestoAssgin(int userid);
        
        public List<GetUserpackagesAssigned> GetUserpackagesAssigned();

        public AssignPromotionPackageDTO GetPromotionPackagesWithDays();


        public PromotionPackages AssignPromotionPackageToUser(AssignPromotionPackage obj);

        public List<GetAllUsersPromotionPackage> getAllUsersPromotionPackages();

        public AssignAdvertisementPackagesDTO GetAdvertisementPackagesAndUsers();


        public UserAdvertisementPackage AssignAdvertisementPackage(UserAdvertisementPackage obj);


        public List<AssignedUserAdvertisementsList> GetAssignedUserAdvertisements();


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


        object GetAllListingFiltersDashboard();




        object GetAllRoles();

         Roles CreateRole(Roles obj);

        Task<Register> AddUser(Register formData);



        Task<Show> AddShow(Show obj);
        Task<Show> UpdateShow(Show obj);

        List<Show> ShowList();

         Show GetShowbyID(int id);

         void ShowDelete(int id);
    }
}
