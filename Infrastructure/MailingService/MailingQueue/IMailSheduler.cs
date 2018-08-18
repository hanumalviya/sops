using MailingService.Contracts;
using System;
using System.Linq;

namespace MailingService.MailingQueue
{
    public interface IMailSheduler
    {
        void SendMessage(Message message);
    }
}
