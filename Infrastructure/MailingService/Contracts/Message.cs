using System;
using System.Linq;

namespace MailingService.Contracts
{
    public class Message
    {
        public string Title { get; set; }

        public string Receiver { get; set; }

        public string Body { get; set; }
    }
}