using P4Connector;
using System.Diagnostics;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("ssl:swarm2.nhtv.nl:1667");
            Authentification auth = new Authentification(server);
            var result = auth.Login("Simon170636", "fahrrad1");
            var session = auth.HasValidSession("Simon170636");
            Debug.Assert(result == session);
            Request request = new Request(server, session);
            var changelists = request.Changelists(Status.Pending);
            Debug.Assert(changelists);
            var changelist = changelists[14179];
            Debug.Assert(changelist);
            var files = request.Files(changelist);
            Debug.Assert(files);
        }
    }
}
