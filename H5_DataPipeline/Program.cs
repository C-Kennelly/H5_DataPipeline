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

            Setup();

            Console.WriteLine();

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        public static void Setup()
        {
            RefreshTeamRosterOlderThanXDays(7);
            //List<t_players> playersForToday = GetListOfPlayers(playerCountGoal);
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
