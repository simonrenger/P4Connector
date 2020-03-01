using P4Connector;
using System.Diagnostics;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("");
            Authentification auth = new Authentification(server);
            var result = auth.Login("", "");
            var session = auth.HasValidSession("");
            Debug.Assert(result == session);
            Request request = new Request(server, session);
            var changelists = request.Changelists(Status.Pending);
            Debug.Assert(changelists);
            var changelist = changelists[14179];
            Debug.Assert(changelist);
            var files = request.Files(changelist);
            Debug.Assert(files);
            var workspaces = request.Workspaces();
            Debug.Assert(workspaces);
        }
    }
}
