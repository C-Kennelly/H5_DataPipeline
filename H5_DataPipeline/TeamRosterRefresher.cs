using H5_DataPipeline.Models;
using HaloSharp;
using HaloSharp.Extension;
using HaloSharp.Exception;
using HaloSharp.Model.Halo5.Profile;
using HaloSharp.Query.Halo5.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;


namespace H5_DataPipeline
{
    class TeamRosterRefresher
    {
        private List<t_players> playersToScan;

        private List<t_teams> currentTeamList;
        private List<t_players_to_teams> currentRosters;

        private List<t_teams> teamsToAdd = new List<t_teams>();
        private List<t_players_to_teams> rosterToUpdate = new List<t_players_to_teams>();

        private DateTime thresholdDateTime;
        private const string noCompanyFoundValue = "NOCOMPANYFOUND";
        private const string spartanCompanySourceString = "Halo Waypoint";
        private int batchSize;

        private HaloClient client;

        public TeamRosterRefresher(int thresholdToUpdateInDays, int companiesToScanAtOneTime)
        {
            thresholdDateTime = DateTime.UtcNow.AddDays(-1 * thresholdToUpdateInDays);
            batchSize = companiesToScanAtOneTime;

            HaloClientFactory haloClientFactory = new HaloClientFactory();
            client = haloClientFactory.MakeClient(Secrets.SecretAPIKey.GetProd(), 200);
        }

        public void RefreshTeamRosters()
        {
            RefreshAllPlayersCompanyRosters();
            //RefreshAllPlayersCustomTeamRosters(); <-TODO: When ready to imlement, should split out two child classes inheriting from a TeamRosterRefresher, once class per source.
        }

        private void RefreshAllPlayersCompanyRosters()
        {
            int playersLeftToScan = 1;

            while(playersLeftToScan >= 0)
            {
                playersLeftToScan = SpartanCompanyRosterSetup();

                ScanPlayers().Wait();

                UpdateDatabase();
            }
        }

        private int SpartanCompanyRosterSetup()
        {
            int playersLeftToScan = 0;
            using (var db = new dev_spartanclashbackendEntities())
            {

                currentTeamList = db.t_teams.Where(x => x.teamSource == spartanCompanySourceString).ToList();
                currentRosters = db.t_players_to_teams.Where(x => x.teamSource == spartanCompanySourceString).ToList();
                playersToScan = db.t_players.Where(player => player.dateCompanyRosterUpdated < thresholdDateTime || player.dateCompanyRosterUpdated == null).Take(batchSize).ToList();

                playersLeftToScan = db.t_players.Where(player => player.dateCompanyRosterUpdated < thresholdDateTime || player.dateCompanyRosterUpdated == null).Count();
            }

            return playersLeftToScan;
        }

        private async Task ScanPlayers()
        {
            int count = 1;
            int total = playersToScan.Count;
            Company company;


            using (var session = client.StartSession())
            {
                foreach (t_players player in playersToScan)
                {
                    Console.WriteLine("Opening {0} of {1}", count, total);
                    OpenPlayerRecord(player);
                    count++;

                    try
                    {
                        var query = new GetPlayerAppearance(player.gamertag);
                        PlayerAppearance playerAppearance = await session.Query(query);
                        company = playerAppearance.company;

                        HandleCompanyResults(player, company);
                    }
                    catch (HaloApiException haloApiException)
                    {
                        HandleAPIExceptions(player, haloApiException);
                    }
                }
            }

        }



        private void OpenPlayerRecord(t_players playerRecord)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                playerRecord.queryStatus = -1;
                db.Entry(playerRecord).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        private void HandleCompanyResults(t_players player, Company company)
        {
            if (company != null)
            {
                t_teams team = new t_teams
                {
                    teamName = company.Name,
                    teamSource = spartanCompanySourceString,
                    lastUpdated = DateTime.UtcNow
                };

                if (DiscoveredNewCompany(team))
                {
                    teamsToAdd.Add(team);
                }

                AddCompanyRosterUpdate(player.gamertag, company.Name);
            }
            else
            {
                AddCompanyRosterUpdate(player.gamertag, noCompanyFoundValue);
            }
        }

        private void HandleAPIExceptions(t_players player, HaloApiException haloApiException)
        {

            Console.WriteLine("Player '{0}' raised apiException with status code: {1}", player.gamertag, haloApiException.HaloApiError.StatusCode);
            Console.WriteLine("     ->" + haloApiException.HaloApiError.Message);

            //throw new NotImplementedException();
            //Handle exceptions w/ player - 404's should be removed from table.

        }

        private bool DiscoveredNewCompany(t_teams company)
        {
            bool result = false;

            if(teamsToAdd.Find(x => x.teamName == company.teamName && x.teamSource == spartanCompanySourceString) == null)
            {
                result = true;
            }

            using(var db = new dev_spartanclashbackendEntities())
            {
                if (db.t_teams.Find(new object[] { company.teamName, spartanCompanySourceString }) == null)
                {
                    result = true;
                }
            }

            return result;
        }

        private void AddCompanyRosterUpdate(string tag, string companyName)
        {
            rosterToUpdate.Add(new t_players_to_teams
                                {
                                    gamertag = tag,
                                    teamName = companyName,
                                    teamSource = spartanCompanySourceString,
                                    lastUpdated = DateTime.UtcNow
                                }
                            );
        }

        private void UpdateDatabase()
        {
            SaveNewTeamsToDatabase();
            SaveRosterChangesToDatabase();
        }

        private void SaveNewTeamsToDatabase()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                foreach (t_teams team in teamsToAdd)
                {
                    currentTeamList.Add(team);
                    db.t_teams.Add(team);
                }
                db.SaveChanges();
            }
        }
        private void SaveRosterChangesToDatabase()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {

                foreach (t_players_to_teams roster in rosterToUpdate)
                {
                    t_players player = db.t_players.Find(roster.gamertag);
                    if (roster.teamName != noCompanyFoundValue)
                    {
                        t_players_to_teams oldRecord = player.t_players_to_teams.FirstOrDefault(x => x.teamSource == spartanCompanySourceString);
                        if(oldRecord != null)
                        {
                            db.t_players_to_teams.Remove(oldRecord);
                        }

                        player.t_players_to_teams.Add(roster);

                    }
                
                    ClosePlayerRecord(player);
                }

                db.SaveChanges();
            }
        }

        private void ClosePlayerRecord(t_players playerRecord)
        {
                playerRecord.queryStatus = 0;
                playerRecord.dateCompanyRosterUpdated = DateTime.UtcNow;
        }


    }
}
