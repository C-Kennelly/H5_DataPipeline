using H5_DataPipeline.Models.DataPipeline;
using HaloSharp.Model.Halo5.Stats;
using System.Collections.Generic;

namespace H5_DataPipeline.Assistants.MatchDetails
{

    public delegate void PlayerMatchHistoryScannedHandler(object sender, PlayerMatchHistoryScannedEventArgs eventArgs);

    public class PlayerMatchHistoryScannedEventArgs
    {
        private List<PlayerMatch> matchHistoryToRecord;
        private t_players playerSubjectOfMatchHistory;
        private int jobIndex;
        

        public PlayerMatchHistoryScannedEventArgs(List<PlayerMatch> matchHistory, t_players player, int jobNumber)
        {
            matchHistoryToRecord = matchHistory;
            playerSubjectOfMatchHistory = player;
            jobIndex = jobNumber;
        }

        public List<PlayerMatch> GetMatchHistoryToRecord()
        {
            return matchHistoryToRecord;
        }

        public t_players GetPlayerSubjectOfMatchHistory()
        {
            return playerSubjectOfMatchHistory;
        }

        public int GetJobIndex()
        {
            return jobIndex;
        }

    }
}
