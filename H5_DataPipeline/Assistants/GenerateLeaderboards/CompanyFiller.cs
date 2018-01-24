using System;
using System.Collections.Generic;
using System.Linq;
using H5_DataPipeline.Models.SpartanClash;
using H5_DataPipeline.Models.DataPipeline;

namespace H5_DataPipeline.Assistants.GenerateLeaderboards
{
    public class CompanyFiller
    {
        private string noCompanyValue;

        public CompanyFiller(string noCompanyValueWaypoint)
        {
            noCompanyValue = noCompanyValueWaypoint;
        }

        public void FillParticipatingCompanies(string noCompanyValue)
        {
            Console.WriteLine("Scanning for new companies...");

            List<t_teams> allSpartanCompanies = GetAllSpartanCompaniesFromDataPipeline();
            List<t_clashdevset> allClanBattlesFromApplicationDatabase = GetAllMatchesFromApplicationDatabase();
            List<string> companyIDs = GenerateListOfUniqueCompanyIDs(allClanBattlesFromApplicationDatabase);

            WriteCompaniesToDatabase(companyIDs.Distinct<string>().ToList(), allSpartanCompanies);

        }

        private List<t_teams> GetAllSpartanCompaniesFromDataPipeline()
        {
            using (var dataPipelineDB = new dev_spartanclashbackendEntities())
            {
                return dataPipelineDB.t_teams.ToList();
            }
        }

        private List<t_clashdevset> GetAllMatchesFromApplicationDatabase()
        {
            using (var spartanCompanyDB = new clashdbEntities())
            {
                return spartanCompanyDB.t_clashdevset.ToList();
            }
        }

        private List<string> GenerateListOfUniqueCompanyIDs(List<t_clashdevset> allMatches)
        {
            List<string> companyIDs = new List<string>();

            foreach (t_clashdevset match in allMatches)
            {
                if (match.Team1_Company != noCompanyValue)
                {
                    companyIDs.Add(match.Team1_Company);
                }

                if (match.Team2_Company != noCompanyValue)
                {
                    companyIDs.Add(match.Team2_Company);
                }
            }

            return companyIDs;
        }

        private void WriteCompaniesToDatabase(List<string> distinctCompanyIDs, List<t_teams> allSpartanCompanies)
        {
            int recordsAdded = 0;

            using (var db = new clashdbEntities())
            {
                foreach (string companyID in distinctCompanyIDs)
                {

                    t_companies newRecord = new t_companies(companyID, allSpartanCompanies.Find(x => x.teamId == companyID).teamName);


                    var query = db.t_companies.Find(newRecord.companyId);
                    if (query == null)
                    {
                        db.t_companies.Add(newRecord);
                        recordsAdded++;
                    }
                }

                try
                {
                    Console.Write("Database needs {0} new companies written...       ", recordsAdded);
                    db.SaveChanges();
                    Console.WriteLine("Write complete!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Database write failed with message {0}.  Companies were not added.", e.InnerException.Message);
                    recordsAdded = 0;
                }
            }
        }


    }
}
