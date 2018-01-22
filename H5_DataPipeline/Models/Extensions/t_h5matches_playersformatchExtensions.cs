using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp;
using HaloSharp.Model.Halo5.Stats.CarnageReport;
using HaloSharp.Model.Halo5.Stats.CarnageReport.Common;
using HaloSharp.Model;
using HaloSharp.Model.Halo5.Stats.Common;
using HaloSharp.Model.Halo5.Stats;
using Newtonsoft.Json;
using H5_DataPipeline.Assistants.MatchParticipants;
using H5_DataPipeline.Assistants.Shared;

namespace H5_DataPipeline.Models
{
    public partial class t_h5matches_playersformatch
    {
        public t_h5matches_playersformatch()
        {

        }

        public t_h5matches_playersformatch(string id)
        {
            matchID = id;
            team1_Players = null;
            team2_Players = null;
            other_Players = null; //TODO update model with check for JSON
            DNF_Players = null;  //TODO update model with check for JSON
        }

        public List<string> ToListOfGamertags()
        {

            List<string> result = new List<string>();

            if (team1_Players != null)
            {
                List<MatchParticipantEntry> workingList = JsonConvert.DeserializeObject<List<MatchParticipantEntry>>(team1_Players);
                foreach (MatchParticipantEntry entry in workingList)
                {
                    result.Add(entry.gamertag);
                }
            }
            if (team2_Players != null)
            {
                List<MatchParticipantEntry> workingList = JsonConvert.DeserializeObject<List<MatchParticipantEntry>>(team1_Players);
                foreach (MatchParticipantEntry entry in workingList)
                {
                    result.Add(entry.gamertag);
                }
            }
            if (other_Players != null)
            {
                List<MatchParticipantEntry> workingList = JsonConvert.DeserializeObject<List<MatchParticipantEntry>>(team1_Players);
                foreach (MatchParticipantEntry entry in workingList)
                {
                    result.Add(entry.gamertag);
                }
            }
            if (DNF_Players != null)
            {
                List<MatchParticipantEntry> workingList = JsonConvert.DeserializeObject<List<MatchParticipantEntry>>(team1_Players);
                foreach (MatchParticipantEntry entry in workingList)
                {
                    result.Add(entry.gamertag);
                }
            }

            return result;
        }

        public t_h5matches_playersformatch(string id, ArenaMatch carnageReport, SpartanCompanyRoster roster)
        {
            matchID = id;

            List<MatchParticipantEntry> team1Players = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> team2Players = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> otherTeamPlayers = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> DNFPlayers = new List<MatchParticipantEntry>();
            

            foreach (ArenaMatchPlayerStat playerStat in carnageReport.PlayerStats)
            {
                string companyIDForTag = roster.GetSpartanCompanyIdFromMemory(playerStat.Player.Gamertag);

                if (playerStat.DNF)
                {
                    DNFPlayers.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag));
                }
                else
                {
                    if (playerStat.TeamId == 0) //Red Team
                    {
                        team1Players.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag));
                    }
                    else if (playerStat.TeamId == 1) //Blue Team
                    {
                        team2Players.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag));
                    }
                    else  //other team or FFA
                    {
                        otherTeamPlayers.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag));
                    }
                }
            }

            team1_Players = JsonConvert.SerializeObject(team1Players);
            team2_Players = JsonConvert.SerializeObject(team2Players);
            other_Players = JsonConvert.SerializeObject(otherTeamPlayers);
            DNF_Players = JsonConvert.SerializeObject(DNFPlayers);

        }

        public t_h5matches_playersformatch(string id, WarzoneMatch carnageReport, SpartanCompanyRoster roster)
        {
            matchID = id;

            List<MatchParticipantEntry> team1Players = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> team2Players = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> otherTeamPlayers = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> DNFPlayers = new List<MatchParticipantEntry>();


            foreach (WarzonePlayerStat playerStat in carnageReport.PlayerStats)
            {
                string companyIDForTag = roster.GetSpartanCompanyIdFromMemory(playerStat.Player.Gamertag);

                if (playerStat.DNF)
                {
                    DNFPlayers.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag));
                }
                else
                {
                    if (playerStat.TeamId == 0) //Red Team
                    {
                        team1Players.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag));
                    }
                    else if (playerStat.TeamId == 1) //Blue Team
                    {
                        team2Players.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag));
                    }
                    else  //other team or FFA
                    {
                        otherTeamPlayers.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag));
                    }
                }
            }

            team1_Players = JsonConvert.SerializeObject(team1Players);
            team2_Players = JsonConvert.SerializeObject(team2Players);
            other_Players = JsonConvert.SerializeObject(otherTeamPlayers);
            DNF_Players = JsonConvert.SerializeObject(DNFPlayers);
        }

        public t_h5matches_playersformatch(string id, CustomMatch carnageReport, SpartanCompanyRoster roster)
        {
            matchID = id;

            List<MatchParticipantEntry> team1Players = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> team2Players = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> otherTeamPlayers = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> DNFPlayers = new List<MatchParticipantEntry>();


            foreach (CustomMatchPlayerStat playerStat in carnageReport.PlayerStats)
            {
                string companyIDForTag = roster.GetSpartanCompanyIdFromMemory(playerStat.Player.Gamertag);

                if (playerStat.DNF)
                {
                    DNFPlayers.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag));
                }
                else
                {
                    if (playerStat.TeamId == 0) //Red Team
                    {
                        team1Players.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag));
                    }
                    else if (playerStat.TeamId == 1) //Blue Team
                    {
                        team2Players.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag));
                    }
                    else  //other team or FFA
                    {
                        otherTeamPlayers.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag));
                    }
                }
            }

            team1_Players = JsonConvert.SerializeObject(team1Players);
            team2_Players = JsonConvert.SerializeObject(team2Players);
            other_Players = JsonConvert.SerializeObject(otherTeamPlayers);
            DNF_Players = JsonConvert.SerializeObject(DNFPlayers);
        }

    }
}
