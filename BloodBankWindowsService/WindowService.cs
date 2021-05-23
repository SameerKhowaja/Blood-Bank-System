using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Timers;

namespace BloodBankWindowsService
{
    class WindowService
    {
        private readonly Timer timer;
        public string question { get; set; }
        static public int waitTime;
        static public Action MyAction { get; set; }
        public WindowService()
        {
            this.timer = new Timer((waitTime * 1000) * 60)
            {
                AutoReset = true
            };
            this.timer.Elapsed += this.RunMyAction;
        }


        private void RunMyAction(object sender, ElapsedEventArgs e)
        {
            try
            {
                var msgs = new string[] { "BLOODBANK: " + question + " Time: " + DateTime.Now.ToString() };
                File.AppendAllLines(ConfigurationManager.AppSettings["success"], msgs);
                MyAction();
                var msg = new string[] { "BLOODBANK: " + question + " Time: " + DateTime.Now.ToString() };
                File.AppendAllLines(ConfigurationManager.AppSettings["success"], msg);
            }
            catch (Exception Ex)
            {
                var msg = new string[] { "BLOODBANK: " + question + " Reason: " + Ex.Message + " Time: " + DateTime.Now.ToString() };
                File.AppendAllLines(ConfigurationManager.AppSettings["failure"], msg);
                
            }

        }

        public void Start(string question)
        {
            this.question = question;
            this.timer.Start();

        }
        public void Stop()
        {

            this.timer.Stop();
        }
    }
}
