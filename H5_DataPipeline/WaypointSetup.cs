using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_DataPipeline.Models
{
    class WaypointSetup
    {

        public static void SetupWaypointSourcesInDatabase()
        {
            AddDefaultTeamSourceRecord();
            AddDefaultTeamsRecords();
        }

        private static void AddDefaultTeamSourceRecord()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_teamsources defaultSpartanCompanySource = db.t_teamsources.Find(t_teamsources.GetWaypointSourceName());
                if (defaultSpartanCompanySource == null)
                {
                    db.t_teamsources.Add(t_teamsources.GetNewDefaultWaypointRecord());
                }
                db.SaveChanges();
            }
        }

        private static void AddDefaultTeamsRecords()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_teams defaultSpartanCompanyNoCompanyFoundTeam = db.t_teams.Find(t_teams.GetNoWaypointCompanyFoundID());
                if (defaultSpartanCompanyNoCompanyFoundTeam == null)
                {
                    db.t_teams.Add(t_teams.GetNewDefaultNoCompanyFoundRecord());
                }

                db.SaveChanges();
            }
        }
    }
}
