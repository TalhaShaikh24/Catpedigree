using ClassLibrary;

namespace WebApi.IRepositories
{
    public interface IListingRepository
    {
        Task<Listing> AddListing(Listing obj);
        Task<Listing> UpdateListing(Listing obj);
        List<Listing> GetHomePageListings();
        List<Listing> GetAllMyListings(int Id);
        Listing GetListingDetailById(int Id);
        List<Package> GetAllPackage();
        List<CatTypes> GetAllCatType();
        List<Category> GetAllCatCategory();

    }

}
