using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp.Model.Halo5.Stats;

namespace H5_DataPipeline
{
    class NewMatchProcesser
    {
        PlayerMatch match;

        public NewMatchProcesser(PlayerMatch matchToProcess)
        {
            match = matchToProcess;
        }

        public void ProcessMatch()
        {
            SaveMatchDetails();
            //SaveMatchPlayers();
            //SaveMatchRanksAndScores();
            //SaveMatchWaypointCompaniesInvolved();
            
            //SaveMatchCustomTeamsInvolved();
        }

        //SetDates on these things, handle new players, and scan companies when needed - and we'll need to save them here

        private void SaveMatchDetails()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_h5matches_matchdetails currentRecord = db.t_h5matches_matchdetails.FirstOrDefault(record => record.matchId == match.Id.MatchId.ToString());

                if (currentRecord == null)
                {
                    db.t_h5matches_matchdetails.Add( new t_h5matches_matchdetails(match) );
                    db.SaveChanges();
                }
            }
        }

        //private void SaveMatchPlayers()
        //{
        //    throw new NotImplementedException();
        //    t_h5matches_playersformatch playersForMatch = new t_h5matches_playersformatch(match);
        //
        //}
        //
        //private void SaveMatchRanksAndScores()
        //{
        //    throw new NotImplementedException();
        //    t_h5matches_ranksandscores ranksAndScores = new t_h5matches_ranksandscores(match);
        //}
        //
        //private void SaveMatchWaypointCompaniesInvolved()
        //{
        //    throw new NotImplementedException();
        //    t_h5matches_teamsinvolved_halowaypointcompanies waypointTeamsInvolved = new t_h5matches_teamsinvolved_halowaypointcompanies(matchRecord.t_h5matches_playersformatch);
        //
        //}

        //private void SaveMatchCustomTeamsInvolved()
        //{
        //    throw new NotImplementedException();
        //    matchRecord.t_h5matches_teamsinvolved_spartanclashfireteams = new t_h5matches_teamsinvolved_spartanclashfireteams(matchRecord.t_h5matches_playersformatch);
        //}


    }
}
