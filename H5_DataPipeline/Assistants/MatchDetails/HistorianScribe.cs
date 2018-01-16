using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp.Model.Halo5.Stats;

namespace H5_DataPipeline.Assistants.MatchDetails
{
    /// <summary>
    /// The scribe does the dirty work of the Historian, taking the query results and recording details, ranks, and scores in the database.
    /// </summary>
    class HistorianScribe
    {
        private t_players playerSubjectOfMatchHistory;
        private List<PlayerMatch> matchHistoryToRecord;

        public HistorianScribe()
        {
            //What settings does he need, or is he just holding processing logic?
        }

        public void RecordMatchHistoryForPlayer(List<PlayerMatch> matchHistory, t_players player)
        {
            playerSubjectOfMatchHistory = player;
            matchHistoryToRecord = matchHistory;


            foreach(PlayerMatch playerMatch in matchHistoryToRecord)
            {
                t_h5matches match = new t_h5matches(playerMatch);

                RecordMatchesAndAssociations(match);
                StoreMatchDetails(playerMatch, match);
                StoreRanksAndScores(playerMatch, match);
            }

            UpdatePlayerMatchHistoryScanned();
        }

        private void RecordMatchesAndAssociations(t_h5matches match)
        {
            //add if not exists
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_h5matches currentRecord = db.t_h5matches.Find(match.matchID);

                if(currentRecord == null)
                {
                    db.t_h5matches.Add(match);
                    db.SaveChanges();
                }
            }

            EnsureAssociationExistsWithCurrentPlayer(match);
        }

        private void EnsureAssociationExistsWithCurrentPlayer(t_h5matches match)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_players_to_h5matches currentRecord = db.t_players_to_h5matches.FirstOrDefault(record => record.gamertag == playerSubjectOfMatchHistory.gamertag && record.matchID == match.matchID);

                if (currentRecord == null)
                {
                    db.t_players_to_h5matches.Add(new t_players_to_h5matches(playerSubjectOfMatchHistory.gamertag, match.matchID));
                    db.SaveChanges();
                }

            }
        }

        private void StoreMatchDetails(PlayerMatch playerMatch, t_h5matches parentRecord)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_h5matches_matchdetails currentRecord = db.t_h5matches_matchdetails.FirstOrDefault(record => record.matchId == playerMatch.Id.MatchId.ToString());

                if (currentRecord == null)
                {
                    t_h5matches_matchdetails details = new t_h5matches_matchdetails(playerMatch);
                    db.t_h5matches_matchdetails.Add(details);
                    db.SaveChanges();
                }

                UpdateMatchDetailsDatesScanned(parentRecord);
            }
        }

        private void UpdateMatchDetailsDatesScanned(t_h5matches match)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_h5matches currentRecord = db.t_h5matches.Find(match.matchID);

                if (currentRecord != null)
                {
                    currentRecord.dateDetailsScan = DateTime.UtcNow;

                    db.SaveChanges();
                }
                else
                {
                    throw new NotImplementedException("Impossibility condition reached - couldn't find parent 'h5matches' record for matchID: " + match.matchID);
                }
            }
        }

        private void StoreRanksAndScores(PlayerMatch playerMatch, t_h5matches parentRecord)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {

                t_h5matches_ranksandscores currentRecord = db.t_h5matches_ranksandscores.FirstOrDefault(record => record.matchId == playerMatch.Id.MatchId.ToString());

                if (currentRecord == null)
                {
                    db.t_h5matches_ranksandscores.Add(new t_h5matches_ranksandscores(playerMatch));
                    db.SaveChanges();
                }
            }

            UpdateMatchResultsDatesScanned(parentRecord);
        }

        private void UpdateMatchResultsDatesScanned(t_h5matches match)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_h5matches currentRecord = db.t_h5matches.Find(match.matchID);

                if (currentRecord != null)
                {
                    currentRecord.dateResultsScan = DateTime.UtcNow;

                    db.SaveChanges();
                }
                else
                {
                    throw new NotImplementedException("Impossibility condition reached - couldn't find parent 'h5matches' record for matchID: " + match.matchID);
                }
            }
        }

        private void UpdatePlayerMatchHistoryScanned()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_players currentPlayerRecord = db.t_players.Find(playerSubjectOfMatchHistory.gamertag);

                if (currentPlayerRecord != null)
                {
                    currentPlayerRecord.dateLastMatchScan = DateTime.UtcNow;

                    db.SaveChanges();
                }
                else
                {
                    throw new NotImplementedException("Impossibility condition reached - couldn't find parent 'players' record for matchID: " + playerSubjectOfMatchHistory.gamertag);
                }
            }
        }








    }
}
