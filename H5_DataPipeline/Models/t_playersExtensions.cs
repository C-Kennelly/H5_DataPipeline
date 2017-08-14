using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_DataPipeline.Models
{
    public partial class t_players
    {

        public t_players(string playerName)
        {
            gamertag = playerName;
            scanThresholdInDays = 7;
        }

        public t_players(string playerName, int customScanThreshold)
        {
            gamertag = playerName;
            scanThresholdInDays = customScanThreshold;
        }

        public bool MatchesReadyToBeSearched()
        {
            return CheckDateAgainstThreshold(dateLastMatchScan);
        }
        public bool CompanyRosterReadyToBeSearched()
        {
            return CheckDateAgainstThreshold(dateCompanyRosterUpdated);
        }
        public bool CustomTeamRosterReadyToBeSearched()
        {
            return CheckDateAgainstThreshold(dateCustomTeamsUpdated);
        }
            
        private bool CheckDateAgainstThreshold(DateTime? dateToCheck)
        {
            DateTime thresholdDateTime = DateTime.UtcNow.AddDays(-1 * scanThresholdInDays);

            if (dateToCheck == null || dateToCheck < thresholdDateTime)
            {
                return true;
            }
            else
            {
                return false;
            }            
        }

        public void UpdateDatabase()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {

                if (FindCurrentRecord() == null)
                {
                    db.t_players.Add(this);
                }
                else
                {
                    db.Entry(this).State = EntityState.Modified;
                }

                db.SaveChanges();

            }
        }

        private t_players FindCurrentRecord()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                return db.t_players.Find(gamertag);
            }
        }



        public void RecordMatchScan()
        {
            dateLastMatchScan = DateTime.UtcNow;
        }

        public void RecordMatchScan(DateTime dateOfLastMatchScan)
        {
            dateLastMatchScan = dateOfLastMatchScan;
        }

        public void RecordCompanyScan()
        {
            dateCompanyRosterUpdated = DateTime.UtcNow;
        }

        public void RecordCompanyScan(DateTime dateOfLastMatchScan)
        {
            dateCompanyRosterUpdated = dateOfLastMatchScan;
        }

        public void RecordCustomTeamScan()
        {
            dateCustomTeamsUpdated = DateTime.UtcNow;
        }

        public void RecordCustomTeamScan(DateTime dateOfLastMatchScan)
        {
            dateCustomTeamsUpdated = dateOfLastMatchScan;
        }

    }
}
