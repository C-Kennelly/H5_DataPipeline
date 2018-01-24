using System;
using System.Collections.Generic;
using System.Linq;
using H5_DataPipeline.Models.DataPipeline;


namespace H5_DataPipeline.Assistants.Shared
{
    public class inMemoryTeamRoster
    {
        private Dictionary<string, string> teamRoster;

        public inMemoryTeamRoster()
        {
            teamRoster = new Dictionary<string, string>();
        }

        public void AddEntry(string gamertag, string teamId)
        {
            teamRoster.Add(gamertag, teamId);
        }

        public void RefreshInMemoryRoster()
        {
            List<t_players_to_teams> dbRecords = new List<t_players_to_teams>();
            using (var db = new dev_spartanclashbackendEntities())
            {
                dbRecords = db.t_players_to_teams.ToList();
            }

            Console.WriteLine("Storing {0} tags in the roster", dbRecords.Count);

            foreach (t_players_to_teams record in dbRecords)
            {
                teamRoster.Add(record.gamertag, record.teamId);
            }
        }

        /// <summary>
        /// Get the teamID associated with the gamertag from the team association table.
        /// Returns an empty string if none exists.
        /// </summary>
        /// <param name="gamertag"></param>
        /// <returns></returns>
        public string GetTeamIDFromGamertag(string gamertag)
        {
            string teamId = "";

            string inMemoryValue = "";

            bool foundTag = teamRoster.TryGetValue(gamertag, out inMemoryValue);
    
            if (foundTag && inMemoryValue != null)
            {
                teamId = inMemoryValue;
            }

            return teamId;
        }



    }
}
