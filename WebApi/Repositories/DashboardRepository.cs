using ClassLibrary;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Reflection;
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

        public object GetAllDropdowns(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.GetMultipleObjects("[sp_GetAllDropdowns]", parameters,gr=>gr.Read<Category>(), gr => gr.Read<CatTypes>(), gr => gr.Read<Package>(), gr => gr.Read<PromotionPackages>());

            return data;
        }

        public dynamic GetJsonDataAsync(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.Get<string>("[sp_GetAllDashboardData]", parameters);

            var jsonObject = JObject.Parse(data);

            return jsonObject["CombinedData"].ToString();
        }

        public List<Listing> GetAllMyListings(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.GetAll<Listing>(@"[dbo].[sp_GetAllMyListing]", parameters);
            return data;
        }
        public Listing GetListingDetailById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Get<Listing>(@"[dbo].[sp_GetListingDetailById]", parameters);
            return data;
        }

        public async Task<Listing> UpdateListing(Listing obj)
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
            parameters.Add("@Id", obj.Id, DbType.Int32, ParameterDirection.Input);
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
           // parameters.Add("Weigth", obj.Weigth, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("Price", obj.Price, DbType.Decimal, ParameterDirection.Input);
           // parameters.Add("Color", obj.Color, DbType.String, ParameterDirection.Input);
           parameters.Add("IsVaccinated", obj.IsVaccinated, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("VideoPath", obj.VideoPath, DbType.String, ParameterDirection.Input);
            parameters.Add("FeatureImage", obj.FeatureImagePath, DbType.String, ParameterDirection.Input);
            parameters.Add("GallaryImages", obj.GallaryImagesPath, DbType.String, ParameterDirection.Input);
            parameters.Add("PedigreeFilePath", obj.PedigreeFilePath, DbType.String, ParameterDirection.Input);
            parameters.Add("@Age", obj.Age, DbType.String, ParameterDirection.Input);
            parameters.Add("@CategoryId", obj.CategoryId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@PackageId", obj.PackageId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@IsActive", false, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@ModifiedBy", obj.ModifiedBy, DbType.Int32, ParameterDirection.Input);
            
            parameters.Add("IsCastration", obj.IsCastration, DbType.Boolean, ParameterDirection.Input);

            parameters.Add("IsSterilization", obj.IsSterilization, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("CatteryName", obj.CatteryName, DbType.String, ParameterDirection.Input);


            var data = _dapper.Insert<Listing>(@"[dbo].[sp_UpdateMyListing]", parameters);
            return data;
        }


        public async Task<Register> UpdateProfile(Register formData)
        {
            string folder = "Profile"; // Relative path

            if (formData.ProfilePic != null)
            {
                //For Profile Picture
                string profileFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(formData.ProfilePic.FileName);
                string profileFilePath = Path.Combine(folder, profileFileName);
                string absoluteProfileFilePath = Path.Combine(_hostingEnvironment.WebRootPath, profileFilePath);

                using (var profileStream = new FileStream(absoluteProfileFilePath, FileMode.Create))
                {
                    await formData.ProfilePic.CopyToAsync(profileStream);
                }

                formData.ProfilePicPath = profileFilePath;


            }

            if (formData.BreederLicense != null)
            {
                //For Breeder License
                string licenseFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(formData.BreederLicense.FileName);
                string licenseFilePath = Path.Combine(folder, licenseFileName);
                string absoluteLicenseFilePath = Path.Combine(_hostingEnvironment.WebRootPath, licenseFilePath);

                using (var licenseStream = new FileStream(absoluteLicenseFilePath, FileMode.Create))
                {
                    await formData.BreederLicense.CopyToAsync(licenseStream);
                }

                formData.BreederLicensePath = licenseFilePath;
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", formData.UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Firstname", formData.Firstname, DbType.String, ParameterDirection.Input);
            parameters.Add("@Lastname", formData.Lastname, DbType.String, ParameterDirection.Input);
            parameters.Add("@Username", formData.Username, DbType.String, ParameterDirection.Input);
            parameters.Add("@Email", formData.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("@Password", formData.Password, DbType.String, ParameterDirection.Input);
            parameters.Add("@ContactNo", formData.ContactNo, DbType.String, ParameterDirection.Input);
            parameters.Add("@Address", formData.Address, DbType.String, ParameterDirection.Input);
            parameters.Add("@ProfileInfo", formData.ProfileInfo, DbType.String, ParameterDirection.Input);
            parameters.Add("@ProfilePicPath", formData.ProfilePicPath, DbType.String, ParameterDirection.Input);
            parameters.Add("@BreederLicensePath", formData.BreederLicensePath, DbType.String, ParameterDirection.Input);
            parameters.Add("@ZoologicalNumber", formData.ZoologicalNumber, DbType.String, ParameterDirection.Input);



            parameters.Add("@Country", formData.Country, DbType.String, ParameterDirection.Input);
            parameters.Add("@City", formData.City, DbType.String, ParameterDirection.Input);
            parameters.Add("@province", formData.Province, DbType.String, ParameterDirection.Input);
            parameters.Add("@DateofBirth", formData.DateofBirth, DbType.Date, ParameterDirection.Input);
            parameters.Add("@FaceBook", formData.FaceBook, DbType.String, ParameterDirection.Input);
            parameters.Add("@Insta", formData.Insta, DbType.String, ParameterDirection.Input);
            parameters.Add("@Twitter", formData.Twitter, DbType.String, ParameterDirection.Input);


            var data = _dapper.Insert<Register>(@"[sp_UpdateProfile]", parameters);

            return data;
        }

    
        public Register GetProfileDetailById(int Id)    
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", Id, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<Register>(@"[sp_GetProfileDetailById]", parameters);

            return data;
        }


        public Listing UpdateListingStatus(int Id, string Status)
        {

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Status", Status, DbType.String, ParameterDirection.Input);


            var data = _dapper.Update<Listing>(@"dbo.[sp_UpdateListingStatus]", parameters);

            return data;

        }

        public List<Listing> GetAllListings()
        {
            DynamicParameters parameters = new DynamicParameters();

            var data = _dapper.GetAll<Listing>(@"[sp_GetAllListings]", parameters);

            return data;
        }

        public object GetListing_ProdictionPackages(int id)
        {

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@id", id, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.GetMultipleObjects("[usp_get_Listing_and_promotionPackages]", parameters, gr => gr.Read<PromotionPackages>(), gr => gr.Read<Listing>());

            return data;
        }

        public Listing Assgin_PromotionPackage_to_List(Listing listing)
        {

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@PromotionPackageId", listing.PromotionPackageId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ListId", listing.Id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CreatedBy", listing.CreatedBy, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.Update<Listing>(@"dbo.[usp_assgin_PromotionPackage_to_List]", parameters);

            return data;
        }

        public bool UploadSelectedGalleryPath(string Path)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Path", Path, DbType.String, ParameterDirection.Input);
            var data = _dapper.Get<bool>(@"[dbo].[sp_UploadSelectedGalleryPath]", parameters);
            return data;
        }


        public bool DeleteSelectedGalleryPath(string Path)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Path", Path, DbType.String, ParameterDirection.Input);
            var data = _dapper.Get<bool>(@"[dbo].[sp_DeleteSelectedGalleryPath]", parameters);
            return data;
        }

        public List<string> GetAllPedigreeGallary()
        {
            DynamicParameters parameters = new DynamicParameters();
          
            var data = _dapper.GetAll<string>(@"[dbo].[sp_GetAllPedigreeGallary]", parameters);
            return data;
        }

        public List<string> GetAllBreederLicense()
        {
            DynamicParameters parameters = new DynamicParameters();

            var data = _dapper.GetAll<string>(@"[dbo].[sp_GetAllBreederLicenseGallary]", parameters);
            return data;
        }
        public List<string> GetAllVideosGallery()
        {
            DynamicParameters parameters = new DynamicParameters();

            var data = _dapper.GetAll<string>(@"[dbo].[sp_GetAllVideosGallery]", parameters);
            return data;
        }

        public int DeleteListingById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Get<int>(@"[dbo].[sp_DeleteListingById]", parameters);
            return data;
        }

        public List<Register> GetAllUsers()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<Register>(@"[sp_GetAllUsers]", parameters);

            return data;
        }

        public List<CouponCodes> GetCouponCodes()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<CouponCodes>(@"[sp_GetAllCouponsCodes]", parameters);

            return data;
        }

        public int ActiveDeactiveCouponCode(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@CouponID", id, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.Update<int>(@"[Sp_ActiveDeactiveCouponCode]", parameters);

            return data;
        }

        public int IsExpireCoupens(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@CouponID", id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Update<int>(@"[sp_IsExpireCoupens]", parameters);

            return data;
        }
    }



}
