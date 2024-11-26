using ClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Data;
using System.Data.Common;
using System.Net.Mail;
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
        private readonly IConfiguration _configuration;

        public AccountController(IAccountRepository repository, IPackagesRepository repositoryPkg, IConfiguration configuration)
        {
            _repository = repository;
            _repositoryPkg = repositoryPkg;
            _configuration = configuration;
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
                        dataObj = claimDTO,
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
                    response.ResponseMsg = "Congratulations! Your registration was successful.";
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

                     SendForgotPasswordEmail(res.Email,res.VerificationCode);

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

        [HttpPost("UserInfo/{id}")]
        public async Task<Response> UserInfo(int id)
        {

            Register claimDTO = null;
            Response response = new Response();

            try
            {
                claimDTO = TokenManager.GetValidateToken(Request);
                if (claimDTO == null) return CustomStatusResponse.GetResponse(401);

                var res = await _repository.UserInfo(id);

          
                    response = CustomStatusResponse.GetResponse(200);
                    response.Data = res;
                    response.Token = TokenManager.GenerateToken(claimDTO);
                    response.ResponseMsg = "Data Fatched successfully!";

                
               
            }
            catch (DbException ex)
            {
                response = CustomStatusResponse.GetResponse(600);
         
                response.Token = TokenManager.GenerateToken(claimDTO);
                response.ResponseMsg = ex.Message;
            }
            catch (Exception ex)
            {
                response = CustomStatusResponse.GetResponse(500);
                response.Token = TokenManager.GenerateToken(claimDTO);
                response.ResponseMsg = ex.Message;
            }

            return response;
        }
        private void SendForgotPasswordEmail(string toEmail, string verificationCode)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                var smtpServer = smtpSettings["Server"];
                int port = int.Parse(smtpSettings["Port"]);
                var senderEmail = smtpSettings["SenderEmail"];
                var username = smtpSettings["Username"];
                var password = smtpSettings["Password"];
                var senderName = smtpSettings["SenderName"];
                bool isEnableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                using (var mail = new MailMessage())
                using (var smtpClient = new SmtpClient(smtpServer))
                {
                    mail.From = new MailAddress(senderEmail, senderName);
                    mail.To.Add(toEmail);
                    mail.Subject = "Password Reset Request";

                    mail.Body = $@"
                    <html>
                    <body>
                        <p>Dear User,</p>
                        <p>We received a request to reset your password. Please use the following verification code to proceed:</p>
                        <h2 style='color: #007bff;'>{verificationCode}</h2>
                        <p>If you did not request this password reset, please ignore this email. Your password will not be changed.</p>
                        <p>Thank you,</p>
                        <p>The Support Team</p>
                    </body>
                    </html>";

                    mail.IsBodyHtml = true;

                    smtpClient.Port = port;
                    smtpClient.Credentials = new System.Net.NetworkCredential(username, password);
                    smtpClient.EnableSsl = true;

                    // Increase timeout for connection
                    smtpClient.Timeout = 200000; // Timeout set to 200 seconds

                    smtpClient.Send(mail);
                }
            }
            catch (SmtpException smtpEx)
            {
                // Log SMTP-specific exceptions
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                // Log other exceptions
                Console.WriteLine($"General Error: {ex.Message}");
            }
        }



    }
}
