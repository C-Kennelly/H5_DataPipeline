﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_DataPipeline.Models
{
    public partial class t_teamsources
    {
        private static string waypointSourceName = "Halo Waypoint";
        private static string waypointSourceURL = "www.halowaypoint.com";
        private static string waypointTeamCommonName = "Spartan Company";

        public static string GetWaypointSourceName() { return waypointSourceName; }
        public static string GetWaypointSourceURL() { return waypointSourceURL; }
        public static string GetWaypointTeamCommonName() { return waypointTeamCommonName; }


        public static t_teamsources GetNewDefaultWaypointRecord()
        {
            return new t_teamsources
            {
                teamSource = GetWaypointSourceName(),
                sourceURL = GetWaypointSourceURL(),
                teamCommonName = GetWaypointTeamCommonName(),
                beganTrackingOnDate = DateTime.UtcNow
            };
        }
    }
}
