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

        public static void RegisterNewPlayersIfNotExist(List<string> playersToRegister, bool silent = true)
        {
            foreach(string player in playersToRegister)
            {
                CreatePlayerIfNotExists(player, silent);
            }
        }

        public static void CreatePlayerIfNotExists(string gamertag, bool silent = true)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_players currentRecord = db.t_players.Find(gamertag);

                if (currentRecord == null)
                {
                    db.t_players.Add(new t_players(gamertag));

                    db.SaveChanges();
                    if(!silent)
                    {
                        Console.WriteLine("Now tracking {0}", gamertag);
                    }
                }
            }
        }


        public static async Task RegisterNewPlayersIfNotExistAsync(List<string> playersToRegister)
        {
            List<Task> saveRecordTasks = new List<Task>(playersToRegister.Count);

            foreach(string player in playersToRegister)
            {
                saveRecordTasks.Add(CreatePlayerIfNotExistsAsync(player));
            }

            await Task.WhenAll(saveRecordTasks.ToArray());
        }

        private static async Task CreatePlayerIfNotExistsAsync(string gamertag)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_players currentRecord = db.t_players.Find(gamertag);

                if (currentRecord == null)
                {
                    t_players newPlayer = new t_players(gamertag);
                    db.t_players.Add(newPlayer);
                    await db.SaveChangesAsync();
                    Console.WriteLine("Now tracking {0}", gamertag);
                }
            }
        }

    }
}
