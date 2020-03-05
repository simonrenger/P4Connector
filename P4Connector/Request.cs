﻿using P4Connector.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P4Connector
{
    public class Request
    {
        public Request(Server server,AuthResult client)
        {
            Debug.Assert(client.Status);
            Server = server;
            Client = client.Client;
        }
        public Server Server { get; set; }
        public Client Client { get; set; }

        public ChangelistResult Changelist(int id,string workspace = null)
        {
            var result = Changelists(Status.None, workspace).FilterById(id);
            return new ChangelistResult(result, result != null);
        }
        public ChangelistsResult Changelists(Status status,string workspace = null)
        {
            var resulst = new List<Changelist>();
            string command = "-p " + Server.Host + " -u " + Client.Username + " changelists -u " + Client.Username + " -l";
            if(status != Status.None)
            {
                command += " -s ";
                command += P4Connector.Changelist.StatusToStr(status);
            }

            var output = Exec.Run(command);
            var list = output.Split('\n');
            for (var index = 0; index < list.Length - 2; index++)
            {
                var entry = list[index];
                var desc = list[index + 2].Trim();
                var match = Regex.Match(entry, @"Change (.*) on (.*) by (.*)@(.*) \*(pending|shelved|submitted)\*");
                if (match.Success)
                {
                    var groups = match.Groups;
                    var changelist = CreateChangelist(groups);
                    changelist.Description = desc;
                    if (workspace == null || workspace == changelist.Workspace)
                    {
                        resulst.Add(changelist);
                    }
                }
            }
            return new ChangelistsResult(resulst, resulst.Count > 0);
        }

        public FilesResult Files(int changeListId){
            return ReuqestFiles(changeListId);
        }
        public FilesResult Files(ChangelistResult changeListId)
        {
            Debug.Assert(changeListId);
            return ReuqestFiles(changeListId.Changelist.Id);
        }
        public FilesResult Shelved(int changeListId){
        return ReuqestFiles(changeListId,true);
        }
        public FilesResult Shelved(ChangelistResult changeListId){
            Debug.Assert(changeListId);
            return ReuqestFiles(changeListId.Changelist.Id,true);
        }
        private FilesResult ReuqestFiles(int changeListId, bool shelved = false)
        {
            var result = new List<File>();
            string command = "-p " + Server.Host + " -u " + Client.Username + " describe -s ";
                if(shelved) command += " -S ";
                command += changeListId;
            var output = Exec.Run(command);
            var list = output.Split('\n');
            for (var index = 0; index < list.Length - 2; index++)
            {
                 var match = Regex.Match(list[index], @"(\.\.\.\s\/\/(.*)\/(.*)#(.*)\s(.*))");
                if (match.Success)
                {
                    var groups = match.Groups;
                    var file = new File();
                    file.Path = groups[0].Value;
                    file.Name = groups[3].Value;
                    file.Revision = groups[4].Value;
                    file.Status = groups[5].Value;
                    result.Add(file);
                }
            }
            return new FilesResult(result,result.Count > 0);
        }

        public WorkspacesResult Workspaces()
        {
            var result = new List<Workspace>();
            string command = "-p " + Server.Host + " -u " + Client.Username + " workspaces -u "+ Client.Username;
            var output = Exec.Run(command);
            var list = output.Split('\n');
            for (var index = 0; index < list.Length - 2; index++)
            {
                 var match = Regex.Match(list[index], @"Client\s(.*)\s([0-9]+\/[0-9]+\/[0-9]+)\sroot\s(.*)\s'Created by(.*)'");
                if (match.Success)
                {

                    var groups = match.Groups;
                    var workspace = new Workspace();
                    workspace.Name = groups[1].Value;
                    workspace.root = groups[3].Value;
                    workspace.Creator = groups[4].Value;
                    workspace.Date = Convert.ToDateTime(groups[2].Value);
                    result.Add(workspace);
                }
            }
            return new WorkspacesResult(result, result.Count > 0);
        }

        private static Changelist CreateChangelist(GroupCollection groups)
        {
            var changelist = new Changelist();
            changelist.Id = Int32.Parse(groups[1].Value);
            changelist.Date = Convert.ToDateTime(groups[2].Value);
            changelist.User = groups[3].Value;
            changelist.Status = P4Connector.Changelist.StringToStatus(groups[5].Value);
            changelist.Workspace = groups[4].Value;
            return changelist;
        }
    }
}
