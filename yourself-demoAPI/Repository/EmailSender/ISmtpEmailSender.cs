namespace yourself_demoAPI.Repository.EmailSender
{
	public interface ISmtpEmailSender
	{
		Task SendAsync(string to, string subject, string body);
	}
}
