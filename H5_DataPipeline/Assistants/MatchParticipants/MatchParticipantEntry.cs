using System.Collections.Generic;
using Newtonsoft.Json;

namespace H5_DataPipeline.Assistants.MatchParticipants
{
    public class MatchParticipantEntry
    {
        public string gamertag;
        public string spartanCompanyId;
        public List<CustomTeamEntry> customTeams;
        public CSREntry previousCSR;
        public CSREntry currentCSR;

        public MatchParticipantEntry()
        {

        }

        public MatchParticipantEntry(string gt, string company, CSREntry previous = null, CSREntry current = null)
        {
            gamertag = gt;
            spartanCompanyId = company;
            customTeams = new List<CustomTeamEntry>(0);
            previousCSR = previous;
            currentCSR = current;
        }

        public MatchParticipantEntry(string gt, string company, List<CustomTeamEntry> teams, CSREntry previous = null, CSREntry current = null)
        {
            gamertag = gt;
            spartanCompanyId = company;
            customTeams = teams;
            previousCSR = previous;
            currentCSR = current;
        }

        public string GetJSON()
        {
            return JsonConvert.SerializeObject(this);
        }


    }
}
