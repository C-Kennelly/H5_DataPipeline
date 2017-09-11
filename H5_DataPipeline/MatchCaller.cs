using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp;
using HaloSharp.Model;
using HaloSharp.Model.Common;
using HaloSharp.Model.Halo5.Stats;
using HaloSharp.Query.Halo5.Stats;
using HaloSharp.Extension;
using HaloSharp.Exception;


namespace H5_DataPipeline
{
    public class MatchCaller
    {
        private const int matchesPerCall = 25;

        public async Task<List<PlayerMatch>> GetH5MatchHistoryForPlayerAfterDate(string tag, DateTime earliestMatchDate, List<Enumeration.Halo5.GameMode> modes, IHaloSession session)
        {
            bool matchesRemaining = true;
            List<PlayerMatch> allMatches = new List<PlayerMatch>();

            while (matchesRemaining)
            {
                try
                {

                    MatchSet<PlayerMatch> matchSet = await session.Query(new GetMatchHistory(tag)
                                                .Skip(allMatches.Count)
                                                .InGameModes(modes) );

                    if (matchSet != null) { allMatches.AddRange(matchSet.Results); }

                    //Console.Write("\rFound {0} matches so far", allMatches.Count);

                        matchesRemaining = CheckIfMatchesRemaining(matchSet, earliestMatchDate);
                    }
                    catch (HaloApiException e)
                    {
                        Console.WriteLine("MatchCaller: The Halo API threw an exception for gamertag {0}, error {1} - {2}.  Stopping calls.", tag, e.HaloApiError.StatusCode, e.HaloApiError.Message);
                        matchesRemaining = false;
                        //TODO -> Handle errors here... removing 404's?  Common class for handling API errors?
                    }

                }
            }
               
            //Console.WriteLine();
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
