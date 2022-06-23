using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3
{
    public class Logs
    {
        private List<string> _logs = new List<string> { "Event log ", "Server log", "System log ", "Change log", "Availability log ", "Resource log" };
        public List<string> Currentlog = new List<string>();
        public string Message { get; set; }
        public int IdLog { get; set; }
        public string LogGenerator()
        {
            Logs log = new Logs();
            Random random = new Random();
            log.Message = _logs[random.Next(0, 6)];
            log.IdLog = random.Next(0, 10000);
            return log.Message + " " + log.IdLog.ToString();
        }
    }
}
