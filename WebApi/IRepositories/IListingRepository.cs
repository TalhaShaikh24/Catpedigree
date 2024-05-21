using ClassLibrary;
using static WebApi.Repositories.ListingRepository;

namespace WebApi.IRepositories
{
    public interface IListingRepository
    {
        Task<Listing> AddListing(Listing obj);
        Task<Listing> UpdateListing(Listing obj);
        List<Listing> GetHomePageListings();
        ListingResult GetAllListingByFilters(ListingFilters obj);
		List<Listing> GetAllMyListings(int Id);
        Listing GetListingDetailById(int Id);
        List<Package> GetAllPackage();
        List<CatTypes> GetAllCatType();
        List<Category> GetAllCatCategory();

    }

}
