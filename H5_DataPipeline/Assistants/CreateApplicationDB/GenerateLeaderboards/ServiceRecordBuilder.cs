using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.SpartanClash;

namespace H5_DataPipeline.Assistants.CreateApplicationDB.GenerateLeaderboards
{
    public class ServiceRecordBuilder
    {

        public void RecalculateWinLossServiceRecords()
        {
            using (var db = new clashdbEntities())
            {
                List<t_companies> allCompanies = db.t_companies.ToList();

                foreach (t_companies company in allCompanies)
                {
                    company.ResetServiceRecord();

                    List<t_clashdevset> matchesForCompany = company.t_clashdevset.ToList();

                    foreach (t_clashdevset match in matchesForCompany)
                    {
                        if (match.IsAWinFor(company))
                        { company.wins++; }
                        else
                        { company.losses++; }
                    }

                    company.FinalizeServiceRecord();
                    
                    try
                    {
                        db.SaveChanges();
                        Console.WriteLine("Built Service Record for {0}", company.companyName);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} database write failed for {1}", e.Message, company.companyId);
                    }
                }
            }
        }
    }
}
