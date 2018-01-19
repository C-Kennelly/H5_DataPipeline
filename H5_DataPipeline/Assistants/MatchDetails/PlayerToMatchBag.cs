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

        public void AddPlayerMatchHistoryToBag(t_players player, List<PlayerMatch> matchHistory)
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
            throw new NotImplementedException();
        }
    }
}
