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
        //private string gamertag;
        //private DateTime dateLastMatchScan;

        public t_players(string playerName)
        {
            gamertag = playerName;
            SetDateLastScanToMinVale();
        }

        private void SetDateLastScanToMinVale()
        {
            RecordMatchScan(DateTime.MinValue);
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

        public bool MatchesReadyToBeSearched(int dayThresholdToSearchMatches)
        {
            DateTime threshold = DateTime.UtcNow.AddDays(-1 * dayThresholdToSearchMatches);

            if (dateLastMatchScan < threshold)
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

    }
}
