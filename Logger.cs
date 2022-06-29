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
        private const int _cycle = 51;
        private readonly object _lock = new object();
        private Action _delegateToBackup = () => { Console.WriteLine("you need make a backup of your logs"); };
        private event Action Backup;
        public int N { get; set; }
        public string Path { get; set; }

        public void GetJson()
        {
            var json = JsonConvert.DeserializeObject<Logger>(File.ReadAllText(@"C:\Users\Admin\source\repos\M3\M3\Config.json"));
            N = json.N;
            Path = json.Path;
        }

        public async Task FileWriterAsync()
        {
            Logs logs = new Logs();
            for (int i = 1; i < _cycle; i++)
            {
                logs.Currentlog.Add(logs.LogGenerator());

                if (i % N == 0)
                {
                    lock (_lock)
                    {
                        for (int d = i - N; d < i; d++)
                        {
                            File.AppendAllText(Path + "\\MainLogs.txt", logs.Currentlog[d] + "\n");
                        }
                    }

                    await Task.Run(() =>
                    {
                        Backup.Invoke();
                        BackUpWriter(logs.Currentlog);
                    });
                }
            }
        }

        public void BackUpWriter(List<string> logs)
        {
            string currentpath = Path + "Backuplogs\\";
            File.WriteAllLines(currentpath + DateTime.UtcNow.ToFileTime() + ".txt", logs);
        }

        public void Config()
        {
            GetJson();
            Backup = _delegateToBackup;
        }
    }
}
