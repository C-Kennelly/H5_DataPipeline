UPDATE t_teams
SET trackingIndex = 0
WHERE trackingIndex >= 0;


UPDATE t_teams
SET trackingIndex = 1
WHERE (teamName = "Filthy Animals")
  OR (teamName = "ExO Delta Gaming")
  OR (teamName = "Dominated")
  OR (teamName ="Allegiance")
  OR (teamName = "Adamant")
  OR (teamId = "0");

SELECT * FROM t_teams WHERE trackingIndex > 0;

/*
DELETE FROM t_teams
WHERE 1=1;
*/

/*Delete Player Tree */
DELETE FROM t_players_to_teams
WHERE 1=1;

DELETE FROM t_players_to_h5matches
WHERE 1=1;

DELETE FROM t_players
WHERE 1=1;

/*Delete Match Tree */
DELETE FROM t_h5matches_matchdetails
WHERE 1=1;

DELETE FROM t_h5matches_playersformatch
WHERE 1=1;

DELETE FROM t_h5matches_ranksandscores
WHERE 1=1;

DELETE FROM t_h5matches_teamsinvolved_halowaypointcompanies
WHERE 1=1;

DELETE FROM t_h5matches_teamsinvolved_spartanclashfireteams
WHERE 1=1;

DELETE FROM t_h5matches
WHERE 1=1;
