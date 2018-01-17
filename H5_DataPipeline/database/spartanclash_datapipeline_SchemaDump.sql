-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               10.2.7-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             9.4.0.5125
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for spartanclash_datapipeline
DROP DATABASE IF EXISTS `spartanclash_datapipeline`;
CREATE DATABASE IF NOT EXISTS `spartanclash_datapipeline` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `spartanclash_datapipeline`;

-- Dumping structure for table spartanclash_datapipeline.t_configoptions
DROP TABLE IF EXISTS `t_configoptions`;
CREATE TABLE IF NOT EXISTS `t_configoptions` (
  `configName` varchar(16) NOT NULL,
  `siteLaunchDate` date NOT NULL,
  `companyClanBattleThreshold` double NOT NULL DEFAULT 0.75,
  PRIMARY KEY (`configName`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table spartanclash_datapipeline.t_configoptions: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_configoptions` DISABLE KEYS */;
INSERT INTO `t_configoptions` (`configName`, `siteLaunchDate`, `companyClanBattleThreshold`) VALUES
	('active', '2018-01-16', 0.75);
/*!40000 ALTER TABLE `t_configoptions` ENABLE KEYS */;

-- Dumping structure for table spartanclash_datapipeline.t_h5matches
DROP TABLE IF EXISTS `t_h5matches`;
CREATE TABLE IF NOT EXISTS `t_h5matches` (
  `matchID` varchar(64) NOT NULL,
  `dateDetailsScan` datetime DEFAULT NULL,
  `dateResultsScan` datetime DEFAULT NULL,
  `datePlayersScan` datetime DEFAULT NULL,
  `dateCompaniesInvolvedUpdated` datetime DEFAULT NULL,
  `dateCustomTeamsUpdated` datetime DEFAULT NULL,
  `queryStatus` int(4) NOT NULL DEFAULT 0,
  PRIMARY KEY (`matchID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table spartanclash_datapipeline.t_h5matches: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_h5matches` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_h5matches` ENABLE KEYS */;

-- Dumping structure for table spartanclash_datapipeline.t_h5matches_matchdetails
DROP TABLE IF EXISTS `t_h5matches_matchdetails`;
CREATE TABLE IF NOT EXISTS `t_h5matches_matchdetails` (
  `matchId` varchar(64) NOT NULL,
  `GameMode` int(32) DEFAULT NULL,
  `HopperId` varchar(64) DEFAULT NULL,
  `MapId` varchar(64) DEFAULT NULL,
  `MapVariant_ResourceType` int(32) DEFAULT NULL,
  `MapVariant_ResourceId` varchar(64) DEFAULT NULL,
  `MapVariant_OwnerType` int(32) DEFAULT NULL,
  `MapVariant_Owner` varchar(64) DEFAULT NULL,
  `GameBaseVariantID` varchar(64) DEFAULT NULL,
  `GameVariant_ResourceID` varchar(64) DEFAULT NULL,
  `GameVariant_ResourceType` int(11) DEFAULT NULL,
  `GameVariant_OwnerType` int(11) DEFAULT NULL,
  `GameVariant_Owner` varchar(64) DEFAULT NULL,
  `MatchCompleteDate` datetime DEFAULT NULL,
  `MatchDuration` varchar(64) DEFAULT NULL,
  `IsTeamGame` binary(50) DEFAULT NULL,
  `SeasonID` varchar(64) DEFAULT NULL,
  PRIMARY KEY (`matchId`),
  CONSTRAINT `fk_matchID_h5matches_matchdetails` FOREIGN KEY (`matchId`) REFERENCES `t_h5matches` (`matchID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table spartanclash_datapipeline.t_h5matches_matchdetails: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_h5matches_matchdetails` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_h5matches_matchdetails` ENABLE KEYS */;

-- Dumping structure for table spartanclash_datapipeline.t_h5matches_playersformatch
DROP TABLE IF EXISTS `t_h5matches_playersformatch`;
CREATE TABLE IF NOT EXISTS `t_h5matches_playersformatch` (
  `matchID` varchar(64) NOT NULL,
  `team1_Players` varchar(1024) DEFAULT NULL,
  `team2_Players` varchar(1024) DEFAULT NULL,
  `other_Players` varchar(1024) DEFAULT NULL,
  `DNF_Players` varchar(1024) DEFAULT NULL,
  PRIMARY KEY (`matchID`),
  CONSTRAINT `fk_matchID_h5matches_playersformatch` FOREIGN KEY (`matchID`) REFERENCES `t_h5matches` (`matchID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `CONSTRAINT_1` CHECK (`team1_Players` is null or json_valid(`team1_Players`)),
  CONSTRAINT `CONSTRAINT_2` CHECK (`team2_Players` is null or json_valid(`team2_Players`))
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table spartanclash_datapipeline.t_h5matches_playersformatch: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_h5matches_playersformatch` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_h5matches_playersformatch` ENABLE KEYS */;

-- Dumping structure for table spartanclash_datapipeline.t_h5matches_ranksandscores
DROP TABLE IF EXISTS `t_h5matches_ranksandscores`;
CREATE TABLE IF NOT EXISTS `t_h5matches_ranksandscores` (
  `matchId` varchar(64) NOT NULL,
  `IsTeamGame` binary(50) NOT NULL,
  `team1_Rank` int(11) NOT NULL DEFAULT -1,
  `team1_Score` int(11) DEFAULT NULL,
  `team2_Rank` int(11) NOT NULL DEFAULT -1,
  `team2_Score` int(11) DEFAULT NULL,
  PRIMARY KEY (`matchId`),
  CONSTRAINT `fk_matchID_h5matches_ranksandscores` FOREIGN KEY (`matchId`) REFERENCES `t_h5matches` (`matchID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table spartanclash_datapipeline.t_h5matches_ranksandscores: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_h5matches_ranksandscores` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_h5matches_ranksandscores` ENABLE KEYS */;

-- Dumping structure for table spartanclash_datapipeline.t_h5matches_teamsinvolved_halowaypointcompanies
DROP TABLE IF EXISTS `t_h5matches_teamsinvolved_halowaypointcompanies`;
CREATE TABLE IF NOT EXISTS `t_h5matches_teamsinvolved_halowaypointcompanies` (
  `matchID` varchar(64) NOT NULL,
  `teamSource` varchar(128) DEFAULT 'Halo Waypoint',
  `team1_Primary` varchar(128) DEFAULT NULL,
  `team2_Primary` varchar(128) DEFAULT NULL,
  `team1_Secondary` varchar(128) DEFAULT NULL COMMENT 'Accomodates Warzone',
  `team2_Secondary` varchar(128) DEFAULT NULL COMMENT 'Accomodates Warzone',
  PRIMARY KEY (`matchID`),
  KEY `fk_teamSourceWaypoint` (`teamSource`),
  CONSTRAINT `fk_matchID_h5matches_teamsinvolved` FOREIGN KEY (`matchID`) REFERENCES `t_h5matches` (`matchID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_teamSourceWaypoint` FOREIGN KEY (`teamSource`) REFERENCES `t_teamsources` (`teamSource`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table spartanclash_datapipeline.t_h5matches_teamsinvolved_halowaypointcompanies: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_h5matches_teamsinvolved_halowaypointcompanies` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_h5matches_teamsinvolved_halowaypointcompanies` ENABLE KEYS */;

-- Dumping structure for table spartanclash_datapipeline.t_h5matches_teamsinvolved_spartanclashfireteams
DROP TABLE IF EXISTS `t_h5matches_teamsinvolved_spartanclashfireteams`;
CREATE TABLE IF NOT EXISTS `t_h5matches_teamsinvolved_spartanclashfireteams` (
  `matchID` varchar(64) NOT NULL,
  `teamSource` varchar(128) DEFAULT 'Spartan Clash',
  `team1_Primary` varchar(128) DEFAULT NULL,
  `team2_Primary` varchar(128) DEFAULT NULL,
  `team1_Secondary` varchar(128) DEFAULT NULL COMMENT 'Accomodates Warzone',
  `team2_Secondary` varchar(128) DEFAULT NULL COMMENT 'Accomodates Warzone',
  PRIMARY KEY (`matchID`),
  KEY `fk_teamSourceWaypoint` (`teamSource`),
  CONSTRAINT `t_h5matches_teamsinvolved_spartanclashfireteams_ibfk_1` FOREIGN KEY (`matchID`) REFERENCES `t_h5matches` (`matchID`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `t_h5matches_teamsinvolved_spartanclashfireteams_ibfk_2` FOREIGN KEY (`teamSource`) REFERENCES `t_teamsources` (`teamSource`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

-- Dumping data for table spartanclash_datapipeline.t_h5matches_teamsinvolved_spartanclashfireteams: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_h5matches_teamsinvolved_spartanclashfireteams` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_h5matches_teamsinvolved_spartanclashfireteams` ENABLE KEYS */;

-- Dumping structure for table spartanclash_datapipeline.t_players
DROP TABLE IF EXISTS `t_players`;
CREATE TABLE IF NOT EXISTS `t_players` (
  `gamertag` varchar(32) NOT NULL,
  `dateLastMatchScan` datetime DEFAULT NULL,
  `dateCompanyRosterUpdated` datetime DEFAULT NULL,
  `dateCustomTeamsUpdated` datetime DEFAULT NULL,
  `scanThresholdInDays` int(11) NOT NULL DEFAULT 7,
  `queryStatus` int(8) NOT NULL DEFAULT 0,
  PRIMARY KEY (`gamertag`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table spartanclash_datapipeline.t_players: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_players` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_players` ENABLE KEYS */;

-- Dumping structure for table spartanclash_datapipeline.t_players_to_h5matches
DROP TABLE IF EXISTS `t_players_to_h5matches`;
CREATE TABLE IF NOT EXISTS `t_players_to_h5matches` (
  `gamertag` varchar(32) NOT NULL,
  `matchID` varchar(64) NOT NULL,
  `dummyColumnAboutAssociation` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`gamertag`,`matchID`),
  KEY `gamertag` (`gamertag`),
  KEY `fk_matchID_players_to_matches` (`matchID`),
  CONSTRAINT `fk_gamertag_players_to_matches` FOREIGN KEY (`gamertag`) REFERENCES `t_players` (`gamertag`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_matchID_players_to_matches` FOREIGN KEY (`matchID`) REFERENCES `t_h5matches` (`matchID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table spartanclash_datapipeline.t_players_to_h5matches: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_players_to_h5matches` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_players_to_h5matches` ENABLE KEYS */;

-- Dumping structure for table spartanclash_datapipeline.t_players_to_teams
DROP TABLE IF EXISTS `t_players_to_teams`;
CREATE TABLE IF NOT EXISTS `t_players_to_teams` (
  `gamertag` varchar(128) NOT NULL,
  `teamId` varchar(128) NOT NULL,
  `role` int(16) NOT NULL DEFAULT 0,
  `lastUpdated` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `joinedDate` datetime DEFAULT NULL,
  `membershipLastModifiedDate` datetime DEFAULT NULL,
  PRIMARY KEY (`gamertag`,`teamId`),
  KEY `gamertag` (`gamertag`),
  KEY `teamId` (`teamId`),
  CONSTRAINT `fk_tplayerstoteams_t_players` FOREIGN KEY (`gamertag`) REFERENCES `t_players` (`gamertag`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_tplayerstoteams_t_teams` FOREIGN KEY (`teamId`) REFERENCES `t_teams` (`teamId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table spartanclash_datapipeline.t_players_to_teams: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_players_to_teams` DISABLE KEYS */;
/*!40000 ALTER TABLE `t_players_to_teams` ENABLE KEYS */;

-- Dumping structure for table spartanclash_datapipeline.t_teams
DROP TABLE IF EXISTS `t_teams`;
CREATE TABLE IF NOT EXISTS `t_teams` (
  `teamId` varchar(128) NOT NULL,
  `teamName` varchar(128) NOT NULL,
  `teamSource` varchar(128) NOT NULL,
  `trackingIndex` int(16) NOT NULL DEFAULT 1,
  `lastUpdated` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `rosterLastUdated` timestamp NULL DEFAULT NULL,
  `parentTeamId` varchar(128) DEFAULT NULL,
  `parentTeamName` varchar(128) DEFAULT NULL,
  `parentTeamSource` varchar(128) DEFAULT NULL,
  PRIMARY KEY (`teamId`),
  KEY `fk_teamsource_teams` (`teamSource`),
  KEY `fk_teamName_teamSource` (`teamName`,`teamSource`),
  CONSTRAINT `fk_teamsource_teams` FOREIGN KEY (`teamSource`) REFERENCES `t_teamsources` (`teamSource`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table spartanclash_datapipeline.t_teams: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_teams` DISABLE KEYS */;
INSERT INTO `t_teams` (`teamId`, `teamName`, `teamSource`, `trackingIndex`, `lastUpdated`, `rosterLastUdated`, `parentTeamId`, `parentTeamName`, `parentTeamSource`) VALUES
	('0', '[NOCOMPANYFOUND]', 'Halo Waypoint', 0, '2018-01-16 22:20:25', NULL, NULL, NULL, NULL);
/*!40000 ALTER TABLE `t_teams` ENABLE KEYS */;

-- Dumping structure for table spartanclash_datapipeline.t_teamsources
DROP TABLE IF EXISTS `t_teamsources`;
CREATE TABLE IF NOT EXISTS `t_teamsources` (
  `teamSource` varchar(128) NOT NULL COMMENT 'Name of the source, like "Halo Waypoint."',
  `sourceURL` varchar(128) NOT NULL COMMENT 'Top level URL for the source, like "www.halowaypoint.com"',
  `teamCommonName` varchar(128) NOT NULL COMMENT 'Printable name for the team, like "Spartan Company"',
  `beganTrackingOnDate` timestamp NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`teamSource`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping data for table spartanclash_datapipeline.t_teamsources: ~0 rows (approximately)
/*!40000 ALTER TABLE `t_teamsources` DISABLE KEYS */;
INSERT INTO `t_teamsources` (`teamSource`, `sourceURL`, `teamCommonName`, `beganTrackingOnDate`) VALUES
	('Halo Waypoint', 'www.halowaypoint.com', 'Spartan Company', '2018-01-16 22:20:24');
/*!40000 ALTER TABLE `t_teamsources` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;