using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp;

namespace H5_DataPipeline.Assistants
{
    /// <summary>
    /// The Quartermaster's job is to figure out who's in what company.  Every morning, he updates the rolls to ensure they're accurate.
    /// </summary>
    class Quartermaster
    {
        IHaloSession haloSession;

        public Quartermaster(IHaloSession session)
        {
            haloSession = session;
        }

        public void UpdateSpartanCompanyRosters()
        {
            List<t_teams> companiesTrackedInDatabase = GetSpartanCompaniesTrackedInDatabase();

            Console.WriteLine("Updating Spartan Company Rosters at: {0}", DateTime.UtcNow);
            Console.WriteLine();

            ProcessCompanies(companiesTrackedInDatabase);

            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Finished updating Spartan Company Rosters at: {0}", DateTime.UtcNow);
        }

        private List<t_teams> GetSpartanCompaniesTrackedInDatabase()
        {
            string waypointSourceName = t_teamsources.GetWaypointSourceName();
            using (var db = new dev_spartanclashbackendEntities())
            {
                return db.t_teams.Where( team => 
                    team.teamSource == waypointSourceName
                ).ToList();
            }
        }

        private void ProcessCompanies(List<t_teams> companies)
        {
            int counter = 0;
            int total = companies.Count;

            foreach (t_teams company in companies)
            {
                counter++;
                Console.Write("\rProcessing {0} of {1}: {2}", counter, total, company.teamName);

                ProcessCompany(company);
            }

            if (total == 1)  //[NOCOMPANYFOUND is the one]
            {
                Console.Write("No Companies for Quartermaster to process.");
            }
        }

        private void ProcessCompany(t_teams team)
        {
            Console.WriteLine("Quartermaster doesn't actually process companies yet.");
            //Check for special company like [NOCOMPANYFOUND]
                //Handle special company
            //For all others
                //Query database to get tracked roster
                //query ID's to get list of current members

                //Resolve differences(current roster, actual roster)
        }




        //public bool IsNewCompany(string companyName)
        //{
        //
        //}
        //
        //public void HandleNewCompany(string companyName)
        //{
        //
        //}

    }
}
