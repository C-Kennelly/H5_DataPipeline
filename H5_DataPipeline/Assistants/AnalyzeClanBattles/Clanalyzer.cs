using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloSharp;
using H5_DataPipeline.Models.DataPipeline;
using H5_DataPipeline.Shared.Config;

namespace H5_DataPipeline.Assistants.AnalyzeClanBattles
{
    /// <summary>
    /// The mortician's job is to know who fought in every battle.  She scans matches for participants and tags clan battles accordingly.
    /// </summary>
    class Clanalyzer
    {
        IHaloSession haloSession;
        SpartanClashSettings spartanClashSettings;

        public Clanalyzer(IHaloSession session, SpartanClashSettings settings)
        {
            haloSession = session;
            spartanClashSettings = settings;

        }

        public void AnalyzeClanBattles()
        {
            AnalyzeMatchesForHaloWaypointClanBattles();
        }

        private void AnalyzeMatchesForHaloWaypointClanBattles()
        {
            Console.WriteLine("Tagging Clan Battles at: {0}", DateTime.UtcNow);
            Console.WriteLine();

            List<t_h5matches> untaggedMatches = GetMatchesWithoutHaloWaypointBattlesTagged();
            ProcessMatches(untaggedMatches);

            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Finished tagging Clan Battles at: {0}", DateTime.UtcNow);
        }

        private List<t_h5matches> GetMatchesWithoutHaloWaypointBattlesTagged()
        {
            DateTime earliestTrackedMatchDate = spartanClashSettings.EarliestTrackedMatchDate();

            using (var db = new dev_spartanclashbackendEntities())
            {
                return db.t_h5matches.Where(match =>
                    match.t_h5matches_teamsinvolved_halowaypointcompanies == null
                    && match.t_h5matches_matchdetails.MatchCompleteDate > earliestTrackedMatchDate
                ).ToList();
            }
        }

        private void ProcessMatches(List<t_h5matches> matches)
        {
            int counter = 0;
            int total = matches.Count;

            foreach (t_h5matches match in matches)
            {
                Console.Write("\rProcessing {0} of {1}: {2}                ", counter, total, match.matchID);

                ProcessMatch(match);

                counter++;
            }

            if (total == 0)
            {
                Console.Write("No matches for Clanalyzer to process.");
            }
        }

        private void ProcessMatch(t_h5matches match)
        {
            t_h5matches_teamsinvolved_halowaypointcompanies waypointCompaniesInvolved = IdentifyWaypointCompaniesPresentBasedOnPlayers(match);

            if(waypointCompaniesInvolved != null)
            {
                SaveWaypointTeamsInvolvedForMatch(waypointCompaniesInvolved, match);
            }
            else
            {
                //TODO - create default record or handle failure.
            }
        }

        private t_h5matches_teamsinvolved_halowaypointcompanies IdentifyWaypointCompaniesPresentBasedOnPlayers(t_h5matches matchToQuery)
        {
            t_h5matches_teamsinvolved_halowaypointcompanies result = null;

            t_h5matches_playersformatch playersForMatch;
            using (var db = new dev_spartanclashbackendEntities())
            {
                playersForMatch = db.t_h5matches_playersformatch.Find(matchToQuery.matchID);
            }

            if(playersForMatch != null)
            {
                result = new t_h5matches_teamsinvolved_halowaypointcompanies(playersForMatch, spartanClashSettings);
            }
            else
            {
                Console.WriteLine("Couldn't find playersForMatch record during Clan Battle Tagging {0}", matchToQuery.matchID);
            }

            return result;
        }

        private void SaveWaypointTeamsInvolvedForMatch(t_h5matches_teamsinvolved_halowaypointcompanies waypointRecord, t_h5matches parentRecord)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_h5matches_teamsinvolved_halowaypointcompanies currentRecord = db.t_h5matches_teamsinvolved_halowaypointcompanies.FirstOrDefault(record =>
                                                                record.matchID == waypointRecord.matchID
                                                            );

                if (currentRecord == null)
                {
                    db.t_h5matches_teamsinvolved_halowaypointcompanies.Add(waypointRecord);
                    db.SaveChanges();
                }
                else
                {
                    //Record exists, don't touch it.
                }


            }

            UpdateWaypointTeamsDatesScanned(parentRecord);

        }

        private void UpdateWaypointTeamsDatesScanned(t_h5matches match)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_h5matches currentRecord = db.t_h5matches.Find(match.matchID);

                if (currentRecord != null)
                {
                    currentRecord.dateCompaniesInvolvedUpdated = DateTime.UtcNow;

                    db.SaveChanges();
                }
                else
                {
                    throw new NotImplementedException("Impossibility condition reached - couldn't find parent 'h5matches' record for matchID: " + match.matchID);
                }
            }
        }

    }
}
