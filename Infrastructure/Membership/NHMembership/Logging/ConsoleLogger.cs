using System;
using System.Linq;

namespace NHMembership.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message, Exception e)
        {
            Console.WriteLine(message);
        }
    }
}
