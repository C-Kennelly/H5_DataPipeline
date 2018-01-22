using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;


namespace H5_DataPipeline.Assistants.Shared
{
    public class SpartanCompanyRoster
    {
        Dictionary<string, string> spartanCompanyRoster;

        public SpartanCompanyRoster()
        {
            spartanCompanyRoster = new Dictionary<string, string>();
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
                spartanCompanyRoster.Add(record.gamertag, record.teamId);
            }
        }

        /// <summary>
        /// Get the teamID associated with the gamertag from the team association table.
        /// Returns an empty string if none exists.
        /// </summary>
        /// <param name="gamertag"></param>
        /// <returns></returns>
        public string GetSpartanCompanyIdFromMemory(string gamertag)
        {
            string teamId = "";

            string inMemoryValue = "";

            bool foundTag = spartanCompanyRoster.TryGetValue(gamertag, out inMemoryValue);
    
            if (foundTag && inMemoryValue != null)
            {
                teamId = inMemoryValue;
            }

            return teamId;
        }

    }


}
