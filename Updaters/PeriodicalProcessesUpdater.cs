using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CSharpLab5.ViewModels;

namespace CSharpLab5.Updaters
{
    class PeriodicalProcessesUpdater
    {
        readonly MainWindowViewModel mainWindowViewModel;
        readonly SynchronizationContext synchronizationContext;

        public PeriodicalProcessesUpdater(MainWindowViewModel mainWindowViewModel)
        {
            synchronizationContext = SynchronizationContext.Current;
            this.mainWindowViewModel =
                mainWindowViewModel ?? throw new ArgumentNullException(nameof(mainWindowViewModel));
        }

        public void StartRefreshing(int collectionRefreshInterval, int processDataRefreshInterval)
       
        {
            if (collectionRefreshInterval < 0 || processDataRefreshInterval < 0)
            { throw new ArgumentException("Interval must be > 0 "); }

            if (mainWindowViewModel.Processes == null)
            { throw new ArgumentNullException(nameof(mainWindowViewModel.Processes)); }

            ProcessesUpdater.UpdateProcessCollection(ProcessFetcher.FetchProcesses(), mainWindowViewModel.Processes);

            Task.Run(() =>
            {
                UpdateProcessesCollectionPeriodicallyAsync(collectionRefreshInterval
                   ).Wait();
            });

        }

        async Task UpdateProcessesCollectionPeriodicallyAsync(int interval
            )
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(interval));

                Debug.Assert(mainWindowViewModel != null);
                Debug.Assert(mainWindowViewModel.Processes != null);

                IEnumerable<Process> processes = ProcessFetcher.FetchProcesses();

                synchronizationContext.Post(_ =>
                {
                }, 1);
              
            }
        }
    }
}