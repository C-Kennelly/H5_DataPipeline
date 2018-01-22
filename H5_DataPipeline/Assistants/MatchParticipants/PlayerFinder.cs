using System;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp;
using HaloSharp.Exception;
using HaloSharp.Extension;
using HaloSharp.Model;
using HaloSharp.Model.Halo5.Stats.CarnageReport;
using HaloSharp.Query.Halo5.Stats.CarnageReport;
using H5_DataPipeline.Assistants.Shared;

namespace H5_DataPipeline.Assistants.MatchParticipants
{
    public class PlayerFinder
    {
        public async Task<t_h5matches_playersformatch> GetPlayersForMatch(t_h5matches match, SpartanCompanyRoster roster, IHaloSession session)
        {
            t_h5matches_playersformatch result = null;
            int? gameMode = GetGameModeForMatch(match);
            
            switch(gameMode)
            {
                case ((int)Enumeration.Halo5.GameMode.Arena):
                {
                    ArenaMatch carnageReport = await GetArenaMatchCarnageReport(match.matchID, session);
                    result = new t_h5matches_playersformatch(match.matchID, carnageReport, roster);
                    break;
                }
                case ((int)Enumeration.Halo5.GameMode.Warzone):
                {
                    WarzoneMatch carnageReport = await GetWarzoneMatchCarnageReport(match.matchID, session);
                    result = new t_h5matches_playersformatch(match.matchID, carnageReport, roster);
                    break;
                }

                case ((int)Enumeration.Halo5.GameMode.Custom):
                {
                    CustomMatch carnageReport = await GetCustomMatchCarnageReport(match.matchID, session);
                    result = new t_h5matches_playersformatch(match.matchID, carnageReport, roster);
                    break;
                }
                case ((int)Enumeration.Halo5.GameMode.Campaign):
                {
                    result = null;
                    break;
                }
                case ((int)Enumeration.Halo5.GameMode.Error): 
                {
                    result = null;
                    break;
                }
                default:
                {
                    result = null;
                    break;
                }
            }

            return result;

        }

        private int? GetGameModeForMatch(t_h5matches matchToFind)
        {
            t_h5matches_matchdetails matchDetails;

            using (var db = new dev_spartanclashbackendEntities())
            {
                matchDetails = db.t_h5matches_matchdetails.Find(matchToFind.matchID);
            }

            if (matchDetails != null)
            {
                return matchDetails.GameMode;
            }
            else
            {
                throw new NotImplementedException("Don't know the game mode, can't find the players!");
            }
        }

        private async Task<ArenaMatch> GetArenaMatchCarnageReport(string matchID, IHaloSession session)
        {
            ArenaMatch arenaCarnageReport = null;

            bool resultFound = false;

            while(resultFound == false)
            {
                resultFound = true;
                try
                {
                    
                    arenaCarnageReport = await session.Query(new GetArenaMatchDetails(new Guid(matchID)));
                }
                catch (HaloApiException haloAPIException)
                {
                    if (haloAPIException.HaloApiError.Message.Contains("Rate limit"))
                    {
                        Console.WriteLine("PlayerFinder: Rate Limit Hit");
                        resultFound = false;
                        await Task.Delay(50);
                    }
                    else
                    {
                        Console.WriteLine("The Halo API threw an exception for match {0}, status code: {1}.  Stopping calls.", matchID, haloAPIException.HaloApiError.StatusCode);
                    }
                }
            }
            return arenaCarnageReport;

               
        }

        private async Task<WarzoneMatch> GetWarzoneMatchCarnageReport(string matchID, IHaloSession session)
        {
            WarzoneMatch warzoneCarnageReport = null;

            bool resultFound = false;

            while (resultFound == false)
            {
                resultFound = true;
                try
                {
                    warzoneCarnageReport = await session.Query(new GetWarzoneMatchDetails(new Guid(matchID)));
                }
                catch (HaloApiException haloAPIException)
                {
                    if (haloAPIException.HaloApiError.Message.Contains("Rate limit"))
                    {
                        Console.WriteLine("PlayerFinder: Rate Limit Hit");
                        resultFound = false;
                        await Task.Delay(50);
                    }
                    else
                    {
                        Console.WriteLine("The Halo API threw an exception for match {0}, status code: {1}.  Stopping calls.", matchID, haloAPIException.HaloApiError.StatusCode);
                    }
                }

            }
            return warzoneCarnageReport;
               
        }

        private async Task<CustomMatch> GetCustomMatchCarnageReport(string matchID, IHaloSession session)
        {
            CustomMatch customCarnageReport = null;
            
            bool resultFound = false;

            while(resultFound == false)
            {
                resultFound = true;
                try
                {
                    customCarnageReport = await session.Query(new GetCustomMatchDetails(new Guid(matchID)));
                }
                catch (HaloApiException haloAPIException)
                {
                    if (haloAPIException.HaloApiError.Message.Contains("Rate limit"))
                    {
                        Console.WriteLine("PlayerFinder: Rate Limit Hit");
                        resultFound = false;
                        await Task.Delay(50);
                    }
                    else
                    {
                        Console.WriteLine("The Halo API threw an exception for match {0}, status code: {1}.  Stopping calls.", matchID, haloAPIException.HaloApiError.StatusCode);
                    }
                }

            }

            return customCarnageReport;
               
        }
    }
}
