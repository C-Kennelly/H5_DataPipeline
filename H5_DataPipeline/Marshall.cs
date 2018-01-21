using H5_DataPipeline.Models;
using H5_DataPipeline.Assistants.CompanyRosters;
using H5_DataPipeline.Assistants.MatchDetails;
using H5_DataPipeline.Assistants.MatchParticipants;
using H5_DataPipeline.Assistants.AnalyzeClanBattles;
using H5_DataPipeline.Shared;
using H5_DataPipeline.Shared.Config;
using HaloSharp;
using System.Threading;

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
            HaloClient haloClient = haloClientFactory.GetDevClient();
            haloSession = haloClient.StartSession();
        }

        private void SetupAssistants()
        {
            quartermaster = new Quartermaster(haloSession);
            historian = new Historian(haloSession, spartanClashSettings);
            mortician = new Mortician(haloSession);
            clanalyzer = new Clanalyzer(haloSession, spartanClashSettings);
        }




        public void DoTheThing()
        {
            Setup();

            quartermaster.UpdateSpartanCompanyRosters();
            historian.RecordRecentGames();
            //mortician.ScanMatchesForParticipants();
            //clanalyzer.AnalyzeClanBattles();
        }

        private void Setup()
        {
            WaypointSetup.SetupWaypointSourcesInDatabase();
        }
    }
}
