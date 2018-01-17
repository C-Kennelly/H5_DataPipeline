using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HaloSharp;
using H5_DataPipeline.Models;
using H5_DataPipeline.Shared.Config;
using HaloSharp.Model;
using HaloSharp.Model.Halo5.Stats;


namespace H5_DataPipeline.Assistants.MatchDetails
{
    /// <summary>
    /// The historian's job is to know the details of every battle.  She goes through tracked players and scans recent games, recording their details and scores.
    /// </summary>
    class Historian
    {
        SpartanClashSettings spartanClashSettings;
        IHaloSession haloSession;

        public event PlayerMatchHistoryScannedHandler PlayerMatchHistoryReadyForDatabaseWrite;
        public void OnPlayerMatchHistoryReadyForDatabaseWrite(object Sender, PlayerMatchHistoryScannedEventArgs e)
        {
            HistorianScribe scribe = new HistorianScribe(e.GetMatchHistoryToRecord(), e.GetPlayerSubjectOfMatchHistory());
            scribe.RecordMatchHistoryForPlayer();
        }

        public Historian(IHaloSession session, SpartanClashSettings settings)
        {
            haloSession = session;
            spartanClashSettings = settings;

            PlayerMatchHistoryReadyForDatabaseWrite += OnPlayerMatchHistoryReadyForDatabaseWrite;
        }

        public void RecordRecentGames()
        {
            List<t_players> trackedWaypointPlayers = GetTrackedPlayersFromWaypoint();

            Console.WriteLine("Updating player Match Histories at: {0}", DateTime.UtcNow);
            Console.WriteLine();

            ProcessPlayers(trackedWaypointPlayers);

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

                //Get all teams in the database from the Waypoint source, excluding the special team for NoCompanyFound.
                List<t_teams> trackedTeamsFromWaypoint = db.t_teams.Where(team =>
                        team.teamSource == waypointSourceName
                        && team.trackingIndex > 0
                        && team.teamId != noCompanyFoundID)
                    .ToList();

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
                        playersOnWaypointTeams.Add(dbp.t_players.Find(rosterEntry.gamertag));
                    }
                });

                //foreach (t_players_to_teams rosterEntries in rosterEntriesFromWayoint)
                //{
                //    playersOnWaypointTeams.Add(db.t_players.Find(rosterEntries.gamertag));
                //}

                return playersOnWaypointTeams;
            }
        }

        private void ProcessPlayers(List<t_players> players)
        {
            int counter = 0;
            int total = players.Count;

            foreach (t_players player in players)
            {
                counter++;
                Console.WriteLine("\rProcessing {0} of {1}: {2}                ",counter, total, player.gamertag);

                ProcessPlayer(player);
            }

            if (total == 0)
            {
                Console.Write("No players for Historian to process.");
            }
        }

        private async Task ProcessPlayer(t_players player)
        {
            MatchCaller matchCaller = new MatchCaller();

            List<PlayerMatch> recentH5MatchHistory = await matchCaller.GetH5MatchHistoryForPlayerAfterDate(
                            player.gamertag,
                            GetDateToSearchFrom(player, spartanClashSettings),
                            spartanClashSettings.GetGameModes(),
                            haloSession
                        );

            PlayerMatchHistoryReadyForDatabaseWrite?.BeginInvoke(this, new PlayerMatchHistoryScannedEventArgs(recentH5MatchHistory, player), null, null);
        }

        public DateTime GetDateToSearchFrom(t_players player, SpartanClashSettings spartanClashSettings)
        {
            DateTime result = spartanClashSettings.LookForNoMatchesEarlierThan();
            DateTime playerRecordScanDate = player.GetEarliestDateToScanMatches();

            if (playerRecordScanDate > result)
            {
                result = playerRecordScanDate;
            }

            return result;
        }

    }
}


