// (c) 2022 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System.Configuration;

namespace SharpJDKWrapper.Configuration
{
    /// <summary>
    /// Simple singleton configuration class
    /// </summary>
    public class Config
    {
        private static Config _instance;
        private static readonly object Padlock = new object();

        private Config()
        {
            JdkDirectory = ConfigurationManager.AppSettings["JDK_DIRECTORY"];
            JarDirectory = ConfigurationManager.AppSettings["JAR_DIRECTORY"];
            ApiAddress = ConfigurationManager.AppSettings["API_ADDRESS"];
        }

        public static Config Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Config();
                        }
                    }
                }
                return _instance;
            }
        }

        public string JdkDirectory { get; set; }
        public string JarDirectory { get; set; }
        public string ApiAddress { get; set; }
    }
}
