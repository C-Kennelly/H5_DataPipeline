using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp.Model;
using H5_DataPipeline.Models;

namespace H5_DataPipeline
{
    //TODO - Make this setup activity a config file, or a read from a database.
    class SpartanClashSettings
    {
        public List<Enumeration.Halo5.GameMode> gameModes = new List<Enumeration.Halo5.GameMode>();
        public DateTime lookForMatchesNoEarlierThan;


        public SpartanClashSettings()
        {
            SetEarliestDate();
            SetDefaultGameModes();
        }

        private void SetEarliestDate()
        {
            lookForMatchesNoEarlierThan = new DateTime(2017, 06, 01, 0, 0, 0);
        }

        private void SetDefaultGameModes()
        {
            gameModes.Add(Enumeration.Halo5.GameMode.Arena);
            gameModes.Add(Enumeration.Halo5.GameMode.Warzone);
            gameModes.Add(Enumeration.Halo5.GameMode.Custom);
        }

        public DateTime GetDateToSearchFrom(t_players player)
        {
            DateTime result = lookForMatchesNoEarlierThan;
            DateTime playerRecordScanDate = player.GetEarliestDateToScanMatches();

            if (playerRecordScanDate > result)
            {
                result = playerRecordScanDate;
            }

            return result;
        }
    }
}
