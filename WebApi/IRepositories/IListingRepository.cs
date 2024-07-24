using ClassLibrary;
using static WebApi.Repositories.ListingRepository;

namespace WebApi.IRepositories
{
    public interface IListingRepository
    {
        Task<Listing> AddListing(Listing obj);
        List<Listing> GetHomePageListings();
        List<Listing> GetTopPageListings();
        List<Listing> GetVetRimmedPageListings();
        ListingResult GetAllListingByFilters(Listing obj);

        object GetAllDropdowns(int Id);

        Listing IsViewPedigreeAllowed(Listing obj);


        List<Package> CheckListingShowValidation(int userId, int id);
        List<CatTypes> GetAllCatType();



        int SelectPackageListingShowValidation(Listing obj);



        Category getCategoryByListingId(int id);
        List<Category> GetAllCategoriesByPackageId(int pkgId);






    }

}
