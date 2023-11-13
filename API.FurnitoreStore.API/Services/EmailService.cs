using API.FurnitoreStore.API.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;


namespace API.FurnitoreStore.API.Services
{
    public class EmailService : IEmailSender
    {
        //se trae la variable global
        private readonly SmtpSettings _smtpSettings;
        //se iguala la variable local a la global
        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //codigo para enviar cualquier email
            try
            {
                var message = new MimeMessage();
                //configurar objeto tipo MimeMessage
                message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = htmlMessage };

                //abrir SMTP client y enviarlo
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpSettings.Server);
                    await client.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
