using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H5_DataPipeline.Models.DataPipeline;
using H5_DataPipeline.Models.SpartanClash;

namespace H5_DataPipeline.Assistants.CreateApplicationDB
{
    /*
    class Craftsman
    {
        private static async void DoTheThing()
        {
            Task t = MetadataGetter.UpdateMetaDataTables();
            t.Wait();

            PullRawMatchesDBInputs(out List<t_players_for_match> warzoneWarlordsPlayersForMatches, out List<t_matches_for_player> warzoneWarlordsMatchesForPlayer);
            BuildClashDevSetMatches(out List<t_clashdevset> clashDevSetMatches, ref warzoneWarlordsPlayersForMatches, ref warzoneWarlordsMatchesForPlayer);
            WriteClashDevSetToDatabase(clashDevSetMatches);

        }

        private static void PullRawMatchesDBInputs(out List<t_h5matches_playersformatch> playersForMatchList, out List<t_h5matches_matchdetails> matchesForPlayerList)
        {
            Console.WriteLine("Pulling inputs from RawMatchesDB");
            using (var db = new dev_spartanclashbackendEntities())
            {
                playersForMatchList = db.t_h5matches_playersformatch.ToList();

                matchesForPlayerList = new List<t_h5matches_matchdetails>(playersForMatchList.Count);
                foreach (t_h5matches_playersformatch match in playersForMatchList)
                {
                    matchesForPlayerList.Add(db.t_matches_for_player.Find(match.MatchId));
                }

                Console.WriteLine("Found {0} Warzone Warlords matches and grabbed {1} from the pool of total matches", playersForMatchList.Count, matchesForPlayerList.Count);

            }
        }

        private static void BuildClashDevSetMatches(out List<t_clashdevset> clashDevSetMatches, ref List<t_players_for_match> warzoneWarlordsPlayersForMatches, ref List<t_matches_for_player> warzoneWarlordsMatchesForPlayer)
        {
            Console.WriteLine("Build clashdevset matches from the inputs");

            clashDevSetMatches = new List<t_clashdevset>(warzoneWarlordsPlayersForMatches.Count);
            foreach (t_players_for_match playerMatch in warzoneWarlordsPlayersForMatches)
            {
                t_matches_for_player matchDetails = warzoneWarlordsMatchesForPlayer.Find(x => x.MatchId == playerMatch.MatchId);

                clashDevSetMatches.Add(new t_clashdevset(matchDetails, playerMatch));


            }

        }

        private static void WriteClashDevSetToDatabase(List<t_clashdevset> clashDevSetMatches)
        {
            Console.WriteLine("Making clashdevset inserts");

            using (var db = new clashdbEntities())
            {
                foreach (t_clashdevset match in clashDevSetMatches)
                {
                    var query = db.t_clashdevset.Find(match.MatchId);

                    try
                    {
                        if (query == null)
                        {
                            db.t_clashdevset.Add(match);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Had problems adding match {0}", match.MatchId);
                    }
                }

                db.SaveChanges();
            }


        }
        


    }
    */
}
