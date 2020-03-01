using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4Connector.Results
{
    public class FilesResult : Result
    {
        public FilesResult(IEnumerable<File> changes, bool status) : base(status)
        {
            files = changes as List<File>;
        }
        public FilesResult() : base(false)
        {
            files = null;
        }
        private List<File> files;

        public IEnumerable<File> Files { get => files; }

    }
}
