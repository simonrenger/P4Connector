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
                ReadClientInformation(ref authResult);
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
                ReadClientInformation(ref authResult);
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
        public void ReadClientInformation(ref AuthResult authClient)
        {
             Debug.Assert(authClient.Status);
                string command = "-p " + server.Host + " -u " + authClient.Client.Username + " client -o";
                var output = Exec.Run(command);
                var filtered = String.Join(Environment.NewLine,output
                    .Split('\n')
                    .Where(entry => !entry.Contains('#'))
                    .Select(entry => entry));
                var client = new Client();
                var match = Regex.Match(filtered, @"Client:(.*)Update",RegexOptions.Singleline);
                if(match.Success) {
                    var result = match.Groups;
                    client.Workspace = result[1].Value.Trim().Replace("\r","").Replace("\t","").Replace("\n","");
                }
                match = Regex.Match(filtered, @"Update:(.*)Access",RegexOptions.Singleline);
                if(match.Success) {
                    var result = match.Groups;
                    client.Update = DateTime.Parse(result[1].Value);
                }
                match = Regex.Match(filtered, @"Access:(.*)Owner",RegexOptions.Singleline);
                if(match.Success) {
                    var result = match.Groups;
                    client.Access = DateTime.Parse(result[1].Value);
                }
                match = Regex.Match(filtered, @"Owner:(.*)Host",RegexOptions.Singleline);
                if(match.Success) {
                    var result = match.Groups;
                    client.Username = result[1].Value.Trim().Replace("\r","").Replace("\t","").Replace("\n","");
                }
                match = Regex.Match(filtered, @"Root:(.*)[^Submit]Options:",RegexOptions.Singleline);
                if(match.Success) {
                    var result = match.Groups;
                    client.Root = result[1].Value.Trim().Replace("\r","").Replace("\t","").Replace("\n","");
                }
                authClient = new AuthResult(client,authClient);
        }
    }
}
