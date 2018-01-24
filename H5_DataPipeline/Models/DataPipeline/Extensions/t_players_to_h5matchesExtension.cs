using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_DataPipeline.Models.DataPipeline
{
    public partial class t_players_to_h5matches
    {
        public t_players_to_h5matches()
        {

        }

        public t_players_to_h5matches(string tag, string id)
        {
            gamertag = tag;
            matchID = id;
            dummyColumnAboutAssociation = null;
        }
    }
}
