using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4Connector.Results
{
    public class WorkspacesResult : Result
    {
        public WorkspacesResult(IEnumerable<Workspace> workspaces, bool status) : base(status)
        {
            this.workspaces = workspaces as List<Workspace>;
        }
        public WorkspacesResult() : base(false)
        {
            workspaces = null;
        }
        private List<Workspace> workspaces;

        public IEnumerable<Workspace> Workspaces { get => workspaces; }

    }
}
