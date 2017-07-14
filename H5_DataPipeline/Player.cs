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

        public void RecordMatchScan(DateTime dateOfLastMatchScan)
        {
            dateLastMatchScan = dateOfLastMatchScan;
        }
    }
}
