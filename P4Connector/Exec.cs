using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4Connector
{
    internal class Exec
    {
        static internal string Run(string p4command, string input = null)
        {
            try
            {
                using (Process p4process = new Process())
                {
                    p4process.StartInfo.UseShellExecute = false;
                    p4process.StartInfo.CreateNoWindow = true;
                    p4process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p4process.StartInfo.RedirectStandardOutput = true;
                    p4process.StartInfo.RedirectStandardError = true;
                    p4process.StartInfo.FileName = @"cmd.exe";
                    if (input != null)
                    {
                        p4process.StartInfo.Arguments = "/c echo " + input + "| p4 " + p4command;
                    }
                    else
                    {
                        p4process.StartInfo.Arguments = "/c p4 " + p4command;
                    }
                    p4process.Start();
                    // Synchronously read the standard output of the spawned process. 
                    StreamReader reader = p4process.StandardOutput;
                    string output = reader.ReadToEnd();
                    output += p4process.StandardError.ReadToEnd();
                    p4process.WaitForExit();
                    Debug.WriteLine(output);
                    return output;
                }
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message);
                return null;
            }
        }
    }
}
