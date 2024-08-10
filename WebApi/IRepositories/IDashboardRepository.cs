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

        Listing UpdateListingStatus(int Id, string Status); 

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
    }
}
