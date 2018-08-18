using System;
using System.Linq;

namespace MailingService.Contracts
{
    public interface IMailingService
    {
        void Send(Message message);
    }
}
