using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.SpartanClash;

namespace H5_DataPipeline.Assistants.CreateApplicationDB.GenerateLeaderboards
{
    public class WaypointLeaderBoarderBuilder
    {
        private int gameThresholdToBeRanked;

        public WaypointLeaderBoarderBuilder(int gameThreshold)
        {
            gameThresholdToBeRanked = gameThreshold;
        }

        /// <summary>
        /// Calculate rank
        /// </summary>
        public void BuildWaypointLeaderboards()
        {
            using (var spartanClashDB = new clashdbEntities())
            {
                List<t_companies> rankableCompaniesOrdered = spartanClashDB.t_companies
                                                        .Where(x => x.total_matches > gameThresholdToBeRanked)
                                                        .OrderByDescending(x => x.win_percent)
                                                        .ThenByDescending(y => y.total_matches)
                                                        .ToList();

                int rankCounter = 0;

                foreach (t_companies company in rankableCompaniesOrdered)
                {
                    rankCounter++;
                    Console.WriteLine("{0}. {1}: {2} games @ {3}% win rate", rankCounter, company.companyName, company.total_matches, Math.Round((double)company.win_percent * 100, 3));
                    company.ResetRank();
                    company.waypointLeaderBoardRank = rankCounter;
                }

                spartanClashDB.SaveChanges();

            }
        }
    }
}
