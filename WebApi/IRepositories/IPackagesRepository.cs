using ClassLibrary;

namespace WebApi.IRepositories
{
    public interface IPackagesRepository
    {
        List<Package> GetAllPackages();
        UserPackages BuyPackage(UserPackages obj);
    }
}
