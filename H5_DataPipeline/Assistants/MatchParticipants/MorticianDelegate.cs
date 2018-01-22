﻿using H5_DataPipeline.Assistants.Shared;
using H5_DataPipeline.Models;

namespace H5_DataPipeline.Assistants.MatchParticipants
{

    public delegate void MatchPlayersReadyToSaveToDatabaseHandler(object sender, MatchPlayersReadyToSaveToDatabaseEventArgs eventArgs);

    public class MatchPlayersReadyToSaveToDatabaseEventArgs
    {

        private t_h5matches_playersformatch playersForMatchRecord;
        private t_h5matches parentMatchRecord;
        private SpartanCompanyRoster roster;
        private Referee referee;
        private int jobId;

        public MatchPlayersReadyToSaveToDatabaseEventArgs(t_h5matches_playersformatch playersRecord, t_h5matches matchRecord, SpartanCompanyRoster inMemoryRoster, Referee callingReferee, int jobNumber)
        {
            playersForMatchRecord = playersRecord;
            parentMatchRecord = matchRecord;
            roster = inMemoryRoster;
            referee = callingReferee;
            jobId = jobNumber;
        }

        public t_h5matches_playersformatch GetPlayersForMatchRecord()
        {
            return playersForMatchRecord;
        }

        public t_h5matches GetParentMatchRecord()
        {
            return parentMatchRecord;
        }

        public SpartanCompanyRoster GetSpartanCompanyRoster()
        {
            return roster;
        }

        public Referee GetReferee()
        {
            return referee;
        }

        public int GetJobID()
        {
            return jobId;
        }

    }

}
