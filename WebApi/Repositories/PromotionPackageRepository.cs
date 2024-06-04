using ClassLibrary;
using Dapper;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json.Nodes;
using WebApi.DBManager;
using WebApi.IRepositories;

namespace WebApi.Repositories
{
    public class PromotionPackageRepository:IPromotionPackageRepository
    {
        private readonly IDapper _dapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public PromotionPackageRepository(IDapper dapper, IWebHostEnvironment hostingEnvironment)
        {
            _dapper = dapper;
            _hostingEnvironment = hostingEnvironment;
        }



     

        public List<PromotionPackages> GetAllPromotionPackages()
        {

            List<PromotionPackages> promotionPackages = new List<PromotionPackages>();

            PromotionPackages promotion = new PromotionPackages();

            DynamicParameters parameters = new DynamicParameters();


            var data = _dapper.GetAll<PromotionPackages>(@"[sp_GetAllPromotionPackages]", parameters);


            List<PromotionCost> promotionCosts = new List<PromotionCost>();


            foreach (var item in data)
            {
                promotionCosts = JsonConvert.DeserializeObject<List<PromotionCost>>(item.Costs);

                promotion = item;

                promotion.promotionCosts=promotionCosts;



                promotionPackages.Add(promotion);

            }

            


            return promotionPackages;
        }

        public PromotionPackages BuyPromotionPackage(PromotionPackages obj)
        {


            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", obj.UserID, DbType.String, ParameterDirection.Input);
            parameters.Add("@PromotionPackagesID", obj.PromotionPackagesID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@SubscriptionDate", DateTime.Now, DbType.String, ParameterDirection.Input);
            // Calculate expiry date by adding 365 days to the subscription date
            DateTime? expiryDate = DateTime.Now.AddDays(365);
            parameters.Add("@ExpiryDate", expiryDate, DbType.String, ParameterDirection.Input);
            parameters.Add("@Days", obj.Days, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@IsActive", true, DbType.String, ParameterDirection.Input);
            parameters.Add("@IsExpired", false, DbType.String, ParameterDirection.Input);
            parameters.Add("@CreatedBy", obj.UserID, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<PromotionPackages>(@"[sp_BuyPromotionPackage]", parameters);

            return data;


        }

        public List<PromotionCost> GetPromotionCost(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", id, DbType.Int32, ParameterDirection.Input);
           
            var data = _dapper.GetAll<PromotionCost>(@"[usp_GetPromotionPackages_Cost]", parameters);

            return data;
        }
    }
}
