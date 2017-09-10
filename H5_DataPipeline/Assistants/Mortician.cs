using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp;
using H5_DataPipeline.Models;

namespace H5_DataPipeline.Assistants
{
    /// <summary>
    /// The mortician's job is to know who fought in every battle.  She scans matches for participants and tags clan battles accordingly.
    /// </summary>
    class Mortician
    {
        IHaloSession haloSession;

        public Mortician(IHaloSession session)
        {
            haloSession = session;
        }


        public void ScanMatchesForParticipants()
        {
            List<t_h5matches> matchesWithoutParticipatnsRecorded = GetMatchesWithoutParticipants();

            Console.WriteLine("Updating Match Participants at: {0}", DateTime.UtcNow);
            Console.WriteLine();

            ProcessMatches(matchesWithoutParticipatnsRecorded);

            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Finished updating Match Participants at: {0}", DateTime.UtcNow);
        }

        private List<t_h5matches> GetMatchesWithoutParticipants()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                return db.t_h5matches.Where(match =>
                  match.t_h5matches_playersformatch == null  
                ).ToList();
            }
        }

        private void ProcessMatches(List<t_h5matches> matches)
        {
            int counter = 0;
            int total = matches.Count;

            foreach (t_h5matches match in matches)
            {
                counter++;
                Console.Write("\rProcessing {0} of {1}: {2}", counter, total, match.matchID);

                ProcessMatch(match);
            }
        }

        private void ProcessMatch(t_h5matches match)
        {
            Console.WriteLine("Mortician doesn't actually process matches yet.");
                //Store players, and what company they were on when the match was saved
        }

    }
}
