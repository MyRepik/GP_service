using System.Diagnostics;
using System.Timers;
using ProcessNames = System.Collections.Generic.List<string>;

namespace GP_service
{
    class GP_ProcessesMonitor
    {
            private readonly Timer _timer_update_black_list;
            private readonly Timer _timer_monitor_processes;

            ProcessNames blackList;

            public GP_ProcessesMonitor()
            {
                // login based on application login
                _timer_update_black_list = new Timer(100000) { AutoReset = true };
                _timer_update_black_list.Elapsed += UpdateBlackListTimerElapsed;
                _timer_monitor_processes = new Timer(5000) { AutoReset = true };
                _timer_monitor_processes.Elapsed += MonitorProcessTimerElapsed;

            }

            private void UpdateBlackListTimerElapsed(object sender, ElapsedEventArgs e)
            {
                // connect to db, update blacklist variable
            }

            private void MonitorProcessTimerElapsed(object sender, ElapsedEventArgs e)
            {
                var processes = Process.GetProcesses();

                foreach (var process in processes)
                {
                    if (blackList.Contains(process.ProcessName) && process.WorkingSet64 > 100000000)
                    {
                        foreach (var proc in Process.GetProcessesByName(process.ProcessName))
                        {
                            proc.Kill();
                        }

                    }
                }
            }

            public void Start()
            {
                _timer_update_black_list.Start();
                _timer_monitor_processes.Start();

            }

            public void Stop()
            {
                //disconnect db
                _timer_update_black_list.Start();
                _timer_monitor_processes.Start();
            }

        }
}

