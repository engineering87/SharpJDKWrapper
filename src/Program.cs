// (c) 2022 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using SharpJDKWrapper.Configuration;
using Microsoft.Owin.Hosting;
using System;
using System.ServiceProcess;

namespace SharpJDKWrapper
{
    static class Program
    {
        /// <summary>
        /// Start the application as Windows service or console mode
        /// </summary>
        static void Main()
        {
            using (WebApp.Start<Startup>(url: Config.Instance.ApiAddress))
            {
                if (Environment.UserInteractive)
                {
                    SharpJDKWrapper service = new SharpJDKWrapper();
                    service.TestStartupAndStop(new string[] { });
                    Console.ReadKey();
                }
                else
                {
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[]
                    {
                        new SharpJDKWrapper()
                    };
                    ServiceBase.Run(ServicesToRun);
                }
            }
        }
    }
}
