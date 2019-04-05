using System;
using System.Diagnostics;

namespace CSharpLab5
{

    class Process : ObservableObject
    {
        #region Fields

        string processName;
        int processId;
        bool isProcessActive;
        double cpuPercentage;
        long bytesNumber;
        int threadsNumber;
        string ownerName;
        string fileName;
        DateTime launchDateTime;
        ProcessModuleCollection modules;
        ProcessThreadCollection threads;

        readonly System.Diagnostics.Process process;

        #endregion


        #region Properties

        public string Name
        {
            get => processName;
            set => SetValue(ref processName, value);
        }
        public int Id
        {
            get => processId;
            set => SetValue(ref processId, value);
        }
        public bool IsProcActive 
        {
            get => isProcessActive;
            set => SetValue(ref isProcessActive, value);
        }
        public double CpuPercentage
        {
            get => cpuPercentage;
            set => SetValue(ref cpuPercentage, value);
        }
        public long BytesNumber
        {
            get => bytesNumber;
            set => SetValue(ref bytesNumber, value);
        }
        public int ThreadsNumber
        {
            get => threadsNumber;
            set => SetValue(ref threadsNumber, value);
        }
        public string OwnerName 
        {
            get => ownerName;
            set => SetValue(ref ownerName, value);
        }
        public string FileName
        {
            get => fileName;
            set => SetValue(ref fileName, value);
        }
        public DateTime LaunchDateTime
        {
            get => launchDateTime;
            set => SetValue(ref launchDateTime, value);
        }
        public ProcessModuleCollection Modules 
        {
            get => modules;
            set => SetValue(ref modules, value);
        }
        public ProcessThreadCollection Threads
        {
            get => threads;
            set => SetValue(ref threads, value);
        }

        #endregion

        #region Constructor

        public Process(System.Diagnostics.Process process)
        {
            if(process == null)
                { throw new ArgumentNullException(nameof(process)); }

            if(process.HasExited)
                { throw new ArgumentException("You are not able to end the process that has exited!"); }

            this.process = process;

            RefreshProperties(process);

        }

        #endregion

        public void Refresh()
        {
            throw new NotImplementedException();
        
            if(process.HasExited)
                { return; }

            process.Refresh();

        }

        public void Kill()
        {
            process.Kill();
        }

        void RefreshProperties(System.Diagnostics.Process upToDateProcess)
        {
            Debug.Assert(!upToDateProcess.HasExited);

            Name = upToDateProcess.ProcessName;
            Id = upToDateProcess.Id;
            IsProcActive = upToDateProcess.Responding;
            CpuPercentage = ProcessHelper.GetCpuPercentage(upToDateProcess);
            BytesNumber = upToDateProcess.WorkingSet64;
            ThreadsNumber = upToDateProcess.Threads.Count;
            OwnerName = ProcessHelper.GetProcessOwner(upToDateProcess);
            FileName = upToDateProcess.MainModule.FileName;
            LaunchDateTime = upToDateProcess.StartTime;
            Modules = upToDateProcess.Modules;
            Threads = upToDateProcess.Threads;
        }

    }
}
