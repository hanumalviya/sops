namespace SOPS.Services.Mail
{
	public interface IMailService
	{
		void SendMail(string title, string message, string mailAddress);
	}
}
