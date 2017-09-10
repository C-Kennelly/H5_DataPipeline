using System;
using System.Collections.Generic;

using H5_DataPipeline.Assistants;


namespace H5_DataPipeline
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, Infinity!");
            Console.WriteLine();

            FieldMarshall marshall = new FieldMarshall();
            marshall.DoTheThing();

            Console.WriteLine();

            Console.WriteLine("Done!");
            Console.ReadLine();
        }



//        private static List<t_players> MakeTestPlayerList()
//        {
//            List<t_players> testPlayerList = new List<t_players> {
//                new t_players("Sn1p3r C")//,
//                new t_players("Black Picture"),
//                new t_players("Randy 355"),
//                new t_players("ADarkerTrev"),
//                new t_players("Ray Benefield")
//            };
//
//            return testPlayerList;
//        }

    }
}
