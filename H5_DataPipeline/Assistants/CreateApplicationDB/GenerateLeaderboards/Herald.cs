using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.DataPipeline;
using H5_DataPipeline.Models.SpartanClash;

namespace H5_DataPipeline.Assistants.CreateApplicationDB.GenerateLeaderboards
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
            Console.WriteLine();
            Console.WriteLine("Filling application database with participating companies...");
            CompanyFiller filler = new CompanyFiller(noCompanyValue);
            filler.FillParticipatingCompanies(noCompanyValue);

            Console.WriteLine();
            Console.WriteLine("Mapping relationships...");
            RelationshipMapper mapper = new RelationshipMapper(threeFourthsCPUCap);
            mapper.MapRelationships();

            Console.WriteLine();
            Console.WriteLine("Building company Service Records...");
            ServiceRecordBuilder builder = new ServiceRecordBuilder();
            builder.RecalculateWinLossServiceRecords();

            Console.WriteLine();
            Console.WriteLine("Building the Waypoint leaderboards rankings...");
            WaypointLeaderBoarderBuilder waypointBuilder = new WaypointLeaderBoarderBuilder(numberOfGamesRequiredToBeRanked);
            waypointBuilder.BuildWaypointLeaderboards();

            UpdateApplicationDatabaseRefreshDate();
        }

        private void UpdateApplicationDatabaseRefreshDate()
        {
            using (var spartanClashDB = new clashdbEntities())
            {
                t_clashmetadata activeRecord = spartanClashDB.t_clashmetadata.Find("active");

                if(activeRecord != null)
                {
                    activeRecord.dataRefreshDate = DateTime.UtcNow;
                }
                else
                {
                    t_clashmetadata newRecord = new t_clashmetadata();
                    newRecord.id = "active";
                    newRecord.dataRefreshDate = DateTime.UtcNow;
                    spartanClashDB.t_clashmetadata.Add(newRecord);
                }

                spartanClashDB.SaveChanges(); 
            }
        }




    }
}
