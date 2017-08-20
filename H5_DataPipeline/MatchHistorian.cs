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

namespace H5_DataPipeline
{
    class MatchHistorian
    {
        HaloClient haloClient;
        private t_players player;
        private List<Enumeration.Halo5.GameMode> gameModes = new List<Enumeration.Halo5.GameMode>();
   
        private List<PlayerMatch> matchHistory;

        public ConcurrentBag<t_h5matches> uniqueMatchesFromMatchHistory = new ConcurrentBag<t_h5matches>();
        //public List<t_h5matches> uniqueMatchesFromMatchHistory = new List<t_h5matches>();

        public MatchHistorian(t_players spartan)
        {
            player = spartan;
            SetDefaultGameModes();
            HaloClientFactory haloClientFactory = new HaloClientFactory();
            haloClient = haloClientFactory.GetProdClient();
        }

        public void SetGameModes(List<Enumeration.Halo5.GameMode> modesToQuery)
        {
            gameModes = modesToQuery;
        }

        private void SetDefaultGameModes()
        {
            gameModes.Add(Enumeration.Halo5.GameMode.Arena);
            gameModes.Add(Enumeration.Halo5.GameMode.Warzone);
            gameModes.Add(Enumeration.Halo5.GameMode.Custom);
        }

        public int BuildUniqueMatchHistoryRecords()
        {
            FindMatchHistory().Wait();

            ConcurrentBag<string> allPlayersFoundInMatches = new ConcurrentBag<string>();

            Parallel.ForEach(matchHistory, match =>
            {
                List<string> playersDiscoveredInMatch = ParallelProcessMatch(match);
                

                foreach(string tag in playersDiscoveredInMatch)
                {
                    allPlayersFoundInMatches.Add(tag);
                }
            });

            Console.WriteLine();

            List<string> uniquePlayers = allPlayersFoundInMatches.Select(x => x).Distinct().ToList();

            RegisterNewPlayers(uniquePlayers);

            //UpdateRostersWhereNecessary(uniquePlayers);
            //ThenTagClanBattles(matchHistory);
            //    TagCompanyBattles(matchHistory);
            //    TagCustomBattles(matchHistory);
            return uniqueMatchesFromMatchHistory.Count;
        }

        private async Task FindMatchHistory()
        {
            MatchCaller matchCaller = new MatchCaller();
            matchHistory = await matchCaller.GetMatchHistoryForPlayerAfterDate(player.gamertag, player.GetEarliestDateToScanMatches(), gameModes, haloClient);
            Console.WriteLine("Finished searching match history, begin processing.");
        }

        public List<string> ParallelProcessMatch(PlayerMatch match)
        {
            bool newMatchFound = true;
            t_h5matches matchRecord = new t_h5matches(match);
            List<string> gamertagsFoundInMatch = new List<string>();
            //open match?

            using (var db = new dev_spartanclashbackendEntities())
            {
                if (matchRecord.AlreadySavedToDatabase())   { newMatchFound = false; }
                else                                        { matchRecord.queryStatus = -1; matchRecord.UpdateDatabase(); }

                MakeNewMatchAssociationIfNotExists(matchRecord);

                if (newMatchFound)
                {
                    NewMatchProcesser newMatchProcessor = new NewMatchProcesser(match, haloClient);
                    gamertagsFoundInMatch = newMatchProcessor.ProcessMatch();
                    

                    uniqueMatchesFromMatchHistory.Add(matchRecord);
                }
                else
                {
                    //What do we do when 
                }

                matchRecord.dateDetailsScan = DateTime.UtcNow;
                matchRecord.datePlayersScan = DateTime.UtcNow;
                matchRecord.dateResultsScan = DateTime.UtcNow;
                matchRecord.queryStatus = 0;
                matchRecord.UpdateDatabase();

                return gamertagsFoundInMatch;
            }
        }

        private void MakeNewMatchAssociationIfNotExists(t_h5matches match)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_players_to_h5matches currentRecord = db.t_players_to_h5matches.FirstOrDefault(record => record.gamertag == player.gamertag && record.matchID == match.matchID);
                
                if(currentRecord == null)
                {
                    db.t_players_to_h5matches.Add(new t_players_to_h5matches(player.gamertag, match.matchID));
                    db.SaveChanges();
                }

            }
        }

        private void RegisterNewPlayers(List<string> playersFromMatches)
        {

            using (var db = new dev_spartanclashbackendEntities())
            {
                Parallel.ForEach(playersFromMatches, gamertag =>
                {
                    t_players currentRecord = db.t_players.FirstOrDefault(x => x.gamertag == gamertag);

                    if(currentRecord == null)
                    {
                        Console.WriteLine("Now tracking {0}", gamertag);
                        t_players newPlayer = new t_players(gamertag);
                        db.t_players.Add(newPlayer);
                        db.SaveChangesAsync();
                        Console.WriteLine("{0} successfully saved", gamertag);
                    }
                });
            }
        }
    }
}
