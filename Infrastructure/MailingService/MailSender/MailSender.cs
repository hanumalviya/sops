using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace MailingService.Mailsender
{
	public class MailSender : IMailSender
	{
		private readonly MailConfiguration _configuration;
		private SmtpClient _client = null;

		public MailSender(MailConfiguration configuration)
		{
            _configuration = configuration;
		}

        public void Connect()
        {
            _client = ConfigureSmtp();
        }

        public void Disconnect()
        {
            _client.Dispose();
            _client = null;
        }

		public void SendMail(string title, string message, string mailAddress)
		{
			using (var mail = new MailMessage(_configuration.EmailFrom, mailAddress))
			{
				mail.Subject = title;
				mail.Body = message;

				_client.Send(mail);
			}
		}

		public void SendMails(string title, string message, IEnumerable<string> mailAddresses)
		{
			using (var mail = new MailMessage())
			{
				mail.From = new MailAddress(_configuration.EmailFrom);
				mail.Subject = title;
				mail.Body = message;
				PrepareDeliveryList(mail.CC, mailAddresses);

				_client.Send(mail);
			}
		}

		private SmtpClient ConfigureSmtp()
		{
			var client = new SmtpClient
			{
				Host = _configuration.Host,
				Port = _configuration.Port,
				EnableSsl = _configuration.EnableSsl,
				DeliveryMethod = _configuration.DeliveryMethod,
				UseDefaultCredentials = _configuration.UseDefaultCredentials,
				Credentials = new NetworkCredential(_configuration.UserName, _configuration.Password),
			};

			return client;
		}

		private void PrepareDeliveryList(MailAddressCollection mailAddressCollection, IEnumerable<string> mailAddresses)
		{
			if (mailAddressCollection == null)
				throw new ArgumentNullException("mailAddressCollection");

			foreach (var mailAddress in mailAddresses)
			{
				mailAddressCollection.Add(new MailAddress(mailAddress));
			}
		}
    }
}
