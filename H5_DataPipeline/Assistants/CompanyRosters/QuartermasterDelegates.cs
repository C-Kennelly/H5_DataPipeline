using H5_DataPipeline.Models;
using HaloSharp.Model.Halo5.Stats;

namespace H5_DataPipeline.Assistants.CompanyRosters
{
    public delegate void CompanyRosterScannedHandler(object sender, CompanyRosterScannedEventArgs eventArgs);

    public class CompanyRosterScannedEventArgs
    {

        private t_teams databaseRecord;
        private SpartanCompany companyAPIResult;

        public CompanyRosterScannedEventArgs(t_teams currentTeamRecord, SpartanCompany apiResult)
        {
            databaseRecord = currentTeamRecord;
            companyAPIResult = apiResult;
        }

        public t_teams GetCurrentTeamRecord()
        {
            return databaseRecord;
        }

        public SpartanCompany GetCompanyAPIResult()
        {
            return companyAPIResult;
        }

    }

}
