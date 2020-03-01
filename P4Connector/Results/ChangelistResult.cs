using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4Connector.Results
{
    public class ChangelistResult : Result
    {
        public ChangelistResult(Changelist change,bool status) : base(status)
        {
            Changelist = change;
        }
        public ChangelistResult(): base(false) {
            Changelist = null;
        }
        public Changelist Changelist { get; }
    }
}
