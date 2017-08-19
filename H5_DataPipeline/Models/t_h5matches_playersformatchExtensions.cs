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

namespace H5_DataPipeline.Models
{
    public partial class t_h5matches_playersformatch
    {


        public t_h5matches_playersformatch(string id)
        {
            matchID = id;
            team1_Players = null;
            team2_Players = null;
            other_Players = null; //TODO update model with check for JSON
            DNF_Players = null;  //TODO update model with check for JSON
        }

        public t_h5matches_playersformatch(PlayerMatch match)
        {
            matchID = match.Id.MatchId.ToString();

            List<string> team1Players = new List<string>();
            List<string> team2Players = new List<string>();
            List<string> otherTeamPlayers = new List<string>();
            List<string> DNFPlayers = new List<string>();

            foreach (Player player in match.Players)
            {
                if (player.Rank == 0) 
                {
                    //Player Rank from https://developer.haloapi.com/docs/services/58acdf27e2f7f71ad0dad84b/operations/58acdf28e2f7f70db4854b3b
                    //   Did Not Finish = 0, 
                    DNFPlayers.Add(player.Identity.Gamertag);
                }
                else
                {
                    if (player.TeamId == 0) //Red Team
                    {
                        team1Players.Add(player.Identity.Gamertag);
                    }
                    else if (player.TeamId == 1) //Blue Team
                    {
                        team2Players.Add(player.Identity.Gamertag);
                    }
                    else  //other team or FFA
                    {
                        otherTeamPlayers.Add(player.Identity.Gamertag);
                    }
                }

            }

            team1_Players = JsonConvert.SerializeObject(team1Players);
            team2_Players = JsonConvert.SerializeObject(team2Players);
            other_Players = JsonConvert.SerializeObject(otherTeamPlayers);
            DNF_Players = JsonConvert.SerializeObject(DNFPlayers);

        }

        public t_h5matches_playersformatch(string id, ArenaMatch carnageReport)
        {
            matchID = id;

            List<string> team1Players = new List<string>();
            List<string> team2Players = new List<string>();
            List<string> otherTeamPlayers = new List<string>();
            List<string> DNFPlayers = new List<string>();
            

            foreach (ArenaMatchPlayerStat playerStat in carnageReport.PlayerStats)
            {
                if (playerStat.DNF)
                {
                    DNFPlayers.Add(playerStat.Player.Gamertag);
                }
                else
                {
                    if (playerStat.TeamId == 0) //Red Team
                    {
                        team1Players.Add(playerStat.Player.Gamertag);
                    }
                    else if (playerStat.TeamId == 1) //Blue Team
                    {
                        team2Players.Add(playerStat.Player.Gamertag);
                    }
                    else  //other team or FFA
                    {
                        otherTeamPlayers.Add(playerStat.Player.Gamertag);
                    }
                }
            }

            team1_Players = JsonConvert.SerializeObject(team1Players);
            team2_Players = JsonConvert.SerializeObject(team2Players);
            other_Players = JsonConvert.SerializeObject(otherTeamPlayers);
            DNF_Players = JsonConvert.SerializeObject(DNFPlayers);

        }

        public t_h5matches_playersformatch(string id, WarzoneMatch carnageReport)
        {
            matchID = id;

            List<string> team1Players = new List<string>();
            List<string> team2Players = new List<string>();
            List<string> otherTeamPlayers = new List<string>();
            List<string> DNFPlayers = new List<string>();


            foreach (WarzonePlayerStat playerStat in carnageReport.PlayerStats)
            {
                if (playerStat.DNF)
                {
                    DNFPlayers.Add(playerStat.Player.Gamertag);
                }
                else
                {
                    if (playerStat.TeamId == 0) //Red Team
                    {
                        team1Players.Add(playerStat.Player.Gamertag);
                    }
                    else if (playerStat.TeamId == 1) //Blue Team
                    {
                        team2Players.Add(playerStat.Player.Gamertag);
                    }
                    else  //other team or FFA
                    {
                        otherTeamPlayers.Add(playerStat.Player.Gamertag);
                    }
                }
            }

            team1_Players = JsonConvert.SerializeObject(team1Players);
            team2_Players = JsonConvert.SerializeObject(team2Players);
            other_Players = JsonConvert.SerializeObject(otherTeamPlayers);
            DNF_Players = JsonConvert.SerializeObject(DNFPlayers);
        }

        public t_h5matches_playersformatch(string id, CustomMatch carnageReport)
        {
            matchID = id;

            List<string> team1Players = new List<string>();
            List<string> team2Players = new List<string>();
            List<string> otherTeamPlayers = new List<string>();
            List<string> DNFPlayers = new List<string>();


            foreach (CustomMatchPlayerStat playerStat in carnageReport.PlayerStats)
            {
                if (playerStat.DNF)
                {
                    DNFPlayers.Add(playerStat.Player.Gamertag);
                }
                else
                {
                    if (playerStat.TeamId == 0) //Red Team
                    {
                        team1Players.Add(playerStat.Player.Gamertag);
                    }
                    else if (playerStat.TeamId == 1) //Blue Team
                    {
                        team2Players.Add(playerStat.Player.Gamertag);
                    }
                    else  //other team or FFA
                    {
                        otherTeamPlayers.Add(playerStat.Player.Gamertag);
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
