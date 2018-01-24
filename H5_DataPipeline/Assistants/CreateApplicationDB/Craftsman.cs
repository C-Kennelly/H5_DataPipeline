using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.DataPipeline;
using H5_DataPipeline.Models.SpartanClash;
using HaloSharp;

namespace H5_DataPipeline.Assistants.CreateApplicationDB
{

    public class Craftsman
    {
        private IHaloSession session;

        public Craftsman(IHaloSession haloSession)
        {
            session = haloSession;
        }


        //TODO - this should pass in a context so we can swap the database with code.  For now, need to use config file.
        public void UpdateApplicationDatabase()
        {
            List<t_h5matches_teamsinvolved_halowaypointcompanies> clanBattles = GetAllTaggedWaypointClanBattles();
            List<string> clanBattleMatchIDs = clanBattles.Select(battle => battle.matchID).ToList();

            List<t_h5matches_matchdetails> detailsForMatches = PullMatchDetailsForClanBattles( clanBattleMatchIDs);
            List<t_h5matches_playersformatch> playersForMatches = PullMatchParticipantsForClanBattles(clanBattleMatchIDs);

            List<t_clashdevset> clashDevSetMatches = BuildClashDevSetMatches(clanBattles, detailsForMatches);

            Console.WriteLine("Pushing inserts to database.");
            ProcessUpdates(clashDevSetMatches, playersForMatches);
        }

        private List<t_h5matches_teamsinvolved_halowaypointcompanies> GetAllTaggedWaypointClanBattles()
        {
            List<t_h5matches_teamsinvolved_halowaypointcompanies> clanBattles = new List<t_h5matches_teamsinvolved_halowaypointcompanies>();
            using (var pipelineDB = new dev_spartanclashbackendEntities())
            {
                List<t_h5matches_teamsinvolved_halowaypointcompanies> dbValues = pipelineDB.t_h5matches_teamsinvolved_halowaypointcompanies
                                                        .Where(battle => battle.team1_Primary != "0" || battle.team2_Primary != "0")
                                                        .ToList();
                if(dbValues != null)
                {
                    clanBattles = dbValues;
                }
            }

            return clanBattles;
        }

        private List<t_h5matches_matchdetails> PullMatchDetailsForClanBattles(List<string> clanBattleMatchIDs)
        {
            List<t_h5matches_matchdetails> clanBattleDetails = new List<t_h5matches_matchdetails>(clanBattleMatchIDs.Count);
            using (var pipelineDB = new dev_spartanclashbackendEntities())
            {
                foreach(string matchID in clanBattleMatchIDs)
                {
                    t_h5matches_matchdetails match = pipelineDB.t_h5matches_matchdetails.Find(matchID);
                    if(match != null)
                    {
                        clanBattleDetails.Add(match);
                    }
                }

            }

            return clanBattleDetails;
        }

        private List<t_h5matches_playersformatch> PullMatchParticipantsForClanBattles(List<string> clanBattleMatchIDs)
        {
            List<t_h5matches_playersformatch> clanBattleParticipants = new List<t_h5matches_playersformatch>(clanBattleMatchIDs.Count);
            using (var pipelineDB = new dev_spartanclashbackendEntities())
            {
                foreach (string matchID in clanBattleMatchIDs)
                {
                    t_h5matches_playersformatch match = pipelineDB.t_h5matches_playersformatch.Find(matchID);
                    if (match != null)
                    {
                        clanBattleParticipants.Add(match);
                    }
                }

            }

            return clanBattleParticipants;
        }

        private List<t_clashdevset> BuildClashDevSetMatches(List<t_h5matches_teamsinvolved_halowaypointcompanies> clanBattles, List<t_h5matches_matchdetails> clanBattleDetails)
        {
            Console.WriteLine("Build new database records...");

            List<t_clashdevset> clashDevSetMatches = new List<t_clashdevset>(clanBattleDetails.Count);

            foreach (t_h5matches_teamsinvolved_halowaypointcompanies clanBattle in clanBattles)
            {
                t_h5matches_matchdetails matchDetails = clanBattleDetails.Find(battle => battle.matchId == clanBattle.matchID);

                clashDevSetMatches.Add(new t_clashdevset(clanBattle, matchDetails));
            }

            return clashDevSetMatches;

        }

        private void ProcessUpdates(List<t_clashdevset> clashDevSetMatches, List<t_h5matches_playersformatch> matchParticipants)
        {
            CraftsmanScribe scribe = new CraftsmanScribe(clashDevSetMatches, matchParticipants, session);
            scribe.AddRecordsToApplicationDatabase();
        }

    }
}
