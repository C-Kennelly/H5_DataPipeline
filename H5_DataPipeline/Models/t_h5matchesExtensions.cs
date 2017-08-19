using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp.Model.Halo5.Stats;
using System.Data.Entity;

namespace H5_DataPipeline.Models
{
    public partial class t_h5matches
    {

        public t_h5matches(string id)
        {
            matchID = id;
            dateDetailsScan = null;
            datePlayersScan = null;
            dateResultsScan = null;
            dateCompaniesInvolvedUpdated = null;
            dateCustomTeamsUpdated = null;
            queryStatus = 0;
        }

        public t_h5matches(PlayerMatch playerName)
        {
            matchID = playerName.Id.MatchId.ToString();
            dateDetailsScan = null;
            datePlayersScan = null;
            dateResultsScan = null;
            dateCompaniesInvolvedUpdated = null;
            dateCustomTeamsUpdated = null;
            queryStatus = 0;
        }

        public bool AlreadySavedToDatabase()
        {
            if (FindCurrentRecordIfExists() == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public t_h5matches FindCurrentRecordIfExists()
        {
            using(var db = new dev_spartanclashbackendEntities())
            {
                return db.t_h5matches.Find(matchID);
            }
        }

        public void UpdateDatabase()
        {

            using (var db = new dev_spartanclashbackendEntities())
            {
                if(FindCurrentRecordIfExists() == null)
                {
                    db.t_h5matches.Add(this);
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
