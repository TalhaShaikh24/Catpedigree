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
        public async Task<Listing> AddListing(Listing obj)
        {
            if (obj.PedigreeFile != null)
            {
                string PedigreeFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.PedigreeFile.FileName);
                string PedigreeFilePath = Path.Combine("UploadImages", PedigreeFileName);
                string PedigreeFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, PedigreeFilePath);

                using (var stream = new FileStream(PedigreeFilePathDirectory, FileMode.Create))
                {
                    await obj.PedigreeFile.CopyToAsync(stream);
                }
                obj.PedigreeFilePath = PedigreeFilePath;
            }

            if (obj.FeatureImageFile != null)
            {
                string FeatureFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.FeatureImageFile.FileName);
                string FeatureFilePath = Path.Combine("UploadImages", FeatureFileName);
                string FeatureFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, FeatureFilePath);

                using (var stream = new FileStream(FeatureFilePathDirectory, FileMode.Create))
                {
                    await obj.FeatureImageFile.CopyToAsync(stream);
                }
                obj.FeatureImagePath = FeatureFilePath;
            }

            if (obj.GalleryImageFiles != null)
            {
                List<string> GalleryPath = new List<string>();

                foreach (var item in obj.GalleryImageFiles)
                {
                    string GalleryFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(item.FileName);
                    string GalleryFilePath = Path.Combine("UploadImages", GalleryFileName);
                    string UploadImagesFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, GalleryFilePath);

                    using (var stream = new FileStream(UploadImagesFilePathDirectory, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                    GalleryPath.Add(GalleryFilePath);
                }

                obj.GallaryImagesPath = string.Join(",", GalleryPath);
            }

            if (obj.VideoFile != null)
            {
                string VideoFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.VideoFile.FileName);
                string VideoFilePath = Path.Combine("UploadVideos", VideoFileName);
                string VideoFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, VideoFilePath);

                using (var stream = new FileStream(VideoFilePathDirectory, FileMode.Create))
                {
                    await obj.VideoFile.CopyToAsync(stream);
                }
                obj.VideoPath = VideoFilePath;
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
            parameters.Add("IsActive", true, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("CreatedBy", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Insert<Listing>(@"[dbo].[sp_AddListing]", parameters);
            return data;
        }

		public class ListingResult
		{
			public List<Listing> Listings { get; set; }
			public int TotalCount { get; set; }
			public int FetchedCount { get; set; }
		}

		public ListingResult GetAllListingByFilters(ListingFilters obj)
		{
			DynamicParameters parameters = new DynamicParameters();
			parameters.Add("@PageNumber", obj.PageNumber);
			parameters.Add("@PageSize", obj.PageSize);

			var data = _dapper.GetAll<Listing>(@"[dbo].[sp_GetAllListingByFilters]", parameters).ToList();
			int totalCount = data.Any() ? data.First().TotalCount : 0;
			int fetchedCount = data.Count;

			return new ListingResult
			{
				Listings = data,
				TotalCount = totalCount,
				FetchedCount = fetchedCount
			};
		}


		public List<Listing> GetHomePageListings()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<Listing>(@"[dbo].[sp_GetHomePageListings]", parameters);
            return data;
        }
      
        public object GetAllDropdowns(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
           
            var data = _dapper.GetMultipleObjects("[sp_GetAllDropdowns]", parameters, gr => gr.Read<Category>(), gr => gr.Read<CatTypes>(), gr => gr.Read<Package>());

            return data;
        }


    }
}
