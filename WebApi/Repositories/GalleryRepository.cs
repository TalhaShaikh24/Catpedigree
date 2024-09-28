using ClassLibrary;
using Dapper;
using System.Data;
using WebApi.DBManager;
using WebApi.IRepositories;

namespace WebApi.Repositories
{
    public class GalleryRepository : IGalleryRepository
    {
        private readonly IDapper _dapper;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public GalleryRepository(IDapper dapper, IWebHostEnvironment hostingEnvironment)
        {
            _dapper = dapper;
            _hostingEnvironment = hostingEnvironment;
        }

        public List<Gallery> GetAllGallery()
        {
            DynamicParameters parameters = new DynamicParameters();

            var data = _dapper.GetAll<Gallery>(@"[sp_GetAllgallary]", parameters);

            return data;
        }
    }

}
