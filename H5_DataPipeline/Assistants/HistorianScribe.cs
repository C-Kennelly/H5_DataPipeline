using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp.Model.Halo5.Stats;

namespace H5_DataPipeline.Assistants
{
    /// <summary>
    /// The scribe does the dirty work of the Historian, taking the query results and recording them in the database.
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
                //t_h5matches match = new t_h5matches(playerMatch);
                //RecordMatchesAndAssociations();
                //StoreMatchDetails();
                //StoreRanksAndScores();

            }
        }

        private void RecordMatchesAndAssociations()
        {

            //UpdatePlayerMatchHistoryScanned();
        }

        private void UpdatePlayerMatchHistoryScanned(t_h5matches match)
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

        private void StoreMatchDetails()
        {

            //UpdateMatchDetailsDatesScanned();
        }

        private void UpdateMatchDetailsDatesScanned(t_h5matches match)
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

        private void StoreRanksAndScores()
        {
            //UpdateMatchResultsDatesScanned();
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


    }
}
