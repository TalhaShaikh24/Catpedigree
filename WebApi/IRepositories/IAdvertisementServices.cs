using ClassLibrary;

namespace WebApi.IRepositories
{
    public interface IAdvertisementServices
    {
        List<AdvertisementPackage> GetAdvertisementPackage();


        UserAdvertisementPackage BuyAdvertisementPackage(UserAdvertisementPackage obj);

        AdvertisementPackageAndUserPackages userAdvertisementPackages(int UserID);

        List<UserAdvertisementPackage> GetallUserAdvertisementForApprovals();


        UserAdvertisementPackage UpdateUserAdvertisementStatus(int Id, string Status);

        Task<UtilizePurchasedAdvertisementPackage> utilizePurchasedAdvertisementPackageAsync(UtilizePurchasedAdvertisementPackage obj);
    }
}
