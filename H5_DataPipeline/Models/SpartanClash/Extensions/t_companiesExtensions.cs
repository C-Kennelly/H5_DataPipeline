using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_DataPipeline.Models.SpartanClash
{
    public partial class t_companies
    {
        public t_companies(string companyID, string printableCompanyName)
        {
            companyId = companyID;
            companyName = printableCompanyName;
            waypointLeaderBoardRank = -1;
            wins = -1;
            losses = -1;
            total_matches = -1;
            win_percent = null;
            times_searched = 0;
        }

        /// <summary>
        /// Reset Service Record components - wins, losses, total_matches and win_percent.
        /// </summary>
        public void ResetServiceRecord()
        {
            wins = 0;
            losses = 0;
            total_matches = 0;
            win_percent = null;
        }

        /// <summary>
        /// Make rank -1 to prep for rewrite.
        /// </summary>
        public void ResetRank()
        {
            waypointLeaderBoardRank = -1;
        }

        /// <summary>
        /// Calculates total_matches and win_percent
        /// </summary>
        public void FinalizeServiceRecord()
        {
            total_matches = wins + losses;

            if (total_matches > 0)
            {
                win_percent = (double)wins / (double)total_matches;
            }
        }

    }
}
