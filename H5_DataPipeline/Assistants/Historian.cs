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
    /// The historian's job is to know the details of every battle.  She goes through tracked players and scans recent games, recording their details and scores.
    /// </summary>
    class Historian
    {
        IHaloSession haloSession;

        public Historian(IHaloSession session)
        {
            haloSession = session;
        }

        public void RecordRecentGames()
        {
            List<t_players> trackedPlayers = GetTrackedPlayers();

            Console.WriteLine("Updating player Match Histories at: {0}", DateTime.UtcNow);
            Console.WriteLine();

            ProcessPlayers(trackedPlayers);

            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Finished updating player Match Histories at: {0}", DateTime.UtcNow);
        }

        private List<t_players> GetTrackedPlayers()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                return db.t_players.ToList();
            }
        }

        private void ProcessPlayers(List<t_players> players)
        {
            int counter = 0;
            int total = players.Count;

            foreach (t_players player in players)
            {
                counter++;
                Console.Write("\rProcessing {0} of {1}: {2}", counter, total, player.gamertag);

                ProcessPlayer(player);
            }

            if (total == 0)
            {
                Console.Write("No matches for Historian to process.");
            }
        }

        private void ProcessPlayer(t_players player)
        {
            Console.WriteLine("Historian doesn't actually process players yet.");
            //GetListOfMatchesBeforeDate();
            //StoreMatchDetails();
            //StoreRanksAndScores();
            //MarkActivityStatus();
        }

    }
}
