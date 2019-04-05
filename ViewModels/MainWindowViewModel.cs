using System.Collections.ObjectModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CSharpLab5.Updaters;
using CSharpLab5.Views;

namespace CSharpLab5.ViewModels
{
    class MainWindowViewModel : ObservableObject
    {
        const int CollectionUpdateInterval = 1;
        const int ProcessesRefreshInterval = 1;

        Process selectedProcess;
        ObservableCollection<Process> processes;
        PeriodicalProcessesUpdater processUpdater;


        public ObservableCollection<Process> Processes
        {
            get => processes;
            set
            {
                if (value == null)
                { throw new NullReferenceException(nameof(value)); }
                SetValue(ref processes, value);
            }
        }
        public Process SelectedProcess
        {
            get => selectedProcess;
            set => SetValue(ref selectedProcess, value);
        }

        public ICommand ShowModulesForSelectedProcessCommand { get; } 
        public ICommand ShowThreadsForSelectedProcessCommand { get; }
        public ICommand OpenSelectedProcessDirCommand { get; }
        public ICommand KillAndRemoveSelectedProcessCommand { get; }


        public MainWindowViewModel()
        {
            Processes = new ObservableCollection<Process>();

            processUpdater = new PeriodicalProcessesUpdater(this);
            processUpdater.StartRefreshing(CollectionUpdateInterval, ProcessesRefreshInterval);

            ShowModulesForSelectedProcessCommand = new RelayCommand(ShowModulesForSelectedProcess, _ => ProcessSelected());
            ShowThreadsForSelectedProcessCommand = new RelayCommand(ShowThreadsForSelectedProcess, _ => ProcessSelected());
            OpenSelectedProcessDirCommand = new DelegateCommandAsync(OpenSelectedProcessDir, _ => ProcessSelected());
            KillAndRemoveSelectedProcessCommand = new RelayCommand(KillAndRemoveSelectedProcess, _ => ProcessSelected());
        }

        #region ContextMenu commands
        void ShowModulesForSelectedProcess(object o)
        {
            Debug.Assert(ProcessSelected());

            var listV = new ModulesInfoView
            {
                DataContext = SelectedProcess,
            };

            var modulesWindow = new Window()
            {
                Width = 450,
                Height = 600,
                Content = listV
            };

            modulesWindow.ShowDialog();
        }

        void ShowThreadsForSelectedProcess(object o)
        {
            Debug.Assert(ProcessSelected());

            var listView = new ListOfThreadView
            {
                DataContext = SelectedProcess,
            };

            var modulesWindow = new Window()
            {
                Width = 450,
                Height = 600,
                Content = listView
            };

            modulesWindow.ShowDialog();
        }

        async Task OpenSelectedProcessDir(object o)
        {
            Debug.Assert(ProcessSelected());

            var k = Path.GetDirectoryName(SelectedProcess.FileName);
            k = $"explorer.exe \"{Path.GetDirectoryName(SelectedProcess.FileName)}\"";
            System.Diagnostics.Process.Start("explorer.exe", $"\"{Path.GetDirectoryName(SelectedProcess.FileName)}\"");
        }

        void KillAndRemoveSelectedProcess(object o)
        {
            Debug.Assert(ProcessSelected());

            SelectedProcess.Kill();
            Processes.Remove(SelectedProcess);
            SelectedProcess = null;
        }
        #endregion

        bool ProcessSelected()
        {
            return SelectedProcess != null;
        }   

    }
}