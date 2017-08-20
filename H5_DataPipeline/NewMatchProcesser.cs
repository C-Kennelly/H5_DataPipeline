using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp.Model.Halo5.Stats;
using HaloSharp;

namespace H5_DataPipeline
{
    class NewMatchProcesser
    {
        PlayerMatch match;
        List<string> playersInMatch;
        HaloClient client;

        public NewMatchProcesser(PlayerMatch matchToProcess, HaloClient haloClient)
        {
            match = matchToProcess;
            client = haloClient;
        }

        public List<string> ProcessMatch()
        {
            t_h5matches_matchdetails matchDetails = SaveMatchDetails();
            SaveMatchPlayers(matchDetails).Wait();
            SaveMatchRanksAndScores();

            return playersInMatch;
        }

        private t_h5matches_matchdetails SaveMatchDetails()
        {
            Console.Write("\r Saving match details...");
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_h5matches_matchdetails currentRecord = db.t_h5matches_matchdetails.FirstOrDefault(record => record.matchId == match.Id.MatchId.ToString());

                if (currentRecord == null)
                {
                    t_h5matches_matchdetails details = new t_h5matches_matchdetails(match);
                    db.t_h5matches_matchdetails.Add(details);
                    db.SaveChanges();
                    return details;
                }
                else
                {
                    return currentRecord;
                }
            }
        }

        private async Task SaveMatchPlayers(t_h5matches_matchdetails matchDetails)
        {
            Console.Write("\r Saving players in match...");
            PlayerFinder playerFinder = new PlayerFinder();
            t_h5matches_playersformatch playersForMatch = await playerFinder.GetPlayersForMatch(matchDetails, client);

            using (var db = new dev_spartanclashbackendEntities())
            {
                t_h5matches_playersformatch currentRecord = db.t_h5matches_playersformatch.FirstOrDefault(record => record.matchID == matchDetails.matchId);

                if (currentRecord == null)
                {
                    db.t_h5matches_playersformatch.Add(playersForMatch);
                    db.SaveChanges();
                    playersInMatch = playersForMatch.ToListOfGamertags();
                }
                else
                {
                    playersInMatch = currentRecord.ToListOfGamertags();
                }
            }

        }
        
        private void SaveMatchRanksAndScores()
        {
            Console.Write("\r Saving ranks and scores...");
            using (var db = new dev_spartanclashbackendEntities())
            {
                
                t_h5matches_ranksandscores currentRecord = db.t_h5matches_ranksandscores.FirstOrDefault(record => record.matchId == match.Id.MatchId.ToString());

                if (currentRecord == null)
                {
                    db.t_h5matches_ranksandscores.Add(new t_h5matches_ranksandscores(match));
                    db.SaveChanges();
                }
            }
        }

    }
}
