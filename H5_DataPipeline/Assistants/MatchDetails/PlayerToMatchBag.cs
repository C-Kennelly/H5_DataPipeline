using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp.Model.Halo5.Stats;

namespace H5_DataPipeline.Assistants.MatchDetails
{

    class PlayerToMatchBag
    {
        private ConcurrentDictionary<t_players, List<PlayerMatch>> matchBag;

        public PlayerToMatchBag()
        {
            matchBag = new ConcurrentDictionary<t_players, List<PlayerMatch>>();
        }

        public void WaitToAddPlayerMatchHistoryToBag(t_players player, List<PlayerMatch> matchHistory)
        {
            bool retry = true;

            while (retry)
            {
                bool result = matchBag.TryAdd(player, matchHistory);
                if (result == true)
                {
                    retry = false;
                }
            }
        }

        public void WriteAllPlayerMatchHistoriesToDatabase()
        {
            foreach(t_players player in matchBag.Keys)
            {
                List<PlayerMatch> matchHistory = WaitForMatchHistory(player);
                if(matchHistory != null)
                {
                    HistorianScribe scribe = new HistorianScribe(player, matchHistory);
                    scribe.RecordMatchHistoryForPlayer();
                }
            }
        }

        private List<PlayerMatch> WaitForMatchHistory(t_players key)
        {
            List<PlayerMatch> matchHistoryValue = null;
            bool retry = true;

            while (retry)
            {
                bool result = matchBag.TryGetValue(key, out matchHistoryValue);
                if (result == true)
                {
                    retry = false;
                }
            }

            return matchHistoryValue;
        }


    }
}
