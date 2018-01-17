using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp;
using HaloSharp.Model.Halo5.Stats;
using H5_DataPipeline.Assistants.Shared;
using System.Threading;

namespace H5_DataPipeline.Assistants.CompanyRosters
{
    /// <summary>
    /// The Quartermaster's job is to figure out who's in what company.  Every morning, he updates the rolls to ensure they're accurate.
    /// </summary>
    class Quartermaster
    {
        IHaloSession haloSession;
        private Referee referee = new Referee();
        public event CompanyRosterScannedHandler CompanyRosterReadyForDatabaseWrite;
        
        public Quartermaster(IHaloSession session)
        {
            haloSession = session;
            CompanyRosterReadyForDatabaseWrite += OnCompanyRosterReadyForDatabaseWrite;
            referee = new Referee();
        }

        public void UpdateSpartanCompanyRosters()
        {
            List<t_teams> companiesTrackedInDatabase = GetSpartanCompaniesTrackedInDatabase();

            Console.WriteLine("Updating Spartan Company Rosters at: {0}", DateTime.UtcNow);
            Console.WriteLine();

            ProcessCompanies(companiesTrackedInDatabase);
            WaitUntilAllJobsAreDone();
            

            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Finished updating Spartan Company Rosters at: {0}", DateTime.UtcNow);
        }

        private List<t_teams> GetSpartanCompaniesTrackedInDatabase()
        {
            string waypointSourceName = t_teamsources.GetWaypointSourceName();
            string noCompanyFoundID = t_teams.GetNoWaypointCompanyFoundID();

            using (var db = new dev_spartanclashbackendEntities())
            {
                return db.t_teams.Where(team =>
                        team.teamSource == waypointSourceName
                        && team.trackingIndex > 0
                        && team.teamId != noCompanyFoundID)
                    .ToList();
            }
        }

        private void ProcessCompanies(List<t_teams> companies)
        {
            int counter = 0;
            int total = companies.Count;

            foreach (t_teams company in companies)
            {
                Console.Write("\rProcessing {0} of {1}: {2}                ", counter, total, company.teamName);

                referee.RegisterJob(counter);
                ProcessCompany(company, counter);

                counter++;
            }
        }

        private async Task ProcessCompany(t_teams team, int jobIndex)
        {            
            if(team.teamId != t_teams.GetNoWaypointCompanyFoundID())
            {
                CompanyCaller companyCaller = new CompanyCaller();

                SpartanCompany companyResult = await companyCaller.GetWaypointCompanyInformation(
                                team.teamId,
                                haloSession
                            );

                if (companyResult != null)
                {
                    CompanyRosterReadyForDatabaseWrite?.BeginInvoke(this, new CompanyRosterScannedEventArgs(team, companyResult, referee, jobIndex), null, null);
                }
            }
        }

        protected virtual void OnCompanyRosterReadyForDatabaseWrite(object Sender, CompanyRosterScannedEventArgs e)
        {
            QuartermasterScribe scribe = new QuartermasterScribe(e.GetCurrentTeamRecord(), e.GetCompanyAPIResult(), e.GetReferee(), e.GetJobID());
            scribe.ResolveDifferencesAndUpdateRosters();
        }


        private void WaitUntilAllJobsAreDone()
        {
            bool jobsAreDone = false;
            while (!jobsAreDone)
            {
                Console.Write("\rWaiting on {0} jobs to complete.                                  ",
                                        referee.GetNumberOfUnfinishedJobs());
                if (referee.AllJobsAreDone())
                {
                    jobsAreDone = true;
                }
                else
                {
                    Thread.Sleep(250);
                }
            }
        }

    }
}
