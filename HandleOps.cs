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
        public IntPtr GetHandle(object info)
        {
            Process p = null;
            IntPtr handle = IntPtr.Zero;
            switch (info.GetType().Name)
            {
                case "Int32":
                    p = Process.GetProcesses().FirstOrDefault(x => x.Id == (int)info);
                    if (p != null)
                    {
                        handle = p.MainWindowHandle;
                        p.Dispose();
                    }
                    return handle;
                case "String":
                    string str = (string)info;
                    if (str.EndsWith(".exe"))
                    {
                        str = ((string)info).Substring(0, str.Length - 4);
                        p = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == str);
                        if (p != null)
                        {
                            handle = p.MainWindowHandle;
                            p.Dispose();
                        }
                        return handle;
                    }
                    else
                    {
                        p = Process.GetProcesses().FirstOrDefault(x => x.MainWindowTitle == (string)info);
                        if (p != null)
                        {
                            handle = p.MainWindowHandle;
                            p.Dispose();
                        }
                        return handle;
                    }
                case "IntPtr":
                    return (IntPtr)info;
                default:
                    return handle;
            }
        }
        public Process GetProcess(object info)
        {
            switch (info.GetType().Name)
            {
                case "IntPtr":
                    return Process.GetProcesses().FirstOrDefault(x => x.MainWindowHandle == (IntPtr)info);
                case "Int32":
                    return Process.GetProcesses().FirstOrDefault(x => x.Id == (int)info);
                case "String":
                    string str = (string)info;
                    if (str.EndsWith(".exe"))
                    {
                        str = str.Substring(0, str.Length - 4);
                        return Process.GetProcesses().FirstOrDefault(x => x.ProcessName == str);
                    }
                    else
                        return Process.GetProcesses().FirstOrDefault(x => x.MainWindowTitle == str);
                default:
                    return null;
            }
        }
        public Process[] GetProcessList(object titleorProcessName)
        {
            object info = titleorProcessName;
            switch (info.GetType().Name)
            {              
                case "String":
                    string str = (string)info;
                    if (str.EndsWith(".exe"))
                    {
                        str = ((string)info).Substring(0, str.Length - 4);
                        return Process.GetProcesses().Where(x => x.ProcessName == str).ToArray();
                    }
                    else
                    {
                        return Process.GetProcesses().Where(x => x.MainWindowTitle == (string)info).ToArray();
                    }
                default:
                    return null;
            }
        }
        public bool IsProcessExists(object info)
        {
            switch (info.GetType().Name)
            {
                case "IntPtr":
                    return (Process.GetProcesses().Any(x => x.MainWindowHandle == (IntPtr)info)) ? true : false;
                case "Int32":
                    return (Process.GetProcesses().Any(x => x.Id == (int)info)) ? true : false;
                case "String":
                    string str = (string)info;
                    if (str.EndsWith(".exe"))
                    {
                        str = ((string)info).Substring(0, str.Length - 4);
                        return (Process.GetProcesses().Any(x => x.ProcessName == str)) ? true : false;
                    }
                    else
                    {
                        return (Process.GetProcesses().Any(x => x.MainWindowTitle == (string)info)) ? true : false;
                    }
                default:
                    return false;
            }
        }
        public void KillProcess(object info)
        {
            Process p = null;
            string namz = info.GetType().Name;
            switch (info.GetType().Name)
            {
                case "Process":
                    p = (Process)info;
                    if (p != null) p.Kill();
                    break;
                case "Process[]":
                    string processName = ((Process[])info)[0].ProcessName;
                    foreach (var pr in (Process[])info)
                    {
                        if (pr != null) pr.Kill();
                    }
                    break;
                case "IntPtr":
                    p = Process.GetProcesses().FirstOrDefault(x => x.MainWindowHandle == (IntPtr)info);
                    if (p != null) p.Kill();
                    break;
                case "Int32":
                    p = Process.GetProcesses().FirstOrDefault(x => x.Id == (int)info);
                    if (p != null) p.Kill();
                    break;
                case "String":
                    string str = (string)info;
                    if (str.EndsWith(".exe"))
                    {
                        str = ((string)info).Substring(0, str.Length - 4);
                        p = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == str);
                        if (p != null) p.Kill();
                        break;
                    }
                    else
                    {
                        p = Process.GetProcesses().FirstOrDefault(x => x.MainWindowTitle == (string)info);
                        if (p != null) p.Kill();
                        break;
                    }
                default:
                    break;
            }
        }
        public void ProcessDisplayMode(object info, DisplayMode mode)
        {
            IntPtr handle = GetHandle(info);
            ShowWindow(handle, (int)mode);
        }
        public void ProcessTransparency(object info, int transparencyPercentage)
        {
            IntPtr handle = GetHandle(info);
            int value = transparencyPercentage * 255 / 100;
            SetLayeredWindowAttributes(handle, 0, Convert.ToByte(value), LWA_ALPHA);
        }
        public void ProcessTopMost(object info, bool isTopmost)
        {
            IntPtr handle = GetHandle(info);
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
