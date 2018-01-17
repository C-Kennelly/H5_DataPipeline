using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp.Model.Halo5.Stats;
using H5_DataPipeline.Assistants.Shared;

namespace H5_DataPipeline.Assistants.CompanyRosters
{

    public class QuartermasterScribe
    {
        private t_teams databaseRecord;
        private SpartanCompany companyAPIResult;
        private Referee referee;
        private int jobId;

        List<string> gamertagsToRemove;
        List<string> gamertagsToAdd;

        public QuartermasterScribe(t_teams currentRecord, SpartanCompany apiResult, Referee eventReferee, int jobNumber)
        {
            databaseRecord = currentRecord;
            companyAPIResult = apiResult;
            referee = eventReferee;
            jobId = jobNumber;
        }

        public void ResolveDifferencesAndUpdateRosters()
        {
            ResolveDifferences();
            MakeUpdatesToRoster();

            referee.MarkJobDone(jobId);
        }

        private void ResolveDifferences()
        {
            if (IsValidComparison())
            {
                List<string> currentListOfPlayers = new List<string>(10);
                List<string> newListOfPlayers = new List<string>(10);

                using (var db = new dev_spartanclashbackendEntities())
                {
                    currentListOfPlayers.AddRange( db.t_teams.Find(databaseRecord.teamId).t_players_to_teams.Select(player => player.gamertag).ToList()    );

                  newListOfPlayers.AddRange(companyAPIResult.Members.Select(member => member.Identity.Gamertag).ToList() );

                }

                gamertagsToRemove = currentListOfPlayers.Except(newListOfPlayers).ToList();
                gamertagsToAdd = newListOfPlayers.Except(currentListOfPlayers).ToList();
            }
        }

        private bool IsValidComparison()
        {
            if (databaseRecord != null
                && companyAPIResult != null
                && databaseRecord.teamId == companyAPIResult.Id.ToString())
            { return true; }

            else { return false; }
        }


        private void MakeUpdatesToRoster()
        {
            if(RosterHasUpdates())
            {
                RemoveRosterEntriesForPlayers(gamertagsToRemove);
                AddRosterEntriesForPlayers(gamertagsToAdd);
            }

            //TODO - UpdateCompanyRanks
        }

        private bool RosterHasUpdates()
        {
            int changeCount = gamertagsToRemove.Count() + gamertagsToAdd.Count();
            //Console.WriteLine("Detected {0} changes for {1}", changeCount, databaseRecord.teamName);

            if (changeCount <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void RemoveRosterEntriesForPlayers(List<string> gamertags)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                foreach (string gamertag in gamertags)
                {
                    t_players_to_teams rosterEntry = db.t_players_to_teams.FirstOrDefault(entry => entry.gamertag == gamertag);

                    if(rosterEntry != null)
                    {
                        db.t_players_to_teams.Remove(rosterEntry);
                    }
                }

                db.SaveChanges();
            }
        }

        private void AddRosterEntriesForPlayers(List<string> gamertags)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                foreach (string gamertag in gamertags)
                {
                    CreatePlayerIfNotExists(gamertag);

                    t_players_to_teams rosterEntry = db.t_players_to_teams.FirstOrDefault(entry => entry.gamertag == gamertag);

                    if (rosterEntry != null)
                    {
                        db.t_players_to_teams.Remove(rosterEntry);
                    }

                    t_players_to_teams newRosterEntry = new t_players_to_teams(companyAPIResult.Id, companyAPIResult.Members.Find(member => member.Identity.Gamertag == gamertag));
                    db.t_players_to_teams.Add(newRosterEntry);
                }

                db.SaveChanges();
            }
        }

        private void CreatePlayerIfNotExists(string gamertag)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_players currentRecord = db.t_players.Find(gamertag);

                if (currentRecord == null)
                {
                    db.t_players.Add(new t_players(gamertag));

                    db.SaveChanges();
                }

            }
        }



    }
}
