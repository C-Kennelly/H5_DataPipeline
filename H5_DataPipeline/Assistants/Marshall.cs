using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using H5_DataPipeline.Assistants;
using HaloSharp;
using HaloSharp.Model;

namespace H5_DataPipeline.Assistants
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

        SpartanClashSettings spartanClashSettings = new SpartanClashSettings();

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
            mortician = new Mortician(haloSession);
            clanalyzer = new Clanalyzer(haloSession);
        }




        public void DoTheThing()
        {
            Setup();

            quartermaster.UpdateSpartanCompanyRosters();
            historian.RecordRecentGames();
            mortician.ScanMatchesForParticipants();
            clanalyzer.AnalyzeClanBattles();
        }

        private void Setup()
        {
            WaypointSetup.SetupWaypointSourcesInDatabase();
        }
    }
}
