using H5_DataPipeline.Models.DataPipeline;

namespace H5_DataPipeline.Models.SpartanClash
{

    public partial class t_clashdevset
    {
        private int leaderboardDefaultStatus = 0;
        private int leaderboardDefaultRank = -1;

        public t_clashdevset(t_h5matches_teamsinvolved_halowaypointcompanies teamsInvolved, t_h5matches_matchdetails matchDetails)
        {
            GameBaseVariantID = matchDetails.GameBaseVariantID;
            GameMode = matchDetails.GameMode;
            GameVariant_Owner = matchDetails.GameVariant_Owner;
            GameVariant_OwnerType = matchDetails.GameVariant_OwnerType;
            GameVariant_ResourceID = matchDetails.GameVariant_ResourceID;
            GameVariant_ResourceType = matchDetails.GameVariant_ResourceType;
            HopperId = matchDetails.HopperId;
            IsTeamGame = matchDetails.IsTeamGame;
            MapId = matchDetails.MapId;
            MapVariant_Owner = matchDetails.MapVariant_Owner;
            MapVariant_OwnerType = matchDetails.MapVariant_OwnerType;
            MapVariant_ResourceId = matchDetails.MapVariant_ResourceId;
            MapVariant_ResourceType = matchDetails.MapVariant_ResourceType;
            MatchCompleteDate = matchDetails.MatchCompleteDate;
            MatchDuration = matchDetails.MatchDuration;
            MatchId = matchDetails.matchId;
            SeasonID = matchDetails.SeasonID;

            Team1_Company = teamsInvolved.team1_Primary;
            Team1_DNFCount = teamsInvolved.team1_DNFCount;
            Team2_Company = teamsInvolved.team2_Primary;
            Team2_DNFCount = teamsInvolved.team2_DNFCount;

            Team1_Rank = leaderboardDefaultRank;
            Team1_Score = null;
            Team2_Score = null;
            Status = leaderboardDefaultStatus;

        }
    }
}
