using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp.Model;
using HaloSharp.Extension;
using HaloSharp.Model.Halo5.Profile;
using HaloSharp.Query.Halo5.Profile;
using HaloSharp;

namespace H5_DataPipeline
{
    class TeamRosterRefresher
    {
        private List<t_players_to_teams> rosterToUpdate;
        private List<t_players> playersToScan;
        private DateTime thresholdDateTime;
        private const string noCompanyFoundValue = "NOCOMPANYFOUND";


        public TeamRosterRefresher(int thresholdToUpdateInDays)
        {
            thresholdDateTime = DateTime.UtcNow.AddDays(-1 * thresholdToUpdateInDays);

            
        }

        public void RefreshTeamRosters()
        {
            RefreshAllPlayersCompanyRosters();
            //RefreshAllPlayersCustomTeamRosters();
        }

        private void RefreshAllPlayersCompanyRosters()
        {
            SetPlayersToScan();
            ScanPlayers();
            //UpdateDatabase();
        }

        private void SetPlayersToScan()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                playersToScan = new List<t_players>(3) { new t_players("Sn1p3r C"), new t_players("Black Picture"), new t_players("Randy 355") };
                    //db.t_players.Where(player => player.dateCompanyRosterUpdated < thresholdDateTime).ToList();
            }
        }

        private void ScanPlayers()
        {
            HaloClientFactory haloClientFactory = new HaloClientFactory();

            foreach (t_players player in playersToScan)
            {
                Task<Company> t = GetPlayerCompany(player.gamertag, haloClientFactory.GetProdClient());
                t.Start();

                Company company = t.Result;
                t.Wait();

                if (company != null)
                {
                    Console.WriteLine("Company for player {0} is {1}", player.gamertag, company.Name);
                }
                else
                {
                    Console.WriteLine("Company for player {0} is {1}", player.gamertag, noCompanyFoundValue);
                }


                //Dostuffwiththecompanyroster
            }
        }

        private async Task<Company> GetPlayerCompany(string gamertag, HaloClient client)
        {
            using (var session = client.StartSession())
            {
                var query = new GetPlayerAppearance(gamertag);

                PlayerAppearance playerAppearance = await session.Query(query);

                return playerAppearance.company;
            }



        }

    }
}
