using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp.Model.Halo5.Stats;
using HaloSharp.Model.Halo5.Stats.Common;
using HaloSharp.Model;
using HaloSharp;
using System.Collections.Concurrent;

namespace H5_DataPipeline.Assistants.Shared
{
    /// <summary>
    /// The helper has functions that haven't been placed anywhere yet.
    /// </summary>
    static class Helper
    {
        public static async Task RegisterNewPlayers(List<string> playersToRegister)
        {
            List<Task> saveRecordTasks = new List<Task>(playersToRegister.Count);

            foreach(string player in playersToRegister)
            {
                saveRecordTasks.Add(RegisterAPlayer(player));
            }

            await Task.WhenAll(saveRecordTasks.ToArray());
        }

        private static async Task RegisterAPlayer(string gamertag)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_players currentRecord = db.t_players.FirstOrDefault(x => x.gamertag == gamertag);

                if (currentRecord == null)
                {
                    t_players newPlayer = new t_players(gamertag);
                    db.t_players.Add(newPlayer);
                    await db.SaveChangesAsync();
                    Console.WriteLine("Now tracking {0}", gamertag);
                }
            }
        }

        //This exists in HistorianScribe;
        public static void MakeNewMatchAssociationIfNotExists(t_players player, t_h5matches match)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_players_to_h5matches currentRecord = db.t_players_to_h5matches.FirstOrDefault(record => record.gamertag == player.gamertag && record.matchID == match.matchID);

                if (currentRecord == null)
                {
                    db.t_players_to_h5matches.Add(new t_players_to_h5matches(player.gamertag, match.matchID));
                    db.SaveChanges();
                }

            }
        }
    }
}
