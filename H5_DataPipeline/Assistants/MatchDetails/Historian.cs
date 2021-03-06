﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HaloSharp;
using H5_DataPipeline.Models.DataPipeline;
using H5_DataPipeline.Shared.Config;
using HaloSharp.Model;
using HaloSharp.Model.Halo5.Stats;
using H5_DataPipeline.Assistants.Shared;


namespace H5_DataPipeline.Assistants.MatchDetails
{
    /// <summary>
    /// The historian's job is to know the details of every battle.  She goes through tracked players and scans recent games, recording their details and scores.
    /// </summary>
    class Historian
    {
        SpartanClashSettings spartanClashSettings;
        IHaloSession haloSession;
        Referee referee;


//        public event PlayerMatchHistoryScannedHandler PlayerMatchHistoryReadyForDatabaseWrite;
//        public void OnPlayerMatchHistoryReadyForDatabaseWrite(object Sender, PlayerMatchHistoryScannedEventArgs e)
//        {
//            HistorianScribe scribe = new HistorianScribe(e.GetPlayerSubjectOfMatchHistory(), e.GetMatchHistoryToRecord(), referee, e.GetJobIndex());
//            scribe.RecordMatchHistoryForPlayer();
//        }

    public Historian(IHaloSession session, SpartanClashSettings settings)
        {
            haloSession = session;
            spartanClashSettings = settings;
//            PlayerMatchHistoryReadyForDatabaseWrite += OnPlayerMatchHistoryReadyForDatabaseWrite;

            referee = new Referee();
        }

        public void RecordRecentGames()
        {
            Console.WriteLine("Updating player Match Histories at: {0}", DateTime.UtcNow);
            Console.WriteLine();

            List<t_players> trackedWaypointPlayers = GetTrackedPlayersFromWaypoint();
            ProcessPlayers(trackedWaypointPlayers);
            referee.WaitUntilAllJobsAreDone();


            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Finished updating player Match Histories at: {0}", DateTime.UtcNow);
        }

        private List<t_players> GetTrackedPlayersFromWaypoint()
        {
            //Only search players who are in a Spartan Company on Waypoint
            using (var db = new dev_spartanclashbackendEntities())
            {

                string waypointSourceName = t_teamsources.GetWaypointSourceName();
                string noCompanyFoundID = t_teams.GetNoWaypointCompanyFoundID();


                double reQueryDays = spartanClashSettings.GetMatchHistoryReQueryDays();
                DateTime reQueryThresholdDate = DateTime.UtcNow.AddDays(-1 * reQueryDays);

                //Get all tracked teams in the database from the Waypoint source, excluding the special team for NoCompanyFound.
                List<t_teams> trackedTeamsFromWaypoint = db.t_teams.Where(team =>
                        team.trackingIndex > 0
                        && team.teamId != noCompanyFoundID
                        && team.teamSource == waypointSourceName
                ).ToList();

                List<t_players_to_teams> rosterEntriesFromWayoint = new List<t_players_to_teams>(trackedTeamsFromWaypoint.Count);

                foreach(t_teams team in trackedTeamsFromWaypoint)
                {
                    List<t_players_to_teams> rosterEntriesForTeam = db.t_players_to_teams.Where(x => x.teamId == team.teamId).ToList();
                    if(rosterEntriesForTeam != null)
                    {
                        rosterEntriesFromWayoint.AddRange(rosterEntriesForTeam);
                    }
                }

                List<t_players> playersOnWaypointTeams = new List<t_players>(rosterEntriesFromWayoint.Count);

                Parallel.ForEach(rosterEntriesFromWayoint, rosterEntry =>
                {
                    using(var dbp = new dev_spartanclashbackendEntities())
                    {
                        t_players player = dbp.t_players.Find(rosterEntry.gamertag);
                        if(player.dateLastMatchScan == null || player.dateLastMatchScan < reQueryThresholdDate)
                        {
                            playersOnWaypointTeams.Add(player);
                        }
                    }
                });

                return playersOnWaypointTeams;
            }
        }

        private void ProcessPlayers(List<t_players> players)
        {
            int counter = 0;
            int total = players.Count;

            foreach (t_players player in players)
            {
                Console.Write("\rProcessing {0} of {1}: {2}                ",counter, total, player.gamertag);

                referee.WaitToRegisterJob(counter);
                ProcessPlayer(player, counter).Wait();

                counter++;
            }
        }

        private async Task ProcessPlayer(t_players player, int jobIndex)
        {
            MatchCaller matchCaller = new MatchCaller();

            List<PlayerMatch> recentH5MatchHistory = await matchCaller.GetH5MatchHistoryForPlayerAfterDate(
                            player.gamertag,
                            player.GetEarliestDateToScanMatches(),
                            spartanClashSettings.GetGameModes(),
                            haloSession
                        );


            //PlayerMatchHistoryReadyForDatabaseWrite?.BeginInvoke(this, new PlayerMatchHistoryScannedEventArgs(recentH5MatchHistory, player, jobIndex), null, null);
            
            HistorianScribe scribe = new HistorianScribe(player, recentH5MatchHistory);
            scribe.RecordMatchHistoryForPlayer();
            referee.WaitToMarkJobDone(jobIndex);
            
        }
    }
}


