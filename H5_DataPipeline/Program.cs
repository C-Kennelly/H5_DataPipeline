using System;
using System.Collections.Generic;
using System.Threading;
using HaloSharp;
using HaloSharp.Model.Halo5.Stats;
using HaloSharp.Query.Halo5.Stats;
using HaloSharp.Extension;

using H5_DataPipeline.Assistants;


namespace H5_DataPipeline
{
    class Program
    {

        static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;

            RunCycle();

            PrintDurationSince(startTime);

            Console.ReadLine();
        }

        public static void RunCycle()
        {
            Console.WriteLine("Hello, Infinity!");
            Console.WriteLine();

            Marshall marshall = new Marshall();
            marshall.DoTheThing();


            Console.WriteLine();
            Console.WriteLine("Done!");
        }

        private static void PrintDurationSince(DateTime startTime)
        {
            DateTime endTime = DateTime.Now;


            TimeSpan span = endTime.Subtract(startTime);

            Console.WriteLine("Cycle took {0} days, {1} hours, {2} minutes, and {3} seconds", span.Days, span.Hours, span.Minutes, span.Seconds);
            Console.WriteLine("In other words, it took {0} minutes", span.TotalMinutes.ToString());


        }

    }
}
