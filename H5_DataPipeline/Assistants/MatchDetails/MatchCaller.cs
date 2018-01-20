using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HaloSharp;
using HaloSharp.Model;
using HaloSharp.Model.Common;
using HaloSharp.Model.Halo5.Stats;
using HaloSharp.Query.Halo5.Stats;
using HaloSharp.Extension;
using HaloSharp.Exception;


namespace H5_DataPipeline.Assistants.MatchDetails
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
                int skipCounter = allMatches.Count();

                MatchSet<PlayerMatch> matchSet = await QuerySpecificMatchesForTagInMode(tag, skipCounter, modes, session);
                    
                if (matchSet != null)
                {
                    allMatches.AddRange(matchSet.Results);

                    matchesRemaining = CheckIfMatchesRemaining(matchSet, earliestMatchDate);
                }
                else
                {
                    matchesRemaining = false;
                }

                //Console.Write("\rFound {0} matches so far for {1}", allMatches.Count, tag);
            }

            return allMatches;
        }

        private async Task<MatchSet<PlayerMatch>> QuerySpecificMatchesForTagInMode(string tag, int skipCounter, List<Enumeration.Halo5.GameMode> modes, IHaloSession session)
        {
            MatchSet<PlayerMatch> result = null;
            bool retry = true;

            while(retry)
            {

                retry = false;

                try
                {
                    MatchSet<PlayerMatch> matchSet = await session.Query(new GetMatchHistory(tag)
                                                        .Skip(skipCounter)
                                                        .InGameModes(modes));
                    result = matchSet;
                }
                catch (HaloApiException e)
                {
                    if (e.HaloApiError.Message.Contains("Rate limit"))
                    {
                        Console.WriteLine("MatchCaller: Rate Limit Hit");
                        Thread.Sleep(250);
                        retry = true;
                    }
                    else
                    {
                        Console.WriteLine("MatchCaller: The Halo API threw an exception for gamertag {0}, error {1} - {2}.  Stopping calls.", tag, e.HaloApiError.StatusCode, e.HaloApiError.Message);
                        result = null;
                    }
                    //TODO -> Handle errors here... removing 404's?  Common class for handling API errors?
                }
        }


            return result;
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
