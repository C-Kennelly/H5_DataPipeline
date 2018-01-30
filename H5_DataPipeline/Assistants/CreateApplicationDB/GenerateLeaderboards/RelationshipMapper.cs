using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.SpartanClash;

namespace H5_DataPipeline.Assistants.CreateApplicationDB.GenerateLeaderboards
{
    public class RelationshipMapper
    {
        private ParallelOptions parallelOptions;

        public RelationshipMapper(ParallelOptions options)
        {
            parallelOptions = options;
        }
        /// <summary>
        /// For each company, manually create the relationship with matches it participated in.
        /// </summary>
        public void MapRelationships()
        {
            using (var spartanClashDB = new clashdbEntities())
            {
                List<t_companies> allCompanies = GetAllCompaniesFromApplicationDatabase(spartanClashDB);

                foreach(t_companies company in allCompanies)
                {
                    List<t_clashdevset> companyMatches = FindAllMatchesForCompany(company, spartanClashDB);
                    SaveAllMatchesForCompany(company, companyMatches, spartanClashDB);
                }
                //Parallel.ForEach(allCompanies, parallelOptions, (company) =>
                //{
                //    List<t_clashdevset> companyMatches = FindAllMatchesForCompany(company, spartanClashDB);
                //    SaveAllMatchesForCompany(company, companyMatches, spartanClashDB);
                //});
            }
        }

        private List<t_companies> GetAllCompaniesFromApplicationDatabase(clashdbEntities spartanClashDB)
        {
            return spartanClashDB.t_companies.ToList();
        }

        private List<t_clashdevset> FindAllMatchesForCompany(t_companies company, clashdbEntities spartanClashDB)
        {
            List<t_clashdevset> companyMatches = new List<t_clashdevset>();

            companyMatches = spartanClashDB.t_clashdevset
                        .Where(
                            x => x.Team1_Company == company.companyId
                            || x.Team2_Company == company.companyId
                        ).ToList();

            return companyMatches;
        }

        private void SaveAllMatchesForCompany(t_companies company, List<t_clashdevset> companyMatches, clashdbEntities spartanClashDB)
        {
            t_companies currentCompanyRecord = spartanClashDB.t_companies.Find(company.companyId);
            foreach (t_clashdevset match in companyMatches)
            {
                currentCompanyRecord.t_clashdevset.Add(match);
            }

            try
            {
                spartanClashDB.SaveChangesAsync();
                Console.WriteLine("Finished mapping for {0}.", company.companyName);

            }
            catch (Exception e)
            {
                Console.WriteLine("Database write failed with message {0}.  Relationships were not mapped for {1}", e.Message, company.companyId);
            }
            Console.WriteLine("");
                       
        }




    }
}
