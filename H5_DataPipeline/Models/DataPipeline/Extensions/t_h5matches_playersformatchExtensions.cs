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

        public t_h5matches_playersformatch(string id, ArenaMatch carnageReport, inMemoryTeamRoster roster, IHaloSession session)
        {
            matchID = id;

            List<MatchParticipantEntry> team1Players = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> team2Players = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> otherTeamPlayers = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> DNFPlayers = new List<MatchParticipantEntry>();
            

            foreach (ArenaMatchPlayerStat playerStat in carnageReport.PlayerStats)
            {
                Helper.CreatePlayerIfNotExists(playerStat.Player.Gamertag);
                string companyIDForTag = roster.GetTeamIDFromGamertag(playerStat.Player.Gamertag);

                
                CSREntry previous = new CSREntry(playerStat.PreviousCsr);
                CSREntry current = new CSREntry(playerStat.CurrentCsr);

                if (playerStat.DNF)
                {
                    DNFPlayers.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag, previous, current));
                }
                else
                {
                    if (playerStat.TeamId == 0) //Red Team
                    {
                        team1Players.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag, previous, current));
                    }
                    else if (playerStat.TeamId == 1) //Blue Team
                    {
                        team2Players.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag, previous, current));
                    }
                    else  //other team or FFA
                    {
                        otherTeamPlayers.Add(new MatchParticipantEntry(playerStat.Player.Gamertag, companyIDForTag, previous, current));
                    }
                }
            }

            team1_Players = JsonConvert.SerializeObject(team1Players);
            team2_Players = JsonConvert.SerializeObject(team2Players);
            other_Players = JsonConvert.SerializeObject(otherTeamPlayers);
            DNF_Players = JsonConvert.SerializeObject(DNFPlayers);


        }

        public t_h5matches_playersformatch(string id, WarzoneMatch carnageReport, inMemoryTeamRoster roster, IHaloSession session)
        {
            matchID = id;

            List<MatchParticipantEntry> team1Players = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> team2Players = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> otherTeamPlayers = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> DNFPlayers = new List<MatchParticipantEntry>();


            foreach (WarzonePlayerStat playerStat in carnageReport.PlayerStats)
            {
                Helper.CreatePlayerIfNotExists(playerStat.Player.Gamertag);
                string companyIDForTag = roster.GetTeamIDFromGamertag(playerStat.Player.Gamertag);

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

        public t_h5matches_playersformatch(string id, CustomMatch carnageReport, inMemoryTeamRoster roster, IHaloSession session)
        {
            matchID = id;

            List<MatchParticipantEntry> team1Players = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> team2Players = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> otherTeamPlayers = new List<MatchParticipantEntry>();
            List<MatchParticipantEntry> DNFPlayers = new List<MatchParticipantEntry>();


            foreach (CustomMatchPlayerStat playerStat in carnageReport.PlayerStats)
            {
                Helper.CreatePlayerIfNotExists(playerStat.Player.Gamertag);
                string companyIDForTag = roster.GetTeamIDFromGamertag(playerStat.Player.Gamertag);

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

        public List<string> ToListOfGamertags()
        {
            List<string> result = new List<string>();

            result.AddRange(GetTeamGamertagsFromField(team1_Players));
            result.AddRange(GetTeamGamertagsFromField(team2_Players));
            result.AddRange(GetTeamGamertagsFromField(other_Players));
            result.AddRange(GetTeamGamertagsFromField(DNF_Players));

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

       
    }
}
