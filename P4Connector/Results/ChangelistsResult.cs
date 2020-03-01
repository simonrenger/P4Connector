using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4Connector.Results
{
    public class ChangelistsResult : Result, IEnumerable<Changelist>
    {
        public ChangelistsResult(IEnumerable<Changelist> changes,bool status) : base(status)
        {
            changelists = changes as List<Changelist>;
        }
        public ChangelistsResult() : base(false)
        {
            changelists = null;
        }
        private List<Changelist> changelists;

        public IEnumerable<Changelist> Changelists { get=> changelists; }

        public IEnumerable<Changelist> FilterByStatus(Status status)
        {
            return changelists.FindAll((Changelist entry) => entry.Status == status);
        }
        public IEnumerable<Changelist> FilterByUser(string user)
        {
            return changelists.FindAll((Changelist entry) => entry.User == user);
        }
        public IEnumerable<Changelist> FilterByWorkspace(string workspace)
        {
            return changelists.FindAll((Changelist entry) => entry.Workspace == workspace);
        }
        public Changelist FilterById(int id)
        {
            return changelists.Find((Changelist entry) => entry.Id == id);
        }

        public ChangelistResult this[int id]
        {
                get{ var changelist = FilterById(id);
                return new ChangelistResult(changelist, changelist != null);
                    }
        }

        public IEnumerator<Changelist> GetEnumerator()
        {
            return changelists.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return changelists.GetEnumerator();
        }

        public int Count => changelists.Count;

    }
}
