using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.DataPipeline;

namespace H5_DataPipeline.Assistants.Shared
{

    /// <summary>
    /// The helper has functions that haven't been placed anywhere yet.
    /// </summary>
    static class Helper
    {

        public static bool CreateUntrackedTeamIfNotExist(string teamID, string teamName, string teamSource, bool silent = true)
        {
            bool result = false;

            using (var db = new dev_spartanclashbackendEntities())
            {
                t_teams currentRecord = db.t_teams.Find(teamID);

                if (currentRecord == null)
                {
                    if (SourceIsValid(teamSource))
                    {

                        db.t_teams.Add(new t_teams(teamID, teamName, teamSource));
                        db.SaveChanges();
                        result = true;
                        if (!silent)
                        {
                            Console.WriteLine("Now tracking {0}", teamName);
                        }
                    }
                }
            }

            return result;
        }

        private static bool SourceIsValid(string teamSource)
        {
            bool isValid = false;
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_teamsources sourceRecord = db.t_teamsources.Find(teamSource);

                if(sourceRecord != null)
                {
                    isValid = true;
                }
            }

            return isValid;
        }


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
