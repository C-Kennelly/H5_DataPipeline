using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp;
using H5_DataPipeline.Models;

namespace H5_DataPipeline.Assistants
{
    /// <summary>
    /// The mortician's job is to know who fought in every battle.  She scans matches for participants and tags clan battles accordingly.
    /// </summary>
    class Clanalyzer
    {
        IHaloSession haloSession;

        public Clanalyzer(IHaloSession session)
        {
            haloSession = session;
        }

        public void AnalyzeClanBattles()
        {
            AnalyzeMatchesForHaloWaypointClanBattles();
            //AnalyzeMatchesForSpartanClashClanBattles();
            //This could go faster if it analyzed all teams together, but calling out that we are only searching for clan battles right now.
        }

        private void AnalyzeMatchesForHaloWaypointClanBattles()
        {
            List<t_h5matches> untaggedMatches = GetMatchesWithoutHaloWaypointBattlesTagged();

            Console.WriteLine("Tagging Clan Battles at: {0}", DateTime.UtcNow);
            Console.WriteLine();

            ProcessMatches(untaggedMatches);

            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Finished tagging Clan Battles at: {0}", DateTime.UtcNow);
        }

        private List<t_h5matches> GetMatchesWithoutHaloWaypointBattlesTagged()
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                return db.t_h5matches.Where(match =>
                  match.t_h5matches_teamsinvolved_halowaypointcompanies == null
                ).ToList();
            }
        }

        private void ProcessMatches(List<t_h5matches> matches)
        {
            int counter = 0;
            int total = matches.Count;

            foreach (t_h5matches match in matches)
            {
                counter++;
                Console.Write("\rProcessing {0} of {1}: {2}", counter, total, match.matchID);

                ProcessMatch(match);
            }
        }

        private void ProcessMatch(t_h5matches match)
        {
            Console.WriteLine("Clanalyzer doesn't actually process matches yet.");
                //Look through records and go for counts on numbers of teams to tag.
        }

    }
}
