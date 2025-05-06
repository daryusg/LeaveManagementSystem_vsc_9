using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Logging;

namespace LeaveManagementSystem.Common;

//cip...111
public class EmailSender(IConfiguration _configuration, ILogger<EmailSender> _logger) : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        //30/04/25 send email from azure app service
        if (Misc.IsAzureEnv())
        {
            try
            {
                var connectionString = _configuration["COMMUNICATION_SERVICES_CONNECTION_STRING"]; //01/05/25 copied from "communicationservice-666 | Try Email" which i emailed myself from @ 1352.
                var emailClient = new EmailClient(connectionString);

                var msg = new Azure.Communication.Email.EmailMessage(
                    senderAddress: _configuration["DefaultEmailAddress"],
                    content: new EmailContent(subject)
                    {
                        Html = htmlMessage
                    },
                    recipients: new EmailRecipients(new List<EmailAddress> { new EmailAddress(email) })
                );
                EmailSendOperation emailSendOperation = await emailClient.SendAsync(WaitUntil.Completed, msg);

                if (emailSendOperation.HasCompleted)
                {
                    _logger.LogInformation($"Email sent: {emailSendOperation.Id}");
                }
                else
                {
                    _logger.LogWarning("Email operation did not complete.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email using ACS");
            }
        }
        else
        {
            var fromAddress = _configuration["EmailSettings:DefaultEmailAddress"];
            var smtpServer = _configuration["EmailSettings:Server"];
            var smtpPort = _configuration["EmailSettings:Port"];

            var msg = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            msg.To.Add(new MailAddress(email));

            using var client = new SmtpClient(smtpServer, Convert.ToInt32(smtpPort));
            await client.SendMailAsync(msg);
        }
    }
}
