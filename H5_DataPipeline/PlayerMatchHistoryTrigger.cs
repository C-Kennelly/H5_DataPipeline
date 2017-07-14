using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_DataPipeline
{
    public class PlayerMatchHistoryTrigger
    {
        private Player player;

        public PlayerMatchHistoryTrigger(Player playerToCheck)
        {
            player = playerToCheck;
        }

        public bool TimeToSearchMatches(int dayThresholdToSearchMatches)
        {
            return false;
        }
    }
}
