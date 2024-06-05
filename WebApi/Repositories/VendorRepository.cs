using ClassLibrary;
using Dapper;
using System.Data;
using WebApi.DBManager;
using WebApi.IRepositories;

namespace WebApi.Repositories
{
    public class VendorRepository : IVendorRepository
    {

        private readonly IDapper _dapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public VendorRepository(IDapper dapper, IWebHostEnvironment hostingEnvironment)
        {
            _dapper = dapper;
            _hostingEnvironment = hostingEnvironment;
        }

        public Vendor GetVednorDataAndList(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();

            Vendor vendor = new Vendor();


            
            parameters.Add("@UserId", Id, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.GetMultipleObjects("[usp_getVednorDetails_Listing]", parameters, gr => gr.Read<Register>(), gr => gr.Read<Listing>());


            vendor.vendorInfo = data.Item1.FirstOrDefault();
            vendor.listings = data.Item2.ToList();


            return vendor;
        }
    }
}
