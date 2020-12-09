using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
        public void ProcessDisplayMode(IntPtr handle, DisplayMode mode)
        {
            ShowWindow(handle, (int)mode);
        }
        public void ProcessTransparency(IntPtr handle, int transparencyPercentage)
        {
            int value = transparencyPercentage * 255 / 100;
            SetLayeredWindowAttributes(handle, 0, Convert.ToByte(value), LWA_ALPHA);
        }
        public void ProcessTopMost(IntPtr handle, bool isTopmost)
        {
            IntPtr hwndInsertAfter = isTopmost ? HWND_TOPMOST : HWND_NOTOPMOST;
            SetWindowPos(handle, hwndInsertAfter, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
        }



        #region DLL Imports
        const uint SWP_NOSIZE = 0x0001; // needed to make top most and set location(0,0)
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_SHOWWINDOW = 0x0040;
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_LAYERED = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
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
