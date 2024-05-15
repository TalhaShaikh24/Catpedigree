using ClassLibrary;
using Dapper;
using Microsoft.SqlServer.Server;
using System.Data;
using WebApi.DBManager;
using WebApi.IRepositories;

namespace WebApi.Repositories
{
    public class ListingRepository : IListingRepository
    {

        private readonly IDapper _dapper;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ListingRepository(IDapper dapper, IWebHostEnvironment hostingEnvironment) {

            _dapper = dapper;
            _hostingEnvironment = hostingEnvironment;
        }
        public Listing AddListing(Listing obj)
        {

            if (obj.PedigreeFile != null)
            {
                string PedigreeFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.PedigreeFile.FileName);
                string PedigreeFilePath = Path.Combine("UploadImages", PedigreeFileName);
                string PedigreeFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, PedigreeFilePath);

                using (var Stream = new FileStream(PedigreeFilePathDirectory, FileMode.Create))
                {
                    obj.PedigreeFile.CopyToAsync(Stream);

                    obj.PedigreeFilePath = "~/"+PedigreeFilePath;

				}
            }

            string FeatureFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.FeatureImageFile.FileName);
            string FeatureFilePath = Path.Combine("UploadImages", FeatureFileName);
            string FeatureFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, FeatureFilePath);

            using (var Stream = new FileStream(FeatureFilePathDirectory, FileMode.Create))
            {
                obj.FeatureImageFile.CopyToAsync(Stream);
				obj.FeatureImagePath = "~/" +FeatureFilePath;
			}

            if (obj.GalleryImageFiles.Count > 0)
            {
                List<string> GallaryPath = new List<string>();

                foreach (var item in obj.GalleryImageFiles)
                {

                    string GalleryFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(item.FileName);
                    string GalleryFilePath = Path.Combine("UploadImages", GalleryFileName);
                    string UploadImagesFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, GalleryFilePath);

                    using (var Stream = new FileStream(UploadImagesFilePathDirectory, FileMode.Create))
                    {

                        item.CopyToAsync(Stream);
                        GallaryPath.Add("~/" + GalleryFilePath);
                    }
                }

                 obj.GallaryImagesPath = String.Join(",", GallaryPath);

            }

			string VideoFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.VideoFile.FileName);
            string VideoFilePath = Path.Combine("UploadVideos", VideoFileName);
            string VideoFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, VideoFilePath);

            using (var Stream = new FileStream(VideoFilePathDirectory, FileMode.Create))
            {
                obj.VideoFile.CopyToAsync(Stream);
				obj.VideoPath = "~/" + VideoFilePath;
			}

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Title", obj.Title, DbType.String, ParameterDirection.Input);
            parameters.Add("Location", obj.Location, DbType.String, ParameterDirection.Input);
            parameters.Add("State", obj.State, DbType.String, ParameterDirection.Input);
            parameters.Add("City", obj.City, DbType.String, ParameterDirection.Input);
            parameters.Add("IsBreerderLicenseUpload", obj.IsBreerderLicenseUpload, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("Phone", obj.Phone, DbType.String, ParameterDirection.Input);
            parameters.Add("Email", obj.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("BreerderName", obj.BreerderName, DbType.String, ParameterDirection.Input);
            parameters.Add("TypeOfCat", obj.TypeOfCat, DbType.Int32, ParameterDirection.Input);
            parameters.Add("ZoologicalNumber", obj.ZoologicalNumber, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@Gender", obj.Gender, DbType.String, ParameterDirection.Input);
            parameters.Add("Description", obj.Description, DbType.String, ParameterDirection.Input);
            parameters.Add("VideoPath", obj.VideoPath, DbType.String, ParameterDirection.Input);
            parameters.Add("FeatureImage", obj.FeatureImagePath, DbType.String, ParameterDirection.Input);
            parameters.Add("GallaryImages", obj.GallaryImagesPath, DbType.String, ParameterDirection.Input);
            parameters.Add("@Age", obj.Age, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CategoryId", obj.CategoryId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("PackageId", obj.PackageId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("IsActive", false, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("CreatedBy", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Insert<Listing>(@"[dbo].[sp_AddListing]", parameters);
            return data;
        }

        public Listing UpdateListing(Listing obj)
        {

            if (obj.PedigreeFile != null)
            {
                string PedigreeFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.PedigreeFile.FileName);
                string PedigreeFilePath = Path.Combine("UploadImages", PedigreeFileName);
                string PedigreeFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, PedigreeFilePath);

                using (var Stream = new FileStream(PedigreeFilePathDirectory, FileMode.Create))
                {
                    obj.PedigreeFile.CopyToAsync(Stream);

                    obj.PedigreeFilePath = "~/" + PedigreeFilePath;

                }
            }

            string FeatureFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.FeatureImageFile.FileName);
            string FeatureFilePath = Path.Combine("UploadImages", FeatureFileName);
            string FeatureFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, FeatureFilePath);

            using (var Stream = new FileStream(FeatureFilePathDirectory, FileMode.Create))
            {
                obj.FeatureImageFile.CopyToAsync(Stream);
                obj.FeatureImagePath = "~/" + FeatureFilePath;
            }

            if (obj.GalleryImageFiles.Count > 0)
            {
                List<string> GallaryPath = new List<string>();

                foreach (var item in obj.GalleryImageFiles)
                {

                    string GalleryFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(item.FileName);
                    string GalleryFilePath = Path.Combine("UploadImages", GalleryFileName);
                    string UploadImagesFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, GalleryFilePath);

                    using (var Stream = new FileStream(UploadImagesFilePathDirectory, FileMode.Create))
                    {

                        item.CopyToAsync(Stream);
                        GallaryPath.Add("~/" + GalleryFilePath);
                    }
                }

                obj.GallaryImagesPath = String.Join(",", GallaryPath);

            }

            string VideoFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.VideoFile.FileName);
            string VideoFilePath = Path.Combine("UploadVideos", VideoFileName);
            string VideoFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, VideoFilePath);

            using (var Stream = new FileStream(VideoFilePathDirectory, FileMode.Create))
            {
                obj.VideoFile.CopyToAsync(Stream);
                obj.VideoPath = "~/" + VideoFilePath;
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", 1, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Title", obj.Title, DbType.String, ParameterDirection.Input);
            parameters.Add("@Location", obj.Location, DbType.String, ParameterDirection.Input);
            parameters.Add("@State", obj.State, DbType.String, ParameterDirection.Input);
            parameters.Add("@City", obj.City, DbType.String, ParameterDirection.Input);
            parameters.Add("@IsBreerderLicenseUpload", obj.IsBreerderLicenseUpload, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@Phone", obj.Phone, DbType.String, ParameterDirection.Input);
            parameters.Add("@Email", obj.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("@BreerderName", obj.BreerderName, DbType.String, ParameterDirection.Input);
            parameters.Add("@TypeOfCat", obj.TypeOfCat, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ZoologicalNumber", obj.ZoologicalNumber, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@Gender", obj.Gender, DbType.String, ParameterDirection.Input);
            parameters.Add("@Description", obj.Description, DbType.String, ParameterDirection.Input);
            parameters.Add("@VideoPath", obj.VideoPath, DbType.String, ParameterDirection.Input);
            parameters.Add("@FeatureImage", obj.FeatureImagePath, DbType.String, ParameterDirection.Input);
            parameters.Add("@GallaryImages", obj.GallaryImagesPath, DbType.String, ParameterDirection.Input);
            parameters.Add("@Age", obj.Age, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CategoryId", obj.CategoryId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@PackageId", obj.PackageId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@IsActive", false, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@ModifiedBy", obj.ModifiedBy, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Insert<Listing>(@"[dbo].[sp_UpdateMyListing]", parameters);
            return data;
        }
        public List<Listing> GetAllMyListings()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<Listing>(@"[dbo].[sp_GetAllMyListing]", parameters);
            return data;
        }
        public List<Category> GetAllCatCategory()
        {

            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<Category>(@"[dbo].[sp_GetAllCategories]", parameters);
            return data;
        }

        public List<CatTypes> GetAllCatType()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<CatTypes>(@"[dbo].[sp_GetAllCatTypes]", parameters);
            return data;
        }

        public List<Package> GetAllPackage()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<Package>(@"[dbo].[sp_GetAllPackage]", parameters);
            return data;
        }

        public Listing GetListingDetailById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id",Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Get<Listing>(@"[dbo].[sp_GetListingDetailById]", parameters);
            return data;
        }
    }
}
