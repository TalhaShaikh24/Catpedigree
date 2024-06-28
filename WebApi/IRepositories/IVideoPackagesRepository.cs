using ClassLibrary;

namespace WebApi.IRepositories
{
    public interface IVideoPackagesRepository
    {
        List<VideoPackages> GetAllVideoPackages();
        int BuyPackage(int Id,int UserId,string? stripeSubscriptionId);

        bool VideoAvailablity(int Id);
    }
}
