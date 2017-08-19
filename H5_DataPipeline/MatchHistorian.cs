using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models;
using HaloSharp.Model.Halo5.Stats;
using HaloSharp.Model.Halo5.Stats.Common;
using HaloSharp.Model;

namespace H5_DataPipeline
{
    class MatchHistorian
    {
        private t_players player;
        private HaloClientFactory haloClientFactory;
        private List<Enumeration.Halo5.GameMode> gameModes = new List<Enumeration.Halo5.GameMode>();
   
        private List<PlayerMatch> matchHistory;

        public List<t_h5matches> uniqueMatchesFromMatchHistory = new List<t_h5matches>();

        public MatchHistorian(t_players spartan)
        {
            player = spartan;
            SetDefaultGameModes();
            haloClientFactory = new HaloClientFactory();
        }

        public void SetGameModes(List<Enumeration.Halo5.GameMode> modesToQuery)
        {
            gameModes = modesToQuery;
        }

        private void SetDefaultGameModes()
        {
            gameModes.Add(Enumeration.Halo5.GameMode.Arena);
            gameModes.Add(Enumeration.Halo5.GameMode.Warzone);
            gameModes.Add(Enumeration.Halo5.GameMode.Custom);
        }

        public int BuildUniqueMatchHistoryRecords()
        {
            FindMatchHistory().Wait();

            //foreach(PlayerMatch match in matchHistory)
            //{
            //    ProcessMatch(match);
            //}
            Parallel.ForEach(matchHistory, match =>
            {
                ProcessMatch(match);
            });

            return uniqueMatchesFromMatchHistory.Count;
        }

        private async Task FindMatchHistory()
        {
            MatchCaller matchCaller = new MatchCaller();
            matchHistory = await matchCaller.GetMatchHistoryForPlayerAfterDate(player.gamertag, player.GetEarliestDateToScanMatches(), gameModes, haloClientFactory.GetDevClient());
        }

        public void ProcessMatch(PlayerMatch match)
        {
            bool newMatchFound = true;
            t_h5matches matchRecord = new t_h5matches(match);

            //open match?

            using (var db = new dev_spartanclashbackendEntities())
            {
                if (matchRecord.AlreadySavedToDatabase())   { newMatchFound = false; }
                else                                        { matchRecord.UpdateDatabase(); }

                MakeNewMatchAssociationIfNotExists(matchRecord);

                if (newMatchFound)
                {
                    HandleNewMatch(matchRecord);
                    //SetDates on these things, handle new players, and scan companies when needed - and we'll need to save them here
                    matchRecord.t_h5matches_matchdetails = new t_h5matches_matchdetails(match);
                    matchRecord.t_h5matches_playersformatch = new t_h5matches_playersformatch(match);
                    //matchRecord.t_h5matches_ranksandscores = new t_h5matches_ranksandscores(match);
                    //matchRecord.t_h5matches_teamsinvolved_halowaypointcompanies = new t_h5matches_teamsinvolved_halowaypointcompanies(matchRecord.t_h5matches_playersformatch);
                    //matchRecord.t_h5matches_teamsinvolved_spartanclashfireteams = new t_h5matches_teamsinvolved_spartanclashfireteams(matchRecord.t_h5matches_playersformatch);
                    uniqueMatchesFromMatchHistory.Add(matchRecord);
                }
                else
                {
                    //What do we do when 
                }


            }
        }

        private void HandleNewMatch(t_h5matches match)
        {

        }

        private void MakeNewMatchAssociationIfNotExists(t_h5matches match)
        {
            using (var db = new dev_spartanclashbackendEntities())
            {
                t_players_to_h5matches currentRecord = db.t_players_to_h5matches.FirstOrDefault(record => record.gamertag == player.gamertag && record.matchID == match.matchID);
                
                if(currentRecord == null)
                {
                    db.t_players_to_h5matches.Add(new t_players_to_h5matches(player.gamertag, match.matchID));
                    db.SaveChanges();
                }

            }
        }
    }
}
