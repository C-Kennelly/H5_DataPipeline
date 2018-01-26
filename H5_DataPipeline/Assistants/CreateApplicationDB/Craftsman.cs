using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.DataPipeline;
using H5_DataPipeline.Models.SpartanClash;
using H5_DataPipeline.Assistants.CreateApplicationDB.GenerateLeaderboards;
using HaloSharp;

namespace H5_DataPipeline.Assistants.CreateApplicationDB
{

    public class Craftsman
    {
        private IHaloSession session;
        private Herald herald;

        public Craftsman(IHaloSession haloSession)
        {
            session = haloSession;
            herald = new Herald();
        }


        //TODO - this should pass in a context so we can swap the database with code.  For now, need to use config file.
        public void LoadApplicationDatabase()
        {
            List<t_h5matches_teamsinvolved_halowaypointcompanies> clanBattles = GetAllTaggedWaypointClanBattles();
            List<string> clanBattleMatchIDs = clanBattles.Select(battle => battle.matchID).ToList();

            List<t_h5matches_matchdetails> detailsForMatches = PullMatchDetailsForClanBattles( clanBattleMatchIDs);
            List<t_h5matches_ranksandscores> ranksandScoresForMatches = PullMatchRanksAndScoresForClanBattles(clanBattleMatchIDs);
            List<t_h5matches_playersformatch> playersForMatches = PullMatchParticipantsForClanBattles(clanBattleMatchIDs);

            List<t_clashdevset> clashDevSetMatches = BuildClashDevSetMatches(clanBattles, detailsForMatches, ranksandScoresForMatches, playersForMatches);



            ProcessUpdates(clashDevSetMatches, playersForMatches);
            herald.CalculateLeaderboards();
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
                    t_h5matches_matchdetails matchDetails = pipelineDB.t_h5matches_matchdetails.Find(matchID);
                    if(matchDetails != null)
                    {
                        clanBattleDetails.Add(matchDetails);
                    }
                }

            }

            return clanBattleDetails;
        }

        private List<t_h5matches_ranksandscores> PullMatchRanksAndScoresForClanBattles(List<string> clanBattleMatchIDs)
        {
            List<t_h5matches_ranksandscores> clanBattleRanksAndScores = new List<t_h5matches_ranksandscores>(clanBattleMatchIDs.Count);
            using (var pipelineDB = new dev_spartanclashbackendEntities())
            {
                foreach (string matchID in clanBattleMatchIDs)
                {
                    t_h5matches_ranksandscores matchRanksAndScores = pipelineDB.t_h5matches_ranksandscores.Find(matchID);
                    if (matchRanksAndScores != null)
                    {
                        clanBattleRanksAndScores.Add(matchRanksAndScores);
                    }
                }

            }

            return clanBattleRanksAndScores;
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

        private List<t_clashdevset> BuildClashDevSetMatches(List<t_h5matches_teamsinvolved_halowaypointcompanies> clanBattles, List<t_h5matches_matchdetails> clanBattleDetails, List<t_h5matches_ranksandscores> clanBattleRanksAndScores, List<t_h5matches_playersformatch> clanBattleParticipants)
        {
            Console.WriteLine("Creating sample records for {0} battles...", clanBattles.Count);

            List<t_clashdevset> clashDevSetMatches = new List<t_clashdevset>(clanBattleDetails.Count);

            foreach (t_h5matches_teamsinvolved_halowaypointcompanies clanBattle in clanBattles)
            {
                t_h5matches_matchdetails matchDetails = clanBattleDetails.Find(battle => battle.matchId == clanBattle.matchID);
                t_h5matches_ranksandscores matchRanksAndScores = clanBattleRanksAndScores.Find(battle => battle.matchId == clanBattle.matchID);
                t_h5matches_playersformatch matchParticipants = clanBattleParticipants.Find(battle => battle.matchID == clanBattle.matchID);

                clashDevSetMatches.Add(new t_clashdevset(clanBattle, matchDetails, matchRanksAndScores, matchParticipants));
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
