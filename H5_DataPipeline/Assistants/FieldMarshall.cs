using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using H5_DataPipeline.Assistants;
using HaloSharp;

namespace H5_DataPipeline.Assistants
{
    /// <summary>
    /// The Field Marshall's job is to handle flow for the assistants.
    /// </summary>
    class FieldMarshall
    {
        HaloClientFactory haloClientFactory;
        HaloClient haloClient;
        IHaloSession haloSession;

        Quartermaster quartermaster;
        Historian historian; 
        Mortician  mortician;
        Clanalyzer clanalyzer;

        public FieldMarshall()
        {
            SetupHaloSharpComponents();            
            SetupAssistants();
        }

        private void SetupHaloSharpComponents()
        {
            haloClientFactory = new HaloClientFactory();
            haloClient = haloClientFactory.GetProdClient();
        }

        private void SetupAssistants()
        {
            quartermaster = new Quartermaster(haloSession);
            historian = new Historian(haloSession);
            mortician = new Mortician(haloClientFactory);
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
