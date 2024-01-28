using MailKit.Net.Smtp;
using MimeKit;

namespace service2.Services
{
    public class EmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly SmtpClient _smtp;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
            try
            {
                _smtp = new SmtpClient();
                _smtp.Connect(_configuration["SMTPHost"],
                    int.Parse(_configuration["SMTPPort"]),
                        MailKit.Security.SecureSocketOptions.StartTls);
                _smtp.Authenticate(_configuration["SMTPUser"], _configuration["SMTPPass"]);
                 { System.Console.WriteLine($"Email init Success ----> {_smtp.IsAuthenticated}"); }
            }
            catch (Exception ex)
            { System.Console.WriteLine($"Email init falied ----> {ex.Message}"); }

        }
        public void SendEmail(string sub, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration["SMTPUser"]));
                email.To.Add(MailboxAddress.Parse($"{sub}"));
                email.Subject = "To Subs";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
                _smtp.Send(email);
            }
            catch (Exception ex)
            { System.Console.WriteLine($"Email send falied ----> {ex.Message}"); }
        }
    }
}