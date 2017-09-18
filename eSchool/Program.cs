using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLinesFix
{
    class Program
    {
        static void Main(string[] args)
        {
            // input address: @"C:\Scripts\CanvasDenyl\input\Users.csv"
            // destination address: @"C:\Scripts\CanvasDenyl\input\ChangePLinesToDeleted.csv"
            // temp input: "C:\Users\mharbinger\Desktop\eSP\input\Users.csv"
            // temp destination: @"C:\Users\mharbinger\Desktop\eSP\input\ChangePLinesToDeleted.csv"

            // Initiate Stopwatch
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Declare file addresses
            string input = @"C:\Scripts\CanvasDenyl\input\Users.csv";
            string destination = @"C:\Scripts\CanvasDenyl\input\ChangePLinesToDeleted.csv";
            string logfile = @"C:\Scripts\CanvasDenyl\PLinesFix\PLinesFixLog.txt";

            // Create new destination file
            clean(destination);

            // Loop through file, make changes, output, record lines modified
            int linesWithP = 0;
            linesWithP = parseModifyCopy(input, destination);
            
            // Replace old file with new file
            shuffle(input, destination);

            // End Stopwatch
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            // Log Result
            using (StreamWriter w = File.AppendText(logfile))
            logResult(linesWithP, ts, w);
        }

        private static void logResult(int LWP, TimeSpan ts, TextWriter w)
        {
            w.Write("\r\n" + LWP + " lines modified on " + DateTime.Now + ", time elapsed: " + ts.Seconds + "." + ts.Milliseconds + " seconds.");
        }

        private static void shuffle(string input, string destination)
        {
            File.Delete(input);
            File.Copy(destination, input);
            File.Delete(destination);
        }

        private static int parseModifyCopy(string input, string destination)
        {
            int linesWithP = 0;
            var lines = File.ReadAllLines(input);

            using (StreamReader fileInput = new StreamReader(input))
            using (StreamWriter fileOutput = new StreamWriter(destination))
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i == 0)
                    {
                        fileOutput.WriteLine(lines[0]);
                    }
                    else
                    {
                        string[] vals = lines[i].Split(',');
                        if (vals[0][0] == 'P')
                        {
                            linesWithP++;
                            vals[7] = "deleted";
                        }

                        fileOutput.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}", vals[0], vals[1], vals[2],
                            vals[3], vals[4], vals[5], vals[6], vals[7]);
                    }
                }

            return linesWithP;
        }

        private static void clean(string destination)
        {
            if (File.Exists(destination))
            {
                File.Delete(destination);
            }
            if (!File.Exists(destination))
            {
             using (FileStream fs = File.Create(destination));
            }
        }
    }
}
