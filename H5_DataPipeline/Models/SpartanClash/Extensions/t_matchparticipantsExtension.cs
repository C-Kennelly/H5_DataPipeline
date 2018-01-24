using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.DataPipeline;

namespace H5_DataPipeline.Models.SpartanClash
{
    public partial class t_matchparticipants
    {
        public t_matchparticipants ()
        {

        }

        public t_matchparticipants(t_h5matches_playersformatch playersForMatchRecord)
        {
            matchId = playersForMatchRecord.matchID;
            team1_Players = playersForMatchRecord.team1_Players;
            team2_Players = playersForMatchRecord.team2_Players;
            other_Players = playersForMatchRecord.other_Players;
            DNF_Players = playersForMatchRecord.DNF_Players;
        }
    }
}
