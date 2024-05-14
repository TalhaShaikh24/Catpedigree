using ClassLibrary;

namespace WebApi.IRepositories
{
    public interface IListingRepository
    {
        Listing AddListing(Listing obj);
        List<Package> GetAllPackage();
        List<CatTypes> GetAllCatType();
        List<Category> GetAllCatCategory();

    }

}
