using ClassLibrary;

namespace WebApi.IRepositories
{
    public interface IAdvertisementServices
    {

        List<PaidAdvertisementsForView>  GetHomeAdvertisments(int Id);
        List<AdvertisementPackage> GetAdvertisementPackage();

        UserAdvertisementPackage BuyAdvertisementPackage(UserAdvertisementPackage obj);

        
        AdvertisementPackageAndUserPackages userAdvertisementPackages(int UserID);

        List<UserAdvertisementPackage> GetallUserAdvertisementForApprovals();


        UserAdvertisementPackage UpdateUserAdvertisementStatus(int Id, string Status);

        Task<UtilizePurchasedAdvertisementPackage> utilizePurchasedAdvertisementPackageAsync(UtilizePurchasedAdvertisementPackage obj);
    }
}
