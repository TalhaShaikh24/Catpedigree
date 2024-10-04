using Microsoft.Extensions.Hosting.Internal;
using System.Net;
using System.Net.Mail;
using WebApi.IRepositories;

namespace WebApi.Repositories
{
    public class EmailRepository: IEmailRepository
    {
        private string BaseUrl = "";
        private readonly IConfiguration _configuration;

        public EmailRepository(IConfiguration configuration)
        {
            BaseUrl = configuration.GetSection("UrlSetting").GetSection("baseApiUrl").Value ?? "";
            _configuration = configuration;
        }

        public void SendRejectionEmail(string toEmail, string reason)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                using (var mail = new MailMessage())
                using (var smtpClient = new SmtpClient(smtpSettings["Server"]))
                {
                    mail.From = new MailAddress(smtpSettings["SenderEmail"], smtpSettings["SenderName"]);
                    mail.To.Add(toEmail);
                    mail.Subject = "Listing Rejection Notice";
                    mail.Body = $"Your listing has been rejected for the following reason:\n\n{reason}";

                    smtpClient.Port = int.Parse(smtpSettings["Port"]);
                    smtpClient.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                    smtpClient.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                    smtpClient.Send(mail);
                }
            }
            catch (SmtpException smtpEx)
            {
                // Log SMTP-specific exceptions
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
                // Handle or rethrow according to your needs
            }
            catch (Exception ex)
            {
                // Log other exceptions
                Console.WriteLine($"General Error: {ex.Message}");
                // Handle or rethrow according to your needs
            }
        }
    }
}
