using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using H5_DataPipeline.Models;

namespace testsH5_DataPipeline
{
    [TestClass]
    public class t_PlayersExtensionsTests
    {
        //Continually scan all matches (each session should be based on last scan date... haven't scanned in 1, 3, 7 days, etc)   
        //(How to determine this order for max efficiency?  Tiers of importance determine frequency of scans?)

        [TestMethod]
        public void MatchesReadyToBeSearchedReturnsFalseWhenWithinThreshold()
        {
            const int numberOfDaysBeforeScanningMatchesAgain = 7;

            t_players player = new t_players("Sn1p3r C");
            player.RecordMatchScan();

            Assert.IsFalse(player.MatchesReadyToBeSearched(numberOfDaysBeforeScanningMatchesAgain));
        }

        [TestMethod]
        public void MatchesReadyToBeSearchedReturnsTrueWhenOutsideOfThreshold()
        {
            const int numberOfDaysBeforeScanningMatchesAgain = 7;

            t_players player = new t_players("Sn1p3r C");
            player.RecordMatchScan(DateTime.UtcNow.AddDays(-2 * numberOfDaysBeforeScanningMatchesAgain));

            Assert.IsTrue(player.MatchesReadyToBeSearched(numberOfDaysBeforeScanningMatchesAgain));
        }
    }
}
