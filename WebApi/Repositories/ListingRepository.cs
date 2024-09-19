using ClassLibrary;
using Dapper;
using Microsoft.IdentityModel.Tokens;
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
            // Function to replace spaces with underscores in file names
            string ReplaceSpaces(string input) => input.Replace(' ', '_');

            if (obj.PedigreeFile != null)
            {
                string PedigreeFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + ReplaceSpaces(Path.GetFileName(obj.PedigreeFile.FileName));
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
                string FeatureFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + ReplaceSpaces(Path.GetFileName(obj.FeatureImageFile.FileName));
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
                    string GalleryFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + ReplaceSpaces(Path.GetFileName(item.FileName));
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
                string VideoFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + ReplaceSpaces(Path.GetFileName(obj.VideoFile.FileName));
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
            parameters.Add("Weigth", obj.Weigth, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("Price", obj.Price, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("Color", obj.Color, DbType.String, ParameterDirection.Input);
            parameters.Add("IsVaccinated", obj.IsVaccinated, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("IsCastration", obj.IsCastration, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("IsSterilization", obj.IsSterilization, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("VideoPath", obj.VideoPath, DbType.String, ParameterDirection.Input);
            parameters.Add("FeatureImage", obj.FeatureImagePath, DbType.String, ParameterDirection.Input);
            parameters.Add("GallaryImages", obj.GallaryImagesPath, DbType.String, ParameterDirection.Input);
            parameters.Add("PedigreeFilePath", obj.PedigreeFilePath, DbType.String, ParameterDirection.Input);
            parameters.Add("@Age", string.IsNullOrEmpty(obj.Age) ? (object)null : obj.Age, DbType.String, ParameterDirection.Input);
            parameters.Add("@CategoryId", obj.CategoryId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("PackageId", obj.PackageId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("IsActive", false, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("CreatedBy", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            parameters.Add("PromotionPackageId", obj.PromotionPackageId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("CatteryName", obj.CatteryName, DbType.String, ParameterDirection.Input);
            parameters.Add("PhoneCode", obj.PhoneCode, DbType.String, ParameterDirection.Input);
            parameters.Add("CountryDialCode", obj.CountryDialCode, DbType.String, ParameterDirection.Input);
            parameters.Add("latitude", obj.latitude, DbType.String, ParameterDirection.Input);
            parameters.Add("longitude", obj.longitude, DbType.String, ParameterDirection.Input);

            var data = _dapper.Insert<Listing>(@"[dbo].[sp_AddListing]", parameters);
            return data;
        }


        public class ListingResult
		{
			public List<Listing> Listings { get; set; }
			public int TotalCount { get; set; }
			public int FetchedCount { get; set; }
		}

		public ListingResult GetAllListingByFilters(Listing obj)
		{
			DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PageNumber", obj.PageNumber);
			parameters.Add("@PageSize", obj.PageSize);
			parameters.Add("@Keyword", obj.Keyword == "" ? null : obj.Keyword);
			parameters.Add("@CategoryId", obj.CategoryId==0?null:obj.CategoryId);
			parameters.Add("@Location", obj.Location == "" ? null : obj.Location);
			parameters.Add("@State", obj.State == "" ? null : obj.State);
			parameters.Add("@City", obj.City == "" ? null : obj.City);
			parameters.Add("@ZipCode", obj.ZipCode == "" ? null : obj.ZipCode);
			parameters.Add("@TypeOfCat", obj.TypeOfCat == 0 ? null : obj.TypeOfCat);

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
		public List<Listing> GetTopPageListings()
        {
            DynamicParameters parameters = new DynamicParameters();

            var data = _dapper.GetAll<Listing>(@"[dbo].[sp_GetTopHomePageListings]", parameters);
            return data;
        }
		public List<Listing> GetVetRimmedPageListings()
        {
            DynamicParameters parameters = new DynamicParameters();

            var data = _dapper.GetAll<Listing>(@"[dbo].[sp_GetVetandRimmedHomePageListings]", parameters);
            return data;
        }
		public Listing RequestListingPrice(int listingID, int userID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserID", userID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("ListingID", listingID, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.Get<Listing>(@"[dbo].[sp_RequestListingPrice]", parameters);
            return data;
        }
      public List<CatTypes> GetAllCatType()
        {
            DynamicParameters parameters = new DynamicParameters();

            var data = _dapper.GetAll<CatTypes>(@"[dbo].[sp_GetAllCatType]", parameters);
            return data;
        }
      
        public object GetAllDropdowns(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
           
            var data = _dapper.GetMultipleObjects("[sp_GetAllDropdowns]", parameters, gr => gr.Read<Category>(), gr => gr.Read<CatTypes>(), gr => gr.Read<Package>(), gr => gr.Read<PromotionPackages>());

            return data;
        }

        

        public void IncrementViewCount(int listingId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ListingId", listingId, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.Get<Listing>(@"[sp_IncrementViewCount]", parameters);

        }
        
        public Listing IsViewPedigreeAllowed(Listing obj)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@UserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ListingId", obj.Id, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.Get<Listing>(@"[sp_IsViewPedigreeAllowed]", parameters);


            return data;
        }

        public List<Package> CheckListingShowValidation(int userId, int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@UserID", userId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ListingID", id, DbType.Int32, ParameterDirection.Input);
     
            var data = _dapper.GetAll<Package>(@"[sp_CheckListingShowValidation]", parameters);

            return data;
        }

        public int SelectPackageListingShowValidation(Listing obj)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@UserID", obj.UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@PackageID", obj.PackageId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CreatedBy", obj.UserId, DbType.Int32, ParameterDirection.Input);
            
            var data = _dapper.Insert<int>(@"[sp_AddListingShowValidation]", parameters);

            return data;
        }

        public Category getCategoryByListingId(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@ListId", id, DbType.Int32, ParameterDirection.Input);
            
            var data = _dapper.Get<Category>(@"[sp_getcategorynamebyListingId]", parameters);

            return data;
        }
        public List<Category> GetAllCategoriesByPackageId(int pkgId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@PackageId", pkgId, DbType.Int32, ParameterDirection.Input);
            
            var data = _dapper.GetAll<Category>(@"[sp_GetAllCategoriesByPackageId]", parameters);

            return data;
        }
    }
}
