using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSharpLab5.Updaters
{
    static class ProcessesUpdater
    {
        public static void UpdateProcessCollection(IEnumerable<Process> newProcesses, ObservableCollection<Process> processes)
        {
            if(processes == null)
            {
                throw new System.ArgumentNullException(nameof(processes));
            }
            processes.Clear();

            foreach (Process p in newProcesses)
            {
                processes.Add(p);
            }
        }

        public static void RefreshData(ObservableCollection<Process> processes)
        {
            foreach (Process data in processes)
            {
                data.Refresh();
            }
        }
    }
}
