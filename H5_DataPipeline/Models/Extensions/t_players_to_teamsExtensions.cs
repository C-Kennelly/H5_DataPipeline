using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp.Model.Halo5.Stats;

namespace H5_DataPipeline.Models
{
    public partial class t_players_to_teams
    {
        public t_players_to_teams()
        {

        }

        public t_players_to_teams(Guid companyId, Member rosterEntry)
        {
            gamertag = rosterEntry.Identity.Gamertag;
            teamId = companyId.ToString();
            role = rosterEntry.Role;

            lastUpdated = DateTime.UtcNow;
            joinedDate = rosterEntry.JoinedDate.ISO8601Date;

            membershipLastModifiedDate = (DateTime)rosterEntry.LastModifiedDate.ISO8601Date;
        }
    }
}
