using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4Connector
{
    public class Client
    {
        public string Workspace { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime Update { get; set; }
        public DateTime Access { get; set; }
        public string Host { get; set; }
        public string Root { get; set; }
    }
}
