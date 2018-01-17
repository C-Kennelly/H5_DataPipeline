using H5_DataPipeline.Models;
using HaloSharp.Model.Halo5.Stats;
using H5_DataPipeline.Assistants.Shared;

namespace H5_DataPipeline.Assistants.CompanyRosters
{
    public delegate void CompanyRosterScannedHandler(object sender, CompanyRosterScannedEventArgs eventArgs);

    public class CompanyRosterScannedEventArgs
    {

        private t_teams databaseRecord;
        private SpartanCompany companyAPIResult;
        private Referee _referee;
        private int jobId;

        public CompanyRosterScannedEventArgs(t_teams currentTeamRecord, SpartanCompany apiResult, Referee referee, int jobNumber)
        {
            databaseRecord = currentTeamRecord;
            companyAPIResult = apiResult;
            _referee = referee;
            jobId = jobNumber;
        }

        public t_teams GetCurrentTeamRecord()
        {
            return databaseRecord;
        }

        public SpartanCompany GetCompanyAPIResult()
        {
            return companyAPIResult;
        }

        public Referee GetReferee()
        {
            return _referee;
        }

        public int GetJobID()
        {
            return jobId;
        }

    }

}
