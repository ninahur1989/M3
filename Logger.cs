using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace M3
{

    public class Logger
    {
        private Logs _log = new Logs();
        private readonly object _lock = new object();
        private const int _cycle = 21;
        private event Action Backup;
        private Action delegateToBackup = () => { Console.WriteLine("you need make a backup of your logs"); };
        public int N { get; set; }
        public string Path { get; set; }

        public void GetJson()
        {
            var json = JsonConvert.DeserializeObject<Logger>(File.ReadAllText(@"C:\Users\Admin\source\repos\M3\M3\Config.json"));
            N = json.N;
            Path = json.Path;
        }

        public async void FileWriterAsync()
        {

            Logs logs = new Logs();
            Task a = null ;
            for (int i = 1; i < _cycle; i++)
            {
                //File.AppendAllText(@"C:\\Users\Admin\source\repos\M3\M3\MainLogs.txt", _log.LogGenerator());
                //File.AppendAllText(@"C:\\Users\Admin\source\repos\M3\M3\MainLogs.txt", " \n");
                //if (i % N == 0)
                //{
                //    await Task.Run(() =>
                //    {
                //        Backup.Invoke();
                //        BackUpWriter();
                //    });
                //}
                logs.Currentlog.Add(logs.LogGenerator());

                if (i % N == 0)
                {
                    if (a != null)
                    {
                        Task.WaitAll(a);
                    }

                    a = new Task(() =>
                    {
                        
                        Backup.Invoke();
                        BackUpWriter();
                    });

                    File.WriteAllLines(@"C:\\Users\Admin\source\repos\M3\M3\MainLogs.txt", logs.Currentlog);
                    a.Start();
                    Console.WriteLine(a.Status);
                    await a;
                }
            }
        }

        public void BackUpWriter()
        {
            string currentpath = Path + "Backuplogs\\";
            File.Copy(Path + "MainLogs.txt", currentpath + DateTime.UtcNow.ToFileTime() + ".txt");
        }

        public void Config()
        {
            GetJson();
            Backup = delegateToBackup;
        }
    }
}
