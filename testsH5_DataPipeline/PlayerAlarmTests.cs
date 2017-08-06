using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using H5_DataPipeline;
using H5_DataPipeline.Models;
using RabbitMQ.Client;

namespace testsH5_DataPipeline
{
    [TestClass]
    public class PlayerAlarmTests
    {
        //This class triggers an alarm when a player needs to be scanned.
        //It should sound an alarm (event) for each kind of scan that could be performed.
        //We probably want to pass it the queue that it should use to send the event to.
        
        [TestMethod]
        public void MatchAlarmSoundsWhenPlayerPassesThresholdDate()
        {
            ConnectionFactory factory = new ConnectionFactory();

            const int numberOfDaysBeforeScanningMatchesAgain = 7;

            List<t_players> inputPlayers = new List<t_players>(1);
            List<t_players> outputPlayers = new List<t_players>(1);
            List<t_players> expectedReturnedPlayers = new List<t_players>(1);

            t_players newPlayer = new t_players("Unit Test Player", numberOfDaysBeforeScanningMatchesAgain);
            inputPlayers.Add(newPlayer);
            expectedReturnedPlayers.Add(newPlayer);

  //          PlayerAlarm playerAlarm = new PlayerAlarm(inputPlayers);

//            outputPlayers = playerAlarm.GenerateList();

            Assert.IsTrue(ReturnTrueIfListsAreEqual(outputPlayers, expectedReturnedPlayers));
            
            
        }

        private bool ReturnTrueIfListsAreEqual<T>(List<T> List1, List<T> List2)
        {
            if(List1 == null && List2 == null)
            {
                return true;
            }

            if (List1 == null || List2 == null)
            {
                return false;
            }

            foreach (T item in List1)
            {
                if(List2.Contains(item))
                {
                    List2.Remove(item);
                }
                else
                {
                    return false;
                }
            }

            if(List2.Count != 0)
            {
                return false;
            }

            return true;
        }
    }
}
