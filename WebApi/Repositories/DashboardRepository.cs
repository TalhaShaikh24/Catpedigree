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
            DashboardDataAnalytics  dashboardDataAnalytics = new DashboardDataAnalytics();  
            
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);

   
            var data = _dapper.GetMultipleObjects("[sp_GetAllDashboardData]", parameters, gr => gr.Read<string>(), gr => gr.Read<UserPromotionPackagesAnalytics>(), gr => gr.Read<UserAdvertisementPackage>());

            var jsonObject = JObject.Parse(data.Item1.FirstOrDefault());

            dashboardDataAnalytics.jsonObject = jsonObject["CombinedData"].ToString();

            dashboardDataAnalytics.assignPromotionPackage=data.Item2.ToList();  
         
            dashboardDataAnalytics.advertisementPackage=data.Item3.ToList();  


            return dashboardDataAnalytics;
        }

        public List<Listing> GetAllMyListings(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.GetAll<Listing>(@"[dbo].[sp_GetAllMyListing]", parameters);
            return data;
        }

        public Listing GetListingFilesById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Get<Listing>(@"[dbo].[sp_GetListingFilesById]", parameters);
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
            parameters.Add("@Id", obj.Id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Title", obj.Title, DbType.String, ParameterDirection.Input);
            parameters.Add("@Country", obj.Country, DbType.String, ParameterDirection.Input);
            parameters.Add("@Location", obj.Location, DbType.String, ParameterDirection.Input);
            parameters.Add("@State", obj.State, DbType.String, ParameterDirection.Input);
            parameters.Add("@City", obj.City, DbType.String, ParameterDirection.Input);
            parameters.Add("@ZipCode", obj.ZipCode, DbType.String, ParameterDirection.Input);
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
            parameters.Add("IsPriceRequest", obj.IsPriceRequest, DbType.Boolean, ParameterDirection.Input);
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
            parameters.Add("PhoneCode", obj.PhoneCode, DbType.String, ParameterDirection.Input);
            parameters.Add("latitude", obj.latitude, DbType.String, ParameterDirection.Input);
            parameters.Add("longitude", obj.longitude, DbType.String, ParameterDirection.Input);
            parameters.Add("FamilyTreeMother", obj.FamilyTreeMother, DbType.String, ParameterDirection.Input);

            parameters.Add("FamilyTreeFather", obj.FamilyTreeFather, DbType.String, ParameterDirection.Input);

            parameters.Add("FatherTested", obj.FatherTested, DbType.String, ParameterDirection.Input);

            parameters.Add("MotherTested", obj.MotherTested, DbType.String, ParameterDirection.Input);

            parameters.Add("DateofBirth", obj.DateofBirth, DbType.Date, ParameterDirection.Input);
            parameters.Add("PartOfAssociation", obj.PartOfAssociation, DbType.String, ParameterDirection.Input);
            parameters.Add("Website", obj.Website, DbType.String, ParameterDirection.Input);

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


        public int UpdateActiveInActiveUser(int UserId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@UserId", UserId, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<int>(@"[sp_ActiveInActiveUser]", parameters);

            return data;
        }


        public Listing UpdateListingStatus(int Id, string Status, string Reason = "")
        {

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Status", Status, DbType.String, ParameterDirection.Input);
            parameters.Add("@Reason", Reason, DbType.String, ParameterDirection.Input);


            var data = _dapper.Update<Listing>(@"dbo.[sp_UpdateListingStatus]", parameters);

            return data;

        }

        public List<Listing> GetAllListings()
        {
            DynamicParameters parameters = new DynamicParameters();

            var data = _dapper.GetAll<Listing>(@"[sp_GetAllListings]", parameters);

            return data;
        }
      
        public List<Gallery> GetAllGallary()
        {
            DynamicParameters parameters = new DynamicParameters();

            var data = _dapper.GetAll<Gallery>(@"[sp_GetAllgallary]", parameters);

            return data;
        }
        public List<string> GetAllMedia()
        {
            DynamicParameters parameters = new DynamicParameters();

            var data = _dapper.GetAll<string>(@"[sp_GetAllMedia]", parameters);

            return data;
        }

     





        public object GetListing_ProdictionPackages(int id)
        {

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@id", id, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.GetMultipleObjects("[usp_get_Listing_and_promotionPackages]", parameters, gr => gr.Read<PromotionPackages>(), gr => gr.Read<Listing>());

            return data;
        }

        public async Task<Listing> Assgin_PromotionPackage_to_List(Listing listing)
        {
            // Function to replace spaces with underscores in file names
            string ReplaceSpaces(string input) => input.Replace(' ', '_');

            if (listing.VideoFile != null)
            {
                string VideoFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + ReplaceSpaces(Path.GetFileName(listing.VideoFile.FileName));
                string VideoFilePath = Path.Combine("UploadVideos", VideoFileName);
                string VideoFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, VideoFilePath);

                using (var stream = new FileStream(VideoFilePathDirectory, FileMode.Create))
                {
                    await listing.VideoFile.CopyToAsync(stream);
                }
                listing.VideoPath = VideoFilePath;
            }

            var isVideoPromotion = listing.VideoFile != null; // Check if video file is present

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@PromotionPackageId", listing.PromotionPackageId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ListId", listing.Id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("VideoPath", listing.VideoPath, DbType.String, ParameterDirection.Input);
            parameters.Add("@IsVideoPromotion", isVideoPromotion, DbType.Boolean, ParameterDirection.Input); // Set based on presence of video file
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

        public ListUsersDTO GetAllUsers()
        {

            ListUsersDTO usersDTO = new ListUsersDTO();

            DynamicParameters parameters = new DynamicParameters();
           
            var data = _dapper.GetMultipleObjects(@"[dbo].[sp_GetAllUsers]", parameters, gr => gr.Read<Register>(), gr => gr.Read<Roles>());
            usersDTO.Register = data.Item1.ToList();
            usersDTO.Roles = data.Item2.ToList();


            return usersDTO;
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

        public int AddGallary(Gallery obj)
        {
            DynamicParameters parameters = new DynamicParameters();

             parameters.Add("@FilePath", obj.FilePath, DbType.String, ParameterDirection.Input);
             parameters.Add("@FileName", obj.FileName, DbType.String, ParameterDirection.Input);
             parameters.Add("@CreatedBy", obj.CreatedBy, DbType.String, ParameterDirection.Input);

            var data = _dapper.Insert<int>(@"[sp_AddGallary]", parameters);

            return data;
        }
        public int AddMedia(Gallery obj)
        {
            DynamicParameters parameters = new DynamicParameters();

             parameters.Add("@FilePath", obj.FilePath, DbType.String, ParameterDirection.Input);
             parameters.Add("@FileName", obj.FileName, DbType.String, ParameterDirection.Input);
             parameters.Add("@CreatedBy", obj.CreatedBy, DbType.String, ParameterDirection.Input);

            var data = _dapper.Insert<int>(@"[sp_AddMedia]", parameters);

            return data;
        }


        public int UpdateRoles(userRolesUpdate obj)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@userId", obj.UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@RolesId", obj.RolesIds, DbType.String, ParameterDirection.Input);

            var data = _dapper.Insert<int>(@"[sp_UpdateUserRoles]", parameters);

            return data;
        }

        public int DeleteUser(int UserId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@UserId", UserId, DbType.Int32, ParameterDirection.Input);
            
            var data = _dapper.Update<int>(@"[sp_DeleteUser]", parameters);

            return data;
        }

        public List<Register> GetAllUsersForPricingPackages()
        {
            DynamicParameters parameters = new DynamicParameters();

            
            var data = _dapper.GetAll<Register>(@"[sp_GetAllUsersForPricingPackages]", parameters);

            return data;
        }

        public List<Package> getAllPackagestoAssgin(int userid)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@UserId", userid, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.GetAll<Package>(@"[sp_getAllPackagestoAssgin]", parameters);

            return data;
        }

        public List<GetUserpackagesAssigned> GetUserpackagesAssigned()
        {
            DynamicParameters parameters = new DynamicParameters();

            
            var data = _dapper.GetAll<GetUserpackagesAssigned>(@"[sp_GetUserpackagesAssigned]", parameters);

            return data;
        }



        public AssignPromotionPackageDTO GetPromotionPackagesWithDays()
        {

            AssignPromotionPackageDTO promotionPackageDTO = new AssignPromotionPackageDTO();


            DynamicParameters parameters = new DynamicParameters();



            var data = _dapper.GetMultipleObjects("[GetPromotionPackageAndUsers]", parameters, gr => gr.Read<AssignPromotionPackage>(), gr => gr.Read<Register>());


            promotionPackageDTO.assignPromotionPackages = data.Item1.ToList();
            
            promotionPackageDTO.Users = data.Item2.ToList();



            return promotionPackageDTO;
        }




        public PromotionPackages AssignPromotionPackageToUser(AssignPromotionPackage obj)
        {


            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PPCID", obj.PPCID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@UserID", obj.userId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@SubscriptionDate", DateTime.Now, DbType.String, ParameterDirection.Input);
            // Calculate expiry date by adding 365 days to the subscription date
            DateTime? expiryDate = DateTime.Now.AddDays(365);
            parameters.Add("@ExpiryDate", expiryDate, DbType.String, ParameterDirection.Input);
            parameters.Add("@IsActive", true, DbType.String, ParameterDirection.Input);
            parameters.Add("@IsExpired", false, DbType.String, ParameterDirection.Input);
            parameters.Add("@CreatedBy", obj.CreatedBy, DbType.String, ParameterDirection.Input);
            parameters.Add("@stripeSubscriptionId", null, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<PromotionPackages>(@"[sp_AssignPromotionPackageToUser]", parameters);

            return data;


        }


        public List<GetAllUsersPromotionPackage> getAllUsersPromotionPackages()
        {

            DynamicParameters parameters = new DynamicParameters();
            var data =_dapper.GetAll<GetAllUsersPromotionPackage>(@"[GetAllUsersPromotionPackage]", parameters);

            return data;


        }


        public AssignAdvertisementPackagesDTO GetAdvertisementPackagesAndUsers()
        {

            AssignAdvertisementPackagesDTO advertisementPackagesDTO = new AssignAdvertisementPackagesDTO();


            DynamicParameters parameters = new DynamicParameters();



            var data = _dapper.GetMultipleObjects("[sp_Get_AdvertisementPackagesAndUsers]", parameters, gr => gr.Read<AssignAdvertisementPackages>(), gr => gr.Read<Register>());


            advertisementPackagesDTO.assignAdvertisementPackages = data.Item1.ToList();

            advertisementPackagesDTO.Users = data.Item2.ToList();



            return advertisementPackagesDTO;
        }


        public UserAdvertisementPackage AssignAdvertisementPackage(UserAdvertisementPackage obj)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserID", obj.UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@AdvertisementPackageID", obj.AdvertisementPackageID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CreatedBy", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@stripeSubscriptionId", obj.stripeSubscriptionId, DbType.String, ParameterDirection.Input);
            var data = _dapper.Get<UserAdvertisementPackage>(@"[dbo].[sp_AssignAdvertisementPackage]", parameters);
            return data;
        }


        public List<AssignedUserAdvertisementsList> GetAssignedUserAdvertisements()
        {

            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<AssignedUserAdvertisementsList>(@"[sp_GetallAssignedUserAdvertisements]", parameters);

            return data;

        }

        #region
        public List<Blog> GetAllAdminBlogs()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<Blog>(@"[sp_GetAllAdminBlogs]", parameters);

            return data;
        }
        public async Task<Blog> AddBlog(Blog obj)
        {

            if (obj.FeatureImage != null)
            {
                string FeatureImageName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.FeatureImage.FileName);
                string FeatureFilePath = Path.Combine("UploadBLogs", FeatureImageName);
                string FeatureFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, FeatureFilePath);

                using (var stream = new FileStream(FeatureFilePathDirectory, FileMode.Create))
                {
                    await obj.FeatureImage.CopyToAsync(stream);
                }
                obj.FeatureImagePath = FeatureFilePath;
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Title", obj.Title, DbType.String, ParameterDirection.Input);
            parameters.Add("@ShortDescription", obj.ShortDescription, DbType.String, ParameterDirection.Input);
            parameters.Add("@FeatureImagePath", obj.FeatureImagePath, DbType.String, ParameterDirection.Input);
            parameters.Add("@Content", obj.Content, DbType.String, ParameterDirection.Input);
            parameters.Add("@BlogCategoryId", obj.BlogCategoryId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@Tags", obj.Tags, DbType.String, ParameterDirection.Input);
            parameters.Add("@CreatedBy", obj.CreatedBy, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<Blog>(@"[sp_AddBlog]", parameters);

            return data;
        }
        public async Task<Blog> UpdateBlog(Blog obj)
        {

            if (obj.FeatureImage != null)
            {
                string FeatureImageName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.FeatureImage.FileName);
                string FeatureFilePath = Path.Combine("UploadBLogs", FeatureImageName);
                string FeatureFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, FeatureFilePath);

                using (var stream = new FileStream(FeatureFilePathDirectory, FileMode.Create))
                {
                    await obj.FeatureImage.CopyToAsync(stream);
                }
                obj.FeatureImagePath = FeatureFilePath;
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@BlogID", obj.BlogID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Title", obj.Title, DbType.String, ParameterDirection.Input);
            parameters.Add("@ShortDescription", obj.ShortDescription, DbType.String, ParameterDirection.Input);
            parameters.Add("@FeatureImagePath", obj.FeatureImagePath, DbType.String, ParameterDirection.Input);
            parameters.Add("@Content", obj.Content, DbType.String, ParameterDirection.Input);
            parameters.Add("@BlogCategoryId", obj.BlogCategoryId, DbType.Int64, ParameterDirection.Input);
            parameters.Add("@Tags", obj.Tags, DbType.String, ParameterDirection.Input);
            parameters.Add("@ModifiedBy", obj.ModifiedBy, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.Get<Blog>(@"[sp_UpdateBlog]", parameters);

            return data;
        }
        public Blog BlogEditById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Insert<Blog>(@"[sp_BlogEditById]", parameters);
            return data;
        }
        public int BlogDeleteById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Insert<int>(@"[sp_BlogDeleteById]", parameters);
            return data;
        }
        public List<BlogCategories> GetAllBlogCategories()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<BlogCategories>(@"[sp_GetAllBlogCategories]", parameters);

            return data;
        }        
        public List<Blog> GetAllDistinctTags()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<Blog>(@"[sp_GetAllDistinctTags]", parameters);

            return data;
        }
        public async Task<BlogCategories> AddBlogCategory(BlogCategories obj)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CategoryName", obj.CategoryName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Description", obj.Description, DbType.String, ParameterDirection.Input);
            parameters.Add("@CreatedOn", DateTime.Now, DbType.String, ParameterDirection.Input);

            var data =  _dapper.Get<BlogCategories>(@"[sp_AddBlogCategory]", parameters);

            return data;
        }
        public async Task<BlogCategories> UpdateBlogCategory(BlogCategories obj)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", obj.Id, DbType.String, ParameterDirection.Input);
            parameters.Add("@CategoryName", obj.CategoryName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Description", obj.Description, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<BlogCategories>(@"[sp_UpdateBlogCategory]", parameters);

            return data;
        }
        public int DeleteBlogCategory(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Insert<int>(@"[sp_DeleteBlogCategoryById]", parameters);
            return data;
        }
        public Reply SendReply(Reply obj)
        {

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CommentId", obj.CommentId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@UserId", obj.UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ReplyText", obj.ReplyText, DbType.String, ParameterDirection.Input);
            var data = _dapper.Insert<Reply>(@"[sp_AddReply]", parameters);
            return data;
        }
        public List<Comment> GetAllCommentsByBlogId(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.GetAll<Comment>(@"[sp_GetAllCommentsByBlogId]", parameters);
            return data;
        }
        public List<Blog> GetAllUnreadComments()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<Blog>(@"[sp_GetAllUnreadComments]", parameters);
            return data;
        }
        public int DeleteCommentById(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Insert<int>(@"[sp_DeleteCommentById]", parameters);
            return data;
        }
        public List<Reply> GetAllReplyByCommentId(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.GetAll<Reply>(@"[sp_GetAllReplyByCommentId]", parameters);
            return data;
        }
        public Reply UpdateReply(Reply obj)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", obj.Id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@UserId", obj.UserId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CommentId", obj.CommentId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ReplyText", obj.ReplyText, DbType.String, ParameterDirection.Input);
            var data = _dapper.Update<Reply>(@"[sp_UpdateReply]", parameters);
            return data;
        }
        public int DeleteReplyId(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Insert<int>(@"[sp_DeleteReplyId]", parameters);
            return data;
        }
        public Comment AddComment(Comment obj)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@BlogID", obj.BlogID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CommentText", obj.CommentText, DbType.String, ParameterDirection.Input);
            parameters.Add("@UserId", obj.UserId, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.Get<Comment>(@"[sp_AddComment]", parameters);

            return data;
        }
        #endregion


        public object GetAllListingFiltersDashboard()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetMultipleObjects(@"[dbo].[sp_GetAllListingFiltersDashboard]", parameters,gr=>gr.Read<Category>(), gr => gr.Read<CatTypes>(), gr => gr.Read<Package>());
          
            return data;
        }


        public object GetAllRoles()
        {

            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<Roles>(@"[sp_GetAllRoles]", parameters);

            return data;

        }

        public async Task<Register> AddUser(Register formData)
        {
            try
            {
                string folder = "Profile"; // Relative path

                string profileFilePath = null;
                string licenseFilePath = null;

                // For Profile Picture
                if (formData.ProfilePic != null)
                {
                    string profileFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(formData.ProfilePic.FileName);
                    profileFilePath = Path.Combine(folder, profileFileName);
                    string absoluteProfileFilePath = Path.Combine(_hostingEnvironment.WebRootPath, profileFilePath);

                    using (var profileStream = new FileStream(absoluteProfileFilePath, FileMode.Create))
                    {
                        await formData.ProfilePic.CopyToAsync(profileStream);
                    }
                }

                // For Breeder License
                if (formData.BreederLicense != null)
                {
                    string licenseFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(formData.BreederLicense.FileName);
                    licenseFilePath = Path.Combine(folder, licenseFileName);
                    string absoluteLicenseFilePath = Path.Combine(_hostingEnvironment.WebRootPath, licenseFilePath);

                    using (var licenseStream = new FileStream(absoluteLicenseFilePath, FileMode.Create))
                    {
                        await formData.BreederLicense.CopyToAsync(licenseStream);
                    }
                }

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Firstname", formData.Firstname, DbType.String, ParameterDirection.Input);
                parameters.Add("@Lastname", formData.Lastname, DbType.String, ParameterDirection.Input);
                parameters.Add("@Username", formData.Username, DbType.String, ParameterDirection.Input);
                parameters.Add("@Email", formData.Email, DbType.String, ParameterDirection.Input);
                parameters.Add("@Password", formData.Password, DbType.String, ParameterDirection.Input);
                parameters.Add("@ContactNo", formData.ContactNo, DbType.String, ParameterDirection.Input);
                parameters.Add("@Address", formData.Address, DbType.String, ParameterDirection.Input);
                parameters.Add("@ProfileInfo", formData.ProfileInfo, DbType.String, ParameterDirection.Input);
                parameters.Add("@ProfilePicPath", profileFilePath != null ? "~/" + profileFilePath : null, DbType.String, ParameterDirection.Input);
                parameters.Add("@BreederLicensePath", licenseFilePath != null ? "~/" + licenseFilePath : null, DbType.String, ParameterDirection.Input);
                parameters.Add("@ZoologicalNumber", formData.ZoologicalNumber, DbType.String, ParameterDirection.Input);
                parameters.Add("@RoleId", formData.Roles, DbType.String, ParameterDirection.Input);


                parameters.Add("@Country", formData.Country, DbType.String, ParameterDirection.Input);
                parameters.Add("@City", formData.City, DbType.String, ParameterDirection.Input);
                parameters.Add("@province", formData.Province, DbType.String, ParameterDirection.Input);
                parameters.Add("@DateofBirth", formData.DateofBirth, DbType.Date, ParameterDirection.Input);
                parameters.Add("@FaceBook", formData.FaceBook, DbType.String, ParameterDirection.Input);
                parameters.Add("@Insta", formData.Insta, DbType.String, ParameterDirection.Input);
                parameters.Add("@Twitter", formData.Twitter, DbType.String, ParameterDirection.Input);


                var data = _dapper.Insert<Register>(@"[sp_AddDashboardUser]", parameters);
                return data;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine("Error in RegisterUser: " + ex.Message);
                throw; // Rethrow the exception to notify the caller about the error
            }
        }

        public async Task<Show> UpdateShow(Show obj)
        {
            // Function to replace spaces with underscores in file names
            string ReplaceSpaces(string input) => input.Replace(' ', '_');

            if (obj.FeatureImage != null)
            {
                string FeatureImageName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.FeatureImage.FileName);
                string FeatureFilePath = Path.Combine("UploadFeatureShows", FeatureImageName);
                string FeatureFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, FeatureFilePath);

                using (var stream = new FileStream(FeatureFilePathDirectory, FileMode.Create))
                {
                    await obj.FeatureImage.CopyToAsync(stream);
                }
                obj.FeatureImagePath = FeatureFilePath;
            }

            if (obj.GallaryImage != null)
            {
                List<string> GalleryPath = new List<string>();

                foreach (var item in obj.GallaryImage)
                {
                    string GalleryFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + ReplaceSpaces(Path.GetFileName(item.FileName));
                    string GalleryFilePath = Path.Combine("UploadGallaryImages", GalleryFileName);
                    string UploadImagesFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, GalleryFilePath);

                    using (var stream = new FileStream(UploadImagesFilePathDirectory, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                    GalleryPath.Add(GalleryFilePath);
                }

                obj.GallaryImagePath = string.Join(",", GalleryPath);
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", obj.ShowId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Title", obj.Title, DbType.String, ParameterDirection.Input);
           
            parameters.Add("@FeatureImagePath", obj.FeatureImagePath, DbType.String, ParameterDirection.Input);
            parameters.Add("@GallaryImagePath", obj.GallaryImagePath, DbType.String, ParameterDirection.Input);
            parameters.Add("@Content", obj.Content, DbType.String, ParameterDirection.Input);
         
            parameters.Add("@CreatedBy", obj.CreatedBy, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<Show>(@"[sp_UpdateShows]", parameters);

            return data;
        }
        public async Task<Show> AddShow(Show obj)
        {
            // Function to replace spaces with underscores in file names
            string ReplaceSpaces(string input) => input.Replace(' ', '_');

            if (obj.FeatureImage != null)
            {
                string FeatureImageName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + Path.GetFileName(obj.FeatureImage.FileName);
                string FeatureFilePath = Path.Combine("UploadFeatureShows", FeatureImageName);
                string FeatureFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, FeatureFilePath);

                using (var stream = new FileStream(FeatureFilePathDirectory, FileMode.Create))
                {
                    await obj.FeatureImage.CopyToAsync(stream);
                }
                obj.FeatureImagePath = FeatureFilePath;
            }

            if (obj.GallaryImage != null)
            {
                List<string> GalleryPath = new List<string>();

                foreach (var item in obj.GallaryImage)
                {
                    string GalleryFileName = Guid.NewGuid().ToString().Substring(0, 5) + "_" + ReplaceSpaces(Path.GetFileName(item.FileName));
                    string GalleryFilePath = Path.Combine("UploadGallaryImages", GalleryFileName);
                    string UploadImagesFilePathDirectory = Path.Combine(_hostingEnvironment.WebRootPath, GalleryFilePath);

                    using (var stream = new FileStream(UploadImagesFilePathDirectory, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                    GalleryPath.Add(GalleryFilePath);
                }

                obj.GallaryImagePath = string.Join(",", GalleryPath);
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Title", obj.Title, DbType.String, ParameterDirection.Input);
           
            parameters.Add("@FeatureImagePath", obj.FeatureImagePath, DbType.String, ParameterDirection.Input);
            parameters.Add("@GallaryImagePath", obj.GallaryImagePath, DbType.String, ParameterDirection.Input);
            parameters.Add("@Content", obj.Content, DbType.String, ParameterDirection.Input);
         
            parameters.Add("@CreatedBy", obj.CreatedBy, DbType.String, ParameterDirection.Input);

            var data = _dapper.Get<Show>(@"[sp_AddShows]", parameters);

            return data;
        }

        public List<Show> ShowList()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<Show>(@"[sp_ShowList]", parameters);

            return data;
        }


        public Show GetShowbyID(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);

            var data = _dapper.Get<Show>(@"[sp_ShowbyID]", parameters);

            return data;
        }




        public void ShowDelete(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);

          _dapper.Update<int>(@"[sp_ShowDelete]", parameters);
            
        }

        public Roles CreateRole(Roles obj)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Role", obj.Role, DbType.String, ParameterDirection.Input);
            var data = _dapper.Insert<Roles>(@"[dbo].[sp_CreateRole]", parameters);
            return data;
        }


        public List<Register> GetAllUsersDropdown()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<Register>(@"[dbo].[sp_GetAllUsersDropdown]", parameters);
            return data;
        }

        public List<Roles> GetAllRoleDropdownByUserId(int UserId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserId", UserId, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.GetAll<Roles>(@"[dbo].[sp_GetAllRoleDropdownByUserId]", parameters);
            return data;
        }

        public List<PermissionScreens> GetAllPermissionScreensDropdown()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<PermissionScreens>(@"[dbo].[sp_GetAllPermissionScreensDropdown]", parameters);
            return data;
        }

        public List<RoleScreenPermission> AddRoleScreenPermission(ScreenPermission obj, int UserId,string Roles)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RoleId", obj.RoleId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ScreenIds", obj.ScreenIds, DbType.String, ParameterDirection.Input);
            parameters.Add("@Roles", Roles, DbType.String, ParameterDirection.Input);
            parameters.Add("@UserId",UserId, DbType.String, ParameterDirection.Input);
            var data = _dapper.GetAll<RoleScreenPermission>(@"[dbo].[AddRoleScreenPermission]", parameters);

            return data;
        }

        public List<DTORoleScreenPermission> GetAllRoleScreenPermissions()
        {
            DynamicParameters parameters = new DynamicParameters();
            var data = _dapper.GetAll<DTORoleScreenPermission>(@"[dbo].[sp_GetRoleScreenPermissions]", parameters);
            
            return data;
        }

        public int DeletePermission(int Id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RoleId", Id, DbType.Int32, ParameterDirection.Input);
            var data = _dapper.Insert<int>(@"[dbo].[sp_DeletePermission]", parameters);

            return data;
        }
    }






}
