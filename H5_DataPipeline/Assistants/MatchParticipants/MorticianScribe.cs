using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp;
using H5_DataPipeline.Models;
using H5_DataPipeline.Shared.Config;
using H5_DataPipeline.Assistants.Shared;
using System.Data.Entity.Validation;

namespace H5_DataPipeline.Assistants.MatchParticipants
{
    class MorticianScribe
    {
        private t_h5matches_playersformatch playersForMatchRecord;
        private t_h5matches parentMatchRecord;
        private SpartanCompanyRoster roster;
        private Referee referee;
        private int jobId;

        public MorticianScribe(t_h5matches_playersformatch playersRecord, t_h5matches matchRecord, SpartanCompanyRoster inMemoryRoster, Referee callingReferee, int jobNumber)
        {
            playersForMatchRecord = playersRecord;
            parentMatchRecord = matchRecord;
            roster = inMemoryRoster;
            referee = callingReferee;
            jobId = jobNumber;
        }


        public void SavePlayersForMatch()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_h5matches_playersformatch currentRecord = db.t_h5matches_playersformatch.FirstOrDefault(record =>
                                                                record.matchID == playersForMatchRecord.matchID
                                                            );
                try
                {
                    if (currentRecord == null)
                    {
                        db.t_h5matches_playersformatch.Add(playersForMatchRecord);
                        db.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Record was null");
                        //Record exists, don't touch it.
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("{0}: {1}", playersForMatchRecord.matchID, e.Message);
                }
                
            }

            UpdatePlayersForMatchDatesScanned(parentMatchRecord);
            referee.WaitToMarkJobDone(jobId);

        }

        private void UpdatePlayersForMatchDatesScanned(t_h5matches match)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_h5matches currentRecord = db.t_h5matches.Find(match.matchID);

                if (currentRecord != null)
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
