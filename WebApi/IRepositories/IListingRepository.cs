using ClassLibrary;
using static WebApi.Repositories.ListingRepository;

namespace WebApi.IRepositories
{
    public interface IListingRepository
    {
        Task<Listing> AddListing(Listing obj);
        List<Listing> GetHomePageListings();
        ListingResult GetAllListingByFilters(Listing obj);

        object GetAllDropdowns(int Id);

        Listing IsViewPedigreeAllowed(Listing obj);


        List<Package> CheckListingShowValidation(int userId);



        int SelectPackageListingShowValidation(Listing obj);



        Category getCategoryByListingId(int id);






    }

}
