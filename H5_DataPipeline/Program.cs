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
            Console.WriteLine("Hello, Infinity!");
            Console.WriteLine();

            Marshall marshall = new Marshall();
            marshall.DoTheThing();

            //DoTheThing();

            Console.WriteLine();

            Console.WriteLine("Done!");
            Console.ReadLine();
        }


        private static async void DoTheThing()
        {
            HaloClientFactory haloClientFactory = new HaloClientFactory();
            HaloClient client = haloClientFactory.GetDevClient();
            string companyId = "a23876ac-321e-497d-933b-65e226d01b2f";

            using (var session = client.StartSession())
            {
                var query = new GetSpartanCompany(new Guid(companyId));
                SpartanCompany result = await session.Query(query);


                Console.WriteLine("Have result!");
            }
        }


    }
}
