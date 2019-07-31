using System;
using System.Diagnostics;

namespace PageNotifier.Worker
{
    public class WebsiteShower : IWebsiteShower
    {
        public void ShowWebsite(string url)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/r explorer \"{url}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            process.WaitForExit();
            Console.Beep();
        }
    }
}