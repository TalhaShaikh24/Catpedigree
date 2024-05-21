using ClassLibrary;
using static WebApi.Repositories.ListingRepository;

namespace WebApi.IRepositories
{
    public interface IListingRepository
    {
        Task<Listing> AddListing(Listing obj);
        List<Listing> GetHomePageListings();
        ListingResult GetAllListingByFilters(ListingFilters obj);
        List<Package> GetAllPackage();
        List<CatTypes> GetAllCatType();
        List<Category> GetAllCatCategory();

    }

}
