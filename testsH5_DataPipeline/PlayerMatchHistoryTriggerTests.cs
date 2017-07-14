using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using H5_DataPipeline;

namespace testsH5_DataPipeline
{
    [TestClass]
    public class PlayerMatchHistoryTriggerTests
    {
        //Continually scan all matches (each session should be based on last scan date... haven't scanned in 1, 3, 7 days, etc)   
        //(How to determine this order for max efficiency?  Tiers of importance determine frequency of scans?)

        [TestMethod]
        public void TimeToScanMatchTestReturnsFalseWhenWithinThreshold()
        {
            const int numberOfDaysBeforeScanningMatchesAgain = 7;
            Player player = new Player("Sn1p3r C");
            player.RecordMatchScan(DateTime.UtcNow);

            PlayerMatchHistoryTrigger playerMatchHistoryTrigger = new PlayerMatchHistoryTrigger(player);

            Assert.IsFalse(playerMatchHistoryTrigger.TimeToSearchMatches(numberOfDaysBeforeScanningMatchesAgain));
        }

        [TestMethod]
        public void TimeToScanMatchTestReturnsTrueWhenOutsideOfThreshold()
        {
            const int numberOfDaysBeforeScanningMatchesAgain = 7;
            Player player = new Player("Sn1p3r C");
            player.RecordMatchScan(DateTime.UtcNow.AddDays(-50));

            PlayerMatchHistoryTrigger playerMatchHistoryTrigger = new PlayerMatchHistoryTrigger(player);

            Assert.IsTrue(playerMatchHistoryTrigger.TimeToSearchMatches(numberOfDaysBeforeScanningMatchesAgain));
        }

        [TestMethod]
        public void TimeToScanMatchTestReturnsTrueWhenOnThreshold()
        {
            const int numberOfDaysBeforeScanningMatchesAgain = 7;
            Player player = new Player("Sn1p3r C");
            player.RecordMatchScan(DateTime.UtcNow.AddDays(-1 * numberOfDaysBeforeScanningMatchesAgain));

            PlayerMatchHistoryTrigger playerMatchHistoryTrigger = new PlayerMatchHistoryTrigger(player);

            Assert.IsTrue(playerMatchHistoryTrigger.TimeToSearchMatches(numberOfDaysBeforeScanningMatchesAgain));
        }
    }
}
