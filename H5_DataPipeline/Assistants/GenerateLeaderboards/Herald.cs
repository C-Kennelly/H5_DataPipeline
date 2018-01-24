using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.DataPipeline;
using H5_DataPipeline.Models.SpartanClash;

namespace H5_DataPipeline.Assistants.GenerateLeaderboards
{
    public class Herald
    {
        private string noCompanyValue;
        private int numberOfGamesRequiredToBeRanked;
        private ParallelOptions threeFourthsCPUCap;

        public Herald()
        {
            noCompanyValue = t_teams.GetNoWaypointCompanyFoundID();
            numberOfGamesRequiredToBeRanked = 10;
            threeFourthsCPUCap = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 1.0)) };
        }

        public void CalculateLeaderboards()
        {
            CompanyFiller companyFiller = new CompanyFiller(noCompanyValue);
            companyFiller.FillParticipatingCompanies(noCompanyValue);


           Console.WriteLine("Mapping relationships...");
            RelationshipMapper relationshipMapper = new RelationshipMapper(threeFourthsCPUCap);
            relationshipMapper.MapRelationships();

           //
           // Console.WriteLine("Building company Service Records");
           // BuildServiceRecords(threeFourthsCPUCap);
           //
           // Console.WriteLine("Building leaderboards by calculating ranks.");
           // BuildLeaderboards(numberOfGamesRequiredToBeRanked);
           //
           // Console.WriteLine("Finished!");
           // Console.ReadLine();
        }




    }
}
