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
                Console.Write("\rProcessing {0} of {1}: {2}                ", counter, total, match.matchID);
                ProcessMatch(match).Wait();
            }

            if(total == 0)
            {
                Console.Write("No matches for Mortician to process.");
            }
        }

        private async Task ProcessMatch(t_h5matches matchToQuery)
        {
            PlayerFinder playerFinder = new PlayerFinder();
            
            t_h5matches_playersformatch playersForMatch = await playerFinder.GetPlayersForMatch(matchToQuery, haloSession);

            if(playersForMatch != null)
            {
                SavePlayersForMatch(playersForMatch, matchToQuery);
            }
            else
            {
                //TODO - remove match or create record indicating bad results
            }
        }

        private void SavePlayersForMatch(t_h5matches_playersformatch playersRecord, t_h5matches parentRecord)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_h5matches_playersformatch currentRecord = db.t_h5matches_playersformatch.FirstOrDefault(record => 
                                                                record.matchID == playersRecord.matchID
                                                            );
            
                if(currentRecord == null)
                {
                    db.t_h5matches_playersformatch.Add(playersRecord);
                    db.SaveChanges();
                }
                else
                {
                    //Record exists, don't touch it.
                }
            
            
            }

            UpdatePlayersForMatchDatesScanned(parentRecord);

        }

        private void UpdatePlayersForMatchDatesScanned(t_h5matches match)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_h5matches currentRecord = db.t_h5matches.Find(match.matchID);

                if(currentRecord != null)
                {
                    currentRecord.datePlayersScan = DateTime.UtcNow;

                    db.SaveChanges();
                }
                else
                {
                    throw new NotImplementedException("Impossibility condition reached - couldn't find parent 'h5matches' record for matchID: " + match.matchID);
                }
            }
        }

    }
}
