using ClassLibrary;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Xml;
using WebApi.DBManager;
using WebApi.IRepositories;
using Formatting = Newtonsoft.Json.Formatting;

namespace WebApi.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IDapper _dapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public DashboardRepository(IDapper dapper, IWebHostEnvironment hostingEnvironment)
        {
            _dapper = dapper;
            _hostingEnvironment = hostingEnvironment;
        }

        public dynamic GetJsonDataAsync(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.Get<string>("[sp_GetAllDashboardData]", parameters);

            var jsonObject = JObject.Parse(data);

            return jsonObject["CombinedData"].ToString();
        }
    }
}
