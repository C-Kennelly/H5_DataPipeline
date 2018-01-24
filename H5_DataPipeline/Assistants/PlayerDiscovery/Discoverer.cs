using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.DataPipeline;
using HaloSharp.Model.Halo5.Profile;
using HaloSharp;
using H5_DataPipeline.Assistants.MatchParticipants;
using H5_DataPipeline.Assistants.Shared;

namespace H5_DataPipeline.Assistants.PlayerDiscovery
{
    class Discoverer
    {
        //returns the ID right now, but is supposed to add the gamertag/company association to the database and roster
        public string QueryForCompanyIDAndUpdateDatabaseAndRoster(string gamertag, inMemoryTeamRoster roster, IHaloSession session)
        {
            
            string result = t_teams.GetNoWaypointCompanyFoundID();
            PlayerFinder playerFinder = new PlayerFinder(gamertag, session);

            PlayerAppearance playerAppearance = playerFinder.QuerySpartanCompanyInfo();
            if (playerAppearance != null && playerAppearance.Company != null)
            {
                string companyId = playerAppearance.Company.Id.ToString();
                if (companyId != null && companyId != "")
                {
                    result = companyId;
                    UpdateDatabaseAndRoster(gamertag, playerAppearance.Company.Id.ToString(), playerAppearance.Company.Name.ToString(), roster);
                }
            }
            else
            {
                UpdateDatabaseAndRoster(gamertag, t_teams.GetNoWaypointCompanyFoundID(), t_teams.GetNoWaypointCompanyFoundString(), roster);
            }

            return result;
        }

        private void UpdateDatabaseAndRoster(string gamertag, string companyID, string teamName, inMemoryTeamRoster roster)
        {
            t_players_to_teams newRecord = UpdateDatabase(gamertag, companyID, teamName);

            if (newRecord != null)
            {
                UpdateRoster(newRecord.gamertag, newRecord.teamId, roster);

            }
        }

        private t_players_to_teams UpdateDatabase(string gamertag, string companyID, string teamName)
        {
            string waypointSourceName = t_teamsources.GetWaypointSourceName();
            Helper.CreateUntrackedTeamIfNotExist(companyID, teamName, waypointSourceName);
            t_players_to_teams result = null;

            using (var db = new dev_spartanclashbackendEntities())
            {

                t_players_to_teams currentRecord = db.t_players_to_teams.Where(record => record.gamertag == gamertag
                                                                            && record.t_teams.teamSource == waypointSourceName)
                                                                            .FirstOrDefault();

                if (currentRecord == null)
                {
                    db.t_players_to_teams.Add(new t_players_to_teams(new Guid(companyID), gamertag));
                }
                else
                {
                    currentRecord.teamId = companyID;
                }

                db.SaveChanges();
            }


            return result;
        }

        private void UpdateRoster(string gamertag, string companyID, inMemoryTeamRoster roster)
        {
            roster.AddEntry(gamertag, companyID);
        }
    }
}
