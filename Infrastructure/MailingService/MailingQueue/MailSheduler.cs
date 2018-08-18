using MailingService.Contracts;
using MailingService.Mailsender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace MailingService.MailingQueue
{
    public class MailSheduler : IMailSheduler
    {
        private readonly Queue<Message> _messages;
        private readonly IMailSender _mailSender;
        private readonly Timer _timer;

        public MailSheduler(MailConfiguration conf)
        {
            _messages = new Queue<Message>();
            _mailSender = new MailSender(conf);
             
            _timer = new Timer(30000);
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
            
        }

        public void SendMessage(Message message)
        {
            _messages.Enqueue(message);
        }

        private static object sendingMessages = new object();
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            lock (sendingMessages)
            {
                int count = _messages.Count;

                if (count > 0)
                {
                    _mailSender.Connect();

                    var list = new List<Message>();


                    for (int i = 0; i < count; i++)
                    {
                        list.Add(_messages.Dequeue());
                    }

                    foreach (var m in list)
                    {
                        _mailSender.SendMail(m.Title, m.Body, m.Receiver);
                    }

                    _mailSender.Disconnect();
                }
            }
        }
    }
}