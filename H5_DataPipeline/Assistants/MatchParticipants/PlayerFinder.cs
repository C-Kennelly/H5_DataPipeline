using System;
using System.Threading.Tasks;
using HaloSharp;
using HaloSharp.Extension;
using HaloSharp.Exception;
using HaloSharp.Model.Halo5.Profile;
using HaloSharp.Query.Halo5.Profile;
using H5_DataPipeline.Models.DataPipeline;
using H5_DataPipeline.Assistants.Shared;

namespace H5_DataPipeline.Assistants.MatchParticipants
{
    class PlayerFinder
    {
        private string gamertag;
        private IHaloSession session;

        private PlayerAppearance playerAppearanceResult;

        public PlayerFinder()
        {

        }

        public PlayerFinder(string tag, IHaloSession haloSession)
        {
            gamertag = tag;
            session = haloSession;
        }

        /// <summary>
        /// Returns the spartan company ID from the Halo API.
        /// If no ID is found, returns the default Waypoint value from the config.
        /// </summary>
        /// <param name="gamertag"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public PlayerAppearance QuerySpartanCompanyInfo()
        {
            Task queryPlayerAppearanceTask = QueryPlayerAppearanceTask();

            queryPlayerAppearanceTask.Wait();
            
            return playerAppearanceResult;
        }

        private async Task QueryPlayerAppearanceTask()
        {
            playerAppearanceResult = await QueryPlayerAppearance();
        }

        private async Task<PlayerAppearance> QueryPlayerAppearance()
        {
            PlayerAppearance result = null;
            bool retry = true;

            while (retry)
            {
                retry = false;
                try
                {
                    var query = new GetPlayerAppearance(gamertag);
                    result = await session.Query(query);
                }
                catch (HaloApiException haloAPIException)
                {
                    if (haloAPIException.HaloApiError.Message.Contains("Rate limit"))
                    {
                        retry = true;
                        await Task.Delay(50);
                    }
                    else if (haloAPIException.HaloApiError.StatusCode == 404)
                    {
                        result = null;
                        using (var db = new dev_spartanclashbackendEntities())
                        {
                            t_teams companyRecord = db.t_teams.Find(gamertag);

                            if (companyRecord != null)
                            {
                                Console.WriteLine("{0} received a 404 from the Halo API, changing tracking index to -1.", companyRecord.teamName);
                                companyRecord.trackingIndex = -1;

                                db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("CompanyCaller: The Halo API threw an exception for company {0}, error {1} - {2}.  Stopping calls.", gamertag, haloAPIException.HaloApiError.StatusCode, haloAPIException.HaloApiError.Message);
                        result = null;
                        //TODO -> Handle errors here... removing 404's?  Common class for handling API errors?
                    }



                }

            }

            return result;
        }
    }

}
