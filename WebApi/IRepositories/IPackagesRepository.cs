using ClassLibrary;

namespace WebApi.IRepositories
{
    public interface IPackagesRepository
    {
        List<Package> GetAllPackages();
        UserPackages BuyPackage(UserPackages obj);
        UserPackages AssignPackage(UserPackages obj);
        public Task<UserPackages> BuyPackageAsync(string UserID, string PackageID, string stripeSubscriptionId);



    }
}
