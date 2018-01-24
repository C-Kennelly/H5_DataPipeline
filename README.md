# Spartan Clash - Halo 5 Data Pipeline
This repository holds the data pipeline that feeds [SpartanClash.com](www.spartanclash.com).  This pipeline mashes up the "Spartan Company" [roster](https://www.halowaypoint.com/en-us#media-ab21c50895234cc0bea295f4e6556cf0) with the post-game match details available from the [Halo 5 API](https://developer.haloapi.com/) to create an aggregated stat experience for groups of Halo fans.


# The Project
There's a lot of ways to learn about the Spartan Clash project.

Periodically, I update a project blog on my [personal website](https://idle-cycles.com/2017/10/upgrading-the-spartan-company/).  Head over there if you want to learn more about the goals of the project and where the high level focus is.

If you're more interested in where my development work is headed, I'm tracking this project in a [public backlog](https://www.pivotaltracker.com/n/projects/1776179).

Finally, there's a frontend to this pipeline, located in the [Spartan Clash](https://github.com/C-Kennelly/SpartanClashCore repository.  Give that repo a shot if you'd like to learn more about the front end experience.

# Setting Up A Dev Environment (Coming Soon!)
This section will explain how to set up a local version of the data pipeline.

The short answer is to clone the repo, execute the schema scripts against your MariaDB 10.2.7 installation, seed some initial test data, and fill in the secrets for the application (API Keys, database connection strings).

However, I'm still working on documenting the specifics, so hold tight on this front, or submit an issue if you're really interested.

# The Code
The section will explain how the Data Pipeline works.

## Introducing the Cast... ##
The best way to explain this code base is to tell a story.  If you've got access to some light folksy music, now's the time to start playing it.  For better or for worse, each stage in the pipeline is represented by a persona, called "Assistants".  Practically speaking, each Assistant is a vertical slice of functionality that accomplishes some goal.  I've laid out the assistants in the order that they execute, so if you read through the titles, you'll get an idea of how the Pipeline works.

The goal is to complete a single iteration of the pipeline in less than 24 hours so that the front end can have fresh data every day.

Long term, I'll probably move these to more professional names, but personifying the stages of the pipeline has proven helpful in reasoning about different stages.

### Meet the Marshall, who runs the show ###
Our first character is the Marshall, and he's the guy in charge of coordinating all of the colorful characters you're about to meet.
He's also probably the simplest of the bunch - all he does is declare what order the pipeline stages (Assistants) will execute.  He's the guy that a management client (like our little command line application in Program.cs) would call.

To get a great overview of the pipeline, head into Marshall's ETL jobs and see what happens in each one, or keep reading.

# Extract #
If you're familiar with ETL, you'll get the basic pattern here.  In the Extract stage, we're trying to solve one problem.
There's data in the Halo API, and it's not stored locally.  So we need to get it!

### Meet the Quartermaster, who tracks the team rosters ###
The first real Assistant is the Quartermaster, and he's the guy in charge of knowing Who's on First.  The pipeline's atomic unit of work is the "clan," basically an ID that represents a team.  The code in Quartermaster's area queries that ID to get a roster of people.

So the Quartermaster starts with a list of teams... let's say we're trying out two teams, the "Furious_Bears" and the "Angry_Unicorns".
- Furious_Bears
- Angry_Unicorns

So the Quartemaster takes those teams and queries [this Halo API endpoint](https://developer.haloapi.com/docs/services/58acdf27e2f7f71ad0dad84b/operations/596968ade2f7f7051870d29f) and gets a list players on each one.  The results might come back something like:
- Furious_Bears : John
- Furious_Bears : Kelly
- Furious_Bears : Linda
- Angry_Unicorns : Locke
- Angry_Unicorns : Buck
- Angry_Unicorns : Vale
  
Now that the Quartermaster knows the latest for who's on what team, he goes to update his records, and makes changes to the database.  Sometimes new people show up, sometimes people change teams, and sometimes people leave teams entirely.  Fundamentally, the Quartermaster provides the rest of the pipeline with and up-to-date team roster, which is critical to aggregating the stats accurately.

Also, instead of two teams, it's [potentially a whole lot more](https://www.halowaypoint.com/en-us/spartan-companies)...  To help deal with the scale, we've got a database column that tells the Quartermaster to ignore certain companies, which proves really useful for the inactive ones.

### Meet the Historian, who looks at match histories and records their details ###
After the Quartermaster has finished, the next Assistant steps in.  The Historian has the list of all the gamertags on the teams we care about thanks to the Quartermaster rosters.  So she puts on her spectacles, and starts querying [this Halo API endpoint](https://developer.haloapi.com/docs/services/58acdf27e2f7f71ad0dad84b/operations/58acdf28e2f7f70db4854b3b) for each gamertag.

The Halo API presently doles out matches in packs of 25, so she thumbs through the report and looks for any new matches that occured *after* the website launched.  If at the end of the results, she still hasn't hit the "first match" date, she'll ask for more results.

Whenver she finds a new match, she'll then query [this Halo API endpoint ](https://developer.haloapi.com/docs/services/58acdf27e2f7f71ad0dad84b/operations/58acdf28e2f7f70db4854b37) for the the match results and then store them in their respective tables.  (She'll adjust that endpoint for Warzone/Customs as needed).

Overall, the Historian has some painstaking work, and this can be one of the longest stages in the pipeline, due to the amount of data that gets pulled down over the rate-limited connection.  Fortunately, the more often the pipeline is run, the less often the Historian finds a new match, which greatly saves on query time and database writes.

### Meet the Mortician, who tracks the people that were in every battle ###
The match details are all in place, but it turns out we're not quite done.  Due to the way HaloSharp returns results, we can't actually see DNF players from their model.  So, the Mortician exists to requery the matchID and populate the books with the names of the players in each match, including the DNF lists.  Overall, he's pretty creepy, and we'd love to get rid of him and move his functionality over to the Historian (which would cut the query time in half).

These values are stored as JSON fields inside the database, and names are paired with the team they were on when they completed the match, allowing for analysis later.

# Transform #
In the Transform stage, we have all the data that we need to perform calculations.  So now we need to massage the data into a usable format.  For the most part, that means doing the analysis we are interested in doing.

### Meet the Clanalyzer, who tags battles that clans participated in ###
Our next Assistant is the Clanalyzer, who looks through match participants and then tags the battles that were "clan battles."  The definition for this is set in the config tables, but it comes down to a match participation threshold... if one team was made up of x% players from one company, then they get to tag the match.  So if it is set at 0.75, then your team gets a clan battle if 3 out of 4 members were on your team, or 6 out of 8.

Free for all is excluded from this system, of course.

# Load #
Finally, for the Load stage, we have the data ready to go.  It's time to get it into the applicatin database where it can be consumed.
Interestingly enough, due to the way I originally wrote this, we actually do a little bit of post-load processing in the Leaderboards area.  It's probably something we'll shift back to the Transform stage as the [Divisions](https://www.pivotaltracker.com/epic/show/3725669) epic materializes.

### Meet the Craftsman, who creates the battle reports ###
The next Assistant assistant is the Craftsman, who is responsbile for crafting the application database.  He looks over the records tagged by the Clanalyzer and combines it with the Morticians findings to generate battle reports for the web app to consume.  In other words, he uploads the subset of matches that are clan battles to the application database and provides the auxillary information that is required to do them.

### Meet the Herald, who celebrates the best teams. ###
The last Assistant actually works for the Craftsman.  The Herald continues the updates of the Application Database, but does some light processing.  First, he fills in the "Service Record" components - basically, for each company, how many wins and losses did you have?

Then he rounds out the pipeline by calculating leaderboard rankings for the web app to display later.  Right now, this is a naive "Win/Loss" ratio for any team that's played more than 10 games.  However, this portion of the Pipeline will likely get more complicated as we put a real ranking system in later.  Good ole' Herald!

# What's Next?
Given time to spend on this code base, I would probably build a small management front end, and some automated deployment infrastructure.  Basically, I'd replace the Program.cs command line interface with a protected API (and maybe MVC management panel).  This would let me manage the pipeline without remoting into a Windows box.

Long term, I'd probably focus on factoring out the [Halo Sharp](https://github.com/C-Kennelly/HaloSharp) components into something that's .Net Core-friendly so that I can host this thing on Linux, which would improve my ability to automate it.

The Assistants are also setup to be pulled out into seperate services with some kind of event-based pattern, using something like RabbitMQ.  However, many stages of the pipeline assume other stages have run first, so it'll need to be a linear flow to start.  It's a rabbit(haha) hole that I'm not quite ready to go down yet, especially because the ever-present API limit prevents a distributed solution from moving very quickly.

Realistically, my next most likely step willl be killing the Morticiian for efficiency and then adding Custom Team support - [Alliances](https://www.pivotaltracker.com/epic/show/3725677), and [Fireteams](https://www.pivotaltracker.com/epic/show/3790817).  If you poke around the pipeline database schema, you'll see the "Team Sources" table, which allows teams from different sources to exist.  However, I need to provide a way for users to create and manage those teams on the front end before this'll be useful.
