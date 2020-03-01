using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4Connector.Results
{
    public class Result
    {
        public Result(bool status)
        {
            Status = status;
        }
        public bool Status { get; }
        public static bool operator ==(Result v1, bool v2)
        {
            return v1.Status == v2;
        }
        public static bool operator !=(Result v1, bool v2)
        {
            return v1.Status != v2;
        }
        public static implicit operator bool(Result auth) => auth.Status;
    }
}
