using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace BloodBankWindowsService
{
    class Host
    {
        public static int RunWindowsService(string SetServiceName, string SetDisplayName, string SetDescription, string question)
        {
            return (int)Convert.ChangeType(HostFactory.Run
                (
                 service =>
                 {
                     service.Service<WindowService>(pg =>
                     {
                         pg.ConstructUsing(processGenerator => new WindowService());
                         pg.WhenStarted(processGenerator => processGenerator.Start(question));
                         pg.WhenStopped(processGenerator => processGenerator.Stop());

                     });
                     service.RunAsLocalSystem();
                     service.SetServiceName(SetServiceName);
                     service.SetDisplayName(SetDisplayName);
                     service.SetDescription(SetDescription);


                 }
                ), typeof(TopshelfExitCode));
        }
    }
}
