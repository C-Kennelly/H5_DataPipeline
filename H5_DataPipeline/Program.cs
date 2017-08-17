﻿using System;
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

        public static async void DoTheThing()
        {
            MatchCaller matchCaller = new MatchCaller();
            PlayerFinder playerFinder = new PlayerFinder();
            HaloClientFactory haloClientFactory = new HaloClientFactory();

            string warzoneMatchID = "03be25c0-7df1-4135-9e61-5257de8191a0";

            List<Enumeration.Halo5.GameMode> gameModes = new List<Enumeration.Halo5.GameMode>();
            gameModes.Add(Enumeration.Halo5.GameMode.Arena);
            gameModes.Add(Enumeration.Halo5.GameMode.Warzone);
            gameModes.Add(Enumeration.Halo5.GameMode.Custom);

            List <t_h5matches> matchHistory = matchCaller.GetMatchHistoryForPlayerAfterDate("Sn1p3r C", DateTime.UtcNow, gameModes, haloClientFactory.GetDevClient());

            foreach(t_h5matches match in matchHistory)
            {
                match.t_h5matches_playersformatch = await playerFinder.GetPlayersForMatch(match.t_h5matches_matchdetails, haloClientFactory.GetDevClient());
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
                new t_players("Sn1p3r C"),
                new t_players("Black Picture"),
                new t_players("Randy 355"),
                new t_players("ADarkerTrev"),
                new t_players("Ray Benefield")
            };

            return testPlayerList;
        }
        //
        //public static void StartTheLoop(List<t_players> playersToScan)
        //{
        //    foreach (t_players player in playersToScan)
        //    {
        //        //Save t_h5matches_matchdetails that we get back
        //        if(player.MatchesReadyToBeSearched())
        //        {
        //            List<t_h5matches> matchHistory = ScanMatchesAfterDate(player.dateLastMatchScan);
        //        
        //
        //            foreach (t_h5matches match in matchHistory)
        //            {
        //                //Save t_carange report based on match_details id.
        //                List<t_h5matches_playersformatch> playersInMatch = ScanCarnageReport(match)
        //
        //
        //                AddNewPlayersAndCompanyInformationToDatabase(playersInMatch);
        //                ThenTagTeamBattlesWithinLastXDays(players);
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("{0} was already scanned in the last 7 days", player);
        //        }
        //
        //    }
        //
        //}
    }
}
