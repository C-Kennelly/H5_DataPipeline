using System;
using System.Collections.Generic;

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
            RunCycle();
        }

        public static void RunCycle()
        {
            Console.WriteLine("Hello, Infinity!");
            Console.WriteLine();

            Marshall marshall = new Marshall();
            marshall.DoTheThing();


            Console.WriteLine();
            Console.WriteLine("Done!");
            Console.ReadLine();

        }

    }
}
