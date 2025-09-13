
using MailKit.Security;
using MimeKit;
using System.Net.Mail;

namespace yourself_demoAPI.Repository.EmailSender
{
	public class SmtpEmailSender : ISmtpEmailSender
	{
		private readonly IConfiguration _Configuration;

		public SmtpEmailSender(IConfiguration configuration)
		{
			_Configuration = configuration;
		}

		public async Task SendAsync(string to, string subject, string body)
		{
			try
			{
				var msg = new MimeMessage();
				msg.From.Add(new MailboxAddress("Yourself Verification", _Configuration["Email:User"]));
				msg.To.Add(MailboxAddress.Parse(to));
				msg.Subject = subject;
				msg.Body = new TextPart("plain") { Text = body };

				using var client = new MailKit.Net.Smtp.SmtpClient();
				await client.ConnectAsync(_Configuration["Email:Host"], 587, SecureSocketOptions.Auto);
				await client.AuthenticateAsync(_Configuration["Email:User"], _Configuration["Email:Pass"]);
				await client.SendAsync(msg);
				await client.DisconnectAsync(true);

				Console.WriteLine($"Email sent to {to}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to send email: {ex.Message}");
				throw;
			}
		}
	}
}
