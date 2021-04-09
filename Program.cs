using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace UiPath_Launcher
{
    class Program
    {

        static String getRobotPath()
        {
            String robotPath = "";
            String mainPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\UiPath\\";
            int mainverMax = 40, subverMax = 20, demoverMax = 10;

            foreach (var dirName in System.IO.Directory.GetDirectories(mainPath, "app*"))
            {
                for (int mainver = mainverMax; mainver > 19; mainver--)
                {
                    Task.Delay(1000);
                    for (int subver = subverMax; subver >= 0; subver--)
                    {
                        for (int demover = demoverMax; demover >= 0; demover--)
                        {
                            if (dirName.ToString().IndexOf(mainver.ToString() + "." + subver.ToString() + "." + demover.ToString()) >= 0)
                            {
                                robotPath = "\"" + dirName.ToString() + "\\UiRobot.exe\"";
                                mainver = -1;
                                subver = -1;
                                demover = -1;
                            }
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
                executePath = robotPath + " -file \"" + Environment.CurrentDirectory + "\\" + jsonObj.RunName + "\"";
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
                Console.WriteLine("[ Launcher Version ]");
                Console.ResetColor();
                Console.WriteLine("2.0");
                Console.WriteLine("");
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
