using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.DataPipeline;
using H5_DataPipeline.Assistants.Shared;
using H5_DataPipeline.Assistants.CompanyRosters;


namespace H5_DataPipeline.CompanyDiscovery
{
    class Discoverer
    {
        public void SearchForNewCompanies(int companyCountThreshold)
        {
            List<t_h5matches_playersformatch> matchesWithPlayers = new List<t_h5matches_playersformatch>();

            using (var db = new dev_spartanclashbackendEntities())
            {
                matchesWithPlayers = db.t_h5matches_playersformatch.Take(1).ToList();
                //.Where(match => match.discoveryRan == null).ToList();

                Console.WriteLine("Found {0} matches to search through.", matchesWithPlayers.Count);
            }

            List<string> unaffiliatedPlayers = GetAllUnaffiliatedPlayersinMatchSet(matchesWithPlayers);

            ScanPlayersForNewCompanies(unaffiliatedPlayers);

            unaffiliatedPlayers.ForEach(item => Console.WriteLine(item));
        }

        private List<string> GetAllUnaffiliatedPlayersinMatchSet(List<t_h5matches_playersformatch> matchSet)
        {
            List<string> result = new List<string>();

            foreach(t_h5matches_playersformatch match in matchSet)
            {
                result.AddRange(match.GetAllUnaffiliatedGamertagsInMatch());
            }
            
            return result;
        }
    }
}
