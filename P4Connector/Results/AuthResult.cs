using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4Connector.Results
{
    public class AuthResult : Result
    {
        internal AuthResult(Client client, bool status): base(status)
        {
            _client = client;
           
        }

        internal AuthResult() : base(false)
        {
            _client = null;
        }

        private Client _client;

        public Client Client {get=>_client; }

    }
}
