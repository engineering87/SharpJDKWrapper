// (c) 2022 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using SharpJDKWrapper.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using SharpJDKWrapper.Entity;

namespace SharpJDKWrapper.Wrapper
{
    /// <summary>
    /// Main wrapper class
    /// </summary>
    public sealed class Wrapper
    {
        #region DllImport

        [StructLayout(LayoutKind.Sequential)]
        internal struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public UInt32 dwProcessId;
            public UInt32 dwThreadId;
        }

        const UInt32 WAIT_OBJECT_0 = 0x00000000;
        const UInt32 INFINITE = 0xFFFFFFFF;

        [StructLayout(LayoutKind.Sequential)]
        internal struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct STARTUPINFO
        {
            public int cb;
            public IntPtr lpReserved;
            public IntPtr lpDesktop;
            public IntPtr lpTitle;
            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;
            public int dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [DllImport("kernel32.dll", EntryPoint = "CreateProcess", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CreateProcess(string appName, StringBuilder cmdLine, IntPtr processAttr, IntPtr threadAttr, bool inheritHandles, int creationFlag, IntPtr environment, IntPtr curDir, ref STARTUPINFO startupInfo, ref PROCESS_INFORMATION processInfo);

        [DllImport("kernel32.dll", EntryPoint = "WaitForSingleObject", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 timeOut);

        [DllImport("kernel32.dll", EntryPoint = "GetExitCodeProcess", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        static extern bool GetExitCodeProcess(IntPtr hHandle, out Int32 timeOut);

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", CallingConvention = CallingConvention.Winapi, SetLastError = true)]
        static extern bool CloseHandle(IntPtr hHandle);

        #endregion

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static Dictionary<string, WrapperProcess> services = new Dictionary<string, WrapperProcess>();

        /// <summary>
        /// Get all the JAR files to execute
        /// </summary>
        public static void ExecuteJar()
        {
            try
            {
                if (!Directory.Exists(Config.Instance.JarDirectory))
                {
                    Logger?.Warn("No JarDirectory found");
                    return;
                }

                var jarFiles = Directory.GetFiles(Config.Instance.JarDirectory, "*.jar", SearchOption.AllDirectories);
                foreach (var jarFile in jarFiles)
                {
                    RunJarProcess(jarFile);
                }
            }
            catch (Exception ex)
            {
                Logger?.Error(ex);
            }
        }

        /// <summary>
        /// Execute the specific JAR file
        /// </summary>
        /// <param name="jarFileName">The Jar to execute</param>
        private static void RunJarProcess(string jarFileName)
        {
            var proc = new Process();

            try
            {
                proc.EnableRaisingEvents = false;
                proc.StartInfo.WorkingDirectory = Config.Instance.JdkDirectory;
                proc.StartInfo.FileName = "java.exe";
                proc.StartInfo.Arguments = string.Concat($"-jar {jarFileName}");

                Logger?.Info($"Starting {jarFileName}...");

                var wrapperProcess = new WrapperProcess(proc, Path.GetFileName(jarFileName));

                services.Add(wrapperProcess.Id.ToString(), wrapperProcess);

                proc.Start();

                Logger?.Info($"{jarFileName} started");

                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Logger?.Error($"Jar {jarFileName} failed to execute {ex.Message}");
            }
        }

        /// <summary>
        /// Get the current list of active Java services
        /// </summary>
        /// <returns></returns>
        public static string GetActiveServices()
        {
            var builder = new StringBuilder();
            builder.Append("{\"services\":[");
            foreach (KeyValuePair<string, WrapperProcess> service in services)
            {
                builder.Append($"\"{service.Value.Id}\",");
            }
            builder.Remove(builder.Length - 1, 1);
            builder.Append("]}");
            return builder.ToString();
        }

        /// <summary>
        /// Get the current count of active Java services
        /// </summary>
        /// <returns></returns>
        public static int GetActiveServicesCount()
        {
            return services.Count;
        }

        /// <summary>
        /// Get the current status of a specific Java service
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool GetStatusService(string id)
        {
            try
            {
                if (services.ContainsKey(id))
                {
                    return services[id].Process.Responding;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger?.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// Kill a specific Java service
        /// </summary>
        /// <param name="id"></param>
        public static void StopProcess(string id)
        {
            try
            {
                if (services.ContainsKey(id))
                {
                    services[id].Process?.Kill();
                    services.Remove(id);

                    Logger?.Info($"Stopped service with id= \"{id}\"");
                }
            }
            catch (Exception ex)
            {
                Logger?.Error(ex);
            }
        }

        /// <summary>
        /// Stop all the Java services
        /// </summary>
        public static void StopProcesses()
        {
            try
            {
                foreach (KeyValuePair<string, WrapperProcess> service in services)
                {
                    service.Value.Process?.Kill();

                    Logger?.Info($"Stopped service with id= \"{service.Key}\"");
                }
            }
            catch (Exception ex)
            {
                Logger?.Error(ex);
            }
        }
    }      
}
