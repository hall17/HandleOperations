using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace HandleOperations
{
    public class HandleOps
    {
        public enum ProcessInfo
        {
            Process = 0,
            Handle = 1,
            ProcessId = 2,
            ProcessName = 3,
            MainWindowTitle = 4,
            MultipleProcess = 5,
        } 
        public IntPtr GetHandle(ProcessInfo type,object info)
        {
            Process p = new Process();
            IntPtr handle = IntPtr.Zero;
            switch (type)
            {
                case ProcessInfo.ProcessId:
                    p = Process.GetProcesses().FirstOrDefault(x => x.Id == (int)info);
                    if (p != null) handle = p.MainWindowHandle;
                    p.Dispose();
                    return handle;
                case ProcessInfo.ProcessName:
                    p = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == (string)info);
                    if (p != null) handle = p.MainWindowHandle;
                    p.Dispose();
                    return handle;
                case ProcessInfo.MainWindowTitle:
                    p = Process.GetProcesses().FirstOrDefault(x => x.MainWindowTitle == (string)info);
                    if (p != null) handle = p.MainWindowHandle;
                    p.Dispose();
                    return handle;
                default:
                    return IntPtr.Zero;
            }
        }
        public Process GetProcess(ProcessInfo type, object info)
        {
            switch (type)
            {
                case ProcessInfo.Handle:
                    return Process.GetProcesses().FirstOrDefault(x => x.MainWindowHandle == (IntPtr)info);
                case ProcessInfo.ProcessId:
                    return Process.GetProcesses().FirstOrDefault(x => x.Id == (int)info);
                case ProcessInfo.ProcessName:
                    return Process.GetProcesses().FirstOrDefault(x => x.ProcessName == (string)info);
                case ProcessInfo.MainWindowTitle:
                    return Process.GetProcesses().FirstOrDefault(x => x.MainWindowTitle == (string)info);
                default:
                    return null;
            }
        }
        public Process[] GetProcessList(ProcessInfo type, object info)
        {
            switch (type)
            {
                case ProcessInfo.ProcessName:
                    return Process.GetProcesses().Where(x => x.ProcessName == (string)info).ToArray();
                case ProcessInfo.MainWindowTitle:
                    return Process.GetProcesses().Where(x => x.MainWindowTitle == (string)info).ToArray();
                default:
                    return null;
            }
        }
        public bool IsProcessExists(ProcessInfo type, object info)
        {
            switch (type)
            {
                case ProcessInfo.Handle:
                    return (Process.GetProcesses().Any(x => x.MainWindowHandle == (IntPtr)info)) ? true : false;
                case ProcessInfo.ProcessId:
                    return (Process.GetProcesses().Any(x => x.Id == (int)info)) ? true : false;
                case ProcessInfo.ProcessName:
                    return (Process.GetProcesses().Any(x => x.ProcessName == (string)info)) ? true : false;
                case ProcessInfo.MainWindowTitle:
                    return (Process.GetProcesses().Any(x => x.MainWindowTitle == (string)info)) ? true : false;
                default:
                    return false;
            }
        }
        public void KillProcess(ProcessInfo type, object info)
        {
            Process p = new Process();
            switch (type)
            {
                case ProcessInfo.Process:
                    p = (Process)info;
                    if (p != null) p.Kill();
                    break;
                case ProcessInfo.Handle:
                    p = Process.GetProcesses().FirstOrDefault(x => x.MainWindowHandle == (IntPtr)info);
                    if (p != null) p.Kill();
                    break;
                case ProcessInfo.ProcessId:
                    p = Process.GetProcesses().FirstOrDefault(x => x.Id == (int)info);
                    if (p != null) p.Kill();
                    break;
                case ProcessInfo.ProcessName:
                    p = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == (string)info);
                    if (p != null) p.Kill();
                    break;
                case ProcessInfo.MainWindowTitle:
                    p = Process.GetProcesses().FirstOrDefault(x => x.MainWindowTitle == (string)info);
                    if (p != null) p.Kill();
                    break;
                case ProcessInfo.MultipleProcess:
                    foreach (var process in (Process[])info)
                    {
                        if (process != null) process.Kill();
                    }
                    break;
                default:
                    break;
            }
        }
        public void ProcessDisplayMode(ProcessInfo type, object info, DisplayMode mode)
        {
            IntPtr handle = GetHandle(type, info);
            ShowWindow(handle, (int)mode);
        }
        public void ProcessTransparency(ProcessInfo type, object info,int transparencyPercentage)
        {
            IntPtr handle = GetHandle(type, info);
            int value = transparencyPercentage * 255 / 100;
            SetLayeredWindowAttributes(handle,0, Convert.ToByte(value), LWA_ALPHA);
        }





        #region DLL Imports
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr handle, int nCmdShow);
        public const int LWA_ALPHA = 0x2;
        public enum DisplayMode
        {
            HIDE = 0,    //Hide,
            SHOWNORMAL = 1,
            SHOWMINIMIZED = 2,
            SHOWMAXIMIZED = 3,
            SHOWNOACTIVATE = 4, // Displays a window in its most recent size and position.            
            SHOW = 5,   // Displays a window in its most recent size and position
            MINIMIZE = 6,
            SHOWMINNOACTIVE = 7,
            SHOWNA = 8, //NA,
            RESTORE = 9, //Restore,          
            SHOWDEFAULT = 10,
        }   // Window conditions for the process
        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
        #endregion
    }
}
