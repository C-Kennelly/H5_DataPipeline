using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp.Model;
using H5_DataPipeline.Models;

namespace H5_DataPipeline.Shared.Config
{
    //TODO - Make this setup activity a config file, or a read from a database.
    public class SpartanClashSettings
    {
        private List<Enumeration.Halo5.GameMode> gameModes = new List<Enumeration.Halo5.GameMode>();
        private void SetDefaultGameModes()
        {
            gameModes.Add(Enumeration.Halo5.GameMode.Arena);
            gameModes.Add(Enumeration.Halo5.GameMode.Warzone);
            gameModes.Add(Enumeration.Halo5.GameMode.Custom);
        }

        public SpartanClashSettings()
        {
            SetDefaultGameModes();
        }

        public List<Enumeration.Halo5.GameMode> GetGameModes()
        {
            return gameModes;
        }

        public DateTime EarliestTrackedMatchDate()
        {
            DateTime earliestDate = new DateTime(2018, 1, 1);

            using (var db = new dev_spartanclashbackendEntities())
            {
                DateTime dbValue = db.t_configoptions.Find("active").siteLaunchDate;
                if (dbValue != null)
                {
                    earliestDate = dbValue;
                }
            }

            return earliestDate;
        }

        public double GetSpartanCompanyClanBattleThreshold()
        {
            double threshold = 0.75;

            using (var db = new dev_spartanclashbackendEntities())
            {
                double dbValue = db.t_configoptions.Find("active").companyClanBattleThreshold;
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (dbValue != null)
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                {
                    threshold = dbValue;
                }
            }

            return threshold;
        }

        public double GetMatchHistoryReQueryDays()
        {
            double matchHistoryReQueryDays = 0.5;

            using (var db = new dev_spartanclashbackendEntities())
            {
                double dbValue = db.t_configoptions.Find("active").matchHistoryReQueryDays;
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (dbValue != null) {
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'

                    matchHistoryReQueryDays = dbValue;
                }
            }

            return matchHistoryReQueryDays;
        }
    }
}
