using System;

namespace NHMembership.Logging
{
    public interface ILogger
    {
        void Log(string message, Exception e);
    }
}