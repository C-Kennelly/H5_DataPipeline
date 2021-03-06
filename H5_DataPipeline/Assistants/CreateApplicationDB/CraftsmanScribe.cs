﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.DataPipeline;
using H5_DataPipeline.Models.SpartanClash;
using HaloSharp;

namespace H5_DataPipeline.Assistants.CreateApplicationDB
{
    public class CraftsmanScribe
    {
        private List<t_clashdevset> clanBattleMatches;
        private List<t_h5matches_playersformatch> clanBattleParticipants;
        private IHaloSession session;

        public CraftsmanScribe(List<t_clashdevset> clashDevSetMatches, List<t_h5matches_playersformatch> matchParticipants, IHaloSession haloSession)
        {
            clanBattleMatches = clashDevSetMatches;
            clanBattleParticipants = matchParticipants;
            session = haloSession;
        }

        public void AddRecordsToApplicationDatabase()
        {
            Console.WriteLine("Scanning for new map metadata.");
            AddNewMetaDataRecords();

            Console.WriteLine("Inserting match records.");
            InsertClashDevSet();

            Console.WriteLine("Inserting participants for matches.");
            InsertMatchParticipants();
        }

        private void AddNewMetaDataRecords()
        {
            Task t = MetadataGetter.UpdateMetaDataTables(session);
            t.Wait();
        }

        private void InsertClashDevSet()
        {
            using (var spartanClashDB = new clashdbEntities())
            {
                foreach (t_clashdevset match in clanBattleMatches)
                {
                    var query = spartanClashDB.t_clashdevset.Find(match.matchId);

                    try
                    {
                        if (query == null)
                        {
                            spartanClashDB.t_clashdevset.Add(match);
                            
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Had problems adding match {0}", match.matchId);
                        Console.WriteLine(e.InnerException.Message);
                        Console.WriteLine();
                    }
                }

                spartanClashDB.SaveChanges();

            }

        }


        private void InsertMatchParticipants()
        {
            using (var spartanClashDB = new clashdbEntities())
            {
                foreach (t_h5matches_playersformatch match in clanBattleParticipants)
                {
                    var query = spartanClashDB.t_matchparticipants.Find(match.matchID);


                    try
                    {
                        if (query == null)
                        {
                            t_matchparticipants record = new t_matchparticipants(match);
                            spartanClashDB.t_matchparticipants.Add(record);
 
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Had problems adding match participants for {0}", match.matchID);
                    }
                }
                spartanClashDB.SaveChanges();
            }

        }


    }
}
