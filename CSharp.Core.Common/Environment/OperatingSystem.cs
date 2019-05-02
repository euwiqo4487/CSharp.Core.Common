using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 作業系統資訊
    /// </summary>
    public static class OperatingSystem
    {
        private static bool is64BitProcess = (IntPtr.Size == 8);
        /// <summary>
        /// 是否64位元，否則32
        /// </summary>
        public static bool Is64Bit = is64BitProcess || InternalCheckIsWow64();

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );

        private static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
