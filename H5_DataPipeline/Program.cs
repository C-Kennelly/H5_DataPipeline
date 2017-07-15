using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_DataPipeline
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        /////////Notes/////////////
        ///The current pipeline...

        //Build a list of gamertags to query.
        //Associate them with companies

        //Scrape HaloWaypoint for all companies.    
        //Scrape HaloWaypoint for all spartans using company names.
        //Find a subset of spartans based on company population. (Histogram approach, exclude those outside of target size.)

        //Query all possible games played by the Spartan Companies.

        //Find a subset of games that match based on playlist/date.

        //Query all matches in the subset to get players.

        //Scan the matches to cross-reference with the company registry.

        //Load the matches into the application database.

        //Process the matches to optimize for website pages.

        //----------------------------------------------------//

        //Can we structure the query portion to get requests and post results to a message queue?

        //Crawl the list of matches and generate a starting set of players.

        //Continually scan all matches (each session should be based on last scan date... haven't scanned in 1, 3, 7 days, etc)   
        //(How to determine this order for max efficiency?  Tiers of importance determine frequency of scans?)

        //Scan the list of players for:
        //Changes to Spartan Company status.    
        //New matches since last scanned date

        //-------------//

        //When a new match is found...
        //Figure out what type it is...
        //Query the right report
        //Write it to the right table as untagged and call Clanalyzer passing matchID



        //Scan it for players - see if there's a new player to add to the database 
        //and (if needed, call player inserter to scan all their matches - last scanned date of 10/27/2015 so scanner takes all their matches since launch)
        //Reference the registry and drop in spartan company names (can we store this as json, or does that make indexing a nightmare?).
        //Generate participating companies
        //If participating companies are found, add it to the app database (some sort of queue to do this?)

        //-------------//

        //When a company change is found...
        //Update the Spartan entry
        //Update last changed date

        //See if the company exists
        //if it doesn't, add it to the database
        //Then, tell the query machine to scan the company to see if there's more members we don't know about (and update those members accordingly)
    }
}
