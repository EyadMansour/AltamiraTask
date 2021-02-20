using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using Shared;
using System;
using System.IO;
using System.Threading.Tasks;
using static System.Security.Authentication.SslProtocols;

namespace Application.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IWebHostEnvironment _env;

        public EmailService(IOptions<EmailSettings> emailSettings, IWebHostEnvironment env)
        {
            _emailSettings = emailSettings.Value;
            _env = env;
        }

        public async Task SendEmailAsync(MailModel model)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));

                mimeMessage.To.Add(new MailboxAddress(model.TargetMail));

                mimeMessage.Subject = model.Subject;
                var multipart = new Multipart("mixed") { new TextPart("html") { Text = model.Content } };
                foreach (var file in model.Attachments)
                {
                    var attachment = new MimePart()
                    {
                        Content = new MimeContent(File.OpenRead(file.FullName), ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(file.FullName)
                    };
                    multipart.Add(attachment);
                }
                mimeMessage.Body = multipart;

                using var client = new SmtpClient
                {
                    ServerCertificateValidationCallback = (s, c, h, e) => true,
                    SslProtocols = Tls | Tls11 | Tls12 | Tls13
                };
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)

                if (_env.IsDevelopment())
                {
                    // The third parameter is useSSL (true if the client should make an SSL-wrapped
                    // connection to the server; otherwise, false).
                    await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, SecureSocketOptions.StartTlsWhenAvailable).ConfigureAwait(false);
                }
                else
                {
                    await client.ConnectAsync(_emailSettings.MailServer).ConfigureAwait(false);
                }

                // Note: only needed if the SMTP server requires authentication
                await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password).ConfigureAwait(false);

                await client.SendAsync(mimeMessage).ConfigureAwait(false);

                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
