using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using WebApi.IRepositories;
using WebApi.Utility;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {

        private readonly IListingRepository _listing;
        private readonly IVideoPackagesRepository _videoPackages;
        public ListingController(IListingRepository listing, IVideoPackagesRepository videoPackages)
        {

            _listing = listing;
            _videoPackages = videoPackages;
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
                            response.ResponseMsg = "Data Save SuccessFully";
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
        
        [HttpPost("GetHomePageListings")]
        public Response GetHomePageListings()
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

		[HttpPost("GetAllListingByFilters")]
		public Response GetAllListingByFilters(ListingFilters obj)
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
					response.Token = null;
					int currentCount = (obj.PageNumber - 1) * obj.PageSize + result.FetchedCount;
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
		public Response GetSingleListing(ListingFilters obj)
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



        [HttpPost("VideoAvailablity/{Id}")]
        public Response VideoAvailablity(int Id)
        {

            Response response = new Response();

            Register claimDTO = null;

            Listing obj = new Listing();

			obj.CreatedBy = claimDTO?.UserId ?? 0;
			obj.Id = Id;


            try
            {

                //claimDTO = TokenManager.GetValidateToken(Request);

                //if (claimDTO == null) return CustomStatusResponse.GetResponse(401);


                var res = _listing.IsViewPedigreeAllowed(obj);

                if (res!=null)
                {
                    response = CustomStatusResponse.GetResponse(200);
                  //  response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "";
                    response.Data = res;

                    return response;

                }
                else
                {
                    response = CustomStatusResponse.GetResponse(200);
                   // response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Please Buy the Plan";
                    response.Data = res;

                    return response;
                }
            }
            catch (DbException ex)
            {

                response = CustomStatusResponse.GetResponse(600);
                response.ResponseMsg = ex.Message;
               // response.Token = TokenManager.GenerateToken(claimDTO);
                return response;
            }
            catch (Exception ex)
            {

                response = CustomStatusResponse.GetResponse(500);
                response.ResponseMsg = ex.Message;
                //response.Token = TokenManager.GenerateToken(claimDTO);
                return response;
            }
        }
    }
}
