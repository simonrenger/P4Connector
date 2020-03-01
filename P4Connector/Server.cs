using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P4Connector
{
    public class Server
    {
        public Server(string host,string port)
        {
            Host = host;
            Port = port;
        }
        public Server(string host)
        {
            Host = host;
        }
        public string Host { get; set; }
        public string Port { get; set; }

        public static bool TestConnection(string host,string port = null)
        {
            var output = Exec.Run("-p " + host + " login -s");
            var match = Regex.Match(output, @"(Perforce client error:)");
            if (match.Success)
            {
                return false;
            }
            else
            {
                return true;
            }

            }
    }
}
