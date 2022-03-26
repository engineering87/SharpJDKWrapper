// (c) 2022 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;
using System.Diagnostics;

namespace SharpJDKWrapper.Entity
{
    /// <summary>
    /// Class for encapsulating the system Process
    /// </summary>
    public class WrapperProcess
    {
        public Guid Id { get; set; }
        public string ServiceFileName { get; set; }
        public Process Process { get; set; }

        /// <summary>
        /// Create a new instance of WrapperProcess
        /// </summary>
        /// <param name="process">The system Process</param>
        public WrapperProcess(Process process)
        {
            this.Process = process;
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// Create a new instance of WrapperProcess
        /// </summary>
        /// <param name="process">The system Process</param>
        /// <param name="serviceFileName">The JAR filename</param>
        public WrapperProcess(Process process, string serviceFileName)
        {
            this.Process = process;
            this.Id = Guid.NewGuid();
            this.ServiceFileName = serviceFileName;
        }
    }
}
