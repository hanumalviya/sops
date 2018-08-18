using System;
using System.Linq;
using System.Net.Mail;

namespace MailingService.Mailsender
{
    public class MailConfiguration
    {
        public string EmailFrom { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public bool EnableSsl { get; set; }

        public SmtpDeliveryMethod DeliveryMethod { get; set; }

        public bool UseDefaultCredentials { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}