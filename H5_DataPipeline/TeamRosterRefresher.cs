﻿using H5_DataPipeline.Models;
using HaloSharp;
using HaloSharp.Extension;
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
            PullCurrentTeams();
            PullCurrentRoster();
            SetPlayersToScan();

            ScanPlayers().Wait();

            UpdateDatabase();
        }

        private void PullCurrentTeams()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                currentTeamList = db.t_teams.Where(x => x.teamSource == spartanCompanySourceString).ToList();
            }
        }

        private void PullCurrentRoster()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                currentRosters = db.t_players_to_teams.Where(x => x.teamSource == spartanCompanySourceString).ToList();
            }
        }

        private void SetPlayersToScan()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                playersToScan = db.t_players.Where(player => player.dateCompanyRosterUpdated < thresholdDateTime || player.dateCompanyRosterUpdated == null).ToList();
            }
        }

        private async Task ScanPlayers()
        {
            HaloClientFactory haloClientFactory = new HaloClientFactory();

            foreach (t_players player in playersToScan)
            {
                OpenPlayerRecord(player);

                Task<Company> t = GetPlayerCompany(player.gamertag, haloClientFactory.GetProdClient());
                Company company = await t;
                t.Wait();
  
                HandleCompanyResults(player, company);
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


        private async Task<Company> GetPlayerCompany(string gamertag, HaloClient client)
        {
            using (var session = client.StartSession())
            {
                var query = new GetPlayerAppearance(gamertag);

                PlayerAppearance playerAppearance = await session.Query(query);

                return playerAppearance.company;
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

        private bool DiscoveredNewCompany(t_teams company)
        {
            return (!teamsToAdd.Contains(company) && currentTeamList.Find(x => x.teamName == company.teamName && x.teamSource == spartanCompanySourceString) == null);
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
