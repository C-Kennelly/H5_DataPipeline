using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;

namespace H5_DataPipeline
{
    class TeamRosterRefresher
    {
        private List<t_players_to_teams> rosterToUpdate;
        private List<t_players> playersToScan;
        private DateTime thresholdDateTime;

        public TeamRosterRefresher(int thresholdToUpdateInDays)
        {
            thresholdDateTime = DateTime.UtcNow.AddDays(-1 * thresholdToUpdateInDays);

            
        }

        public void RefreshTeamRosters()
        {
            RefreshAllPlayersCompanyRosters();
            //RefreshAllPlayersCustomTeamRosters();
        }

        private void RefreshAllPlayersCompanyRosters()
        {
            SetPlayersToScan();
            ScanPlayers();
            //UpdateDatabase();
        }

        private void SetPlayersToScan()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                playersToScan = db.t_players.Where(player => player.dateCompanyRosterUpdated < thresholdDateTime).ToList();
            }
        }

        private void ScanPlayers()
        {
            foreach(t_players player in playersToScan)
            {
                //string companyName = GetCompanyNameForPlayer(player);

                
            }
        }
    }
}
