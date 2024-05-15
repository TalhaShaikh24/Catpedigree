using ClassLibrary;

namespace WebApi.IRepositories
{
    public interface IListingRepository
    {
        Listing AddListing(Listing obj);
        Listing UpdateListing(Listing obj);
        List<Listing> GetAllMyListings();
        Listing GetListingDetailById(int Id);
        List<Package> GetAllPackage();
        List<CatTypes> GetAllCatType();
        List<Category> GetAllCatCategory();

    }

}
