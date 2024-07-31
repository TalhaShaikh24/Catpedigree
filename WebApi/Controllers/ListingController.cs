using ClassLibrary;
using Google.Type;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mollie.Api.Models;
using System.Data.Common;
using WebApi.IRepositories;
using WebApi.Utility;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private string BaseUrl = "";
        private readonly IListingRepository _listing;
        private readonly IVideoPackagesRepository _videoPackages;

        private readonly ICurrencyConverterService _currencyConverterService;
        private readonly string _imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadImages");
        public ListingController(IListingRepository listing, IVideoPackagesRepository videoPackages, ICurrencyConverterService currencyConverterService, IConfiguration configuration)
        {

            _listing = listing;
            _videoPackages = videoPackages;
            _currencyConverterService = currencyConverterService;   
           BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";
           
        }

        [HttpPost("AddListing")]

        public async Task<Response> AddListing([FromForm] Listing obj)
        {
        
            Response response = new Response();

            Register claimDTO = null;

            Listing res = null;

            try
            {
               
                claimDTO = TokenManager.GetValidateToken(Request);
               
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                obj.CreatedBy = claimDTO.UserId;


                if (obj.VideoFile != null)
                {
                    var result =  _videoPackages.VideoAvailablity(claimDTO.UserId);

                    if(result)
                    {
                        res = await _listing.AddListing(obj);
                      
                        if (res == null) return CustomStatusResponse.GetResponse(320);
                        
                        else
                        {
                            response = CustomStatusResponse.GetResponse(200);
                            response.Token = TokenManager.GenerateToken(claimDTO);
                            response.ResponseMsg = "Listing Submitted Successfuly";
                            response.Data = res;

                            return response;
                        }
                    }
                    else
                    {
                        response = CustomStatusResponse.GetResponse(403);
                        response.Token = TokenManager.GenerateToken(claimDTO);
                        response.Data =null;
                        return response;

                    }

                }
              
                else
                {
                     res = await _listing.AddListing(obj);

                    if (res == null) return CustomStatusResponse.GetResponse(320);
                   
                    else
                    {
                        response = CustomStatusResponse.GetResponse(200);
                        response.Token = TokenManager.GenerateToken(claimDTO);
                        response.ResponseMsg = "Data Save SuccessFully";
                        response.Data = res;

                        return response;
                    }
                }
            }
            catch (DbException ex)
            {

                response = CustomStatusResponse.GetResponse(600);
                response.ResponseMsg = ex.Message;
                response.Token = TokenManager.GenerateToken(claimDTO);
                return response;
            }
            catch (Exception ex)
            {

                response = CustomStatusResponse.GetResponse(500);
                response.ResponseMsg = ex.Message;
                response.Token = TokenManager.GenerateToken(claimDTO);
                return response;
            }
        }   
        
        [HttpPost("GetHomePageListings/{currency}")]
        public async Task<Response> GetHomePageListings(string currency)
        {
            Response response = new Response();

       

            try
            {


                var res = _listing.GetHomePageListings();
                if (res.Count>0)
                {
                    decimal rate = await  _currencyConverterService.GetExchangeRate("EUR", currency);

                    foreach (var (item, index)  in res.Select((item, index) => (item, index)))
                    {

                        res[index].Price = Math.Round((item.Price * rate), 2);


                    }

                }



                if (res == null) return CustomStatusResponse.GetResponse(320);
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = null;
                    response.Data = res;
                    return response;
                }


            }
            catch (DbException ex)
            {

                response = CustomStatusResponse.GetResponse(600);

                response.ResponseMsg = ex.Message;

                return response;
            }
            catch (Exception ex)
            {

                response = CustomStatusResponse.GetResponse(500);
                response.ResponseMsg = "Internal server error!";
                return response;
            }
        }

        [HttpPost("GetTopPageListings")]
        public Response GetTopPageListings()
        {
            Response response = new Response();

       

            try
            {


                var res = _listing.GetTopPageListings();

                if (res == null) return CustomStatusResponse.GetResponse(320);
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = null;
                    response.Data = res;
                    return response;
                }


            }
            catch (DbException ex)
            {

                response = CustomStatusResponse.GetResponse(600);

                response.ResponseMsg = ex.Message;

                return response;
            }
            catch (Exception ex)
            {

                response = CustomStatusResponse.GetResponse(500);
                response.ResponseMsg = "Internal server error!";
                return response;
            }
        }
        [HttpPost("GetVetRimmedPageListings")]
        public Response GetVetRimmedPageListings()
        {
            Response response = new Response();

       

            try
            {


                var res = _listing.GetVetRimmedPageListings();

                if (res == null) return CustomStatusResponse.GetResponse(320);
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = null;
                    response.Data = res;
                    return response;
                }


            }
            catch (DbException ex)
            {

                response = CustomStatusResponse.GetResponse(600);

                response.ResponseMsg = ex.Message;

                return response;
            }
            catch (Exception ex)
            {

                response = CustomStatusResponse.GetResponse(500);
                response.ResponseMsg = "Internal server error!";
                return response;
            }
        }

		[HttpPost("GetAllCategoriesByPackageId/{pkgId}")]
        public Response GetAllCategoriesByPackageId(int pkgId)
        {
            Response response = new Response();
            Register claimDTO = null;


            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _listing.GetAllCategoriesByPackageId(pkgId);

                if (res == null) return CustomStatusResponse.GetResponse(320);
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = res;
                    return response;
                }


            }
            catch (DbException ex)
            {

                response = CustomStatusResponse.GetResponse(600);

                response.ResponseMsg = ex.Message;

                return response;
            }
            catch (Exception ex)
            {

                response = CustomStatusResponse.GetResponse(500);
                response.ResponseMsg = "Internal server error!";
                return response;
            }
        }

		[HttpPost("GetAllListingByFilters")]
		public async Task<Response> GetAllListingByFilters([FromBody] Listing obj)
		{
            
            Response response = new Response();

			try
			{
                var result = _listing.GetAllListingByFilters(obj);

				if (result == null || result.Listings == null || !result.Listings.Any())
				{
					return CustomStatusResponse.GetResponse(320);
				}
				else
				{
					response = CustomStatusResponse.GetResponse(200);
                    
                    int currentCount = (obj.PageNumber - 1) * obj.PageSize + result.FetchedCount;

                    if (result.Listings.Count>0)
                    {

                        decimal rate = await _currencyConverterService.GetExchangeRate("EUR", obj.Currency);

                        foreach (var (item, index) in result.Listings.Select((item, index) => (item, index)))
                        {

                            result.Listings[index].Price = Math.Round((item.Price * rate), 2);


                        }

                    }

					response.Data = new
					{
						Listings = result.Listings,
						TotalCount = result.TotalCount,
						CurrentCount = currentCount
					};
					return response;
				}
			}
			catch (DbException ex)
			{
				response = CustomStatusResponse.GetResponse(600);
				response.ResponseMsg = ex.Message;
				return response;
			}
			catch (Exception ex)
			{
				response = CustomStatusResponse.GetResponse(500);
				response.ResponseMsg = "Internal server error!";
				return response;
			}
		}
        
        [HttpPost("GetSingleListing")]
		public Response GetSingleListing(Listing obj)
		{
			Response response = new Response();
            


            try
            {
                
                var res = _listing.GetHomePageListings();

                if (res == null) return CustomStatusResponse.GetResponse(320);
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = null;
                    response.Data = res;
                    return response;
                }


            }
			catch (DbException ex)
			{
				response = CustomStatusResponse.GetResponse(600);
				response.ResponseMsg = ex.Message;
				return response;
			}
			catch (Exception ex)
			{
				response = CustomStatusResponse.GetResponse(500);
				response.ResponseMsg = "Internal server error!";
				return response;
			}
		}
        

        [HttpPost("GetAllDropdowns")]
        public Response GetAllDropdowns()
        {
            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = _listing.GetAllDropdowns(claimDTO.UserId);

                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Data save successfully!";

                }
                return response;

            }
            catch (DbException ex)
            {
                response = CustomStatusResponse.GetResponse(600);
                response.Token = TokenManager.GenerateToken(claimDTO);
                response.ResponseMsg = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.Token = TokenManager.GenerateToken(claimDTO);
                response.ResponseMsg = ex.Message;
                return response;
            }

        }

         [HttpPost("GetAllCatType")]
        public Response GetAllCatType()
        {
            
            Response response = new Response();

            try
            {
                
                var res = _listing.GetAllCatType();

                if (res != null)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.ResponseMsg = "Data fetched successfully!";

                }
                return response;

            }
            catch (DbException ex)
            {
                response = CustomStatusResponse.GetResponse(600);
                response.ResponseMsg = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.ResponseMsg = ex.Message;
                return response;
            }

        }



        [HttpPost("VideoAvailablity/{Id}")]
        public Response VideoAvailablity(int Id)
        {
            Response response = new Response();
            Register claimDTO = null;
            Listing obj = new Listing();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                obj.CreatedBy = claimDTO?.UserId ?? 0;
                obj.Id = Id;

                      


                if (claimDTO!=null)
                {

                          

                    if (claimDTO.RoleIds.Contains("Vendor"))
                    {


                         Category category=   _listing.getCategoryByListingId(Id);

                        if (category.CategoryName== "Pedigree")
                        {
                            var data = _listing.CheckListingShowValidation(obj.CreatedBy,Id);

                            if (data.Count > 0)
                            {
                                response.Token = TokenManager.GenerateToken(claimDTO);
                                response.Status = 115;

                                response.Data = new
                                {

                                    Listing = new Listing(),
                                    Package = data



                                };

                                return response;
                            }

                        }







                    }
                }



                var res = _listing.IsViewPedigreeAllowed(obj);

                if (res != null)
                {
                    response = CustomStatusResponse.GetResponse(200);
                    if (claimDTO != null)
                    {
                        response.Token = TokenManager.GenerateToken(claimDTO);
                    }
                    response.ResponseMsg = "";
                    response.Data = new
                    {

                        Listing = res,
                        Package = new List<Package>()



                    };

                }
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.ResponseMsg = "Please Buy the Plan";

                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = new
                    {

                        Listing = new Listing(),
                        Package = new List<Package>()



                    };
                }
            }
            catch (DbException ex)
            {
                response = CustomStatusResponse.GetResponse(600);
                response.ResponseMsg = ex.Message;
            }
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.ResponseMsg = ex.Message;
            }

            return response;
        }

        [HttpPost("VideoAvailablity/{Id}/{currency}")]
        public async Task<Response> VideoAvailablity(int Id, string currency)
        {
            Response response = new Response();
            Register claimDTO = null;
            Listing obj = new Listing();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);

                obj.CreatedBy = claimDTO?.UserId ?? 0;
                obj.Id = Id;

                // Additional logic here, if needed, using the currency parameter

                if (claimDTO != null)
                {
                    if (claimDTO.RoleIds.Contains("Vendor"))
                    {
                        Category category = _listing.getCategoryByListingId(Id);

                        if (category.CategoryName == "Pedigree")
                        {
                            var data = _listing.CheckListingShowValidation(obj.CreatedBy, Id);

                            if (data.Count > 0)
                            {

                                decimal rate = await _currencyConverterService.GetExchangeRate("EUR", currency);

                                foreach (var (item, index) in data.Select((item, index) => (item, index)))
                                {

                                    data[index].Price = Math.Round((decimal)(item.Price * rate), 2);


                                }

                                response.Token = TokenManager.GenerateToken(claimDTO);
                                response.Status = 115;

                                response.Data = new
                                {
                                    Listing = new Listing(),
                                    Package = data,
                                    Currency = currency // Include currency in the response
                                };

                                return response;
                            }
                        }
                    }
                }

                var res = _listing.IsViewPedigreeAllowed(obj);

                if (res != null)
                {

                    decimal rate = await _currencyConverterService.GetExchangeRate("EUR", currency);

                 

                     res.Price = Math.Round((decimal)(res.Price * rate), 2);


                   

                    response = CustomStatusResponse.GetResponse(200);
                    if (claimDTO != null)
                    {
                        response.Token = TokenManager.GenerateToken(claimDTO);
                    }
                    response.ResponseMsg = "";
                    response.Data = new
                    {
                        Listing = res,
                        Package = new List<Package>(),
                        Currency = currency // Include currency in the response
                    };
                }
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.ResponseMsg = "Please Buy the Plan";
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = new
                    {
                        Listing = new Listing(),
                        Package = new List<Package>(),
                        Currency = currency // Include currency in the response
                    };
                }
            }
            catch (DbException ex)
            {
                response = CustomStatusResponse.GetResponse(600);
                response.ResponseMsg = ex.Message;
            }
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.ResponseMsg = ex.Message;
            }

            return response;
        }





        [HttpPost("SelectPackageListingShowValidation/{Id}")]
        public Response SelectPackageListingShowValidation(int Id)
        {
            Register claimDTO = null;
            Response response = new Response();
            Listing listing = new Listing();
            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                listing.PackageId = Id;
                listing.UserId = claimDTO.UserId;
                listing.CreatedBy = claimDTO.UserId;


                var res = _listing.SelectPackageListingShowValidation(listing);



                if (res>0)
                {

                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Data save successfully!";

                }
                return response;

            }
            catch (DbException ex)
            {
                response = CustomStatusResponse.GetResponse(600);
                response.Token = TokenManager.GenerateToken(claimDTO);
                response.ResponseMsg = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.Token = TokenManager.GenerateToken(claimDTO);
                response.ResponseMsg = ex.Message;
                return response;
            }

        }


        [HttpPost("GetAllGallery")]
        public Response GetAllGallery()
        {

            Response response = new Response();
            try
            {



                if (!Directory.Exists(_imagesPath))
                {
                    response = CustomStatusResponse.GetResponse(600);
                    response.ResponseMsg = "Image directory not found.";
                    response.Data = null;
                    return response;
                }

                var images = Directory.GetFiles(_imagesPath)
                   .Select(filePath => new Gallery
                   {
                       Id = Path.GetFileNameWithoutExtension(filePath).GetHashCode(),
                       FileName = Path.GetFileName(filePath),
                       FilePath = $"{BaseUrl}UploadImages/{Path.GetFileName(filePath)}?v={System.DateTime.UtcNow.Ticks}"
                   })
                   .OrderByDescending(g => g.FilePath)
                   .ToList();

                if (images == null) return CustomStatusResponse.GetResponse(320);
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = images;
                    return response;
                }
            }
            catch (DbException ex)
            {

                response = CustomStatusResponse.GetResponse(600);

                response.ResponseMsg = ex.Message;

                return response;
            }
            catch (Exception ex)
            {

                response = CustomStatusResponse.GetResponse(500);
                response.ResponseMsg = "Internal server error!";
                return response;
            }
        }
    }
}
