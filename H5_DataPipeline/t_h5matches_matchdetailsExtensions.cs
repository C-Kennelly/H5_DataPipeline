using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp.Model.Halo5.Stats;

namespace H5_DataPipeline.Models
{
    public partial class t_h5matches_matchdetails
    {

        public t_h5matches_matchdetails(PlayerMatch match)
        {
            matchId = match.Id.MatchId.ToString();
            GameMode = (int)match.Id.GameMode;
            HopperId = match.HopperId.ToString();
            MapId = match.MapId.ToString();
            MapVariant_ResourceType = (int)match.MapVariant.ResourceType;
            MapVariant_ResourceId = match.MapVariant.ResourceId.ToString();
            MapVariant_OwnerType = (int)match.MapVariant.OwnerType;
            MapVariant_Owner = match.MapVariant.Owner.ToString();
            GameBaseVariantID = match.GameBaseVariantId.ToString();
            GameVariant_ResourceID = match.GameVariant.ResourceId.ToString();
            GameVariant_ResourceType = (int)match.GameVariant.ResourceType;
            GameVariant_OwnerType = (int)match.GameVariant.OwnerType;
            GameVariant_Owner = match.GameVariant.Owner.ToString();
            MatchCompleteDate = match.MatchCompletedDate.ISO8601Date;
            MatchDuration = match.MatchDuration.ToString();

            if (match.IsTeamGame)  //store new byte array representing 1 (true)
            { IsTeamGame = new byte[] { Byte.Parse(1.ToString()) }; }
            else                  //store new byte array representing 0 (false)
            { IsTeamGame = new byte[] { Byte.Parse(0.ToString()) }; }

            SeasonID = match.SeasonId.ToString();

        }
    }
}
