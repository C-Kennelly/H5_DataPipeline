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

            List<t_players> unitTestPlayers = new List<t_players>(1);
            t_players newPlayer = new t_players("Unit Test Player", numberOfDaysBeforeScanningMatchesAgain);
            unitTestPlayers.Add(newPlayer);


            PlayerAlarm playerAlarm = new PlayerAlarm(messageQueue, unitTestPlayers);



            //Assert Alarm happens
            Assert.Fail();

        }

//        private void MakeTestQueue()
//        {
//            var factory = new ConnectionFactory()
//            {
//                HostName = "localhost"
//            };
//
//            using (var connection = factory.CreateConnection())
//            using (var channel = connection.CreateModel())
//            {
//                channel.QueueDeclare(queue: "unitTestQueue",
//                                     durable: false,
//                                     exclusive: false,
//                                     autoDelete: false,
//                                     arguments: null);
//            }
//
//        }
    }
}
