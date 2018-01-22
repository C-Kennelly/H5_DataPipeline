using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using H5_DataPipeline.Shared.Config;
using H5_DataPipeline.Assistants.MatchParticipants;

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
        }

        public t_h5matches_teamsinvolved_halowaypointcompanies(t_h5matches_playersformatch playersForMatchRecord, SpartanClashSettings settings)
        {
            matchID = playersForMatchRecord.matchID;
            teamSource = t_teamsources.GetWaypointSourceName();
          
            CalculateTeam1ClanParticipation(playersForMatchRecord, settings);
            CalculateTeam2ClanParticipation(playersForMatchRecord, settings);

            team1_DNFCount = CalculateSpartanCompanyDNFPresence(team1_Primary, playersForMatchRecord);
            team2_DNFCount = CalculateSpartanCompanyDNFPresence(team2_Primary, playersForMatchRecord);
        }

        private void CalculateTeam1ClanParticipation(t_h5matches_playersformatch playersForMatch, SpartanClashSettings settings)
        {
            List<MatchParticipantEntry> team1ParticipantEntries = JsonConvert.DeserializeObject<List<MatchParticipantEntry>>(playersForMatch.team1_Players);

            List<string> candidateCompanyIDs = GenerateCandidateCompanyIDs(team1ParticipantEntries);

            team1_Primary = CalculatePrimaryTeamParticipationFromCandidates(candidateCompanyIDs, team1ParticipantEntries.Count, settings);
        }

        private void CalculateTeam2ClanParticipation(t_h5matches_playersformatch playersForMatch, SpartanClashSettings settings)
        {
            List<MatchParticipantEntry> team2ParticipantEntries = JsonConvert.DeserializeObject<List<MatchParticipantEntry>>(playersForMatch.team2_Players);

            List<string> candidateCompanyIDs = GenerateCandidateCompanyIDs(team2ParticipantEntries);

            team2_Primary = CalculatePrimaryTeamParticipationFromCandidates(candidateCompanyIDs, team2ParticipantEntries.Count, settings);
        }

        /// <summary>
        /// Get list of all non-Default SpartanCompanyID's from a list of participants.
        /// </summary>
        /// <param name="participantEntries"></param>
        /// <returns></returns>
        private List<string> GenerateCandidateCompanyIDs(List<MatchParticipantEntry> participantEntries)
        {
            List<string> candidateCompanyIDs = new List<string>();

            foreach (MatchParticipantEntry entry in participantEntries)
            {
                if (entry.spartanCompanyId != "" && entry.spartanCompanyId != null)
                {
                    candidateCompanyIDs.Add(entry.spartanCompanyId);
                }
            }

            return candidateCompanyIDs;
        }

        private string CalculatePrimaryTeamParticipationFromCandidates(List<string> candidateCompanyIDs, int teamSize, SpartanClashSettings settings)
        {
            string primaryTeam = t_teams.GetNoWaypointCompanyFoundID(); ;

            foreach (var group in candidateCompanyIDs.GroupBy(id => id))
            {
                //If the group is larger than the threshold based on gamertags that finished on team 1...
                //But ignore teams of 1 to filter free-for-all and solo-finishes.
                if (group.Count() >= GetClanBattleThreshold(teamSize, settings) 
                    && group.Count() > 1)
                {
                    primaryTeam = group.Key;
                }
            }

            return primaryTeam;
        }

        private double GetClanBattleThreshold(int teamSize, SpartanClashSettings settings)
        {
            return (teamSize * settings.GetSpartanCompanyClanBattleThreshold());
        }

        public int CalculateSpartanCompanyDNFPresence(string clanID, t_h5matches_playersformatch playersForMatch)
        {
            if (clanID == "" || clanID == null)
            {
                return -1;
            }

            List<MatchParticipantEntry> team1ParticipantEntries = JsonConvert.DeserializeObject<List<MatchParticipantEntry>>(playersForMatch.DNF_Players);

            int dnfCounter = 0;

            foreach (MatchParticipantEntry entry in team1ParticipantEntries)
            {
                if (entry.spartanCompanyId == clanID)
                {
                    dnfCounter++;
                }
            }

            return dnfCounter;
        }

    }
}
