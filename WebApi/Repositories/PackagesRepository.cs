using ClassLibrary;
using Dapper;
using System.Data;
using WebApi.DBManager;
using WebApi.IRepositories;

namespace WebApi.Repositories
{
    public class PackagesRepository:IPackagesRepository
    {
        private readonly IDapper _dapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public PackagesRepository(IDapper dapper, IWebHostEnvironment hostingEnvironment)
        {
            _dapper = dapper;
            _hostingEnvironment = hostingEnvironment;
        }

        public List<Package> GetAllPackages()
        {

            DynamicParameters parameters = new DynamicParameters();

            var data = _dapper.GetAll<Package>(@"[sp_GetAllPricingPlans]", parameters);

            return data;
        }
        public  UserPackages BuyPackage(UserPackages obj)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", obj.UserID, DbType.String, ParameterDirection.Input);
            parameters.Add("@PackageID", obj.PackageID, DbType.String, ParameterDirection.Input);
            parameters.Add("@SubscriptionDate", DateTime.Now, DbType.String, ParameterDirection.Input);
            // Calculate expiry date by adding 365 days to the subscription date
            DateTime? expiryDate = DateTime.Now.AddDays(365);
            parameters.Add("@ExpiryDate", expiryDate, DbType.String, ParameterDirection.Input);
            parameters.Add("@RemainingListings", obj.RemainingListings, DbType.String, ParameterDirection.Input);
            parameters.Add("@IsActive", true, DbType.String, ParameterDirection.Input);
            parameters.Add("@IsExpired", false, DbType.String, ParameterDirection.Input);
            parameters.Add("@CreatedBy", obj.UserID, DbType.String, ParameterDirection.Input);
            parameters.Add("@stripeSubscriptionId", obj.stripeSubscriptionId, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<UserPackages>(@"[sp_BuyPackage]", parameters);

            return data;
        }


        public async Task<UserPackages> BuyPackageAsync(string UserID, string PackageID, string stripeSubscriptionId)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", UserID, DbType.String, ParameterDirection.Input);
            parameters.Add("@PackageID", PackageID, DbType.String, ParameterDirection.Input);
            parameters.Add("@SubscriptionDate", DateTime.Now, DbType.String, ParameterDirection.Input);
            // Calculate expiry date by adding 365 days to the subscription date
            DateTime? expiryDate = DateTime.Now.AddDays(365);
            parameters.Add("@ExpiryDate", expiryDate, DbType.String, ParameterDirection.Input);
            parameters.Add("@IsActive", true, DbType.String, ParameterDirection.Input);
            parameters.Add("@IsExpired", false, DbType.String, ParameterDirection.Input);
            parameters.Add("@CreatedBy", UserID, DbType.String, ParameterDirection.Input);
            parameters.Add("@stripeSubscriptionId", stripeSubscriptionId, DbType.String, ParameterDirection.Input);

            var data = await  _dapper.GetAsync<UserPackages>(@"[sp_BuyPackage]", parameters);

            return data;
        }

        public UserPackages AssignPackage(UserPackages obj)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", obj.UserID, DbType.String, ParameterDirection.Input);
            parameters.Add("@PackageID", obj.PackageID, DbType.String, ParameterDirection.Input);
            parameters.Add("@SubscriptionDate", DateTime.Now, DbType.String, ParameterDirection.Input);
            // Calculate expiry date by adding 365 days to the subscription date
            DateTime? expiryDate = DateTime.Now.AddDays(365);
            parameters.Add("@ExpiryDate", expiryDate, DbType.String, ParameterDirection.Input);
            parameters.Add("@RemainingListings", obj.RemainingListings, DbType.String, ParameterDirection.Input);
            parameters.Add("@IsActive", true, DbType.String, ParameterDirection.Input);
            parameters.Add("@IsExpired", false, DbType.String, ParameterDirection.Input);
            parameters.Add("@CreatedBy", obj.CreatedBy, DbType.String, ParameterDirection.Input);
            parameters.Add("@stripeSubscriptionId", obj.stripeSubscriptionId, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<UserPackages>(@"[sp_BuyPackage]", parameters);

            return data;
        }
    }
}
