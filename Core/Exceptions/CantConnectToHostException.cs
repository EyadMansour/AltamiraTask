using System;

namespace Core.Exceptions
{
    public class CantConnectToHostException : Exception
    {
        public CantConnectToHostException(string host = "host") : base($"Cant Connect to {host} !")
        {

        }
    }
}
