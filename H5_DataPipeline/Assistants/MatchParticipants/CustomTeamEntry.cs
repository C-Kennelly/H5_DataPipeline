using Newtonsoft.Json;

namespace H5_DataPipeline.Assistants.MatchParticipants
{
    public class CustomTeamEntry
    {
        public string teamId;
        public string teamType;

        public CustomTeamEntry()
        {

        }

        public CustomTeamEntry(string teamName, string teamTypeName)
        {
            teamId = teamName;
            teamType = teamTypeName;
        }

        public string GetJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
