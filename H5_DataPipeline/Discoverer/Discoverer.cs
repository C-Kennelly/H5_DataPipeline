using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.DataPipeline;
using H5_DataPipeline.Assistants.Shared;
using H5_DataPipeline.Assistants.CompanyRosters;
using HaloSharp;
using H5_DataPipeline.Shared;
using H5_DataPipeline.Shared.Config;



namespace H5_DataPipeline.CompanyDiscovery
{
    public partial class Discoverer
    {
        IHaloSession haloSession;

        private void SetupHaloSharpComponents()
        {
            HaloClientFactory haloClientFactory = new HaloClientFactory();
            HaloClient haloClient = haloClientFactory.GetProdClient();
            haloSession = haloClient.StartSession();
        }

        public void SearchForNewCompanies(int companyCountThreshold)
        {

            SoftSearch();           //Search players we know about, but haven't checked.  
            //HardSearch();         //Search all matches for all players.

            
        }

        private void SoftSearch()
        {
            SetupHaloSharpComponents();
            

            List<string> unaffiliatedPlayers = GetUnaffiliatedPlayersFromDatabase();

            Console.WriteLine("Loading in-memory roster");
            inMemoryTeamRoster roster = new inMemoryTeamRoster();
            roster.RefreshInMemoryRoster();


            Console.WriteLine();
            Console.WriteLine("Iterating through players");

            int i = 0;

            /*
            Parallel.ForEach(unaffiliatedPlayers, player =>
            {
                string company = QueryForCompanyIDAndUpdateDatabaseAndRoster(player, roster, haloSession);
                //Console.WriteLine("{0} is on {1}.", player, company);
            });
            */
            
            foreach (string player in unaffiliatedPlayers)
            {
                string company = QueryForCompanyIDAndUpdateDatabaseAndRoster(player, roster, haloSession);
                Console.WriteLine("{0} of {1}:     {2} is on {3}", i, unaffiliatedPlayers.Count, player, company);
                i++;
            }
            
        }

        private List<string> GetUnaffiliatedPlayersFromDatabase()
        {
            List<string> result = new List<string>();

            using (var db = new dev_spartanclashbackendEntities())
            {
                Console.WriteLine("Grabbing all players in database..");
                List<string> allPlayers = db.t_players.Select(record => record.gamertag).ToList();
                Console.WriteLine("Found {0} player records", allPlayers.Count);

                Console.WriteLine("Grabbing affiliated players in database...");
                List<string> affiliatedPlayers = db.t_players_to_teams.Select(record => record.gamertag).ToList();
                Console.WriteLine("Found {0} player records", affiliatedPlayers.Count);

                Console.WriteLine("Down-selecting to only affiliated players");
                result = allPlayers.Except(affiliatedPlayers).ToList();
                Console.WriteLine("Returning {0} player records", result.Count);
            }

            return result;
        }

        private void HardSearch()
        {
            List<t_h5matches_playersformatch> matchesWithPlayers = new List<t_h5matches_playersformatch>();

            using (var db = new dev_spartanclashbackendEntities())
            {
                matchesWithPlayers = db.t_h5matches_playersformatch.Take(1).ToList();
                //.Where(match => match.discoveryRan == null).ToList();

                Console.WriteLine("Found {0} matches to search through.", matchesWithPlayers.Count);
            }

            List<string> unaffiliatedPlayers = GetAllUnaffiliatedPlayersinMatchSet(matchesWithPlayers);

            //ScanPlayersForNewCompanies(unaffiliatedPlayers);

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
