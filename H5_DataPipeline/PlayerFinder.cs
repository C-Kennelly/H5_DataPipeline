using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp;
using HaloSharp.Model.Halo5.Stats.CarnageReport;
using HaloSharp.Query.Halo5.Stats.CarnageReport;
using HaloSharp.Model.Halo5.Stats.CarnageReport.Common;
using HaloSharp.Extension;
using HaloSharp.Exception;
using HaloSharp.Model;
using System.Threading;

namespace H5_DataPipeline
{
    public class PlayerFinder
    {

        public async Task<t_h5matches_playersformatch> GetPlayersForMatch(t_h5matches_matchdetails match, HaloClient client)
        {
            t_h5matches_playersformatch result = null;

            switch(match.GameMode)
            {
                case ((int)Enumeration.Halo5.GameMode.Arena):
                {
                    ArenaMatch carnageReport = await GetArenaMatchCarnageReport(match.matchId, client);
                    result = new t_h5matches_playersformatch(match.matchId, carnageReport);
                    break;
                }
                case ((int)Enumeration.Halo5.GameMode.Warzone):
                {
                    WarzoneMatch carnageReport = await GetWarzoneMatchCarnageReport(match.matchId, client);
                    result = new t_h5matches_playersformatch(match.matchId, carnageReport);
                    break;
                }

                case ((int)Enumeration.Halo5.GameMode.Custom):
                {
                    CustomMatch carnageReport = await GetCustomMatchCarnageReport(match.matchId, client);
                    result = new t_h5matches_playersformatch(match.matchId, carnageReport);
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


        private async Task<ArenaMatch> GetArenaMatchCarnageReport(string matchID, HaloClient client)
        {
            ArenaMatch arenaCarnageReport = null;

            using (var session = client.StartSession())
            {
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
                        if (haloAPIException.HaloApiError.StatusCode == 429)
                        {
                            resultFound = false;
                            Thread.Sleep(500);
            
                        }
                        Console.WriteLine("PlayerFinderArnea: The Halo API threw an exception for match {0}, status code: {1}.  Stopping calls.", matchID, haloAPIException.HaloApiError.StatusCode);
                    }
                }
                return arenaCarnageReport;

            }
        }

        private async Task<WarzoneMatch> GetWarzoneMatchCarnageReport(string matchID, HaloClient client)
        {
            WarzoneMatch warzoneCarnageReport = null;

            using (var session = client.StartSession())
            {
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
                        if (haloAPIException.HaloApiError.StatusCode == 429)
                        {
                            resultFound = false;
                            Thread.Sleep(500);
                        }
                        Console.WriteLine("PlayerFinderWarzone: The Halo API threw an exception for match {0}, status code: {1}.  Stopping calls.", matchID, haloAPIException.HaloApiError.StatusCode);
                    }

                }
                return warzoneCarnageReport;
            }
        }

        private async Task<CustomMatch> GetCustomMatchCarnageReport(string matchID, HaloClient client)
        {
            CustomMatch customCarnageReport = null;

            using (var session = client.StartSession())
            {
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
                        if (haloAPIException.HaloApiError.StatusCode == 429)
                        {
                            resultFound = false;
                            Thread.Sleep(500);
                        }
                        Console.WriteLine("PlayerFinderCustom: The Halo API threw an exception for match {0}, status code: {1}.  Stopping calls.", matchID, haloAPIException.HaloApiError.StatusCode);
                    }

                }

                return customCarnageReport;
            }
        }
    }
}
