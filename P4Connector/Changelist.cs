using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4Connector
{
    public enum Status
    {
        Pending,
        Shelved,
        Submitted,
        None
    };
    public class Changelist
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public IEnumerable<File> Files {get;set;}
        public IEnumerable<File> Shelved {get;set;}
        public DateTime Date { get; set; }
        public string User { get; set; }
        public string Workspace { get; set; }

        public static string StatusToStr(Status status)
        {
            switch (status)
            {
                case Status.Pending: return "pending";
                case Status.Shelved: return "shelved";
                case Status.Submitted: return "submitted";
                default:             return null;
            }
        }
        public static Status StringToStatus(string status)
        {
            switch (status)
            {
                case "Pending":
                case "pending": return Status.Pending;
                case "shelved":
                case "Shelved": return Status.Shelved;
                case "Commited":
                case "commited":
                case "submitted":
                case "Submitted": return Status.Submitted;
                default: return Status.None;
            }
        }
    }
}
