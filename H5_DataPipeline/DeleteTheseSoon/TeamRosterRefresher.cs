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
        private const string noCompanyFoundID = "0";
        private string spartanCompanySourceString = t_teamsources.GetWaypointSourceName();
        private const int queryOpenStatus = -1;
        private const int queryClosedStatus = 0;
        private int batchSize;

        public TeamRosterRefresher(int thresholdToUpdateInDays, int maxCompaniesToScanAtOneTime)
        {
            thresholdDateTime = DateTime.UtcNow.AddDays(-1 * thresholdToUpdateInDays);
            batchSize = maxCompaniesToScanAtOneTime;
        }

        public void RefreshAllTeamRosters()
        {
            RefreshAllPlayersCompanyRosters();
            //RefreshAllPlayersCustomTeamRosters(); <-TODO: When ready to imlement, should split out two child classes inheriting from a TeamRosterRefresher, once class per source.
        }

        public void RefreshTeamRostersForListOfPlayers(List<t_players> listOfPlayers)
        {
            RefreshSpecificPlayersCompanyRosters(listOfPlayers);
 //           RefreshSpecificPlayersCompanyRosters(listOfPlayers);
        }

        private void RefreshSpecificPlayersCompanyRosters(List<t_players> specificPlayersToScan)
        {
            WaypointSetup.SetupWaypointSourcesInDatabase();

            SpartanCompanyCurrentRosterSetup();

            SetPlayersToScanFromSpecificList(specificPlayersToScan);

            Console.WriteLine("Started batch at {0}", DateTime.UtcNow.ToString());
            ScanPlayers().Wait();

            UpdateDatabase();
        }

        private void SetPlayersToScanFromSpecificList(List<t_players> specificPlayersToScan)
        {

            playersToScan = new List<t_players>(specificPlayersToScan.Count);

            using (var db = new dev_spartanclashbackendEntities())
            {
                foreach (t_players player in specificPlayersToScan)
                {
                    t_players currentRecord = db.t_players.Find(player.gamertag);

                    if(currentRecord == null)
                    {
                        player.UpdateDatabase();
                        currentRecord = player;
                    }

                    if(currentRecord.dateCompanyRosterUpdated < thresholdDateTime 
                        || currentRecord.dateCompanyRosterUpdated == null
                        || currentRecord.queryStatus == queryOpenStatus )
                    {
                        playersToScan.Add(player);
                    }
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("{0} players remaining to be scanned.", playersToScan.Count);
            }
        }

        private void SpartanCompanyCurrentRosterSetup()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                currentTeamList = db.t_teams.Where(team => team.teamSource == spartanCompanySourceString).ToList();
                currentRosters = db.t_players_to_teams.Where(roster => roster.t_teams.teamSource == spartanCompanySourceString).ToList();
            }
        }


        private void RefreshAllPlayersCompanyRosters()
        {
            int playersLeftToScan = 1;

            WaypointSetup.SetupWaypointSourcesInDatabase();

            while(playersLeftToScan >= 0)
            {
                SpartanCompanyCurrentRosterSetup();
                playersLeftToScan = SetPlayersToScanBasedOnBatchSize();

                Console.WriteLine("Started batch at {0}",DateTime.UtcNow.ToString());
                ScanPlayers().Wait();

                UpdateDatabase();
            }
        }

        private int SetPlayersToScanBasedOnBatchSize()
        {
            int playersLeftToScan = 0;

            using (var db = new dev_spartanclashbackendEntities())
            {
                playersToScan =     db.t_players.Where(player => player.dateCompanyRosterUpdated < thresholdDateTime 
                    || player.dateCompanyRosterUpdated == null
                    || player.queryStatus == queryOpenStatus)
                    .Take(batchSize).ToList();

                playersLeftToScan = db.t_players.Where(player => player.dateCompanyRosterUpdated < thresholdDateTime 
                    || player.dateCompanyRosterUpdated == null
                    || player.queryStatus == queryOpenStatus)
                    .Count();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("{0} players remaining to be scanned.", playersLeftToScan);

            return playersLeftToScan;
        }

        private async Task ScanPlayers()
        {
            int count = 0;
            int total = playersToScan.Count;
            Company company;

            HaloClientFactory haloClientFactory = new HaloClientFactory();
            var client = haloClientFactory.GetProdClient();

            using (var session = client.StartSession())
            {
                foreach (t_players player in playersToScan)
                {
                    OpenPlayerRecord(player);
                    count++;

                    Console.Write("\rScanning players {0} of {1}", count, total);

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
                playerRecord.queryStatus = queryOpenStatus;
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
                    teamId = company.Id.ToString(),
                    teamName = company.Name,
                    teamSource = spartanCompanySourceString,
                    lastUpdated = DateTime.UtcNow
                };

                if (DiscoveredNewCompany(team))
                {
                    teamsToAdd.Add(team);
                }

                AddCompanyRosterUpdate(player.gamertag, company.Id.ToString());
            }
            else
            {
                AddCompanyRosterUpdate(player.gamertag, noCompanyFoundID);
            }
        }

        private void HandleAPIExceptions(t_players player, HaloApiException haloApiException)
        {

            //Console.WriteLine("Player '{0}' raised apiException with status code: {1}", player.gamertag, haloApiException.HaloApiError.StatusCode);
            //Console.WriteLine("     -> Removing player from database.");

            if(haloApiException.HaloApiError.StatusCode == 404)
            {
                using (var db = new dev_spartanclashbackendEntities())
                {
                    t_players playerRecord = db.t_players.Find(player.gamertag);
                    db.t_players.Remove(playerRecord);
                    db.SaveChanges();
                }
            }

        }

        private bool DiscoveredNewCompany(t_teams company)
        {
            bool result = false;

            using (var db = new dev_spartanclashbackendEntities())
            {
                if (db.t_teams.Find(company.teamId) == null)
                {
                    result = true;
                }
                else
                {
                    if (teamsToAdd.Find(x => x.teamName == company.teamName && x.teamSource == spartanCompanySourceString) == null)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        private void AddCompanyRosterUpdate(string tag, string companyId)
        {
            rosterToUpdate.Add(new t_players_to_teams
                                {
                                    gamertag = tag,
                                    teamId = companyId,
                                    lastUpdated = DateTime.UtcNow
                                }
                            );
        }

        private void UpdateDatabase()
        {
            try
            {
                SaveNewTeamsToDatabase();
            }
            catch (Exception e)
            {
                Console.WriteLine("New Team caught exception {0} with message: {0}", e.HResult, e.Message);
            }

            try
            {
                SaveRosterChangesToDatabase();
            }
            catch (Exception e)
            {
                Console.WriteLine("RosterUpdate caught exception {0} with message: {0}", e.HResult, e.Message);
            }


            Console.WriteLine();
        }

        private void SaveNewTeamsToDatabase()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                foreach (t_teams team in teamsToAdd)
                {
                    Console.Write("\rSaving new team: {0}                              ", team.teamName);

                    t_teams currentRecord = db.t_teams.FirstOrDefault(x => x.teamId == team.teamId);
                    if(currentRecord == null)
                    {
                        currentTeamList.Add(team);
                        db.t_teams.Add(team);
                    }
                    db.SaveChanges();
                }   
            }

            teamsToAdd = new List<t_teams>();
        }

        private void SaveRosterChangesToDatabase()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                string waypointSourceName = t_teamsources.GetWaypointSourceName();

                foreach (t_players_to_teams roster in rosterToUpdate)
                {
                    Console.Write("\rUpdating roster info for: {0}                              ", roster.gamertag);
                    t_players player = db.t_players.FirstOrDefault(x => x.gamertag == roster.gamertag);
                    if (roster.teamId != noCompanyFoundID)
                    {
                        t_players_to_teams oldRecord = db.t_players_to_teams.FirstOrDefault(x => x.gamertag == player.gamertag && x.t_teams.teamSource == waypointSourceName);
                        if(oldRecord != null)
                        {
                            if(oldRecord.teamId == roster.teamId)
                            {
                                oldRecord.lastUpdated = DateTime.UtcNow;
                            }
                            else
                            {
                                db.t_players_to_teams.Remove(oldRecord);
                                db.SaveChanges();
                                player.t_players_to_teams.Add(roster);
                            }
                        }
                        else
                        {
                            player.t_players_to_teams.Add(roster);
                        }

                    }
                
                    ClosePlayerRecord(player);
                }

                db.SaveChanges();
            }

            rosterToUpdate = new List<t_players_to_teams>();
        }

        private void ClosePlayerRecord(t_players playerRecord)
        {
                playerRecord.queryStatus = queryClosedStatus;
                playerRecord.dateCompanyRosterUpdated = DateTime.UtcNow;
        }

    }
}
