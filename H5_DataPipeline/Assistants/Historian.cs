using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp;
using H5_DataPipeline.Models;
using HaloSharp.Model;
using HaloSharp.Model.Halo5.Stats;


namespace H5_DataPipeline.Assistants
{
    /// <summary>
    /// The historian's job is to know the details of every battle.  She goes through tracked players and scans recent games, recording their details and scores.
    /// </summary>
    class Historian
    {
        SpartanClashSettings spartanClashSettings;
        IHaloSession haloSession;

        public Historian(IHaloSession session, SpartanClashSettings settings)
        {
            haloSession = session;
            spartanClashSettings = settings;
        }

        public void RecordRecentGames()
        {
            List<t_players> trackedPlayers = GetTrackedPlayers();

            Console.WriteLine("Updating player Match Histories at: {0}", DateTime.UtcNow);
            Console.WriteLine();

            ProcessPlayers(trackedPlayers);

            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Finished updating player Match Histories at: {0}", DateTime.UtcNow);
        }

        private List<t_players> GetTrackedPlayers()
        {
            //Only search players who are in a Spartan Company on Waypoint
            using (var db = new dev_spartanclashbackendEntities())
            {

                string waypointSourceName = t_teamsources.GetWaypointSourceName();
                string noCompanyFoundID = t_teams.GetNoWaypointCompanyFoundID();

                //Get all teams in the database from the Waypoint source, excluding the special team for NoCompanyFound.
                List<t_teams> teamsFromWaypoint = db.t_teams.Where(team =>
                        team.teamSource == waypointSourceName
                        && team.teamId != noCompanyFoundID)
                    .ToList();

                List<t_players_to_teams> rosterEntriesFromWayoint = new List<t_players_to_teams>(teamsFromWaypoint.Count);

                foreach(t_teams team in teamsFromWaypoint)
                {
                    List<t_players_to_teams> rosterEntriesForTeam = db.t_players_to_teams.Where(x => x.teamId == team.teamId).ToList();
                    if(rosterEntriesForTeam != null)
                    {
                        rosterEntriesFromWayoint.AddRange(rosterEntriesForTeam);
                    }
                }

                List<t_players> playersOnWaypointTeams = new List<t_players>(rosterEntriesFromWayoint.Count);
                foreach (t_players_to_teams rosterEntries in rosterEntriesFromWayoint)
                {
                    t_players playerInRoster = db.t_players.Find(rosterEntries.gamertag);
                    if(playerInRoster!= null)
                    {
                        playersOnWaypointTeams.Add(playerInRoster);
                    }
                }

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
                Console.Write("\rProcessing {0} of {1}: {2}                ",counter, total, player.gamertag);

                ProcessPlayer(player).Wait();
            }

            if (total == 0)
            {
                Console.Write("No players for Historian to process.");
            }
        }

        private async Task ProcessPlayer(t_players player)
        {
            MatchCaller matchCaller = new MatchCaller();
            HistorianScribe scribe = new HistorianScribe();


            List<PlayerMatch> recentH5MatchHistory = await matchCaller.GetH5MatchHistoryForPlayerAfterDate(
                            player.gamertag,
                            spartanClashSettings.GetDateToSearchFrom(player),
                            spartanClashSettings.gameModes,
                            haloSession
                        );

            scribe.RecordMatchHistoryForPlayer(recentH5MatchHistory, player);
        }

    }
}


