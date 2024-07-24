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
        List<Register> GetAllUsers();
    }
}
