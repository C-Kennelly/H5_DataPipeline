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

            Setup();

            while(playersLeftToScan >= 0)
            {
                playersLeftToScan = SpartanCompanyRosterSetup();

                ScanPlayers().Wait();

                UpdateDatabase();
            }
        }

        private void Setup()
        {
            AddDefaultTeamSourceRecord();
            AddDefaultTeamsRecords();
        }

        private void AddDefaultTeamSourceRecord()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_teamsources defaultSpartanCompanySource = db.t_teamsources.Find(t_teamsources.GetWaypointSourceName());
                if (defaultSpartanCompanySource == null)
                {
                    db.t_teamsources.Add(t_teamsources.GetNewDefaultWaypointRecord());
                }
                db.SaveChanges();
            }
        }

        private void AddDefaultTeamsRecords()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_teams defaultSpartanCompanyNoCompanyFoundTeam = db.t_teams.Find(t_teams.GetNoWaypointCompanyFoundID());
                if (defaultSpartanCompanyNoCompanyFoundTeam == null)
                {
                    db.t_teams.Add(t_teams.GetNewDefaultNoCompanyFoundRecord());
                }

                db.SaveChanges();
            }
        }

        private int SpartanCompanyRosterSetup()
        {
            int playersLeftToScan = 0;
            using (var db = new dev_spartanclashbackendEntities())
            {
                currentTeamList = db.t_teams.Where(team => team.teamSource == spartanCompanySourceString).ToList();
                currentRosters = db.t_players_to_teams.Where(roster => roster.t_teams.teamSource == spartanCompanySourceString).ToList();
                playersToScan = db.t_players.Where(player => player.dateCompanyRosterUpdated < thresholdDateTime || player.queryStatus == queryOpenStatus || player.dateCompanyRosterUpdated == null).Take(batchSize).ToList();

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

            Console.WriteLine("Player '{0}' raised apiException with status code: {1}", player.gamertag, haloApiException.HaloApiError.StatusCode);
            Console.WriteLine("     -> Removing player from database.");

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
            SaveNewTeamsToDatabase();
            SaveRosterChangesToDatabase();
        }

        private void SaveNewTeamsToDatabase()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                foreach (t_teams team in teamsToAdd)
                {
                    t_teams currentRecord = db.t_teams.FirstOrDefault(x => x.teamId == team.teamId);
                    if(currentRecord == null)
                    {
                        currentTeamList.Add(team);
                        db.t_teams.Add(team);
                    }
                    db.SaveChanges();
                }
                
            }
        }
        private void SaveRosterChangesToDatabase()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                string waypointSourceName = t_teamsources.GetWaypointSourceName();

                foreach (t_players_to_teams roster in rosterToUpdate)
                {
                    t_players player = db.t_players.FirstOrDefault(x => x.gamertag == roster.gamertag);
                    if (roster.teamId != noCompanyFoundID)
                    {
                        t_players_to_teams oldRecord = db.t_players_to_teams.FirstOrDefault(x => x.gamertag == player.gamertag && x.t_teams.teamSource == waypointSourceName);
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
                playerRecord.queryStatus = queryClosedStatus;
                playerRecord.dateCompanyRosterUpdated = DateTime.UtcNow;
        }


    }
}
