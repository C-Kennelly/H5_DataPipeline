# H5_DataPipeline
The [Spartan Clash](https://github.com/C-Kennelly/SpartanClashCore) data pipeline, organized around single company flow.  I'll have a better explanation of the code once it's in a more stable state.  In the meantime, you can take a look at the next section to see the old README that explained how I used to process the data.  We're still making the same output in the new repository (for now), but this time it's not scattered across 5 codebases.

# How We (Used To) Get The Data
For a brief overview of what goes on to produce the data for [Spartan Clash](https://github.com/C-Kennelly/SpartanClash), I'm mashing up the "Spartan Company" [roster](https://www.halowaypoint.com/en-us#media-ab21c50895234cc0bea295f4e6556cf0) with the post-game match details available from the [Halo 5 API](https://developer.haloapi.com/).

First, I scrape the entirety of this [registry](https://www.halowaypoint.com/en-us/spartan-companies)... all 5000 or so pages.  This gets me a list of  Spartan Companies, which I then proceed to scrape members using the [Quartermaster](https://github.com/C-Kennelly/Quartermaster) library.  Taking into account a responsible rate of page loads, this whole process spans about 24-straight hours.  I do not have this series of foreach loops published in a public repo here, nor the custom scraping code requried to get all ~50,000 group names.  Don't scrape webpages, kids.

All this data is placed in a local MySQL instance, and it's just the start of my clunky, data-mashing pipeline.  Once I have the full list of members and their associated groups, I need to filter down to a [working set](https://github.com/C-Kennelly/H5_GetWorkingSet) of the member data I want to pull.  

Now comes the [raw match querying](https://github.com/C-Kennelly/H5_RawMatchCaller) for the Halo 5 API.  When I'm done maxing out the external API's rate limit, I'll have a list of every game and the list of players who participated.

Enter "[Clanalyzer](https://github.com/C-Kennelly/H5_Clanalyzer)," stage left.   This CPU-eating monster (ripe for for horizontal scaling, if I had the funds) will cross-check each and every match for participants that meet the "Clan Battle" criteria - a certain threshold of players from the same Spartan Company.  Clanalyzer tags the matches (~5% to 10% hit rate for normal conditions) that we'll need more details on.

Now that we've limited the matches to a (hopefully) manageable subset, it's one more pass through the rate-limited wringer for the Halo 5 API.  We're using the Carnage Report (post-game scorecard) for each match to determine [which "clan" gets the win](https://github.com/C-Kennelly/H5_RankFinder), and which "clan" gets the loss.

Finally, our rate-limited work is complete.  The output of all of this gets exported to the application database via SQL script, and the SpartanClash front end benefits from the snappy response time of pre-mashed data as it displays an authoritative leaderboard experience to any of the 300,000 players (~60K active) who are members of these companies.

The big challenge will be automating this periodic refresh of data in some sort of way that doesn't anger my consumer ISP.  ;)  Currently, I'm only using this system for special events such as the weekly "[Warzone Warlords](https://www.halowaypoint.com/en-us/forums/6e35355aecdf4fd0acdaee3cc4156fd4/topics/update-on-warzone-12-man-fireteams/bbe49650-65bd-47ce-a1b3-94ae202ab367/posts)
" playlist. 
