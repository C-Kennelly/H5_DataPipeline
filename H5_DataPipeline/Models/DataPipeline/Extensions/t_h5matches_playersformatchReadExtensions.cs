using System;
using System.Collections.Generic;
using H5_DataPipeline.Assistants.MatchParticipants;
using H5_DataPipeline.Assistants.Shared;
using HaloSharp;
using HaloSharp.Model.Halo5.Stats.CarnageReport;
using Newtonsoft.Json;
using HaloSharp.Model.Halo5.Stats;


namespace H5_DataPipeline.Models.DataPipeline
{
    public partial class t_h5matches_playersformatch
    {
        public List<string> GetAllGamertagsInMatch()
        {
            List<string> result = new List<string>();

            result.AddRange(GetTeamGamertagsFromField(team1_Players));
            result.AddRange(GetTeamGamertagsFromField(team2_Players));
            result.AddRange(GetTeamGamertagsFromField(other_Players));
            result.AddRange(GetTeamGamertagsFromField(DNF_Players));

            return result;
        }

        public List<string> GetAllUnaffiliatedGamertagsInMatch()
        {
            List<string> result = new List<string>();

            result.AddRange(GetUnaffiliatedTeamGamertagsFromField(team1_Players));
            result.AddRange(GetUnaffiliatedTeamGamertagsFromField(team2_Players));
            result.AddRange(GetUnaffiliatedTeamGamertagsFromField(other_Players));
            result.AddRange(GetUnaffiliatedTeamGamertagsFromField(DNF_Players));

            return result;
        }

        public List<string> GetTeamGamertagsFromField(string JSONParticipantField)
        {
            List<string> result = new List<string>();

            if (JSONParticipantField != null)
            {
                List<MatchParticipantEntry> workingList = JsonConvert.DeserializeObject<List<MatchParticipantEntry>>(JSONParticipantField);
                foreach (MatchParticipantEntry entry in workingList)
                {
                    result.Add(entry.gamertag);
                }
            }

            return result;
        }

        public List<string> GetUnaffiliatedTeamGamertagsFromField(string JSONParticipantField)
        {
            List<string> result = new List<string>();

            if (JSONParticipantField != null)
            {
                List<MatchParticipantEntry> workingList = JsonConvert.DeserializeObject<List<MatchParticipantEntry>>(JSONParticipantField);
                foreach (MatchParticipantEntry entry in workingList)
                {
                    if (entry.spartanCompanyId == "")
                    {
                        result.Add(entry.gamertag);
                    }
                }
            }

            return result;
        }
    }
}
