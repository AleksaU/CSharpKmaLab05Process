using System;
using System.Diagnostics;
using System.Management;

namespace CSharpLab5
{
    static class ProcessHelper
    {
        
        public static double GetCpuPercentage(System.Diagnostics.Process process)
        {
            using(var pcProcess = 
                new PerformanceCounter("Process", "% Processor Time", process.ProcessName))
            {
                return pcProcess.NextValue();
            }
        }

        public static string GetProcessOwner(System.Diagnostics.Process process)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + process.Id;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                var argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                 
                    return argList[1] + "\\" + argList[0];
                }
            }

            return "NO OWNER";
        }
    }
}
