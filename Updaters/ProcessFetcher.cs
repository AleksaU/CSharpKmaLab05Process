using System;
using System.Collections.Generic;

namespace CSharpLab5.Updaters
{
    static class ProcessFetcher
    {
        static readonly HashSet<string> InaccessibleProcesses;

        static ProcessFetcher()
        {
            InaccessibleProcesses = new HashSet<string>
            {
                "bash", 
                "services",
                "svchost",
                "smss",
                "SecurityHealthService",
                "wininit",
                "csrss",
                "init",
                "Memory Compression",
                "System",
                "Idle",
                "git",
                "git-remote-http"
            };
        }

        public static IEnumerable<Process> FetchProcesses()
        {
            var res =  new List<Process>();

            System.Diagnostics.Process[] processes = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in processes)
            {
                try
                {
                    if (InaccessibleProcesses.Contains(p.ProcessName) || p.HasExited)
                        { continue; }

                    res.Add(new Process(p));
                }
                catch (System.ComponentModel.Win32Exception e)
                {
                    Console.WriteLine($"Error in: {p.ProcessName} , {ProcessHelper.GetProcessOwner(p)}");
                }
        }            
            return res;
        }
    }
}
