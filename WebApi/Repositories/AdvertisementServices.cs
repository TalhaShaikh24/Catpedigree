using ClassLibrary;
using Dapper;
using System.Data;
using System.Net.NetworkInformation;
using WebApi.DBManager;
using WebApi.IRepositories;

namespace WebApi.Repositories
{
    public class AdvertisementServices: IAdvertisementServices
    {
        private readonly IDapper _dapper;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AdvertisementServices(IDapper dapper, IWebHostEnvironment hostingEnvironment)
        {
            _dapper = dapper;
            _hostingEnvironment = hostingEnvironment;
        }

        public List<PaidAdvertisementsForView> GetHomeAdvertisments(int Id)
        {
            PaidAdvertisementsForView paidAdvertisements = new PaidAdvertisementsForView();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@AdvertismentId", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.GetAll<PaidAdvertisementsForView>(@"[dbo].[sp_GetAllAdvertismentsByAdvertismentId]", parameters);
            return data;
        }
        public List<PaidAdvertisementsForView> GetSidebarAdvertisments(int Id)
        {
            PaidAdvertisementsForView paidAdvertisements = new PaidAdvertisementsForView();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@AdvertismentId", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.GetAll<PaidAdvertisementsForView>(@"[dbo].[sp_GetAllSidebarAdvertismentsByAdvertismentId]", parameters);
            return data;
        }

        public UserAdvertisementPackage BuyAdvertisementPackage(UserAdvertisementPackage obj )
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", obj.UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@AdvertisementPackageID", obj.AdvertisementPackageID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CreatedBy", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@stripeSubscriptionId", obj.stripeSubscriptionId, DbType.String, ParameterDirection.Input);
            var data = _dapper.Get<UserAdvertisementPackage>(@"[dbo].[sp_BuyAdvertisementPackage]", parameters);
            return data;
        }    
        
        public async Task<UserAdvertisementPackage> BuyAdvertisementPackage(string UserId, string AdvertisementPackageID, string stripeSubscriptionId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID",  UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@AdvertisementPackageID", AdvertisementPackageID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CreatedBy", UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@stripeSubscriptionId", stripeSubscriptionId, DbType.String, ParameterDirection.Input);
            var data = await _dapper.GetAsync<UserAdvertisementPackage>(@"[dbo].[sp_BuyAdvertisementPackage]", parameters);
            return data;
        }

        public List<AdvertisementPackage> GetAdvertisementPackage()
        {
            DynamicParameters parameters = new DynamicParameters();
            
            var data = _dapper.GetAll<AdvertisementPackage>(@"[sp_GetAdvertisementPackages]", parameters);

            return data;


        }

        public List<UserAdvertisementPackage> GetallUserAdvertisementForApprovals()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<UserAdvertisementPackage>(@"[dbo].[sp_GetallUserAdvertisementForApprovals]", parameters);
            return data;
        }

        public UserAdvertisementPackage UpdateUserAdvertisementStatus(int Id, string Status)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Status", Status, DbType.String, ParameterDirection.Input);


            var data = _dapper.Update<UserAdvertisementPackage>(@"dbo.[sp_UpdateUserAdvertisementStatus]", parameters);

            return data;
        }

        public AdvertisementPackageAndUserPackages userAdvertisementPackages(int UserID)
        {
            AdvertisementPackageAndUserPackages packageAndUserPackages = new AdvertisementPackageAndUserPackages();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserId", UserID, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.GetMultipleObjects(@"[dbo].[sp_selectPackagewithAdd]", parameters, gr => gr.Read<UserAdvertisementPackage>(), gr => gr.Read<UserAdvertisementPackage>());
         
            packageAndUserPackages.dropdown = data.Item1.ToList();
            packageAndUserPackages.userAdvertisementPackages = data.Item2.ToList();



            return packageAndUserPackages;
        }

        public async Task<UtilizePurchasedAdvertisementPackage> utilizePurchasedAdvertisementPackageAsync(UtilizePurchasedAdvertisementPackage obj)
        {
            DynamicParameters parameters = new DynamicParameters();



            if (obj.AddFile != null)
            {
                string PedigreeFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.AddFile.FileName);
                string PedigreeFilePath = Path.Combine("UploadImages", PedigreeFileName);
                string PedigreeFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, PedigreeFilePath);

                using (var stream = new FileStream(PedigreeFilePathDirectory, FileMode.Create))
                {
                    await obj.AddFile.CopyToAsync(stream);
                }
                obj.FilePath = PedigreeFilePath;
            }

            parameters.Add("@UserID", obj.UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@UserAdvertisementPackageID", obj.UserAdvertisementPackageID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@FilePath", obj.FilePath, DbType.String, ParameterDirection.Input);


            var data = _dapper.Insert<UtilizePurchasedAdvertisementPackage>(@"dbo.[sp_UtilizePurchasedAdvertisementPackage]", parameters);

            return data;
        }
    }
}
