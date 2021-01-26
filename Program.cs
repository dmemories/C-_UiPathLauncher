using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UiPath_Launcher
{
    class Program
    {

        static String getRobotPath()
        {
            String robotPath = "";
            int maxVer = 20;
            for (int i = 40; i >= 19; i--)
            {
                Task.Delay(1000);
                for (int j = maxVer; j >= 0; j--)
                {
                    for (int k = maxVer; k >= 0; k--)
                    {
                        robotPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\UiPath\\app-" + i.ToString() + "." + j.ToString() + "." + k.ToString();
                        if (Directory.Exists(robotPath))
                        {
                            robotPath = "\"" + robotPath + "\\UiRobot.exe\"";
                            i = -1;
                            j = -1;
                            k = -1;
                        }
                    }
                }
            }
            return robotPath;
        }

        static void Main(string[] args)
        {
            
            String robotPath = getRobotPath();
            String configFilePath = Environment.CurrentDirectory + "\\UiPath Launcher.json";
            String executePath;
            Boolean confirmRun;
            Boolean pauseWhenFinished;

            if (File.Exists(configFilePath))
            {
                String jsonTxt = System.IO.File.ReadAllText(configFilePath);
                var jsonObj = JsonConvert.DeserializeAnonymousType(jsonTxt, new { RunName = "", WantPauseWhenFinished = "", WantConfirmRun = "" });
                executePath = robotPath + " execute --file \"" + Environment.CurrentDirectory + "\\" + jsonObj.RunName + "\"";
                confirmRun = ((jsonObj.WantConfirmRun == "1") ? true : false);
                pauseWhenFinished = ((jsonObj.WantPauseWhenFinished == "1") ? true : false);
            }
            else
            {
                executePath = robotPath + " execute --file \"" + Environment.CurrentDirectory + "\\Main.xaml\"";
                confirmRun = true;
                pauseWhenFinished = false;
            }

            if (confirmRun)
            {
                /* Console.WriteLine("[Robot Path]");
                 Console.WriteLine(robotPath);
                 Console.WriteLine("");*/
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ Execute Path ]");
                Console.ResetColor();
                Console.WriteLine(executePath);
                Console.WriteLine("");
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Press any key to run !");
                Console.ResetColor();
                Console.ReadLine();
            }
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Running...");
            Console.ResetColor();
            System.Diagnostics.Process process = new System.Diagnostics.Process();
             System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
             startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
             startInfo.FileName = "cmd.exe";
             startInfo.RedirectStandardInput = true;
             startInfo.RedirectStandardOutput = true;
             startInfo.CreateNoWindow = true;
             startInfo.UseShellExecute = false;
             process.StartInfo = startInfo;
             process.Start(); 
             process.StandardInput.WriteLine(executePath);
             process.StandardInput.Flush();
             process.StandardInput.Close();
             process.WaitForExit();

            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Finished !");
            Console.ResetColor();
            if (pauseWhenFinished) {
                Console.ReadLine();
            }

        }
    }
}
