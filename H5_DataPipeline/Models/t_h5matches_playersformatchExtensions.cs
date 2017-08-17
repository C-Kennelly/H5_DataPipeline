using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp;
using HaloSharp.Model.Halo5.Stats.CarnageReport;
using HaloSharp.Model;
using HaloSharp.Model.Halo5.Stats.Common;

namespace H5_DataPipeline.Models
{
    public partial class t_h5matches_playersformatch
    {
        public t_h5matches_playersformatch(string id)
        {
            matchID = id;
            team1_Players = null;
            team2_Players = null;
            //other_Players = null;
            //DNF_Players = null;  //TODO update model with check for JSON
            
            //team3_Players = null;
            //team4_Players = null;
            //team5_Players = null;
            //team6_Players = null;
            //team7_Players = null;
            //team8_Players = null;
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
                if (!(playerStat.DNF))
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

            string team1players = JsonConvert.

        }

        public t_h5matches_playersformatch(string id, WarzoneMatch carnageReport)
        {
            throw new NotImplementedException();
        }

        public t_h5matches_playersformatch(string id, CustomMatch carnageReport)
        {
            throw new NotImplementedException();
        }
    }

    
}
