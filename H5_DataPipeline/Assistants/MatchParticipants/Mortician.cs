using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp;
using H5_DataPipeline.Models;
using H5_DataPipeline.Shared.Config;
using H5_DataPipeline.Assistants.Shared;

namespace H5_DataPipeline.Assistants.MatchParticipants
{
    /// <summary>
    /// The mortician's job is to know who fought in every battle.  She scans matches for participants and tags clan battles accordingly.
    /// </summary>
    class Mortician
    {
        private IHaloSession haloSession;
        private SpartanClashSettings spartanClashSettings;
        private Referee referee;
        private inMemoryTeamRoster inMemoryRoster;

        public event MatchPlayersReadyToSaveToDatabaseHandler MatchPlayersReadyToSaveToDatabase;

        public Mortician(IHaloSession session, SpartanClashSettings settings)
        {
            haloSession = session;
            spartanClashSettings = settings;
            referee = new Referee();
            inMemoryRoster = new inMemoryTeamRoster();


            MatchPlayersReadyToSaveToDatabase += OnMatchPlayersReadyToSaveToDatabase;
        }


        public void ScanMatchesForParticipants()
        {
            Console.WriteLine("Updating Match Participants at: {0}", DateTime.UtcNow);
            Console.WriteLine();

            inMemoryRoster.RefreshInMemoryRoster();
            List<t_h5matches> matchesWithoutParticipatnsRecorded = GetMatchesSinceSiteLaunchWithoutParticipants();
            ProcessMatches(matchesWithoutParticipatnsRecorded);
            referee.WaitUntilAllJobsAreDone();

            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Finished updating Match Participants at: {0}", DateTime.UtcNow);
        }

        private List<t_h5matches> GetMatchesSinceSiteLaunchWithoutParticipants()
        {
            DateTime earliestTrackedMatchDate = spartanClashSettings.EarliestTrackedMatchDate();

            using (var db = new dev_spartanclashbackendEntities())
            {
                return db.t_h5matches.Where(match =>
                  match.t_h5matches_playersformatch == null
                  && match.t_h5matches_matchdetails.MatchCompleteDate > earliestTrackedMatchDate
                ).Take(1).ToList();
            }
        }

        private void ProcessMatches(List<t_h5matches> matches)
        {
            int counter = 0;
            int total = matches.Count;

            foreach (t_h5matches match in matches)
            {
                Console.Write("\rProcessing {0} of {1}: {2}                ", counter, total, match.matchID);

                referee.WaitToRegisterJob(counter);
                ProcessMatch(match, counter);

                counter++;
            }

            if(total == 0)
            {
                Console.Write("No matches for Mortician to process.");
            }
        }

        private async Task ProcessMatch(t_h5matches matchToQuery, int jobNumber)
        {
            ParticipantFinder playerFinder = new ParticipantFinder();   
            t_h5matches_playersformatch playersForMatch = await playerFinder.GetPlayersForMatch(matchToQuery, inMemoryRoster, haloSession);

            if(playersForMatch != null)
            {
                MatchPlayersReadyToSaveToDatabase?.BeginInvoke(this, new MatchPlayersReadyToSaveToDatabaseEventArgs(playersForMatch, matchToQuery, inMemoryRoster, referee, jobNumber), null, null);
            }
            else
            {
                referee.WaitToMarkJobDone(jobNumber);
            }
        }

        protected virtual void OnMatchPlayersReadyToSaveToDatabase(object Sender, MatchPlayersReadyToSaveToDatabaseEventArgs e)
        {
            MorticianScribe scribe = new MorticianScribe(e.GetPlayersForMatchRecord(), e.GetParentMatchRecord(), e.GetSpartanCompanyRoster(), e.GetReferee(), e.GetJobID());
            scribe.SavePlayersForMatch();
        }
    }
}
