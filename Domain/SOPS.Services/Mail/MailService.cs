using MailingService.Contracts;
using MailingService.Mailsender;
using System;
using System.Linq;

namespace SOPS.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly IMailingService _service;

        public MailService(MailConfiguration conf)
        {
            _service = new MailingService.Contracts.MailingService(conf);
        }

        public void SendMail(string title, string message, string mailAddress)
        {
            _service.Send(new Message() { Title = title, Body = message, Receiver = mailAddress });
        }
    }
}
