using P4Connector.Results;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;
using System;

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
        // Unshelves all files!
        public bool Unshelve(ChangelistResult changelist)
        {
            return Unshelve(changelist, new ChangelistResult());
        }
        // Unshelves all files!
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
       public bool Shelve(ChangelistResult changelist){
            if(!changelist || changelist.Changelist.Files == null) return false;
            var fileList = new List<File>(changelist.Changelist.Files);
            if(fileList.Count <= 0) return false;
            var files = fileList.Select(item => item.Path);
            return Shelve(files,changelist.Changelist.Id);
        }
       public bool Shelve(IEnumerable<string> files,ChangelistResult changelist){
            if(!changelist) return false;
            return Shelve(files,changelist.Changelist.Id);
        }
        public bool Shelve(IEnumerable<string> files,int id,string workspace = null){
            //p4 shelve --parallel=0 -f -c 139446
            var command =  "-p " + Server.Host + " -u " + Client.Username;
            if(workspace != null){
            command += " -c "+workspace;
            }else if(Client.Workspace != null){
                command += " -c "+Client.Workspace;
            }
            command += " shelve --parallel=0 -f -c "+id+" "+string.Join(" ",files);
            var output = Exec.Run(command);
            var match = Regex.Match(output, @"shelved");
            return match.Success;
        }        
        public bool UpdateChangelist(int changelist,string description,string workspace = null){
            var command =  "-p " + Server.Host + " -u " + Client.Username;
            var subcommand =  "-p " + Server.Host + " -u " + Client.Username;
            if(workspace != null){
            command += " -c "+workspace;
            subcommand += " -c "+workspace;
            }else if(Client.Workspace != null){
                command += " -c "+Client.Workspace;
                subcommand += " -c "+Client.Workspace;
            }
            command += "--field Description=\""+description+"\"  change -o "+changelist;
            command +=" | "+subcommand;
            var output = Exec.Run(command);
            var match = Regex.Match(output, @"updated");
            return match.Success;
        }

        public ChangelistResult NewChangelist(string description,string workspace = null){
            var command =  "-p " + Server.Host + " -u " + Client.Username;
            var subcommand =  "p4 -p " + Server.Host + " -u " + Client.Username;
            if(workspace != null){
            command += " -c "+workspace;
            subcommand += " -c "+workspace;
            }else if(Client.Workspace != null){
                command += " -c "+Client.Workspace;
                subcommand += " -c "+Client.Workspace;
                workspace = Client.Workspace;
            }
            command += " --field Description=\""+description+"\" --field \"Files=\"  change -o ";
            subcommand += " change -i";
            command +=" | "+subcommand;
            var output = Exec.Run(command);
            var match = Regex.Match(output, @"Change\s([0-9]+)\screated");
            if(match.Success){
                return new ChangelistResult(
                   new Changelist{
                   Id = Int32.Parse(match.Groups[1].Value),
                   Description = description,
                   Workspace = workspace,
                   User = Client.Username,
                   Status = Status.Pending,
                   Date = DateTime.Now
                   },
                   true
                );
            }
            return new ChangelistResult(null,false);
            

        }

        public bool MoveFiles(IEnumerable<string> files,int destChangelist = -1,string workspace = null){
            var command =  "-p " + Server.Host + " -u " + Client.Username;
            if(workspace != null){
            command += " -c "+workspace;
            }else if(Client.Workspace != null){
                command += " -c "+Client.Workspace;
            }
            command += " reopen ";
            if(destChangelist != -1){
            command += " -c "+destChangelist;
            }else{
            command +=" -c default";
            }
            command += string.Join(" ",files);
             var output = Exec.Run(command);
             var match = Regex.Match(output, @"reopened");
            return match.Success;
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