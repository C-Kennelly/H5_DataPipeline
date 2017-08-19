using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp.Model.Halo5.Stats;

namespace H5_DataPipeline.Models
{
    public partial class t_h5matches
    {

        public t_h5matches(string id)
        {
            matchID = id;
            dateDetailsScan = null;
            datePlayersScan = null;
            dateResultsScan = null;
            dateCompaniesInvolvedUpdated = null;
            dateCustomTeamsUpdated = null;
            queryStatus = 0;
        }

        public t_h5matches(PlayerMatch playerName)
        {
            matchID = playerName.Id.MatchId.ToString();
            dateDetailsScan = null;
            datePlayersScan = null;
            dateResultsScan = null;
            dateCompaniesInvolvedUpdated = null;
            dateCustomTeamsUpdated = null;
            queryStatus = 0;
        }
    }
}
