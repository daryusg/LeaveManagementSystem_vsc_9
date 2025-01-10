using System.Net.Mail;

namespace LeaveManagementSystem.Web.Services.Email;

//cip...111
public class EmailSender(IConfiguration _configuration) : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
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
