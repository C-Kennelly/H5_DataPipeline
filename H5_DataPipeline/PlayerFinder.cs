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
                default:
                {
                    throw new NotImplementedException();
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
                try
                {
                    arenaCarnageReport = await session.Query(new GetArenaMatchDetails(new Guid(matchID)));
                }
                catch (HaloApiException haloAPIException)
                {
                    Console.WriteLine("The Halo API threw an exception for match {0}, status code: {1}.  Stopping calls.", matchID, haloAPIException.HaloApiError.StatusCode);
                }

                return arenaCarnageReport;
            }
        }

        private async Task<WarzoneMatch> GetWarzoneMatchCarnageReport(string matchID, HaloClient client)
        {
            WarzoneMatch warzoneCarnageReport = null;

            using (var session = client.StartSession())
            {
                try
                {
                    warzoneCarnageReport = await session.Query(new GetWarzoneMatchDetails(new Guid(matchID)));
                }
                catch (HaloApiException haloAPIException)
                {
                    Console.WriteLine("The Halo API threw an exception for match {0}, status code: {1}.  Stopping calls.", matchID, haloAPIException.HaloApiError.StatusCode);
                }

                return warzoneCarnageReport;
            }
        }

        private async Task<CustomMatch> GetCustomMatchCarnageReport(string matchID, HaloClient client)
        {
            CustomMatch customCarnageReport = null;

            using (var session = client.StartSession())
            {
                try
                {
                    customCarnageReport = await session.Query(new GetCustomMatchDetails(new Guid(matchID)));
                }
                catch (HaloApiException haloAPIException)
                {
                    Console.WriteLine("The Halo API threw an exception for match {0}, status code: {1}.  Stopping calls.", matchID, haloAPIException.HaloApiError.StatusCode);
                }

                return customCarnageReport;
            }
        }
    }
}
