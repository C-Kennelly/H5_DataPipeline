using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp.Model.Halo5.Stats;

namespace H5_DataPipeline.Models
{
    public partial class t_h5matches_ranksandscores
    {

        public t_h5matches_ranksandscores()
        {

        }

        public t_h5matches_ranksandscores(PlayerMatch match)
        {
            matchId = match.Id.MatchId.ToString();
            if (match.IsTeamGame)  //store new byte array representing 1 (true)
            { IsTeamGame = new byte[] { Byte.Parse(1.ToString()) }; }
            else                  //store new byte array representing 0 (false)
            { IsTeamGame = new byte[] { Byte.Parse(0.ToString()) }; }

            foreach(Team team in match.Teams)
            {
                if(team.Id == 0)
                {
                    team1_Rank = team.Rank;
                    team1_Score = (int)team.Score;
                }
                else if(team.Id == 1)
                {
                    team2_Rank = team.Rank;
                    team2_Score = (int)team.Score;
                }
                else
                {
                    //Do nothing, as we are not tracking scores for multi-team games.
                }
            }

        }
    }
}
