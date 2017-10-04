using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServicePyramid
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServicePyramid()
            };
            ServiceBase.Run(ServicesToRun);

            //if (Environment.UserInteractive)
            //{
            //    var service = new ServicePyramid();
            //    service.RunAsConsole();
            //}
            //else
            //{
            //    var servicesToRun = new ServiceBase[]
            //    {
            //        new ServicePyramid()
            //    };
            //    ServiceBase.Run(servicesToRun);
            //}
        }
    }
}
