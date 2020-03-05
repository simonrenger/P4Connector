using P4Connector.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4Connector
{
    public class P4
    {
        private Server server_;
        private Authentification auth_;
        private Request request_ = null;
        private Action action_ = null;
        private AuthResult user_;
        public P4(string host,string port = null)
        {
            if (Server.TestConnection(host, port)){
                server_ = new Server(host, port);
                auth_ = new Authentification(server_);
            }
            else
            {
                throw new Exception("Cannot connect to Server");
            }
        }

        public bool Auth(string username,string password,string workspace = null)
        {
            user_ = auth_.Login(username, password, workspace);
            if(user_){
                request_ = new Request(server_, user_);
             }
            return user_;
        }
        public ChangelistsResult Changelists(Status status = Status.None,string workspace = null) { return request_.Changelists(status,workspace); }
        public ChangelistResult Changelist(int id,string workspace = null) { return request_.Changelist(id, workspace); }
        public bool IsLoggedIn(string username,string workspace = null) {
            user_ = auth_.HasValidSession(username,workspace);
            return user_;
        }
        public AuthResult AuthResult { get=>user_; }
        public Request Request { get {
                if (request_ == null)
                {
                    request_ = new Request(server_, user_);
                }
                return request_; } }
        public Action Action
        {
            get
            {
                if (action_ == null)
                {
                    action_ = new Action(server_, user_);
                }
                return action_;
            }
        }
    }
}
