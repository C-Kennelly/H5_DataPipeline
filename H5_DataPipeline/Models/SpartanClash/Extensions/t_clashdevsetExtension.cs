using H5_DataPipeline.Models.DataPipeline;
using System.Linq;

namespace H5_DataPipeline.Models.SpartanClash
{

    public partial class t_clashdevset
    {
        private int leaderboardDefaultStatus = 0;
        private int leaderboardDefaultRank = -1;

        public t_clashdevset(t_h5matches_teamsinvolved_halowaypointcompanies teamsInvolved, t_h5matches_matchdetails matchDetails, t_h5matches_ranksandscores matchRanksAndScores, t_h5matches_playersformatch matchParticipants)
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
            matchId = matchDetails.matchId;
            SeasonID = matchDetails.SeasonID;

            Team1_Company = teamsInvolved.team1_Primary;
            Team1_DNFCount = teamsInvolved.team1_DNFCount;
            Team2_Company = teamsInvolved.team2_Primary;
            Team2_DNFCount = teamsInvolved.team2_DNFCount;

            Team1_Gamertag = matchParticipants.GetTeamGamertagsFromField(matchParticipants.team1_Players).FirstOrDefault();
            if (Team1_Gamertag == null) { Team1_Gamertag = ""; }

            Team2_Gamertag = matchParticipants.GetTeamGamertagsFromField(matchParticipants.team2_Players).FirstOrDefault();
            if(Team2_Gamertag == null) { Team2_Gamertag = ""; }

            Team1_CSR = SetCSRFromField(matchParticipants.team1_Players);
            Team2_CSR = SetCSRFromField(matchParticipants.team2_Players);

            Team1_Rank = matchRanksAndScores.team1_Rank;
            Team2_Rank = matchRanksAndScores.team2_Rank;
            Team1_Score = matchRanksAndScores.team1_Score;
            Team2_Score = matchRanksAndScores.team2_Score;
            Status = leaderboardDefaultStatus;


        }

        public bool IsAWinFor(t_companies company)
        {

            if (company.companyId == Team1_Company || company.companyId == Team1_Company)
            {
                //Company is on team 1 
                if (Team1_Rank == 1) { return true; }
                else { return false; }
            }
            else
            {
                //Company is on team 2 
                if (Team1_Rank == 2) { return true; }
                else { return false; }
            }
        }

        private int SetCSRFromField(string teamPlayersJSON)
        {
            return 0;
            //Probably want to write an extenstion method on t_h5matches_playersformatch that spits out list of CSR's given a field.
            //Then want this method to calculate a team CSR based on available info.
            //NOTE THAT THIS IS AN INT
            //Also, there's probably a matter of Designation and Tiers not accounted for with only one field, so need to figure that out from design perspective.
        }


    }
}
