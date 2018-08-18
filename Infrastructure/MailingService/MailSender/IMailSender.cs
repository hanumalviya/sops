using System.Collections.Generic;

namespace MailingService.Mailsender
{
	public interface IMailSender
	{
		void SendMail(string title, string message, string mailAddress);
		void SendMails(string title, string message, IEnumerable<string> mailAddresses);

        void Connect();
        void Disconnect();
	}
}
