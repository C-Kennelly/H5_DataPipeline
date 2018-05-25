using H5_DataPipeline.Models.DataPipeline;
using H5_DataPipeline.Assistants.CompanyRosters;
using H5_DataPipeline.Assistants.MatchDetails;
using H5_DataPipeline.Assistants.MatchParticipants;
using H5_DataPipeline.Assistants.AnalyzeClanBattles;
using H5_DataPipeline.Assistants.CreateApplicationDB;
using H5_DataPipeline.Shared;
using H5_DataPipeline.Shared.Config;

using HaloSharp;

namespace H5_DataPipeline
{
    /// <summary>
    /// The Field Marshall's job is to handle flow for the assistants.
    /// </summary>
    class Marshall
    {
        IHaloSession haloSession;

        Quartermaster quartermaster;
        Historian historian;
        Mortician mortician;
        Clanalyzer clanalyzer;
        Craftsman craftsman;

        SpartanClashSettings spartanClashSettings;

        public Marshall()
        {
            spartanClashSettings = new SpartanClashSettings();
            SetupHaloSharpComponents();            
            SetupAssistants();
        }

        private void SetupHaloSharpComponents()
        {
            HaloClientFactory haloClientFactory = new HaloClientFactory();
            HaloClient haloClient = haloClientFactory.GetProdClient();
            haloSession = haloClient.StartSession();
        }

        private void SetupAssistants()
        {
            quartermaster = new Quartermaster(haloSession);
            historian = new Historian(haloSession, spartanClashSettings);
            mortician = new Mortician(haloSession, spartanClashSettings);
            clanalyzer = new Clanalyzer(haloSession, spartanClashSettings);
            craftsman = new Craftsman(haloSession);
            
        }




        public void ExecuteSpartanClashETL()
        {
            Setup();

            //Extract();
            Transform();
            Load();

        }

        private void Extract()
        {
            quartermaster.UpdateSpartanCompanyRosters();
            historian.RecordRecentGames();
            mortician.ScanMatchesForParticipants();
        }

        private void Transform()
        {
            clanalyzer.AnalyzeClanBattles();
        }

        private void Load()
        {
            craftsman.LoadApplicationDatabase();
        }

        private void Setup()
        {
            WaypointSetup.SetupWaypointSourcesInDatabase();
        }
    }
}
