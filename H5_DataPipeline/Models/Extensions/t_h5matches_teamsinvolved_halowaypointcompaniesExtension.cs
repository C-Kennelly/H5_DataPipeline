using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace H5_DataPipeline.Models
{
    public partial class t_h5matches_teamsinvolved_halowaypointcompanies
    {

        public t_h5matches_teamsinvolved_halowaypointcompanies()
        {

        }

        public t_h5matches_teamsinvolved_halowaypointcompanies(string id)
        {
            matchID = id;
            teamSource = teamSource = t_teamsources.GetWaypointSourceName();
            team1_Primary = null;
            team2_Primary = null;

            //TODO - delete these from model
            team1_Secondary = null;
            team2_Secondary = null;
        }

        public t_h5matches_teamsinvolved_halowaypointcompanies(t_h5matches_playersformatch playersForMatchRecord, SpartanClashSettings settings)
        {
            matchID = playersForMatchRecord.matchID;
            teamSource = t_teamsources.GetWaypointSourceName();

            
            CalculateTeam1PrimaryAndSecondary(playersForMatchRecord, settings);
            CalculateTeam2PrimaryAndSecondary(playersForMatchRecord, settings);

        }



        private void CalculateTeam1PrimaryAndSecondary(t_h5matches_playersformatch playersForMatch, SpartanClashSettings settings)
        {
            //Defaults, in case there an no players that finished the match.
            team1_Secondary = null;
            team1_Primary = t_teams.GetNoWaypointCompanyFoundID();
            List<string> candidateCompanyIDs = new List<string>();

            List<string> team1Gamertags = JsonConvert.DeserializeObject<List<string>>(playersForMatch.team1_Players);

            //Add every company we can find
            foreach (string tag in team1Gamertags)
            {
                candidateCompanyIDs.Add(t_players.GetSpartanCompanyIDForGamertag(tag));
            }

            bool clanBattleFound = false;

            //Now evaluate the company candidates to see if this was a clan battle - Group them up by common ID's....            
            foreach (var group in candidateCompanyIDs.GroupBy(id => id))
            {
               //If the group is larger than the threshold based on gamertags that finished on team 1...
               //But ignore teams of 1 to filter free-for-all and solo-finishes.
               if (group.Count() >= (team1Gamertags.Count * settings.spartanCompanyClanBattleThreshold) && group.Count() > 1)
               {
                   team1_Primary = group.Key;
                   clanBattleFound = true;
               }
            
            }
        }

        private void CalculateTeam2PrimaryAndSecondary(t_h5matches_playersformatch playersForMatch, SpartanClashSettings settings)
        {
            //Defaults, in case there an no players that finished the match.
            team2_Secondary = null;
            team2_Primary = t_teams.GetNoWaypointCompanyFoundID();
            List<string> candidateCompanyIDs = new List<string>();

            List<string> team2Gamertags = JsonConvert.DeserializeObject<List<string>>(playersForMatch.team2_Players);

            //Add every company we can find
            foreach (string tag in team2Gamertags)
            {
                candidateCompanyIDs.Add(t_players.GetSpartanCompanyIDForGamertag(tag));
            }

            //Now evaluate the company candidates to see if this was a clan battle - Group them up by common ID's....            
            foreach (var group in candidateCompanyIDs.GroupBy(id => id))
            {
                //If the group is larger than the threshold based on gamertags that finished on team 1...
                //But ignore teams of 1 to filter free-for-all and solo-finishes.
                if (group.Count() >= (team2Gamertags.Count * settings.spartanCompanyClanBattleThreshold) && group.Count() > 1)
                {
                    team2_Primary = group.Key;
                }
                else
                {
                    //Default to no company found
                    team2_Primary = t_teams.GetNoWaypointCompanyFoundID();
                }
            }
        }
    }
}
