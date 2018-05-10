using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.DataPipeline;


namespace H5_DataPipeline.CompanyDiscovery
{
    class Discoverer
    {
        public void SearchForNewCompanies(int companyCountThreshold)
        {
            List<t_h5matches_playersformatch> matchesWithPlayers = new List<t_h5matches_playersformatch>();

            using(var db = new dev_spartanclashbackendEntities())
            {
                matchesWithPlayers = db.t_h5matches_playersformatch.Take(1).ToList();
                    //.Where(match => match.discoveryRan == null).ToList();

                Console.WriteLine("Found {0} matches to search through.", matchesWithPlayers.Count);
            }

            foreach(t_h5matches_playersformatch match in matchesWithPlayers)
            {
                Console.WriteLine("MatchID: {0}", match.matchID);
                List<string> unaffiliatedPlayers = match.GetAllUnaffiliatedGamertagsInMatch();

                PrintListofString(unaffiliatedPlayers);
            }


        }

        private void PrintListofString(List<string> list)
        {
            foreach(string item in list)
            {
                Console.WriteLine(item);
            }
        }
    }
}
