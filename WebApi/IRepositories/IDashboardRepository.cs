using ClassLibrary;

namespace WebApi.IRepositories
{
    public interface IDashboardRepository
    {
        dynamic GetJsonDataAsync(int userId);
        object GetAllDropdowns();
        List<Listing> GetAllMyListings(int Id);
        List<Listing> GetAllListings();
        Listing GetListingDetailById(int Id);

        Task<Listing> UpdateListing(Listing obj);

        Task<Register> UpdateProfile(Register obj);

        Listing UpdateListingStatus(int Id, string Status);

        Register GetProfileDetailById(int Id);

    }
}
