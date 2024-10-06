using LigaWeb.Helpers.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;

namespace LigaWeb.Helpers.Impl
{
    public class MailHelper : IMailHelper 
    {
        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Response SendEmail(string to, string subject, string body)
        {
            var nameFrom = _configuration["Mail:Namefrom"];
            var from = _configuration["Mail:From"];
            var smpt = _configuration["Mail:Smtp"];
            var port = int.Parse(_configuration["Mail:Port"]);
            var password = _configuration["Mail:Password"];
            var passwordApp = _configuration["Mail:PasswordApp"];

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(nameFrom, from));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;

            var bodybuilder = new BodyBuilder
            {
                HtmlBody = body,
            };

            message.Body = bodybuilder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect(smpt, port, false);
                    client.Authenticate(from, passwordApp);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.ToString()
                };
            }

            return new Response
            {
                IsSuccess = true
            };
        }
    }
}
