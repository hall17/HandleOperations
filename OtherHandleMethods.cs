using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    class OtherHandleMethods
    {



        public IntPtr GetHandleByProcessName(string processName)
        {
            IntPtr handle = IntPtr.Zero;
            Process p = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == processName);
            if (p != null) handle = p.MainWindowHandle;
            p.Dispose();
            return handle;
        }
        public IntPtr GetHandleByTitle(string mainWindowTitle)
        {
            IntPtr handle = IntPtr.Zero;
            Process p = Process.GetProcesses().FirstOrDefault(x => x.MainWindowTitle == mainWindowTitle);
            if (p != null) handle = p.MainWindowHandle;
            p.Dispose();
            return handle;
        }
        public IntPtr GetHandleById(int processId)
        {
            IntPtr handle = IntPtr.Zero;
            Process p = Process.GetProcesses().FirstOrDefault(x => x.Id == processId);
            if (p != null) handle = p.MainWindowHandle;
            p.Dispose();
            return handle;
        }
        public Process[] GetProcessList(string processName)
        {
            return Process.GetProcesses().Where(x => x.ProcessName == processName).ToArray();
        }

        public Process GetProcessByProcessName(string processName)
        {
            return Process.GetProcesses().FirstOrDefault(x => x.ProcessName == processName);
        }
        public Process GetProcessByHandle(IntPtr mainWindowHandle)
        {
            return Process.GetProcesses().FirstOrDefault(x => x.MainWindowHandle == mainWindowHandle);
        }
        public Process GetProcessById(int processId)
        {
            return Process.GetProcesses().FirstOrDefault(x => x.Id == processId);
        }
        public Process GetProcessByTitle(string mainWindowTitle)
        {
            return Process.GetProcesses().FirstOrDefault(x => x.MainWindowTitle == mainWindowTitle);
        }


        public bool IsProcessExists(int processId)
        {
            return (Process.GetProcesses().Any(x => x.Id == processId)) ? true : false;
        }
        public bool IsProcessExists(string processName)
        {
            return (Process.GetProcesses().Any(x => x.ProcessName == processName)) ? true : false;
        }
        public bool IsProcessExists(IntPtr mainWindowHandle)
        {
            return (Process.GetProcesses().Any(x => x.MainWindowHandle == mainWindowHandle)) ? true : false;
        }
        public bool IsProcessExistsTitle(string mainWindowTitle)
        {
            return (Process.GetProcesses().Any(x => x.MainWindowTitle == mainWindowTitle)) ? true : false;
        }


        public void KillProcess(Process process)
        {
            if (process != null) process.Kill();
        }
        public void KillProcessByProcessName(string processName)
        {
            // multiple process check need to be implemented.
            Process process = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == processName);
            if (process != null) process.Kill();
        }
        public void KillProcessByTitle(string mainWindowTitle)
        {
            // multiple process check need to be implemented.
            Process process = Process.GetProcesses().FirstOrDefault(x => x.MainWindowTitle == mainWindowTitle);
            if (process != null) process.Kill();
        }
        public bool KillProcessByHandle(IntPtr mainWindowHandle)
        {
            Process process = Process.GetProcesses().FirstOrDefault(x => x.MainWindowHandle == mainWindowHandle);
            if (process != null && process.Id != 0) process.Kill();
            return Process.GetProcesses().Any(x => x.MainWindowHandle == mainWindowHandle) ? false : true;
        }
        public bool KillProcesses(Process[] processes)
        {
            string processName = processes[0].ProcessName;
            foreach (var p in processes)
            {
                if (p != null) p.Kill();
            }
            return Process.GetProcesses().Any(x => x.ProcessName == processName) ? false : true;
        }
    }
}
