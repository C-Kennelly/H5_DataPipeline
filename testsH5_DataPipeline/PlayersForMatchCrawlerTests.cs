using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace testsH5_DataPipeline
{
    [TestClass]
    public class PlayersForMatchCrawlerTests
    {
        //Crawl the list of matches and generate a starting set of players.

        [TestMethod]
        public void TestMethod1()
        {
            //Use a mock data structure instead of the actual tables to test Linq Queries
            //Dependecy Injection could be cool.  Pass a list to the comparison.  Crawler's read function can read into to memory from list.
            //Don'test the write/read logic.  That's the ORM's job.
            //Instead, test the comparison logic.
            //Use sample records
        }
    }
}
