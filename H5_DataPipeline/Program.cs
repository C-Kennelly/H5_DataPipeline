using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using H5_DataPipeline.Secrets;
using HaloSharp.Model;
using HaloSharp.Model.Halo5.Profile;
using HaloSharp.Query.Halo5.Profile;
using HaloSharp.Extension;

namespace H5_DataPipeline
{
    class Program
    {

        static void Main(string[] args)
        {

            Console.WriteLine("Hello, Infinity!");

            //Setup();


            DoTheThing();

            Console.WriteLine();

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        public static void Setup()
        {
            RefreshTeamRosterOlderThanXDays(7);
            //List<t_players> playersForToday = GetListOfPlayers(playerCountGoal);
        }

        public static void DoTheThing()
        {
            List<t_players> testPlayers = MakeTestPlayerList();
            int playersSearched = 0;
            int countOfMatchesFound;


            List<t_players> playersToScan = GetListOfPlayersToScan();
            int playersLeftToScan = playersToScan.Count();

            while (playersLeftToScan > 0)
            {
                foreach (t_players player in playersToScan)
                {
                    playersSearched++;
                    Console.WriteLine("Scanning player {0} of {1}: {2} at {3}", playersSearched, playersToScan.Count, player.gamertag, DateTime.UtcNow);

                    player.queryStatus = -1;
                    player.UpdateDatabase();

                    MatchHistorian matchHistorian = new MatchHistorian(player);

                    try
                    {
                        countOfMatchesFound = matchHistorian.BuildUniqueMatchHistoryRecords();
                        player.RecordMatchScan();
                        player.queryStatus = 0;
                        player.UpdateDatabase();
                        Console.WriteLine("Found {0} unique matches for player,", countOfMatchesFound);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    Console.WriteLine();


                }
                playersToScan = GetListOfPlayersToScan();
                playersLeftToScan = playersToScan.Count();

            }



            Console.WriteLine("Did the thing!");
        }

        private static List<t_players> GetListOfPlayersToScan()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                return db.t_players.Where   ( player =>
                                                player.dateLastMatchScan == null
                                            ||  player.queryStatus == -1
                                            ).ToList();
            }
        }

        public static void RefreshTeamRosterOlderThanXDays(int days)
        {
            TeamRosterRefresher rosterRefresher = new TeamRosterRefresher(days, 1000);

            //rosterRefresher.RefreshAllTeamRosters();
            //rosterRefresher.RefreshAllTeamRosters();
        }

        private static List<t_players> MakeTestPlayerList()
        {
            List<t_players> testPlayerList = new List<t_players> {
                new t_players("Sn1p3r C")//,
//                new t_players("Black Picture"),
//                new t_players("Randy 355"),
//                new t_players("ADarkerTrev"),
//                new t_players("Ray Benefield")
            };

            return testPlayerList;
        }

    }
}
