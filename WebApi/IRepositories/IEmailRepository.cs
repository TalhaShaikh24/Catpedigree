namespace WebApi.IRepositories
{
    public interface IEmailRepository
    {
        void SendRejectionEmail(string toEmail, string reason);
    }
}
