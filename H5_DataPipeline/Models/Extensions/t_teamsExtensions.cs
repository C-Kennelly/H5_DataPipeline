using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_DataPipeline.Models
{
    public partial class t_teams
    {
        private static string noWaypointCompanyFound = "0";
        private static string noWaypointCompanyFoundString = "[NOCOMPANYFOUND]";

        public static string GetNoWaypointCompanyFoundID() { return noWaypointCompanyFound; }
        public static string GetNoWaypointCompanyFoundString() { return noWaypointCompanyFoundString; }

        public static t_teams GetNewDefaultNoCompanyFoundRecord()
        {
            return new t_teams
            {
                teamId = GetNoWaypointCompanyFoundID(),
                teamName = GetNoWaypointCompanyFoundString(),
                teamSource = t_teamsources.GetWaypointSourceName(),
                beganTrackingDate = DateTime.UtcNow,
                lastUpdated = DateTime.UtcNow,
                parentTeamId = null,
                parentTeamName = null,
                parentTeamSource = null
            };
        }

    }
}
