using FinalProject.Data.Models.SendEmailModel;
using FinalProject.App.Helper.EmailSettings;
using Microsoft.Extensions.Options;

using FinalProject.App.Utility.EmailSettings;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace FinalProject.App.Helper.EmailSettings
{
    public class EmailSettings : IEmailSettings
    {
        private readonly EmailSetting _options;

        public EmailSettings(IOptions<EmailSetting> options)
        {
            _options = options.Value;
        }
        public void SendEmail(Email email)
        {
            var mail = new MimeMessage()
            {
                Sender=MailboxAddress.Parse(_options.Email),
                Subject=email.Subject
            };
            mail.To.Add(MailboxAddress.Parse(email.To));
            mail.From.Add(MailboxAddress.Parse(_options.Email));

            var body = new BodyBuilder();
            body.TextBody = email.Body;

            mail.Body = body.ToMessageBody();
            using var smtp = new SmtpClient();

            smtp.Connect( _options.Host,_options.Port,SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email, _options.Password);

            smtp.Send(mail);

            smtp.Disconnect(true);
        }

        
    }
}
