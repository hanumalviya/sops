using MailingService.MailingQueue;
using MailingService.Mailsender;
using System;
using System.Linq;

namespace MailingService.Contracts
{
    public class MailingService : IMailingService
    {
        private readonly IMailSheduler _mailSheduler;

        public MailingService(MailConfiguration config)
        {
            _mailSheduler = new MailSheduler(config);
        }

        public void Send(Message message)
        {
            _mailSheduler.SendMessage(message);
        }
    }
}
