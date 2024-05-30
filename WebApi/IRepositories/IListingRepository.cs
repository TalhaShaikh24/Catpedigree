using ClassLibrary;
using static WebApi.Repositories.ListingRepository;

namespace WebApi.IRepositories
{
    public interface IListingRepository
    {
        Task<Listing> AddListing(Listing obj);
        List<Listing> GetHomePageListings();
        ListingResult GetAllListingByFilters(ListingFilters obj);

        object GetAllDropdowns(int Id);

        Listing IsViewPedigreeAllowed(Listing obj);

    }

}
