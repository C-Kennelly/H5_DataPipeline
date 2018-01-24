using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using H5_DataPipeline.Models.DataPipeline;

namespace testsH5_DataPipeline
{
    [TestClass]
    public class t_PlayersExtensionsTests
    {
        //Continually scan all matches (each session should be based on last scan date... haven't scanned in 1, 3, 7 days, etc)   
        //(How to determine this order for max efficiency?  Tiers of importance determine frequency of scans?)

        const int numberOfDaysBeforeScanningMatchesAgain = 7;

        [TestMethod]
        public void MatchesReadyToBeSearchedReturnsFalseWhenWithinThreshold()
        {     
            t_players player = new t_players("UNITTESTPLAYER", numberOfDaysBeforeScanningMatchesAgain);
            player.RecordMatchScan(); 

            Assert.IsFalse(player.MatchesReadyToBeSearched());
        }

        [TestMethod]
        public void MatchesReadyToBeSearchedReturnsTrueWhenOutsideOfThreshold()
        {
            t_players player = new t_players("UNITTESTPLAYER", numberOfDaysBeforeScanningMatchesAgain);
            player.RecordMatchScan(DateTime.UtcNow.AddDays(-2 * player.scanThresholdInDays));

            Assert.IsTrue(player.MatchesReadyToBeSearched());
        }

        [TestMethod]
        public void CompanyRosterReadyToBeSearchedReturnsFalseWhenWithinThreshold()
        {
            t_players player = new t_players("UNITTESTPLAYER", numberOfDaysBeforeScanningMatchesAgain);
            player.RecordCompanyScan();

            Assert.IsFalse(player.CompanyRosterReadyToBeSearched());
        }

        [TestMethod]
        public void CompanyRosterReadyToBeSearchedReturnsTrueWhenOutsideOfThreshold()
        {
            t_players player = new t_players("UNITTESTPLAYER", numberOfDaysBeforeScanningMatchesAgain);
            player.RecordCompanyScan(DateTime.UtcNow.AddDays(-2 * player.scanThresholdInDays));

            Assert.IsTrue(player.CompanyRosterReadyToBeSearched());
        }

        [TestMethod]
        public void CustomTeamRosterReadyToBeSearchedReturnsFalseWhenWithinThreshold()
        {
            t_players player = new t_players("UNITTESTPLAYER", numberOfDaysBeforeScanningMatchesAgain);
            player.RecordCustomTeamScan();

            Assert.IsFalse(player.CustomTeamRosterReadyToBeSearched());
        }

        [TestMethod]
        public void CustomTeamRosterReadyToBeSearchedReturnsTrueWhenOutsideOfThreshold()
        {
            t_players player = new t_players("UNITTESTPLAYER", numberOfDaysBeforeScanningMatchesAgain);
            player.RecordCustomTeamScan(DateTime.UtcNow.AddDays(-2 * player.scanThresholdInDays));

            Assert.IsTrue(player.CustomTeamRosterReadyToBeSearched());
        }
    }
}
