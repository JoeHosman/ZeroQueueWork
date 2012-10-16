using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDPClientLibrary
{
    public class MDPClient
    {
        private readonly string _providerAddress;

        public MDPClient(string providerAddress)
        {
            _providerAddress = providerAddress;
        }

        public string SendMessage(string message, string service)
        {
            string reply = string.Empty;

            return reply;
        }
    }
}
