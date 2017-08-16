using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp;
using HaloSharp.Model;
using HaloSharp.Model.Common;
using HaloSharp.Model.Halo5.Stats;
using HaloSharp.Query.Halo5.Stats;
using HaloSharp.Extension;
using HaloSharp.Exception;


namespace H5_DataPipeline.Properties
{
    class MatchCaller
    {
        private const int matchesPerCall = 25;

        public async Task<List<PlayerMatch>> GetAllMatchesForPlayerAfterDate(string tag, DateTime earliestMatchDate, List<Enumeration.Halo5.GameMode> modes,  HaloClient client)
        {
            bool matchesRemaining = true;
            List<PlayerMatch> allMatches = new List<PlayerMatch>();

            using (var session = client.StartSession())
            {
                while (matchesRemaining)
                {
                    try
                    {
                        MatchSet<PlayerMatch> matchSet = await session.Query(new GetMatchHistory(tag).Take(matchesPerCall).Skip(allMatches.Count)
                                                    .InGameModes(modes) );

                        if (matchSet != null) { allMatches.AddRange(matchSet.Results); }

                        matchesRemaining = CheckIfMatchesRemaining(matchSet, earliestMatchDate);
                    }
                    catch (HaloApiException e)
                    {
                        Console.WriteLine("The Halo API threw an exception for gamertag {0}, status code: {1}.  Stopping calls.", tag, e.HaloApiError.StatusCode);
                        matchesRemaining = false;
                        //TODO -> Handle errors here... removing 404's?  Common class for handling API errors?
                    }
                }
            }
            return allMatches;
        }

        private bool CheckIfMatchesRemaining(MatchSet<PlayerMatch> lastHaloAPIMatchResult, DateTime lastMatchDate)
        {
            if (lastHaloAPIMatchResult != null 
                    && lastHaloAPIMatchResult.Results.Count == matchesPerCall 
                    && lastHaloAPIMatchResult.Results.Last<PlayerMatch>().MatchCompletedDate.ISO8601Date >= lastMatchDate )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
