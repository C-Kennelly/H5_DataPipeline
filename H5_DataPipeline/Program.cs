using System;
using System.Threading;

namespace H5_DataPipeline
{
    class Program
    {
        private const double hoursBetweenQueries = 12.00;

        static void Main(string[] args)
        {

            while(true)
            {
                DateTime cycleStart = DateTime.UtcNow;
                
                TimedCycle();
                
                DateTime cycleEnd = DateTime.UtcNow;
                TimeSpan span = cycleEnd.Subtract(cycleStart);
                
                if (span.TotalHours < hoursBetweenQueries)
                {
                    WaitForDuration((hoursBetweenQueries - span.TotalHours));
                }
            
            }


        }
        
        private static void WaitForDuration(double hoursToWait)
        {
            double fractionalMilliseconds = hoursToWait * 360 * 1000;

            int milliseconds = (int)Math.Round(fractionalMilliseconds, 0);

            Console.WriteLine();
            Console.WriteLine("Waiting for {0} hours before next cycle.", hoursToWait);
            Console.WriteLine();
            Thread.Sleep(milliseconds);
        }

        private static void TimedCycle()
        {
            DateTime startTime = DateTime.Now;

            RunCycle();

            PrintDurationSince(startTime);

        }

        private static void RunCycle()
        {
            Console.WriteLine("Hello, Infinity!");
            Console.WriteLine();

            Marshall marshall = new Marshall();
            marshall.ExecuteSpartanClashETL();


            Console.WriteLine();
            Console.WriteLine("Done!");
        }

        private static void PrintDurationSince(DateTime startTime)
        {
            DateTime endTime = DateTime.Now;
            TimeSpan span = endTime.Subtract(startTime);
            Console.WriteLine("Cycle took {0} days, {1} hours, {2} minutes, and {3} seconds", span.Days, span.Hours, span.Minutes, span.Seconds);
        }



        

    }
}
