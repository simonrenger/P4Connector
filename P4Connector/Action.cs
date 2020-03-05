using P4Connector.Results;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace P4Connector
{
    public class Action
    {
        public Action(Server server, AuthResult client)
        {
            Debug.Assert(client.Status);
            Server = server;
            Client = client.Client;
        }
        public bool Unshelve(ChangelistResult changelist)
        {
            return Unshelve(changelist, new ChangelistResult());
        }
        public bool Unshelve(ChangelistResult changelist, ChangelistResult target)
        {
            if (changelist)
            {
                string command = "-p " + Server.Host + " -u " + Client.Username + " -c "+Client.Workspace+" unshelve  -s " + changelist.Changelist.Id;
                if (target) { command += " -c " + target.Changelist.Id; } else
                {
                    command += " -c " + changelist.Changelist.Id;
                }
                var output = Exec.Run(command); //TODOD: Verfiy if it actually works!
                var match = Regex.Match(output, @"unshelved");
                return match.Success;
            }
            else
            {
                return false;
            }
        }

        public bool Logout() {
                string command = "-p " + Server.Host + " -u " + Client.Username + " logout";
                var output = Exec.Run(command); //TODOD: Verfiy if it actually works!
                var match = Regex.Match(output, @"logged\sout");
                return match.Success;
        }

        public Server Server { get; }
        public Client Client { get; }
    }

}