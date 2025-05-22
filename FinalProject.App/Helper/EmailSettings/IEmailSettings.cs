using FinalProject.Data.Models.SendEmailModel;

namespace FinalProject.App.Helper.EmailSettings
{
    public interface IEmailSettings
    {
        public void SendEmail(Email email);
    }
}
