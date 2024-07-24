using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Data;
using System.Data.Common;
using WebApi.IRepositories;
using WebApi.Utility;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _repository;
        private readonly IPackagesRepository _repositoryPkg;

        public AccountController(IAccountRepository repository, IPackagesRepository repositoryPkg)
        {
            _repository = repository;
            _repositoryPkg = repositoryPkg;
        }

       
        [HttpPost("Authenticate")]
        public Response Authenticate([FromBody] Register obj)
        {
            Response response = new Response();
            Register claimDTO = null;
            try
            {

                claimDTO=_repository.Authenticate(obj);

                if (claimDTO == null) return CustomStatusResponse.GetResponse(320);
                else
                { 

                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.Data = new
                    {
                        DataObj = claimDTO,
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
                response.ResponseMsg = ex.Message;
                return response;
            }
        }


        [HttpPost("RegisterUser")]
        public async Task<Response> RegisterUser([FromForm] Register formData)
        {
            Response response = new Response();

            try
            {
                var res = await _repository.RegisterUser(formData);

                if (res != null)
                {
                    // Create a new user package
                    UserPackages userPkg = new UserPackages
                    {
                        UserID = res.UserId,
                        PackageID = 1,
                        SubscriptionDate = DateTime.Now,
                        ExpiryDate = DateTime.Now.AddDays(365), // Calculate expiry date by adding 365 days
                        RemainingListings = 999,
                        IsActive = true,
                        IsExpired = false
                    };

                    // Buy the package
                    var respKG = _repositoryPkg.BuyPackage(userPkg);

                    // Prepare the response
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = null;
                    response.Data = res;
                    response.ResponseMsg = "You have registered successfully!";
                }
                else
                {
                    response = CustomStatusResponse.GetResponse(500);
                    response.Token = null;
                    response.ResponseMsg = "Failed to register user."; // Handle the case where res is null
                }
            }
            catch (DbException ex)
            {
                response = CustomStatusResponse.GetResponse(600);
                response.Token = null;
                response.ResponseMsg = ex.Message;
            }
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.Token = null;
                response.ResponseMsg = ex.Message;
            }

            return response;
        }


        [HttpPost("Logout")]
        public Response PostLogout()
        {
            Response response;
            try
            {
                response = new Response();

                string token = Request.Headers["Authorization"];
                if (token != null)
                {
                    TokenManager.RemoveToken(token);
                }

                response = CustomStatusResponse.GetResponse(200);
                response.Data = null;
                response.Token = null;
                return response;

            }
            catch (DbException ex)
            {

                response = CustomStatusResponse.GetResponse(600);
                response.Token = null;
                response.ResponseMsg = ex.Message;


                return response;
            }
            catch (Exception ex)
            {

                response = CustomStatusResponse.GetResponse(500);
                response.Token = null;
                response.ResponseMsg = "Internal server error!";
                return response;
            }
        }

        [HttpPost("ForgotPassword")]
        public  Response ForgotPassword([FromBody] ForgotPassword obj)
        {
            Response response = new Response();

            try
            {
                var res =  _repository.ForgotPassword(obj);

                if (res != null)
                {
                   
                    // Prepare the response
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = null;
                    response.Data = res;
                    response.ResponseMsg = "The verification code has been send to your email.";
                }
                else
                {
                    response = CustomStatusResponse.GetResponse(500);
                    response.Token = null;
                    response.ResponseMsg = "Failed to forget password."; // Handle the case where res is null
                }
            }
            catch (DbException ex)
            {
                response = CustomStatusResponse.GetResponse(600);
                response.Token = null;
                response.ResponseMsg = ex.Message;
            }
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.Token = null;
                response.ResponseMsg = ex.Message;
            }

            return response;
        }
        
        [HttpPost("ResetPassword")]
        public  Response ResetPassword([FromBody] ForgotPassword obj)
        {
            Response response = new Response();

            try
            {
                var res =  _repository.ResetPassword(obj);

                if (res != null)
                {
                   
                    // Prepare the response
                    response = CustomStatusResponse.GetResponse(200);
                    response.Token = null;
                    response.Data = res;
                    response.ResponseMsg = "The password has been changed successfully!";
                }
                else
                {
                    response = CustomStatusResponse.GetResponse(500);
                    response.Token = null;
                    response.ResponseMsg = "Failed to change password."; // Handle the case where res is null
                }
            }
            catch (DbException ex)
            {
                response = CustomStatusResponse.GetResponse(600);
                response.Token = null;
                response.ResponseMsg = ex.Message;
            }
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.Token = null;
                response.ResponseMsg = ex.Message;
            }

            return response;
        }

    }
}
