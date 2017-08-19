using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp.Model.Halo5.Stats;
using HaloSharp.Model.Halo5.Stats.Common;
using HaloSharp.Model;

namespace H5_DataPipeline
{
    class MatchHistorian
    {
        HaloClientFactory haloClientFactory = new HaloClientFactory();

        List<Enumeration.Halo5.GameMode> gameModes = new List<Enumeration.Halo5.GameMode>();

        public MatchHistorian()
        {
            gameModes.Add(Enumeration.Halo5.GameMode.Arena);
            gameModes.Add(Enumeration.Halo5.GameMode.Warzone);
            gameModes.Add(Enumeration.Halo5.GameMode.Custom);

        }

        public async void thing()
        {
            MatchCaller matchCaller = new MatchCaller();

            List<PlayerMatch> matchHistory = await matchCaller.GetMatchHistoryForPlayerAfterDate("Sn1p3r C", new DateTime(2017, 7, 1), gameModes, haloClientFactory.GetDevClient());

            foreach (PlayerMatch match in matchHistory)
            {
                ProcessMatch(match);
            }
        }

        public void ProcessMatch(PlayerMatch match)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_h5matches matchRecord = new t_h5matches(match);
                
                if(matchRecord.AlreadyExists())
                {
                    //Probably don't need to do this
                }

                //SetDates on these things, handle new players, and scan companies when needed - or just return the built match and do it elsewhere

                matchRecord.t_h5matches_matchdetails = new t_h5matches_matchdetails(match);
                matchRecord.t_h5matches_playersformatch = new t_h5matches_playersformatch(match);
                matchRecord.t_h5matches_ranksandscores = new t_h5matches_ranksandscores(match);
                matchRecord.t_h5matches_teamsinvolved_halowaypointcompanies = new t_h5matches_teamsinvolved_halowaypointcompanies(matchRecord.t_h5matches_playersformatch);
                matchRecord.t_h5matches_teamsinvolved_spartanclashfireteams = new t_h5matches_teamsinvolved_spartanclashfireteams(matchRecord.t_h5matches_playersformatch);
            }

        }
    }
}
