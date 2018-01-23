using System.Collections.Generic;
using Newtonsoft.Json;

namespace H5_DataPipeline.Assistants.MatchParticipants
{
    public class MatchParticipantEntry
    {
        public string gamertag;
        public string spartanCompanyId;
        public List<CustomTeamEntry> customTeams;

        public MatchParticipantEntry()
        {

        }

        public MatchParticipantEntry(string gt, string company)
        {
            gamertag = gt;
            spartanCompanyId = company;
            customTeams = new List<CustomTeamEntry>(0);
        }

        public MatchParticipantEntry(string gt, string company, List<CustomTeamEntry> teams)
        {
            gamertag = gt;
            spartanCompanyId = company;
            customTeams = teams;
        }

        public string GetJSON()
        {
            return JsonConvert.SerializeObject(this);
        }


    }
}
