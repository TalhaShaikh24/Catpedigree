using ClassLibrary;
using Dapper;
using System.Data;
using WebApi.DBManager;
using WebApi.IRepositories;

namespace WebApi.Repositories
{
    public class VideoPackagesRepository : IVideoPackagesRepository
    {
        private readonly IDapper _dapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public VideoPackagesRepository(IDapper dapper, IWebHostEnvironment hostingEnvironment)
        {
            _dapper = dapper;
            _hostingEnvironment = hostingEnvironment;
        }

        public int BuyPackage(int Id,int UserId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@UserId",UserId, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.Insert<int>(@"[sp_BuyVideoPackages]", parameters);

            return data;
        }

        public List<VideoPackages> GetAllVideoPackages()
        {

            DynamicParameters parameters = new DynamicParameters();
          

            var data = _dapper.GetAll<VideoPackages>(@"[sp_GetAllVideoPackages]", parameters);

            return data;
        }

        public bool VideoAvailablity(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.Insert<bool>(@"[sp_VideoAvailablity]", parameters);

            return data;
        }
    }
}
