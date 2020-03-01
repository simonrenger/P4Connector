using P4Connector.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P4Connector
{
    public class Authentification
    {
        public Authentification(Server server_)
        {
            server = server_;
        }
        private Server server;
        public AuthResult Login(string username, string password){
            var output = Exec.Run("-p " + server.Host + " -u " + username + " login", password);
            var match = Regex.Match(output, @"\b(invalid|failed)\b");
            if (match.Success)
            {
                return new AuthResult(new Client() { Username = username }, false);
            }
            else
            {
                var client = new Client();
                client.Username = username;
                var authResult = new AuthResult(client, true);
                RetriveToken(ref authResult);
                return authResult;
            }

        }
        public AuthResult HasValidSession(string username)
        {
            var output = Exec.Run("-p " + server.Host + " login -s");
            var match = Regex.Match(output, @"("+ Regex.Escape(username) +")");
            if (match.Success)
            {
                var client = new Client();
                client.Username = username;
                var authResult = new AuthResult(client, true);
                RetriveToken(ref authResult);
                return authResult;
            }
            return new AuthResult(new Client() { Username = username }, false);
        }
        public void RetriveToken(ref AuthResult authResult)
        {
            Debug.Assert(authResult.Status);
            if (!authResult.Status) return;
            var client = authResult.Client;
            var output = Exec.Run("-p " + server.Host + " -u " + client.Username + " tickets");
            var match = Regex.Match(output, @"\b(.*)\s((.*))\b");
            if (match.Success && match.Groups.Count == 4)
            {
                var token = match.Groups[3];
                client.Token = token.Value;
            }
            else
            {
                authResult = new AuthResult(client, false);
            }
         }
    }
}
