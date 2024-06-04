using ClassLibrary;

namespace WebApi.IRepositories
{
    public interface IPromotionPackageRepository
    {
        List<PromotionPackages> GetAllPromotionPackages();

        PromotionPackages BuyPromotionPackage(PromotionPackages obj);

        List<PromotionCost> GetPromotionCost(int id);

    }
}
