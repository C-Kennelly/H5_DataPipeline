using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_DataPipeline
{
    public class Player
    {
        private string gamertag { get; }
        private DateTime dateLastMatchScan;

        public Player(string playerName)
        {
            gamertag = playerName;
            dateLastMatchScan = DateTime.MinValue;
        }

        public void RecordMatchScan()
        {
            dateLastMatchScan = DateTime.UtcNow;
        }

        public void RecordMatchScan(DateTime dateOfLastMatchScan)
        {
            dateLastMatchScan = dateOfLastMatchScan;
        }

        //TODO: this threshhold should be stored in config, probably not passed in.
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

    }
}
