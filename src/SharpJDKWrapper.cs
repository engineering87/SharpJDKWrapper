// (c) 2022 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using NLog;
using System;
using System.ServiceProcess;

namespace SharpJDKWrapper
{
    public partial class SharpJDKWrapper : ServiceBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public SharpJDKWrapper()
        {
            InitializeComponent();
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);

            Logger?.Info("Starting SharpJDKWrapper in debug mode....");

            Console.ReadLine();

            this.OnStop();
        }

        protected override void OnStart(string[] args)
        {
            Logger?.Info("Starting SharpJDKWrapper....");

            Wrapper.Wrapper.ExecuteJar();

            Logger?.Info("SharpJDKWrapper started");
        }

        protected override void OnStop()
        {
            Logger?.Info("Stopping SharpJDKWrapper....");

            Wrapper.Wrapper.StopProcesses();

            Logger?.Info("SharpJDKWrapper stopped");
        }
    }
}
